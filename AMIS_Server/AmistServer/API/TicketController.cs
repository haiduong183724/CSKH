using AmistServer.API;
using AmistServer.Object;
using CustomerCors.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSKHServer.API
{
    public class TicketController : BaseEntityController<Ticket>
    {
        IBaseService<Ticket> _ticketService;
        ITicketRepo _ticketRepo;
        #region Instructor
        public TicketController(IBaseService<Ticket> apartmentService,
        ITicketRepo apartmentRepo) : base(apartmentService, apartmentRepo)
        {
            _ticketService = apartmentService;
            _ticketRepo = apartmentRepo;
        }
        #endregion
    }
}
