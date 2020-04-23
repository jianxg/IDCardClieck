using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCardClieck.Model
{
    public class User_InfosItem
    {
        /// <summary>
		/// Id
        /// </summary>		
		private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// Account
        /// </summary>		
        private string _account;
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }
        /// <summary>
        /// Password
        /// </summary>		
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        /// <summary>
        /// CDKey
        /// </summary>		
        private string _cdkey;
        public string CDKey
        {
            get { return _cdkey; }
            set { _cdkey = value; }
        }
        /// <summary>
        /// RegistrationNum
        /// </summary>		
        private string _registrationnum;
        public string RegistrationNum
        {
            get { return _registrationnum; }
            set { _registrationnum = value; }
        }
        /// <summary>
        /// CreateDate
        /// </summary>		
        private DateTime _createdate;
        public DateTime CreateDate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        /// <summary>
        /// Creator
        /// </summary>		
        private string _creator;
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        /// <summary>
        /// UpdateDate
        /// </summary>		
        private DateTime _updatedate;
        public DateTime UpdateDate
        {
            get { return _updatedate; }
            set { _updatedate = value; }
        }
        /// <summary>
        /// Mdifier
        /// </summary>		
        private string _mdifier;
        public string Mdifier
        {
            get { return _mdifier; }
            set { _mdifier = value; }
        }
    }
}
