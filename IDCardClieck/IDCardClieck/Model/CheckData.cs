using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCardClieck.Model
{
    public class CheckData
    {
        public string result { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public List<CheckDataListModel> data { get; set; }

    }

    public class CheckModel
    {
        public int tempPropID { get; set; }
        public string propName { get; set; }
    }

    public class CheckDataListModel
    {
        public string propID { get; set; }
        public string propValue { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string propName { get; set; }
        public string punit { get; set; }
        public string pscope { get; set; }
        public int highLowMark { get; set; }
    }

}
