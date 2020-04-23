using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCardClieck.Model
{
   public class SelectCheckData
    {
        public string result { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public SelectCheckDataData data { get; set; }
    }

    public class SelectCheckDataData
    {
        public List<CheckDataListModel> checkDataList { get; set; }
    }
}
