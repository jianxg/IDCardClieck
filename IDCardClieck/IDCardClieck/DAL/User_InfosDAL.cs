using IDCardClieck.Common;
using IDCardClieck.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCardClieck.DAL
{
    public class User_InfosDAL
    {
        public bool Exists(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from User_Infos");
            strSql.Append(" where ");
            strSql.Append(" Id = @Id  ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)
            };
            parameters[0].Value = Id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(User_InfosItem model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into User_Infos(");
            strSql.Append("Account,Password,CDKey,RegistrationNum,CreateDate,Creator,UpdateDate,Mdifier");
            strSql.Append(") values (");
            strSql.Append("@Account,@Password,@CDKey,@RegistrationNum,@CreateDate,@Creator,@UpdateDate,@Mdifier");
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                        new SqlParameter("@Account", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Password", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@CDKey", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@RegistrationNum", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@CreateDate", SqlDbType.DateTime) ,
                        new SqlParameter("@Creator", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@UpdateDate", SqlDbType.DateTime) ,
                        new SqlParameter("@Mdifier", SqlDbType.NVarChar,50)

            };

            parameters[0].Value = model.Account;
            parameters[1].Value = model.Password;
            parameters[2].Value = model.CDKey;
            parameters[3].Value = model.RegistrationNum;
            parameters[4].Value = model.CreateDate;
            parameters[5].Value = model.Creator;
            parameters[6].Value = model.UpdateDate;
            parameters[7].Value = model.Mdifier;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {

                return Convert.ToInt32(obj);

            }

        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(User_InfosItem model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update User_Infos set ");

            strSql.Append(" Account = @Account , ");
            strSql.Append(" Password = @Password , ");
            strSql.Append(" CDKey = @CDKey , ");
            strSql.Append(" RegistrationNum = @RegistrationNum , ");
            strSql.Append(" CreateDate = @CreateDate , ");
            strSql.Append(" Creator = @Creator , ");
            strSql.Append(" UpdateDate = @UpdateDate , ");
            strSql.Append(" Mdifier = @Mdifier  ");
            strSql.Append(" where Id=@Id ");

            SqlParameter[] parameters = {
                        new SqlParameter("@Id", SqlDbType.Int,4) ,
                        new SqlParameter("@Account", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Password", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@CDKey", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@RegistrationNum", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@CreateDate", SqlDbType.DateTime) ,
                        new SqlParameter("@Creator", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@UpdateDate", SqlDbType.DateTime) ,
                        new SqlParameter("@Mdifier", SqlDbType.NVarChar,50)

            };

            parameters[0].Value = model.Id;
            parameters[1].Value = model.Account;
            parameters[2].Value = model.Password;
            parameters[3].Value = model.CDKey;
            parameters[4].Value = model.RegistrationNum;
            parameters[5].Value = model.CreateDate;
            parameters[6].Value = model.Creator;
            parameters[7].Value = model.UpdateDate;
            parameters[8].Value = model.Mdifier;
            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from User_Infos ");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)
            };
            parameters[0].Value = Id;


            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 批量删除一批数据
        /// </summary>
        public bool DeleteList(string Idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from User_Infos ");
            strSql.Append(" where ID in (" + Idlist + ")  ");
            int rows = SqlHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public User_InfosItem GetModel(int Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id, Account, Password, CDKey, RegistrationNum, CreateDate, Creator, UpdateDate, Mdifier  ");
            strSql.Append("  from User_Infos ");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)
            };
            parameters[0].Value = Id;


            User_InfosItem model = new User_InfosItem();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Id"].ToString() != "")
                {
                    model.Id = int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
                }
                model.Account = ds.Tables[0].Rows[0]["Account"].ToString();
                model.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                model.CDKey = ds.Tables[0].Rows[0]["CDKey"].ToString();
                model.RegistrationNum = ds.Tables[0].Rows[0]["RegistrationNum"].ToString();
                if (ds.Tables[0].Rows[0]["CreateDate"].ToString() != "")
                {
                    model.CreateDate = DateTime.Parse(ds.Tables[0].Rows[0]["CreateDate"].ToString());
                }
                model.Creator = ds.Tables[0].Rows[0]["Creator"].ToString();
                if (ds.Tables[0].Rows[0]["UpdateDate"].ToString() != "")
                {
                    model.UpdateDate = DateTime.Parse(ds.Tables[0].Rows[0]["UpdateDate"].ToString());
                }
                model.Mdifier = ds.Tables[0].Rows[0]["Mdifier"].ToString();

                return model;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM User_Infos ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" * ");
            strSql.Append(" FROM User_Infos ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return SqlHelper.Query(strSql.ToString());
        }

    }
}
