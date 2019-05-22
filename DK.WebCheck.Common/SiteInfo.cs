using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DK.WebCheck.Common
{
    public class SiteInfo
    {
        public bool _IsChecked { get; set; }
        public int ID { get; set; }

        [Display(Name = "Site Name/Description")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "URL")]
        public string Url { get; set; }

        [Display(Name = "Online")]
        public bool IsOnline { get; set; }

        [Display(Name = "Status")]
        public string HttpStatus { get; set; }

        [Display(Name = "Response Time")]
        public string ResponseTime { get; set; }

        public SiteInfo()
        { }

        public SiteInfo(string siteName, string siteDescription, string siteUrl)
        {
            this.Name = siteName;
            this.Description = siteDescription;
            this.Url = siteUrl;
        }

        public List<SiteInfo> GetSites()
        {
            //TODO: Move to config file...
            SiteInfo[] sites = new SiteInfo[]{
            new SiteInfo { ID=1, Name="denniskhau.com", Description="", Url="https://denniskhau.com/"},
            new SiteInfo { ID=1, Name="dennisdoes.net", Description="", Url="https://dennisdoes.net/"}
            };

            return sites.ToList();
        }
    }
}
