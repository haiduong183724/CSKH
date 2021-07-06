using CustomerCors.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmistServer.Object
{
    public class Ticket : User
    {
        public Guid TicketId { get; set; }
        public Guid DepartmentId { get; set; }
        [BaseNoDB]
        public string  DepartmentName { get; set; }
        public string PhoneNumber { get; set; }
        public string  Feedback { get; set; }
        public DateTime TimeAccept { get; set; }
        public string Answer { get; set; }
        public bool TicketStatus { get; set; }
        public DateTime HandleTime { get; set; }
    }   
}
