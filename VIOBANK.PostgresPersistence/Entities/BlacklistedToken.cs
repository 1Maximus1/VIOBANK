
namespace VIOBANK.PostgresPersistence.Entities
{
    public class BlacklistedToken
    {
        public string Token { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
