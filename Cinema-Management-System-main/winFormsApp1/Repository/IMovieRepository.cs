using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Models;

namespace WinFormsApp1.Repository
{
    /// <summary>
    /// Interface for the MovieRepository
    /// functionality: return list of movies to each page.
    /// </summary>
    public interface IMovieRepository
    {
        Task<ICollection<Movie>> GetMoviesPageAsync(int page, int pageSize);
    }
}
