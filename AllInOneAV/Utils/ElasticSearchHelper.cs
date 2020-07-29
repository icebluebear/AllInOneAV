using Model.JavModels;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ElasticSearchHelper
    {
        public static void InsertAV(string index, string title, string actress, string director, string category, string company, string publisher)
        {
            AvElasticSearchItem item = new AvElasticSearchItem()
            {
                actress = actress,
                category = category,
                company = company,
                direcotr = director,
                publisher = publisher,
                title = title
            };

            RestClient client = new RestClient("http://www.cainqs.com:9200");

            client.Post(JsonConvert.SerializeObject(item), index + "/_doc");
        }
    }
}
