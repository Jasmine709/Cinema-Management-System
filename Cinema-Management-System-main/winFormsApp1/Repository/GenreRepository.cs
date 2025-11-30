using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Data;
using WinFormsApp1.Models;

namespace WinFormsApp1.Repository
{
    public class GenreException : Exception
    {
        public GenreException() { }

        public GenreException(string message) : base(message) { }

        public GenreException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class GenreRepository : IRepository<Genre>
    {
        private readonly CinemaAppDbcontext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(GenreRepository));

        public GenreRepository(CinemaAppDbcontext context)
        {
            _context = context;
        }
        

        public async Task<bool> AddAsync(Genre entity)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Genres ON;");
                    if (!_context.Genres.Any(g => g.GenreId == entity.GenreId))
                    {

                        _context.Genres.Add(entity);
                    }
                    //else
                    //{
                        //throw new GenreException("Genre already exists");
                    //}
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return true;
                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();
                    log.Error(ex.Message);
                    return false;
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var genre = await _context.Genres.FindAsync(id);
                if (genre == null)
                {
                    throw new GenreException("Genre not found");
                }
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre?> GetByIdAsync(int id)
        {
            try
            {
                var genre = await _context.Genres.FindAsync(id);
                return genre ?? throw new GenreException("Genre not found");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
           
        }

        public async Task<bool> UpdateAsync(Genre entity)
        {
            try
            {
                _context.Genres.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
    }
}
