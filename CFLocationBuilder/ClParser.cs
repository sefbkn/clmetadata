using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFLocationBuilder
{
    public class ClParser
    {
        public static IEnumerable<ClCategory> ParseCategoriesFromHtml(string categoriesHtml)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(categoriesHtml);

            var categories = from rootNode in htmlDocument.DocumentNode.SelectNodes("//*[@class='col']")
                             let categoryCode = rootNode.GetAttributeValue("id", null)
                             where categoryCode != null
                             let categoryName = rootNode.SelectSingleNode(".//h4[@class='ban']").InnerText.Trim()
                             let subcategoryNodes = rootNode.SelectNodes(".//li/a")
                             where subcategoryNodes != null
                             select new ClCategory()
                             {
                                 Name = categoryName,
                                 Code = categoryCode,
                                 SubCategories =
                                      from subCatNode in subcategoryNodes
                                      let subcategoryCode = subCatNode.GetAttributeValue("href", null)
                                      let subcategoryName = subCatNode.InnerText.Trim().Replace("&nbsp;", " ")
                                      where subcategoryCode != null
                                      select new ClSubCategory()
                                      {
                                          Code = subcategoryCode,
                                          Name = subcategoryName
                                      }
                             };

            return categories;
        }

        public static IEnumerable<ClCountry> ParseRegionsFromHtml(string locationsHtml)
        {
            // Load the html into HTMLAgilityPack
            // for parsing.
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(locationsHtml);

            // Filter out non-relevant nodes.
            var lastRegion = String.Empty;
            var lastCountry = String.Empty;
            var regions =
                   (from bodyNode in htmlDocument.DocumentNode.SelectSingleNode("//section[@class='body']").Descendants()
                    let isCountryNode = bodyNode.Name == "h1"
                    let isRegionsNode = (bodyNode.Name == "div" && bodyNode.GetAttributeValue("class", null) == "colmask")
                    where isCountryNode || isRegionsNode
                    let countryName = isCountryNode ? (lastCountry = bodyNode.InnerText.Trim()) : lastCountry
                    where isRegionsNode
                    from rootNode in bodyNode.SelectNodes(".//div[contains(@class, 'box')]")
                    from childNode in rootNode.ChildNodes.Where(n => n.NodeType == HtmlNodeType.Element)
                    let isH4 = childNode.Name == "h4"
                    let isUl = childNode.Name == "ul"
                    where isH4 || isUl
                    // Dirty mutability... I just wanna use linq ;(
                    // The html is formatted such that cities are preceeded by the h4 containing the "Region/City"
                    let region = isH4 ? (lastRegion = childNode.InnerText.Trim()) : lastRegion
                    where isUl
                    from linkNode in childNode.SelectNodes(".//li/a")
                    let subregion = new ClSubRegion()
                    {
                        Name = linkNode.InnerText,
                        RegionName = region,
                        Url = linkNode.GetAttributeValue("href", null),
                        Country = countryName
                    }
                    where subregion.Url != null
                    group subregion by new { subregion.RegionName, subregion.Country } into region
                    group region by region.Key.Country into country
                    select new ClCountry()
                    {
                        Name = country.Key,
                        Regions = country.Select(x => new ClRegion(){ Name = x.Key.RegionName, SubRegions = x })
                    }).ToList();

            return regions;
        }
    }
}
