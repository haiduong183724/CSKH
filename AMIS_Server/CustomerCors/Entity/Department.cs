using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerCors.Entity
{
    public class Department
    {
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
