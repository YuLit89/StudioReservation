using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioReservation.Service
{
    public enum ReservationStatus
    {
        None = 0,
        Opening = 1 ,
        Lock = 2 ,
        Booked = 3,
        Closed = 4,
        Cancel = 5
    }
}
