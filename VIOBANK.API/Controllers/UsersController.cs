﻿using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql.BackendMessages;
using VIOBANK.API.Contracts.User;
using VIOBANK.Application.Services;
using VIOBANK.Domain.Enums;
using VIOBANK.Domain.Models;
using VIOBANK.Infrastructure;

namespace VIOBANK.Controllers
{
    [Authorize]
    [ApiController]
    [Route("profile")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserService _userService;
        private readonly IValidator<UserProfileDTO> _userValidator;
        private readonly IValidator<int> _userIdValidator;
        private readonly IJwtProvider _jwtProvider;
        private readonly CloudinaryService _cloudinaryService;


        public UsersController(CloudinaryService cloudinaryService, ILogger<UsersController> logger, UserService userService, IValidator<UserProfileDTO> userValidator, IJwtProvider jwtProvider, IValidator<int> userIdValidator)
        {
            _logger = logger;
            _userService = userService;
            _userValidator = userValidator;
            _jwtProvider = jwtProvider;
            _userIdValidator = userIdValidator;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        [RequirePermissions(PermissionEnum.Read)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                _logger.LogInformation("User Profile Request");

                var token = HttpContext.Request.Cookies["tasty-cookies"];

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { status = "error", message = "Token is missing in cookies" });
                }

                var claims = _jwtProvider.GetClaimsFromToken(token);
                var userId = int.Parse(User.FindFirst("userId")?.Value);

                var validationResult = await _userIdValidator.ValidateAsync(userId);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { status = "error", message = validationResult.Errors.Select(e => e.ErrorMessage).ToArray() });
                }

                var profile = await _userService.GetUserById(userId);

                var userProfileDTO = new UserProfileDTO
                {
                    Name = profile.Name,
                    Surname = profile.Surname,
                    Email = profile.Email,
                    Phone = profile.Phone,
                    Employment = new EmploymentDTO
                    {
                        Type = profile.Employment.Type,
                        Income = profile.Employment.Income
                    },
                    IdCard = profile.IdCard,
                    TaxNumber = profile.TaxNumber,
                    Registration = profile.Registration
                };

                return Ok(userProfileDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting profile: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Internal server error" });
            }
        }

        [HttpPut]
        [Route("update")]
        [RequirePermissions(PermissionEnum.Update)]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDTO userDTO)
        {
            try
            {
                _logger.LogInformation("Updating a User Profile");

                var token = HttpContext.Request.Cookies["tasty-cookies"];

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { status = "error", message = "Token is missing in cookies" });
                }

                var claims = _jwtProvider.GetClaimsFromToken(token);
                var userId = int.Parse(User.FindFirst("userId")?.Value);

                var validationResultId = await _userIdValidator.ValidateAsync(userId);
                if (!validationResultId.IsValid)
                {
                    return BadRequest(new { status = "error", message = validationResultId.Errors.Select(e => e.ErrorMessage).ToArray() });
                }

                var validationResult = await _userValidator.ValidateAsync(userDTO);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { status = "error", message = validationResult.Errors.Select(e => e.ErrorMessage).ToArray() });
                }

                var user = await _userService.GetUserById(userId);

                user.Name = userDTO.Name;
                user.Surname = userDTO.Surname;
                user.Email = userDTO.Email;
                user.Phone = userDTO.Phone;
                user.IdCard = userDTO.IdCard;
                user.TaxNumber = userDTO.TaxNumber;
                user.Registration = userDTO.Registration;

                if (userDTO.Employment != null)
                {
                    if (user.Employment == null)
                    {
                        user.Employment = new Employment();
                    }
                    user.Employment.Type = userDTO.Employment.Type;
                    user.Employment.Income = userDTO.Employment.Income;
                }

                await _userService.UpdateUser(user);

                return Ok(new { status = "success", message = "Profile updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating profile: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Internal server error" });
            }
        }

        [HttpPost("upload-avatar")]
        [RequirePermissions(PermissionEnum.Update)]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File upload error");

            var token = HttpContext.Request.Cookies["tasty-cookies"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { status = "error", message = "Token is missing in cookies" });
            }

            var claims = _jwtProvider.GetClaimsFromToken(token);
            var userId = int.Parse(User.FindFirst("userId")?.Value);

            var validationResultId = await _userIdValidator.ValidateAsync(userId);
            if (!validationResultId.IsValid)
            {
                return BadRequest(new { status = "error", message = validationResultId.Errors.Select(e => e.ErrorMessage).ToArray() });
            }

            try
            {
                using var stream = file.OpenReadStream();
                var imageUrl = await _cloudinaryService.UploadImageAsync(stream, file.FileName, userId.ToString());
                return Ok(new { AvatarUrl = imageUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
        }

        [HttpGet("avatar")]
        [RequirePermissions(PermissionEnum.Read)]
        public IActionResult GetUserAvatar()
        {
            var token = HttpContext.Request.Cookies["tasty-cookies"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { status = "error", message = "Token is missing in cookies" });
            }

            var claims = _jwtProvider.GetClaimsFromToken(token);
            var userId = int.Parse(User.FindFirst("userId")?.Value);

            try
            {
                var avatarUrl = _cloudinaryService.GetUserAvatarUrl(userId.ToString());
                return Ok(new { AvatarUrl = avatarUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new {status = "error", message = ex.Message});
            }
        }
    }
}
