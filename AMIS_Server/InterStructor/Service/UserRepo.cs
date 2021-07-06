using CustomerCors.Entity;
using CustomerCors.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterStructor.Service
{
    public class UserRepo:BaseRepo<User>, IUserRepo
    {
    }
}
