using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCardClieck.Model
{
    public class HistoryCheckData
    {
        public string result { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public List<HistoryCheckDataDataModel> data { get; set; }
    }

    public class HistoryCheckDataDataModel
    {
        public string propID { get; set; }
        public string orderNo { get; set; }

        public string propValue { get; set; }

        public string propName { get; set; }

        public string propTime { get; set; }

        public int cellWidth { get; set; }
    }
}
