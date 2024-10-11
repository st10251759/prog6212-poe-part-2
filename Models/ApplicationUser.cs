using Microsoft.AspNetCore.Identity;

namespace ST10251759_PROG6212_POE.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        // List of claims linked to the user
        public virtual ICollection<Claim> Claims { get; set; }
    }
}
