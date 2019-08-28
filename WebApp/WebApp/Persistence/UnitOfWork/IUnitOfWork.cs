using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPriceListRepository priceListRepository { get; set; }
        ITicketPriceRepository ticketPriceRepository { get; set; }
        ITicketRepository ticketRepository { get; set; }
        ITicketTypeRepository ticketTypeRepository { get; set; }
        IUserTypeRepository userTypeRepository { get; set; }
        int Complete();
    }
}
