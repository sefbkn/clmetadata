using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CFLocationBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            // Retrieve the locations from craigslist site.
            //var baseUrl = "https://www.craigslist.org/about/sites";
            var baseUrl = "file:///C:/Users/sef/Desktop/craigslist_dump.htm";
            var webClient = new WebClient();
            var locationsHtml = webClient.DownloadString(baseUrl);

            var regions = ClRegion.ParseRegionsFromHtml(locationsHtml);

            var targetDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var json = JsonConvert.SerializeObject(regions, Formatting.Indented);
            var filePath = Path.Combine(targetDirectory, "cl_data.json");
            File.WriteAllText(filePath, json);
            Console.WriteLine(json);
            Console.ReadLine();
        }
    }
}
