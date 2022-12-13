using Microsoft.AspNetCore.Identity;

namespace AspNetBiodiv.Core.Web.Entities
{
    public class BiodivUser : IdentityUser
    {
        [PersonalData]
        public string Commune { get; set; }
    }
}
