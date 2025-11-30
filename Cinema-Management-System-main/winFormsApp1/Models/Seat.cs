using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;//non-nullable reference type
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsAvailable { get; set; }

        public ICollection<Ticket> Ticket { get; set; } = [];
    }
}
