using VIOBANK.API.Contracts.User;
using VIOBANK.Domain.Models;

namespace VIOBANK.API.Contracts.Admin
{
    public class AdminUserRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public EmploymentDTO Employment { get; set; }
        public string IdCard { get; set; }
        public string TaxNumber { get; set; }
        public string Registration { get; set; }
        public string CardPassword { get; set; }
        public List<string> Roles { get; set; }
        public string CardCurrency { get; set; }
    }
}
