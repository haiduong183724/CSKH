using CustomerCors.ErrorMsg;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerCors.Interface
{
    public interface IBaseRepo<BaseEntity>
    {
        /// <summary>
        /// Hàm lấy tất cả danh sách
        /// </summary>
        /// <returns>Một đối tượng ServiceResult thông báo kết quả danh sách các nhân viên </returns>
        public ServiceResult GetAll();
        /// <summary>
        /// Hàm lấy các thực thể dựa trên ID
        /// </summary>
        /// <param name="entityId">hàm truyền vào Id của đối tượng</param>
        /// <returns>Một đối tượng ServiceResult thông báo kết quả + nhân viên phù hợp </returns>
        /// CreateBy:NHDUONG 29/5
        public ServiceResult GetbyId(Guid entityId);
        /// <summary>
        /// Hàm xóa 1 bản ghi 
        /// </summary>
        /// <param name="Id">Truyền vào ID của bản ghi</param>
        /// <returns>Một đối tượng ServiceResult thông báo kết quả + số bản ghi bị xóa</returns>
        /// /// CreateBy:NHDUONG 29/5
        public ServiceResult Delete(Guid Id);
        /// <summary>
        /// Hàm chỉnh sửa một đối tượng
        /// </summary>
        /// <param name="entity">Một đối tượng cần chỉnh sửa</param>
        /// <param name="entityId">ID của đối tượng chỉnh sửa</param>
        /// <returns>Một đối tượng ServiceResult thông báo kết quả + số bản ghi chỉnh sửa </returns>
        /// /// CreateBy:NHDUONG 29/5
        public ServiceResult Update(BaseEntity entity);
        /// <summary>
        /// Hàm thêm một đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm vào</param>
        /// <returns>Một đối tượng ServiceResult thông báo kết quả + số bản ghi thêm mới</returns>
        /// /// CreateBy:NHDUONG 29/5
        public ServiceResult Insert(BaseEntity entity);
        /// <summary>
        /// lấy số bản ghi có trong CSDL của đối tương
        /// </summary>
        /// <returns></returns>
        public int getNumberRecord(string condition);
        /// <summary>
        /// Hàm lấy các bản ghi theo điều kiện
        /// </summary>
        /// <param name="index"> số trang muốn lấy</param>
        /// <param name="number"> số bản ghi muốn lấy</param>
        /// <param name="condition"> điều kiện lấy bản ghi</param>
        /// <returns></returns>
        public ServiceResult GetByCondition(int index, int number, string condition);
        /// <summary>
        /// Hàm kiểm tra mã nhân viên đã tồn tại hay chưa
        /// </summary>
        /// <param name="code">Mã nhân viên</param>
        /// <returns> True: đã tồn tại  False: Chưa</returns>
        public bool CheckCodeExits(string code);
    }
}
