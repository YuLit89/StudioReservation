using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string Image { get; set; }
        public string Size { get; set; }
        public string[] StyleArr { get; set; }
        public string Style { get; set; }
        [Range(0, 9999)]
        public decimal Rate { get; set; }
        public decimal RateOri { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }

    public class RoomViewDetail
    {
        public Room Room { get; set; }
        public int Error { get; set; }
    }
}
