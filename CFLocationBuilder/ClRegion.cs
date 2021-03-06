﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace CFLocationBuilder
{
    public class ClRegion
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "sub_regions")]
        public IEnumerable<ClSubRegion> SubRegions { get; set; }        
    }
}
