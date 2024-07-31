using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioRoomType.DataModel
{
    public class RoomsViewModel
    {
       public List<Room> Rooms { get; set; }
       public int Error { get; set; }
    }

    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Size { get; set; }
        public string[] StyleArr { get; set; }
        public string Style { get; set; }
        public decimal Rate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
