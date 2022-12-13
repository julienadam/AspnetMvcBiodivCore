using AspNetBiodiv.Core.Web.Entities;
using Microsoft.AspNetCore.Identity;

namespace AspNetBiodiv.Core.Web.Services.UserData;

public class IdentityUserDataService : IUserDataService
{
    private readonly UserManager<BiodivUser> userManager;

    public IdentityUserDataService(UserManager<BiodivUser> userManager)
    {
        this.userManager = userManager;
    }
    public async Task<string?> GetCommuneForUserAsync(string? email)
    {
        if (email == null)
        {
            return null;
        }

        var user = await userManager.FindByEmailAsync(email);
        return user?.Commune;
    }
}