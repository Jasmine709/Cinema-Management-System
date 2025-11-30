using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WinFormsApp1.Models
{
    public class Genre
    {
        [Key]
        [JsonPropertyName("id")]
        public int GenreId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
        
        public ICollection<Movie> Movies { get; set; } = [];
    }
}
