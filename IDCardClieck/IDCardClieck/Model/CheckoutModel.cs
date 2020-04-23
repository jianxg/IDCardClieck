using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCardClieck.Model
{
    public class CheckoutModel
    {
        private string _registerCodeName = "registerCode";
        private string _path = "Software\\IDCardRegisterCode";

        /// <summary>
        /// 注册表获取结果 0已注册 1未注册 
        /// </summary>
        public int res { get; set; }

        /// <summary>
        /// 激活码
        /// </summary>
        public string sericalNumber { get; set; }

        /// <summary>
        /// 注册码
        /// </summary>
        public string registerCode { get; set; }

        /// <summary>
        /// 注册表地址
        /// </summary>
        public string path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// 注册表名称
        /// </summary>
        public string registerCodeName
        {
            get { return _registerCodeName; }
            set { _registerCodeName = value; }
        }
    }
}
