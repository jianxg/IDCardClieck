using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCardClieck.Model
{
    public class ResultJson_Zytzbs
    {
        public string result { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public htmlData data { get; set; }
    }

    public class htmlData
    {
        public string cid { get; set; }

        public string cName { get; set; }

        public string cContent { get; set; }
    }

}
