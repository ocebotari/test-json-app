using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyApp
{
    public class ReadJsonFile : IReadJsonFile
    {
        private string _file;
        public ReadJsonFile(string file)
        {
            _file = file;
        }

        public List<JsonModel> Read()
        {
            var lines = File.ReadLines(_file);
            var json = string.Join("", lines);

            return JsonModel.FromJson(json);
        }
    }

    public interface IReadJsonFile
    {
        List<JsonModel> Read();
    }
}
