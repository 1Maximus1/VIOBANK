namespace VIOBANK.Domain.Models
{
    public class Contact
    {
        public int ContactId { get; set; }
        public int UserId { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactCard { get; set; }

        public User User { get; set; }
    }
}
