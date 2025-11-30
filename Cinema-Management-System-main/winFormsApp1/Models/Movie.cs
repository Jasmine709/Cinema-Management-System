using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WinFormsApp1.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;
        [JsonPropertyName("original_title")]
        public string? OriginalTitle { get; set; }
        public ICollection<Genre> Genres { get; set; } = [];
        [JsonPropertyName("original_language")]
        public string OriginalLanguage { get; set; } = null!;
        [JsonPropertyName("release_date")]
        public DateTime? ReleaseDate { get; set; }
        [JsonPropertyName("overview")]
        public string? Overview { get; set; }
        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }
        [JsonPropertyName("backdrop_path")]
        public string? BackdropPath { get; set; }

        public ICollection<Showing> Showings { get; set; } = [];
    }
}
