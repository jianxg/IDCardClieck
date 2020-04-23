using IDCardClieck.DAL;
using IDCardClieck.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCardClieck.BLL
{
    public class User_InfosServer
    {
        private readonly User_InfosDAL dal = new User_InfosDAL();
        public User_InfosServer()
        { }

        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Id)
        {
            return dal.Exists(Id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(User_InfosItem model)
        {
            return dal.Add(model);

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(User_InfosItem model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {

            return dal.Delete(Id);
        }
        /// <summary>
        /// 批量删除一批数据
        /// </summary>
        public bool DeleteList(string Idlist)
        {
            return dal.DeleteList(Idlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public User_InfosItem GetModel(int Id)
        {

            return dal.GetModel(Id);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<User_InfosItem> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<User_InfosItem> DataTableToList(DataTable dt)
        {
            List<User_InfosItem> modelList = new List<User_InfosItem>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                User_InfosItem model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new User_InfosItem();
                    if (dt.Rows[n]["Id"].ToString() != "")
                    {
                        model.Id = int.Parse(dt.Rows[n]["Id"].ToString());
                    }
                    model.Account = dt.Rows[n]["Account"].ToString();
                    model.Password = dt.Rows[n]["Password"].ToString();
                    model.RegistrationNum = dt.Rows[n]["RegistrationNum"].ToString();
                    if (dt.Rows[n]["CreateDate"].ToString() != "")
                    {
                        model.CreateDate = DateTime.Parse(dt.Rows[n]["CreateDate"].ToString());
                    }
                    model.Creator = dt.Rows[n]["Creator"].ToString();
                    if (dt.Rows[n]["UpdateDate"].ToString() != "")
                    {
                        model.UpdateDate = DateTime.Parse(dt.Rows[n]["UpdateDate"].ToString());
                    }
                    model.Mdifier = dt.Rows[n]["Mdifier"].ToString();


                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }
        #endregion
    }
}
