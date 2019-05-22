using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using DK.WebCheck.Common;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace DK.WebCheck.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new Service() 
			};
            ServiceBase.Run(ServicesToRun);
        }

        static List<SiteInfo> PollSites()
        {
            var sites = new DK.WebCheck.Common.SiteInfo().GetSites();
            Stopwatch stopwatch = new Stopwatch();
            List<SiteInfo> resultList = new List<SiteInfo>();
            SiteInfo selectedSite = new SiteInfo();

            try
            {
                HttpWebRequest request;
                HttpWebResponse response;
                Stream stream;

                foreach (SiteInfo site in sites)
                {
                    if (site._IsChecked == true) { continue; }

                    stopwatch = new Stopwatch();
                    stopwatch.Start();

                    selectedSite = site;
                    request = (HttpWebRequest)WebRequest.Create(site.Url);

                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                        stream = response.GetResponseStream();

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            site.IsOnline = true;
                        }
                        else
                        {
                            site.IsOnline = false;
                        }

                        var results = new StreamReader(stream).ReadToEnd();
                        stopwatch.Stop();
                        site.ResponseTime = stopwatch.ElapsedMilliseconds.ToString();
                        site.HttpStatus = response.StatusCode.ToString();
                        site._IsChecked = true;
                        resultList.Add(site);
                    }
                    catch (WebException exWeb)
                    {
                        stopwatch.Stop();
                        var exResponse = exWeb.Response as HttpWebResponse;

                        if (exResponse != null)
                        {
                            selectedSite.ResponseTime = stopwatch.ElapsedMilliseconds.ToString();
                            selectedSite.HttpStatus = exResponse.StatusCode.ToString();
                            selectedSite.IsOnline = true;
                        }
                        else
                        {
                            selectedSite.ResponseTime = null;
                            selectedSite.HttpStatus = exWeb.Message.Replace("'", "");
                            selectedSite.IsOnline = false;
                        }

                        selectedSite._IsChecked = true;
                        resultList.Add(selectedSite);
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
            }

            return resultList;
        }
    }
}
