using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CFLocationBuilder
{
    public class ClCountry
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "regions")]
        public IEnumerable<ClRegion> Regions { get; set; }
    }
}
