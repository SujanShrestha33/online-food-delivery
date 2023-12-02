using Microsoft.AspNetCore.Identity;

namespace OnlineFoodOrdering.Models.Entity
{
    public class ApplicationUsers : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
    }
}
