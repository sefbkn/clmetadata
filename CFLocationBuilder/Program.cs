using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HtmlAgilityPack;

namespace CFLocationBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            // Retrieve the locations from craigslist site.
            //var baseLocationsUrl = "file:///C:/Users/sef/Desktop/craigslist_dump.htm";
            //var baseCategoriesUrl = "file:///C:/Users/sef/Desktop/cl_categories.htm";

            var baseLocationsUrl = "https://www.craigslist.org/about/sites";
            var baseCategoriesUrl = "http://auburn.craigslist.org";
            var webClient = new WebClient();

            var locationsHtml = webClient.DownloadString(baseLocationsUrl);
            var categoriesHtml = webClient.DownloadString(baseCategoriesUrl);

            // Pull categories from a listings page and retrieve all of the relevant information.

            var regions = ClRegion.ParseRegionsFromHtml(locationsHtml);
            var categories = ClCategory.ParseCategoriesFromHtml(categoriesHtml);

            // Write data to json files on the desktop.
            var targetDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var json = JsonConvert.SerializeObject(new { 
                categories = categories,
                locations = regions
            }, Formatting.Indented);

            var metadataFileOut = Path.Combine(targetDirectory, "cl_metadata.json");

            File.WriteAllText(metadataFileOut, json);
            
            Console.WriteLine(json);
            Console.ReadLine();
        }
    }
}
