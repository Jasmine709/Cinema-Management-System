using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Models;
using WinFormsApp1.Data;
using Microsoft.EntityFrameworkCore;

namespace WinFormsApp1.Repository
{
    /// <summary>
    /// CRUD and query for a range of Showings selected by a filter(like movie, date, room, etc.)
    /// retrieve showings by movie -> at the showing choose the room and time
    /// </summary>
    public class ShowingRepository : IRepository<Showing>
    {
        private readonly CinemaAppDbcontext _context;
        public enum ShowingFilter
        {
            Movie,
            Room
        }

        public ShowingRepository(CinemaAppDbcontext context)
        {
            _context = context;
        }
        public async Task<Showing?> GetByIdAsync(int showingId)
        {
            return await _context.Showings.FindAsync(showingId);
        }

        public ICollection<Showing> GetShowingsByFK(int id, ShowingFilter showingFilter)
        {
            switch (showingFilter)
            {
                case ShowingFilter.Room:
                    return _context.Showings
                                     .Where(s => s.RoomId == id)
                                     .ToList();
                case ShowingFilter.Movie:
                    return _context.Showings.Include(r => r.Room)
                                     .Where(s => s.MovieId == id)
                                     .ToList();
                default:
                    return [];
            }

        }




        public ICollection<Showing> GetShowingsByDateAndMovie(DateTime date, int MovieId)
        {
            return _context.Showings
                             .Where(s => s.StartTime.Date == date.Date && s.MovieId == MovieId)
                             .ToList();
        }

        public ICollection<Showing> GetShowingsByDate(DateTime date)
        {
            return _context.Showings
                             .Where(s => s.StartTime.Date == date.Date)
                             .ToList();
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime startTime, DateTime endTime)
        {

            bool isOverlapping = await _context.Showings
                .AnyAsync(s => s.RoomId == roomId &&
                               ((s.StartTime < endTime) && (s.EndTime > startTime)));
            return !isOverlapping;
        }

        public async Task<bool> AddAsync(Showing entity)
        {
            await _context.Showings.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var showing = await _context.Showings.FindAsync(id);
                if (showing == null)
                {
                    return false;
                }
                _context.Showings.Remove(showing);
                bool result = await _context.SaveChangesAsync() > 0;
                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<IEnumerable<Showing>> GetAllAsync()
        {
            return await _context.Showings.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Showing entity)
        {
            try
            {
                _context.Showings.Update(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<decimal> ProfitInDate(DateTime date)//price in Ticket table, by Showing id find the ticket for each show
                                                              //and sum the price
        {
            var profit = await _context.Showings
                         .Where(s => s.StartTime.Date == date.Date)
                         .SelectMany(s => s.Tickets)
                         .SumAsync(t => t.Price);

            return profit;
        }

        public Showing? GetById(int id)
        {
            return _context.Showings.Find(id);
        }

        public Room? GetRoomByShowingId(int showingId)
        {
            var room = _context.Showings
                             .Where(s => s.ShowingId == showingId)
                             .Select(s => s.Room)
                             .FirstOrDefault();
            return room;
        }
        public async Task ClearSeatsByShowingIdAsync(int showingId)
        {

            var roomId = await _context.Showings
                .Where(s => s.ShowingId == showingId)
                .Select(s => s.RoomId)
                .FirstOrDefaultAsync();

            var room = await _context.Rooms.FindAsync(roomId);
            if (room != null)
            {
                var seats = await _context.Seats
                    .Where(seat => seat.RoomId == room.RoomId)
                    .ToListAsync();

                foreach (var seat in seats)
                {
                    seat.IsAvailable = false; // 假设有 IsReserved 属性来标记座位状态
                }

                await _context.SaveChangesAsync();
            }
        }

        public ICollection<Ticket>GetTicketsByShowingId(int showingId)
        {
            return _context.Tickets
                .Where(t => t.ShowingId == showingId)
                .ToList();
        }
}

}

