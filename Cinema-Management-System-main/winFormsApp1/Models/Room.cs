using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public string? Name { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public int MaximunRow { get; set; }
        [Required]
        public int MaximunCol { get; set; }
        public ICollection<Seat> Seats { get; set; } = [];
        public ICollection<Showing> Showings { get; set; } = [];
        
    }
}
