using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ViewModel
{
    public class BlobViewModel
    {

        [JsonProperty(PropertyName = "Image  URI")]
        public string URI { get; set; }


    }
}
