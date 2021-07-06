using CustomerCors.Entity;
using CustomerCors.ErrorMsg;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerCors.Interface
{
    public interface IDepartmentRepo:IBaseRepo<Department>
    {
        public ServiceResult getByName(string Name);
    }
}
