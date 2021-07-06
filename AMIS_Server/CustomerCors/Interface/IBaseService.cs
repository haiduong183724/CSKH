using CustomerCors.ErrorMsg;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerCors.Interface
{
    public interface IBaseService<BaseEntity>
    {
        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity">Đối tượng thêm mới</param>
        /// <returns>Số bản ghi thêm mới thành công</returns>
        /// CreateBy: NHDUONG 29/5
        public ServiceResult Insert(BaseEntity entity);
        /// <summary>
        /// Chỉnh sửa
        /// </summary>
        /// <param name="entity"> Đối tượng chỉnh sửa, Id của đối tượng chỉnh sửa</param>
        /// <returns> số bản ghi được chỉnh sửa</returns>
        /// CreadteBy: NHDUONG 29/5
        public ServiceResult Update(BaseEntity entity);
    }
}
