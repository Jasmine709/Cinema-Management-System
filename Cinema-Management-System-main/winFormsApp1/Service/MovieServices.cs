using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WinFormsApp1.Models;
using WinFormsApp1.Repository;
using log4net;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WinFormsApp1.Data;

namespace WinFormsApp1.Service
{
    /// <summary>
    /// function  include fetch data from API and save to database
    /// </summary>
    public class MovieServices
    {
       
        private readonly MovieRepository _movieRepository;
        private readonly GenreRepository _genreRepository;
        private static readonly ILog log = LogManager.GetLogger(typeof(MovieServices));
        
       public MovieServices(MovieRepository movieRepository, GenreRepository genreRepository)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
        }

        public async Task<Movie> CreateMovieAsync(string title, string originalTitle, string originalLanguage, DateTime releaseDate, string overview, string posterPath, string backdropPath)
        {
            Movie movie = new Movie
            {
                Title = title,
                OriginalTitle = originalTitle,
                OriginalLanguage = originalLanguage,
                ReleaseDate = releaseDate,
                Overview = overview,
                PosterPath = posterPath,
                BackdropPath = backdropPath
            };
            await _movieRepository.AddAsync(movie);
            return movie;
        }

        public async Task ImportDataAsync()
        {
            string apiKey = "979fae458b07a480dcd0e113b6485947";
            string apiBaseUrl = "https://api.themoviedb.org/3/movie/now_playing";
            string apiUrl = $"{apiBaseUrl}?language=en-US&page=1&api_key={apiKey}";
            using var client = new HttpClient();
            try
            {
                var response = await client.GetStringAsync(apiUrl);
                var movieResult = JsonSerializer.Deserialize<ApiMovie>(response);
                if (movieResult != null && movieResult.Results != null && movieResult.Results.Count > 0)
                {
                    List<MovieDto> results = movieResult.Results.Take(20).ToList();
                    foreach (var movieJson in results)
                    {
                       
                        var movie = new Movie
                        {
                            Title = movieJson.Title,
                            OriginalTitle = movieJson.OriginalTitle,
                            OriginalLanguage = movieJson.OriginalLanguage,
                            ReleaseDate = DateTime.Parse(movieJson.ReleaseDate),
                            Overview = movieJson.Overview,
                            PosterPath = movieJson.PosterPath,
                            BackdropPath = movieJson.BackdropPath,
                            

                        };
                        foreach (var genreId in movieJson.GenreIds)
                        {
                            var genre = await _genreRepository.GetByIdAsync(genreId);
                            if (genre != null)
                            {
                                movie.Genres.Add(genre);
                            }
                           
                        }
                       
                        try
                        {
                            await _movieRepository.AddAsync(movie);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }
                    }

                 
                }
            }
            catch (HttpRequestException ex)
            {
                log.Error(ex.Message);
            }
        }
    }
    public class MovieDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("original_title")]
        public string OriginalTitle { get; set; }

        [JsonPropertyName("genre_ids")]
        public List<int> GenreIds { get; set; }

        [JsonPropertyName("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; }

        [JsonPropertyName("overview")]
        public string Overview { get; set; }

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; }

        [JsonPropertyName("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonPropertyName("popularity")]
        public double Popularity { get; set; }

        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; set; }

        [JsonPropertyName("vote_count")]
        public int VoteCount { get; set; }
    }

    public class ApiMovie
    {
        [JsonPropertyName("results")]
        public List<MovieDto> Results { get; set; }
    }
}
