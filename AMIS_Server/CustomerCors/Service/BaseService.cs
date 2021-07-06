using CustomerCors.Entity;
using CustomerCors.ErrorMsg;
using CustomerCors.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CustomerCors.Service
{
    public class BaseService<BaseEntity> : IBaseService<BaseEntity>
    {
        IBaseRepo<BaseEntity> _baseRepo;
        #region Instruction
        public BaseService(IBaseRepo<BaseEntity> baseRepo)
        {
            _baseRepo = baseRepo;
        }

        #endregion

        #region ExcuteInterface
        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Thông tin của đối tượng thêm mới</param>
        /// <returns>Thêm mới thành công: 1, Thất bại: Exception</returns>
        /// CreatedBy NHDUONG(21/6/2021)
        public ServiceResult Insert(BaseEntity entity)
        {
            ServiceResult result = new ServiceResult();
            ValidateData(entity, result);
            if (!result.isValid)
            {
                return result;
            }
            else
            {
                //var properties = entity.GetType().GetProperties();
                //var entityCode = "";
                //foreach (var prop in properties)
                //{
                //    var propertiesCode = prop.GetCustomAttributes(typeof(BaseEntityCode), true);
                //    if (propertiesCode.Length > 0)
                //    {
                //        entityCode = (string)prop.GetValue(entity);
                //        var modifiedCode = entityCode.Substring(3).PadLeft(6, '0');
                //        prop.SetValue(entity, "NV-"+ modifiedCode);
                //    }
                //    if(prop.Name == "CreatedDate")
                //    {
                //        prop.SetValue(entity, DateTime.Now);
                //    }
                //}
                //entityCode = entityCode.Substring(3);
                //entityCode = entityCode.PadLeft(6, '0');
                //var res = _baseRepo.CheckCodeExits("NV-"+entityCode);
                //if(res == true)
                //{
                //    throw new Exception(Properties.Resources.Msg_Duplicate_Employee_Err);
                //}
                //CustomeValidate(entity);
                return _baseRepo.Insert(entity);
            }
        }
        /// <summary>
        /// Chỉnh sửa một bản ghi
        /// </summary>
        /// <param name="entity">Thông tim của bản ghi đó</param>
        /// <returns></returns>
        /// CreatedBy NHDUONG(21/6/2021)
        public ServiceResult Update(BaseEntity entity)
        {
            ServiceResult result = new ServiceResult();
            ValidateData(entity, result);
            if (!result.isValid)
            {
                return result;
            }
            else
            {
                // Kiểm tra mã của đối tượng
                // Lấy đối tượng trong database;
                var properties = entity.GetType().GetProperties();
                var entityCode = "";
                var entityDBcode = "";
                var entityId = new Guid();
                foreach (var prop in properties)
                {
                    var propertiesCode = prop.GetCustomAttributes(typeof(BaseEntityId), true);
                    if (propertiesCode.Length > 0)
                    {
                        entityId = (Guid)prop.GetValue(entity);
                    }
                }
                // Nếu ID chưa có trong DB, chuyển thành thêm mới
                var entityDB = _baseRepo.GetbyId(entityId);
                if (entityDB.Data.Count == 0)
                {
                    return Insert(entity);
                }
                var entityInDB = entityDB.Data[0];
                foreach (var prop in properties)
                {
                    var propertiesCode = prop.GetCustomAttributes(typeof(BaseEntityCode), true);
                    if (propertiesCode.Length > 0)
                    {
                        entityCode = (string)prop.GetValue(entity);
                        var modifiedCode = entityCode.Substring(3).PadLeft(6, '0');
                        prop.SetValue(entity, "NV-" +modifiedCode);
                        entityDBcode = (string)prop.GetValue(entityInDB);
                    }
                    if (prop.Name == "ModifiedDate")
                    {
                        prop.SetValue(entity, DateTime.Now);
                    }
                }
                var res = false;
                // Nếu mã nhân viên bị thay đổi, kiểm tra có bị trùng hay không
                if(entityDBcode != entityCode)
                {
                    var modifiedCode = entityCode.Substring(3).PadLeft(6, '0');
                    res = _baseRepo.CheckCodeExits("NV-"+modifiedCode);
                }
                if (res == true)
                {
                    throw new Exception(Properties.Resources.Msg_Duplicate_Employee_Err);
                }
                //CustomeValidate(entity);
                if (result.isValid == false)
                {
                    return result;
                }
                CustomeValidate(entity);
                return _baseRepo.Update(entity);
            }
            
        }
        #endregion
        /// <summary>
        /// hàm validate dữ liệu.
        /// </summary>
        private void ValidateData(BaseEntity entity, ServiceResult result) {
            var properties = entity.GetType().GetProperties();
            // Duyệt các thuộc tính của Entity
            foreach(var prop in properties)
            {
                var propValue = prop.GetValue(entity);
                var propName = prop.Name;
                var propertiesRequired = prop.GetCustomAttributes(typeof(BaseRequired), true);
                var propertiesLength = prop.GetCustomAttributes(typeof(BaseMaxLength), true);
                var propertiesNumber = prop.GetCustomAttributes(typeof(MISANumber), true);
                // Kiểm tra độ dài
                if (propertiesLength.Length > 0)
                {
                    if (propValue != null)
                    {
                        if (prop.GetValue(entity).ToString().Length > (propertiesLength[0] as BaseMaxLength).maxlength)
                        {
                            throw new Exception(Properties.Resources.Msg_Over_Length);
                        }
                    }
                }
                // Kiểm tra chuỗi số
                if (propertiesNumber.Length > 0)
                {
                    Regex regex = new Regex(@"^[0-9]+$");
                    if(propValue != null)
                    {
                        if (!string.IsNullOrEmpty(propValue.ToString()) && !regex.IsMatch(propValue.ToString()))
                        {
                            throw new Exception(Properties.Resources.Msg_Invalid_NumberField);
                        }
                    }
                    
                }
                // Kiểm tra giá trị tiền có âm hay không
                if (propValue != null && propValue.GetType() == typeof(double))
                {
                    if((double)propValue < 0)
                    {
                        throw new Exception(Properties.Resources.Msg_Invalid_Money);
                    }
                }
                // Kiểm tra thời gian trong phạm vi 1/1/2001 => 1/1/2025
                if (propValue != null && propValue.GetType() == typeof(DateTime))
                {
                    // chuyển từ kiểu datetime sang int : Năm*512 + tháng *64 + ngày
                    var minDate = 1900 * 512 + 1 * 64 + 1;
                    var maxDate = 2100 * 512 + 1 * 64 + 1;
                    // Lấy giá trị datetime nhận được
                    DateTime date = (DateTime)propValue;
                    var DateValue = date.Year * 512 + date.Month * 64 + date.Day;
                    if(DateValue>maxDate || DateValue< minDate)
                    {
                        throw new Exception(Properties.Resources.Msg_Invalid_Date);
                    }
                }
                // Kiểm tra các trường bắt buộc nhập
                
                if (propertiesRequired.Length > 0)
                {
                    if(propValue == null || propName.ToString() == string.Empty)
                    {
                        throw new Exception(Properties.Resources.Msg_Required_Err);
                    }
                }
            }
        }
        /// <summary>
        /// Validate của đối tượng
        /// </summary>
        /// <param name="entity">thông tin của đối tượng</param>
        protected virtual void CustomeValidate(BaseEntity entity)
        {
        }
    }
}
