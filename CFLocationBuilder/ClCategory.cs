using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFLocationBuilder
{
    public class ClCategory
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "sub_categories")]
        public IEnumerable<ClSubCategory> SubCategories { get; set; }

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
    }
}
