using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WinFormsApp1.Data;
using WinFormsApp1.Models;
using WinFormsApp1.Repository;


namespace WinFormsApp1.Service
{
    /// <summary>
    /// Deserializes the JSON object from the API call
    /// </summary>
    public class GenreApiResult
    {
        [JsonPropertyName("genres")]
        public List<Genre> Genres { get; set; }
    }
    /// <summary>
    /// Business logic for the GenreRepository
    /// </summary>
    public class GenreService
    {
        private readonly GenreRepository _genreRepository;
        private readonly HttpClient _httpClient;

        private static readonly ILog log = LogManager.GetLogger(typeof(GenreService));

        public GenreService(GenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
            

        }
        public async Task ImportDataAsync()
        {
            string apiKey = "979fae458b07a480dcd0e113b6485947";
            string apiToken = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI5NzlmYWU0NThiMDdhNDgwZGNkMGUxMTNiNjQ4NTk0NyIsIm5iZiI6MTcyOTcwMTIxNy43NjgxODMsInN1YiI6IjY3MTdhZTM1NTVlY2Y3YTk0MGY3YTJhNyIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.KAAZ_S3BblKOUkSSTqBqpY6I3zBJcwM9rDSk6n0hcI8";
            string apiBaseUrl = "https://api.themoviedb.org/3/genre/movie/list";
            string apiUrl = $"{apiBaseUrl}?language=en-US&api_key={apiKey}";
            using var client = new HttpClient();
            try
            {

                var response = await client.GetStringAsync(apiUrl);

                var genreResult = JsonSerializer.Deserialize<GenreApiResult>(response);


                if (genreResult != null && genreResult.Genres.Count > 0)
                {
                    List<Genre> genres = genreResult.Genres;
                    foreach (var genre in genres)
                    {
                        try
                        {
                            await _genreRepository.AddAsync(genre);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
    }
}
