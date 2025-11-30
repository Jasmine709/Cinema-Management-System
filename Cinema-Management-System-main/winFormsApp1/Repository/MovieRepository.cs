using log4net;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.Devices;
using WinFormsApp1.Data;
using WinFormsApp1.Models;
//need to get showing for specific start time

namespace WinFormsApp1.Repository
{
    public class MovieException : Exception
    {
        public MovieException() { }

        public MovieException(string message) : base(message) { }

        public MovieException(string message, Exception innerException) : base(message, innerException) { }
    }


    public class MovieRepository(CinemaAppDbcontext context) : IMovieRepository, IRepository<Movie>
    {
        private readonly CinemaAppDbcontext _context = context;
        private static readonly ILog log = LogManager.GetLogger(typeof(MovieRepository));

        public async Task<bool> AddAsync(Movie entity)
        {
            foreach (var genre in entity.Genres)
            {
                var state = _context.Entry(genre).State;
                if (state == EntityState.Detached)
                {
                    _context.Genres.Attach(genre);
                }   
            }
            try
            {
                if (_context.Movies.Any(m => m.Title == entity.Title))
                {
                    throw new MovieException("Movie already exists");
                }
                _context.Movies.Add(entity);
                await _context.SaveChangesAsync();
                return true;

            } catch (DbUpdateConcurrencyException ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Delete a movie from the database
        /// keep consistency 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var movie = await _context.Movies.FindAsync(id);
                if (movie == null)
                {
                    throw new MovieException("Movie not found");
                }
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                log.Error(ex.Message);
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(id);
                return movie == null ? throw new MovieException("Movie not found") : movie;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                log.Error(ex.Message);
                return null;
            }
            
        }

        public async Task<ICollection<Movie>> GetMoviesPageAsync(int page, int pageSize)
        {
            return await _context.Movies
                        .Include(m => m.Genres)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
         
        }

        public async Task<bool> UpdateAsync(Movie entity)
        {
            try
            {
                _context.Movies.Update(entity);
                await _context.SaveChangesAsync();
                return true;

            } catch (DbUpdateConcurrencyException ex)
            {
                log.Error(ex.Message);
                return false;
            }
            
        }


     
    }
    
}

