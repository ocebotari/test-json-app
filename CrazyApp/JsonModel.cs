using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyApp
{
    public partial class JsonModel
    {
        [JsonProperty("PrimaryKey")]
        public Dictionary<string, PrimaryKey> PrimaryKey { get; set; }

        [JsonProperty("Values")]
        public Dictionary<string, PrimaryKey> Values { get; set; }
    }

    public partial class JsonModel
    {
        public static List<JsonModel> FromJson(string json) => JsonConvert.DeserializeObject<List<JsonModel>>(json, Converter.Settings);
    }

    public partial struct PrimaryKey
    {
        public double? Double;
        public string String;

        public static implicit operator PrimaryKey(double Double) => new PrimaryKey { Double = Double };
        public static implicit operator PrimaryKey(string String) => new PrimaryKey { String = String };

        public override string ToString()
        {
            return Double.HasValue ? Double.ToString() : String;
        }
    }
}
