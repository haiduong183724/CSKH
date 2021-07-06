using AmistServer.API;
using CustomerCors.Entity;
using CustomerCors.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSKHServer.API
{
    public class UserController : BaseEntityController<User>
    {
        IBaseService<User> _userService;
        IUserRepo _userRepo;
        #region Instructor
        public UserController(IBaseService<User> apartmentService,
        IUserRepo apartmentRepo) : base(apartmentService, apartmentRepo)
        {
            _userService = apartmentService;
            _userRepo = apartmentRepo;
        }
        #endregion
    }
}
