using CustomerCors.Entity;
using CustomerCors.ErrorMsg;
using CustomerCors.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using MySqlConnector;
using System.Data;
using System.Linq;
namespace InterStructor.Service
{
    public class DepartmentRepo : BaseRepo<Department>, IDepartmentRepo
    {
        public ServiceResult getByName(string Name)
        {
            var sqlCommand = $"call Proc_Get{tableName}ByName(@DepartmentName)";
            DynamicParameters param = new DynamicParameters();
            param.Add("@DepartmentName", Name);
            var result = new ServiceResult();
            try
            {
                var entity = this.Connect().Query<Department>(sqlCommand, param: param).ToList();
                if (entity == null)
                {
                    result.ErrorMsg.Add(CustomerCors.Properties.Resources.Msg_Nocontent);
                }
                else
                {
                    result.ErrorMsg.Add(CustomerCors.Properties.Resources.Msg_Sucessfull);
                    result.Data.Add(entity);
                }

            }
            catch
            {
                throw new Exception(CustomerCors.Properties.Resources.Msg_BackEnd_Err);
            }
            return result;

        }
    }
}
