using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CFLocationBuilder
{
    public class ClRegion
    {
        public string Name { get; set; }
        public IEnumerable<ClSubRegion> SubRegions { get; set; }

        public static IEnumerable<ClRegion> ParseRegionsFromHtml(string locationsHtml)
        {
            // Load the html into HTMLAgilityPack
            // for parsing.
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(locationsHtml);

            // Filter out non-relevant nodes.
            var lastRegion = String.Empty;
            var regions =
                   (from rootNode in htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'box')]")
                    from childNode in rootNode.ChildNodes.Where(n => n.NodeType == HtmlNodeType.Element)
                    let isH4 = childNode.Name == "h4"
                    let isUl = childNode.Name == "ul"
                    where isH4 || isUl
                    // Dirty mutability... I just wanna use linq ;(
                    // The html is formatted such that cities are preceeded by the h4 containing the "Region/City"
                    let _ = isH4 ? (lastRegion = childNode.InnerText.Trim()) : "" 
                    where isUl
                    from linkNode in childNode.SelectNodes(".//li/a")
                    let subregion = new ClSubRegion()
                    {
                        Name = linkNode.InnerText,
                        RegionName = lastRegion,
                        Url = linkNode.GetAttributeValue("href", null)
                    }
                    where subregion.Url != null
                    group subregion by subregion.RegionName into region
                    select new ClRegion()
                    {
                        Name = region.Key,
                        SubRegions = region
                    }).ToList();

            return regions;
        }
    }
}
