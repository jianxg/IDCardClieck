using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCardClieck.Model
{
   public class ResultJSON
    {
        public string result { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public List<CheckModel> hotCheckItemList { get; set; }

        public List<CheckModel> checkItemList { get; set; }
    }
}
