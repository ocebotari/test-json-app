using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyApp
{
    public class JsonFileComparator
    {
        private readonly ILogger _logger;
        private readonly IReadJsonFile _fileReader1;
        private readonly IReadJsonFile _fileReader2;

        private List<JsonModel> _json1;
        private List<JsonModel> _json2;

        public JsonFileComparator(IReadJsonFile fileReader1, IReadJsonFile fileReader2, ILogger logger)
        {
            if (fileReader1 == null) throw new ArgumentNullException(nameof(fileReader1));
            if (fileReader2 == null) throw new ArgumentNullException(nameof(fileReader2));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _fileReader1 = fileReader1;
            _fileReader2 = fileReader2;
            _logger = logger;
        }

        private void ReadFiles()
        {
            _json1 = _fileReader1.Read();
            _json2 = _fileReader2.Read();
        }

        public void DOWork()
        {
            try
            {
                ReadFiles();

                var dictA = CreateDictionary(_json1);

                var dictB = CreateDictionary(_json2);

                var missingKeys = dictA.Keys.Except(dictB.Keys).ToList();
                missingKeys.ForEach(k => Console.WriteLine(k));

                missingKeys = dictB.Keys.Except(dictA.Keys).ToList();
                missingKeys.ForEach(k => Console.WriteLine(k));

                var intersected = dictA.Keys.Where(k => dictB.ContainsKey(k)).ToList();
                intersected.ForEach(k => Console.WriteLine(k));

                _logger.WriteLine("Values which are not equal between A and B");

                CompareValues(intersected, dictA, dictB);

                _logger.WriteLine($"Primary keys which exist in A but not in B ({missingKeys.Count}):");
                _logger.WriteLine($"Primary keys which exist in B but not in A ({missingKeys.Count}):");
                _logger.WriteLine($"Primary keys exist in B and in A ({intersected.Count}):");


            }
            catch (Exception ex)
            {
                _logger.WriteLine(ex.Message);
            }
        }

        private static Dictionary<string, Dictionary<string, PrimaryKey>> CreateDictionary(List<JsonModel> list)
        {
            var dict = new Dictionary<string, Dictionary<string, PrimaryKey>>();
            list.ForEach(l =>
            {
                var f = "{0}:{1}";
                var key = CreateDictKey(l.PrimaryKey, f);
                if (dict.ContainsKey(key))
                    dict[key] = l.Values;
                else
                    dict.Add(key, l.Values);
            });

            return dict;
        }
        
        private static string CreateDictKey(Dictionary<string, PrimaryKey> dict, string format)
        {
            List<string> keys = new List<string>();
            dict.Keys.ToList().ForEach(k =>
            {
                keys.Add(string.Format(format, k, dict[k].ToString()));
            });

            var key = string.Join(",", keys);

            return key;
        }

        private void CompareValues(List<string> keys, Dictionary<string, Dictionary<string, PrimaryKey>> dictA, Dictionary<string, Dictionary<string, PrimaryKey>> dictB)
        {
            foreach (var key in keys)
            {
                var val1 = dictA[key];
                var val2 = dictB[key];
                if (val1 == val2) continue;

                string values = null;
                string tmpString = null;
                JsonModel jModel = new JsonModel();

                foreach (var item in val1)
                {
                    string jKey = item.Key;
                    var jValue = item.Value;
                    if (val2.ContainsKey(jKey) && !jValue.Equals(val2[jKey]))

                    {
                        tmpString = string.Concat(tmpString, $"{jKey}:{jValue.ToString()},");
                    }
                }
                if (!string.IsNullOrWhiteSpace(tmpString))
                {
                    var jsonValues = JsonConvert.SerializeObject(tmpString.TrimEnd(','), PrimaryKeyConverter.Singleton);
                    values = $"Values:{jsonValues}";
                }

                if (values != null)
                {
                    var jsonKey = JsonConvert.SerializeObject(key, PrimaryKeyConverter.Singleton);
                    var str = JsonConvert.SerializeObject($"[{{PrimaryKey: {jsonKey}, {values}}}]");
                    _logger.WriteLine(str.Replace("\n", "").Replace("\t", ""));
                }
            }
        }
    }
}
