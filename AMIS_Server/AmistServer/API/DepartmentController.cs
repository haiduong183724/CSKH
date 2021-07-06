using CustomerCors.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerCors.Entity;
using CustomerCors.ErrorMsg;

namespace AmistServer.API
{

    public class DepartmentController : BaseEntityController<Department>
    {
        IBaseService<Department> _departmentService;
        IDepartmentRepo _departmentRepo;
        #region Instructor
        public DepartmentController(IBaseService<Department> apartmentService,
        IDepartmentRepo apartmentRepo) : base(apartmentService, apartmentRepo)
        {
            _departmentService = apartmentService;
            _departmentRepo = apartmentRepo;
        }
        #endregion
        [HttpGet("getName/{Name}")]
        public IActionResult GetByName(string Name)
        {
            try
            {
                var customers = _departmentRepo.getByName(Name);
                return Ok(customers);
            }
            catch (Exception e)
            {
                var err = new ServiceResult();
                err.isValid = false;
                err.ErrorMsg.Add(e.Message);
                return BadRequest(err);
            }
        }
    }
}
