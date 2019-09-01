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
        ILineRepository lineRepository { get; set; }
        IStationRepository stationRepository { get; set; }
        ITimeTableRepository timeTableRepository { get; set; }
        IVehicleRepository vehicleRepository { get; set; }

        int Complete();
    }
}
