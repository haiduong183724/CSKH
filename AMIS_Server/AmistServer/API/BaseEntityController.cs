using CustomerCors.ErrorMsg;
using CustomerCors.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmistServer.API
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class BaseEntityController<BaseEntity> : ControllerBase
    {
        #region Properties
        IBaseService<BaseEntity> _baseService;
        IBaseRepo<BaseEntity> _baseRepo;
        #endregion

        #region Instructor
        public BaseEntityController(IBaseService<BaseEntity> baseService,
        IBaseRepo<BaseEntity> baseRepo)
        {
            _baseService = baseService;
            _baseRepo = baseRepo;
        }
        #endregion
        #region API 
        /// <summary>
        /// Lấy tất cả các bản ghi trong CSDL
        /// </summary>
        /// <returns> danh sách các bản ghi trong csdl</returns>
        /// CreatedBy NHDUONG(21/6/2021)
        [HttpGet]
        public IActionResult Get()
        {
            try 
            {
                var customers = _baseRepo.GetAll();
                return Ok(customers);
            }
            catch(Exception e)
            {
                var err = new ServiceResult();
                err.isValid = false;
                err.ErrorMsg.Add(e.Message);
                return BadRequest(err);
            }
        }
        /// <summary>
        /// Lấy một bản ghi dựa theo id
        /// </summary>
        /// <param name="entityId">Id của bản ghi</param>
        /// <returns>Bản ghi tương ứng, Exception nếu không có</returns>
        /// CreatedBy NHDUONG(21/6/2021)
        [HttpGet("{entityId}")]
        public IActionResult Get(Guid entityId)
        {
            try
            {
                var customer = _baseRepo.GetbyId(entityId);
                if(customer == null)
                {
                    throw new Exception(CustomerCors.Properties.Resources.Msg_Nocontent);
                }
                return Ok(customer);
            }
            catch (Exception e)
            {
                var err = new ServiceResult();
                err.isValid = false;
                err.ErrorMsg.Add(e.Message);
                return BadRequest(err);
            }
        }
        /// <summary>
        /// Lấy số lượng của các bản ghi tương ứng với một từ khóa tìm kiếm
        /// </summary>
        /// <param name="keyword">từ khóa tìm kiếm</param>
        /// <returns> số lượng bản ghi</returns>
        /// CreatedBy NHDUONG(21/6/2021)
        [HttpPost("NumRecord")]
        public IActionResult Post([FromBody] string keyword)
        {
            try
            {
                var res = _baseRepo.getNumberRecord(keyword);
                return Ok(res);
            }
            catch (Exception e)
            {
                var err = new ServiceResult();
                err.isValid = false;
                err.ErrorMsg.Add(e.Message);
                return BadRequest(err);
            }
        }
        /// <summary>
        /// Lấy các bản ghi theo trang và từ khóa tìm kiếm
        /// </summary>
        /// <param name="index">số trang</param>
        /// <param name="number">số bản ghi một trang</param>
        /// <param name="keyWord">từ khóa tìm kiếm</param>
        /// <returns>các bản ghi tương ứng</returns>
        /// CreatedBy NHDUONG(21/6/2021)
        [HttpPost("Find")]
        public IActionResult Post(int index, int number, [FromBody] string keyWord)
        {
            try
            {
                var res = _baseRepo.GetByCondition(index, number, keyWord);
                return Ok(res);
            }
            catch (Exception e)
            {
                var err = new ServiceResult();
                err.isValid = false;
                err.ErrorMsg.Add(e.Message);
                return BadRequest(err);
            }
        }
        /// <summary>
        /// Thêm mới một đối tượng
        /// </summary>
        /// <param name="entity">thông tin của đối tượng</param>
        /// <returns>thành công : OK, thất bại: BadRequest </returns>
        /// CreatedBy NHDUONG(21/6/2021)
        [HttpPost]
        public IActionResult Post(BaseEntity entity)
        {
            try
            {
                var res = _baseService.Insert(entity);
                return Ok(res);
            }
            catch (Exception e)
            {
                var err = new ServiceResult();
                err.isValid = false;
                err.ErrorMsg.Add(e.Message);
                return BadRequest(err);
            }
        }
        /// <summary>
        /// Chỉnh sửa thông tin của một đối tượng
        /// </summary>
        /// <param name="entity">thông tin của đối tượng</param>
        /// <returns>thành công: Ok, thất bại :BadRequest </returns>
        /// CreatedBy NHDUONG(21/6/2021)
        [HttpPut]
        public IActionResult Put(BaseEntity entity)
        {
            try
            {
                var res = _baseService.Update(entity);
                return Ok(res);
            }
            catch (Exception e)
            {
                var err = new ServiceResult();
                err.isValid = false;
                err.ErrorMsg.Add(e.Message);
                return BadRequest(err);
            }
        }
        /// <summary>
        /// Xóa một đối tượng dựa trên id
        /// </summary>
        /// <param name="entityId">Id của dối tượng cần xóa</param>
        /// <returns>Thành công: Ok, Thất bại: BadRequest</returns>
        /// CreatedBy NHDUONG(21/6/2021)
        [HttpDelete("{entityId}")]
        public IActionResult Delete(Guid entityId)
        {
            try
            {
                var res = _baseRepo.Delete(entityId);
                return Ok(res);
            }
            catch (Exception e)
            {
                var err = new ServiceResult();
                err.isValid = false;
                err.ErrorMsg.Add(e.Message);
                return BadRequest(err);
            }
        }
        #endregion

    }
}
