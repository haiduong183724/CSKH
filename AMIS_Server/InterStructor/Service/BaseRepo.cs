using CustomerCors.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using MySqlConnector;
using System.Data;
using System.Linq;
using CustomerCors.ErrorMsg;
using CustomerCors.Entity;

namespace InterStructor.Service
{
    public class BaseRepo<BaseEntity> : IBaseRepo<BaseEntity>
    {
        #region Instructor
        protected string tableName;
        public BaseRepo()
        {
            tableName = typeof(BaseEntity).Name;
        }
        #endregion
        #region ConnectDataBase
        /// <summary>
        /// Hàm kết nối tới DataBase
        /// </summary>
        /// <returns>Một connector tới DataBase</returns>
        /// CreatedBy NHDUONG(21/6/2021)
        protected IDbConnection Connect()
        {
            var connectionString = "" +
                "host = localhost;" +
                "port = 3306;" +
                "database = cham-soc-khach-hang;" +
                "user id = root;" +
                "password = 0matkhau;";

            return new MySqlConnection(connectionString);
        }
        #endregion
        /// <summary>
        /// Lấy tất cả các bản ghi trong CSDL
        /// </summary>
        /// <returns>Danh sách các bản ghi</returns>
        public ServiceResult GetAll()
        {
            var result = new ServiceResult();
            var sqlCommand = $"call Proc_Get{tableName}s()";
            try
            {
                var entities = this.Connect().Query<BaseEntity>(sqlCommand).ToList();
                result.Data.Add(entities);
                if (entities.Count == 0)
                {
                    result.ErrorMsg.Add(CustomerCors.Properties.Resources.Msg_Nocontent);
                }

                else
                {
                    result.ErrorMsg.Add(CustomerCors.Properties.Resources.Msg_Sucessfull);
                }
            }
            catch
            {
                throw new Exception(CustomerCors.Properties.Resources.Msg_BackEnd_Err);
            }
            return result;
        }
        /// <summary>
        /// Lấy một bản ghi theo Id
        /// </summary>
        /// <param name="entityId">Id của bản ghi muốn lấy</param>
        /// <returns>bản ghi tương ứng, Exception nếu không có</returns>
        public ServiceResult GetbyId(Guid entityId)
        {
            var sqlCommand = $"call Proc_Get{tableName}ById(@entityId)";
            DynamicParameters param = new DynamicParameters();
            param.Add("@entityId", entityId);
            var result = new ServiceResult();
            try
            {
                var entity = this.Connect().QueryFirstOrDefault<BaseEntity>(sqlCommand, param: param);
                if(entity == null)
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
        /// <summary>
        /// THêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Thông tin của bản ghi muốn thêm</param>
        /// <returns>1 nếu thêm thành công, Exception nếu thất bại</returns>
        /// CreatedBy NHDUONG(21/6/2021)
        public ServiceResult Insert(BaseEntity entity)
        {
            var sqlCommandField = "";
            var sqlCommandValue = "";
            var properties = entity.GetType().GetProperties();
            DynamicParameters _param = new DynamicParameters();
            int i = 0;
            foreach (var pro in properties)
            {
                i++;
                // Không thêm các thuộc tính không có trong CSDL
                var propertiesRequired = pro.GetCustomAttributes(typeof(BaseNoDB), true);
                if (propertiesRequired.Length == 0) 
                {
                    var propName = pro.Name;
                    if (i == properties.Length)
                    {
                        sqlCommandField += $"{propName}";
                        sqlCommandValue += $"@{propName}";
                    }
                    else
                    {
                        sqlCommandField += $"{propName},";
                        sqlCommandValue += $"@{propName},";
                    }
                }
            }
            var sqlCommand = $"insert into {tableName} ({sqlCommandField}) values ({sqlCommandValue})";
            foreach (var pro in properties)
            {
                var propName = pro.Name;
                var propValue = pro.GetValue(entity);
                if (propName == $"{tableName}Id")
                {
                    propValue = Guid.NewGuid().ToString();
                }
                _param.Add($"@{propName}", propValue);
            }
            var result = new ServiceResult();
            try
            {
                var res = this.Connect().Execute(sqlCommand, param: _param);
                result.Data.Add(res);
                result.ErrorMsg.Add("Thêm dữ liệu thành công!");
            }
            catch 
            {
                throw new Exception(CustomerCors.Properties.Resources.Msg_BackEnd_Err);
            }
            return result;
        }
        /// <summary>
        /// Chỉnh sửa thông tin của một bản ghi
        /// </summary>
        /// <param name="entity"> Thông tin của một bản ghi</param>
        /// <returns>Thành công: 1, Thất bại: Exception</returns>
        public ServiceResult Update(BaseEntity entity)
        {
            var sqlCommandValue = "";
            var properties = entity.GetType().GetProperties();
            var condition = "";
            DynamicParameters param = new DynamicParameters();
            
            var i = 0;
            foreach (var prop in properties)
            {
                
                var propName = prop.Name;
                var propValue = prop.GetValue(entity);
                var propertiesRequired = prop.GetCustomAttributes(typeof(BaseNoDB), true);
                if(propertiesRequired.Length == 0) 
                {
                    if (i == 0)
                    {
                        condition = $"{propName} = @{propName}";
                    }
                    else if (i == properties.Length - 1)
                    {
                        sqlCommandValue += $"{propName} = @{propName}";
                    }
                    else
                    {

                        sqlCommandValue += $"{propName} = @{propName},";
                    }
                }
                i++;

            }
            // Khởi tạo câu lệnh truy vấn
            var sqlCommand = $"UPDATE {tableName} SET {sqlCommandValue} WHERE {condition}";
            foreach (var pro in properties)
            {
                var propName = pro.Name;
                var propValue = pro.GetValue(entity);
                param.Add($"@{propName}", propValue);
            }
            var result = new ServiceResult();
            // Request tới server
            try
            {
                var res = this.Connect().Execute(sqlCommand, param: param);
                result.Data.Add(res);
                result.ErrorMsg.Add(CustomerCors.Properties.Resources.Msg_Insert_Successfull);
            }
            catch 
            {
                throw new Exception(CustomerCors.Properties.Resources.Msg_BackEnd_Err);
            }
            return result;
        }
        /// <summary>
        /// Xóa thông tin 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// CreatedBy NHDUONG(21/6/2021)
        public ServiceResult Delete(Guid entityId)
        {
            var sqlCommand = $"call Proc_Delete{tableName}ById(@entityId)";
            DynamicParameters param = new DynamicParameters();
            param.Add("@entityId", entityId);
            var result = new ServiceResult();
            try
            {
                var rowEffect = this.Connect().Execute(sqlCommand, param: param);
                result.Data.Add(rowEffect);
                if(rowEffect == 0)
                {
                    throw new Exception(CustomerCors.Properties.Resources.Msg_Nocontent);
                }
                else
                {
                    result.ErrorMsg.Add(CustomerCors.Properties.Resources.Msg_Delete_Successfull);
                }
                
            }
            catch 
            {
                throw new Exception(CustomerCors.Properties.Resources.Msg_BackEnd_Err);
            }
            return result;
        }
        /// <summary>
        /// Lấy số lượng bản ghi dựa theo mã
        /// </summary>
        /// <param name="keywork">từ khóa tìm kiếm</param>
        /// <returns>số lượng bản ghi tương ứng</returns>
        public int getNumberRecord(string keywork)
        {
            var sqlcommand = $"call Proc_Get{tableName}NumberRecord(@condition)";
            DynamicParameters param = new DynamicParameters();
            param.Add("@condition", keywork);
            try
            {
                var res = this.Connect().QueryFirstOrDefault<int>(sqlcommand,param:param);
                return res;
            }
            catch
            {
                throw new Exception(CustomerCors.Properties.Resources.Msg_BackEnd_Err);
            }
        }
        /// <summary>
        /// Lấy các bản ghi phân trang theo điều kiên
        /// </summary>
        /// <param name="index">số trang của bản ghi</param>
        /// <param name="number"> số bản ghi một trang</param>
        /// <param name="keyword">từ khóa tìm kiếm</param>
        /// <returns>Các bản ghi tương ứng</returns>
        /// CreatedBy NHDUONG(21/6/2021)
        public ServiceResult GetByCondition(int index, int number, string keyword)
        {
            
            var sqlCommand = $"call Proc_Get{tableName}sByKW(@st, @numrecord, @kw)";
            DynamicParameters param = new DynamicParameters();
            var rs = new ServiceResult();
            param.Add("@st", (index-1)*number);
            param.Add("@numrecord", number);
            param.Add("@kw", keyword);
            try
            {
                var res = this.Connect().Query<BaseEntity>(sqlCommand, param: param).ToList();
                rs.Data.Add(res);
                return rs;
            }
            catch
            {
                throw new Exception(CustomerCors.Properties.Resources.Msg_BackEnd_Err);
            }
        }
        /// <summary>
        /// Kiểm tra mã đã tồn tại hay chưa
        /// </summary>
        /// <param name="code">Mã cần kiểm tra</param>
        /// <returns>True: đã tồn tại , False: chưa tồn tại</returns>
        /// CreatedBy NHDUONG(21/6/2021)
        public bool CheckCodeExits(string code)
        {
            string sqlcommand = $"call Proc_Check{tableName}CodeExist(@Code)";
            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", code);
            try 
            {
                var res = this.Connect().QueryFirstOrDefault<bool>(sqlcommand, param: param);
                return res;
            }
            catch
            {
                throw new Exception(CustomerCors.Properties.Resources.Msg_BackEnd_Err);
            }
        }
    }
}
