using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VIOBANK.Application.Services;
using VIOBANK.Domain.Enums;
using VIOBANK.Infrastructure;
using VIOBANK.API.Contracts.Admin;
using VIOBANK.API.Contracts.Auth;
using VIOBANK.Domain.Models;
using FluentValidation;
using VIOBANK.API.Contracts.User;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace VIOBANK.Controllers
{
    [Authorize]
    [ApiController]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly UserService _userService;
        private readonly PermissionService _permissionService;
        private readonly IValidator<RegisterUserDTO> _registerValidator;
        private readonly IValidator<int> _userIdValidator;
        private readonly CardService _cardService;
        private readonly AesEncryptionService _aesEncryptionService;

        public AdminController(AesEncryptionService aesEncryptionService, CardService cardService, IValidator<int> userIdValidator, IValidator<RegisterUserDTO> registerValidator, UserService userService, PermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
            _registerValidator = registerValidator;
            _userIdValidator = userIdValidator;
            _cardService = cardService;
            _aesEncryptionService = aesEncryptionService;
        }

        [HttpGet("users")]
        [RequirePermissions(PermissionEnum.ManageUsers)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users.Select(u => new
            {
                u.UserId,
                u.Email,
                Roles = u.Roles.Select(r => r.Name).ToList(),
                u.Name,
                u.Surname
            }));
        }

        [HttpGet("permissions/{userId}")]
        [RequirePermissions(PermissionEnum.ManageUsers)]
        public async Task<IActionResult> GetUserPermissions(int userId)
        {
            var permissions = await _userService.GetPermissionsList(userId);
            if (permissions == null)
            {
                return NotFound(new { status = "error", message = "User not found or has no permissions" });
            }

            var permissionsValue = permissions.Select(p => p.ToString()).ToList();
            return Ok(new { userId, permissions = permissionsValue });
        }

        [HttpPost("add-user")]
        [RequirePermissions(PermissionEnum.ManageUsers)]
        public async Task<IActionResult> AddUser([FromBody] AdminUserRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { status = "error", message = "Email and password are required" });
            }

            var validationResultReg = await _registerValidator.ValidateAsync(new RegisterUserDTO
            {
                Name = request.Name,
                Surname = request.Surname,
                Phone = request.Phone,
                Email = request.Email,
                Employment = new EmploymentDTO
                {
                    Type = request.Employment.Type,
                    Income = request.Employment.Income,
                },
                IdCard = request.IdCard,
                TaxNumber = request.TaxNumber,
                Registration = request.Registration,
                Password = request.Password,
                CardPassword = request.CardPassword
            });
            if (!validationResultReg.IsValid)
            {
                return BadRequest(new { status = "error", message = validationResultReg.Errors.Select(e => e.ErrorMessage).ToArray() });
            }

            var user = new User
            {
                Name = request.Name,
                Surname = request.Surname,
                Phone = request.Phone,
                Email = request.Email,
                Employment = new Employment
                {
                    Type = request.Employment.Type,
                    Income = request.Employment.Income,
                },
                IdCard = request.IdCard,
                TaxNumber = request.TaxNumber,
                Registration = request.Registration,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userService.CreateUser(user, request.Password, request.CardPassword, request.Roles, request.CardCurrency);
            if (!result.Success)
            {
                return BadRequest(new { status = "error", message = result.Message });
            }

            return Ok(new { status = "success", message = "User created successfully", userId = result.UserId });
        }

        [HttpDelete("delete-user/{userId}")]
        [RequirePermissions(PermissionEnum.ManageUsers)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound(new { status = "error", message = "User not found" });
            }

            await _userService.DeleteUser(userId);
            return Ok(new { status = "success", message = "User deleted successfully" });
        }

        [HttpPut("cards/change-amount")]
        [RequirePermissions(PermissionEnum.ManageCards)]
        public async Task<IActionResult> ChangeCardAmount ([FromBody] CardAmountUpdateDto cardAmountUpdate)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                     .Select(e => e.ErrorMessage)
                                     .ToArray();
                return BadRequest(new { status = "error", message = errors });
            }

            var card = await _cardService.GetCardByNumber(cardAmountUpdate.CardNumber);

            if (card == null)
            {
                return NotFound(new { status = "error", message = "Card not found" });
            }

            card.Balance = cardAmountUpdate.Amount;

            await _cardService.UpdateCard(card);
            return Ok(new { status = "success", message = "Card amount updated" });
        }

        [HttpGet("cards/info")]
        [RequirePermissions(PermissionEnum.ManageCards)]
        public async Task<IActionResult> GetCards()
        {
            var cards = await _cardService.GetAllCards();

            if (cards == null)
            {
                return NotFound(new { status = "error", message = "User not found" });
            }

            return Ok(new { status = "success", message = cards.Select(c => new {
                c.CardId,
                c.CardNumber,
                c.HolderName,
                ExpiryDate = c.ExpiryDate.ToLocalTime().ToString("yyyy-MM-dd"),
                c.Type,
                c.Bank,
                c.Status,
                c.Balance,
                c.Account.Currency,
                CreatedAt = c.CreatedAt.ToLocalTime().ToString("yyyy-MM-dd"),
                CardPassword = _aesEncryptionService.Decrypt(c.CardPassword),
                c.Cvc,
                c.Account.AccountNumber,
                c.Account.User.Surname,
                c.Account.User.Name,
                c.Account.User.Email,
                c.Account.User.Phone
            })});
        }
    }
}
