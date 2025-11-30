using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        [ForeignKey("Seat")]
        public int SeatId { get; set; }
        public Seat Seat { get; set; } = null!;
        [ForeignKey("Showing")]
        public int ShowingId { get; set; }
        public Showing Showing { get; set; } = null!;
        public DateTime PurchaseTime { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Price { get; set; }
    }
}
