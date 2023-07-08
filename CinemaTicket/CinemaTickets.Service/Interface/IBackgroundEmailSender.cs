using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTickets.Service.Interface
{
    public interface IBackgroundEmailSender
    {
        Task DoWork();
    }
}
