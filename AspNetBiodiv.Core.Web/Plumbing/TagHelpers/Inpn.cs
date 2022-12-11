using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetBiodiv.Core.Web.Plumbing.TagHelpers
{
    public class InpnTagHelper : TagHelper
    {
        private const string RootUrl = "https://inpn.mnhn.fr/espece/cd_nom/";
        public int Code { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", RootUrl + Code);
            output.Content.SetContent(output.GetChildContentAsync().Result.GetContent());
        }
    }
}
