namespace AspNetBiodiv.Core.Web.Services.UserData
{
    public interface IUserDataService
    {
        Task<string?> GetCommuneForUserAsync(string? email);
    }
}
