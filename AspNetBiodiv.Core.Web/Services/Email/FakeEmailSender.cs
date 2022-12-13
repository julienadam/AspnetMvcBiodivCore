using Microsoft.Extensions.Options;

namespace AspNetBiodiv.Core.Web.Services.Email
{
    public class ConfirmationEmailOptions
    {
        public string From { get; set; } = "";
        public string AdminAddress { get; set; } = "";
        public bool ShouldCcAdmins { get; set; } = false;
    }

    public interface IEmailSender
    {
        void SendThankYouEmail(string email, string detailsUrl);
    }

    public class FakeEmailSender : IEmailSender
    {
        private readonly IOptionsSnapshot<ConfirmationEmailOptions> options;
        
        public FakeEmailSender(IOptionsSnapshot<ConfirmationEmailOptions> options)
        {
            this.options = options;
        }

        public void SendThankYouEmail(string email, string detailsUrl)
        {
            var fs = File.CreateText(Path.Combine(Path.GetTempPath(), "Email" + Guid.NewGuid() + ".txt"));
            fs.WriteLine("From :" + options.Value.From);
            fs.WriteLine("To: " + email);
            if (options.Value.ShouldCcAdmins)
            {
                fs.WriteLine("cc: " + options.Value.AdminAddress);
            }
            fs.WriteLine("Subject : Merci pour votre contribution");
            fs.WriteLine("Body : Vous pouvez voir votre observation à cette adresse " + detailsUrl);
            fs.Close();
        }
    }
}
