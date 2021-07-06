using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerCors.Entity
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string  Password { get; set; }
        public string Email { get; set; }
    }
}
