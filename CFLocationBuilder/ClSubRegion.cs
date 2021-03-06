﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFLocationBuilder
{
    public class ClSubRegion
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string RegionName { get; set; }

        [JsonIgnore]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}
