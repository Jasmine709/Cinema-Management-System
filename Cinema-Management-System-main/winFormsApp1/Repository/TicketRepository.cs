using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Data;
using WinFormsApp1.Models;
using WinFormsApp1.Service;

namespace WinFormsApp1.Repository
{
    public class TicketRepository : IRepository<Ticket>
    {
        private CinemaAppDbcontext _context;

        public TicketRepository(CinemaAppDbcontext context)
        {
            _context = context;
        }


        public async Task<bool> AddAsync(Ticket entity)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    UpdateSeatAvailability(entity.SeatId);
                    await _context.Tickets.AddAsync(entity);
                    await _context.SaveChangesAsync();
                    transaction.Commit(); // This commits the transaction
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    
                    return false;
                }
            }

        }
        /// <summary>
        /// if use this method, the seat component need to tie with ticket id
        /// </summary>
        /// <param name="id">Ticket ID</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            {
                try
                {
                    var ticket = await GetByIdAsync(id);
                    if (ticket == null)
                    {
                        throw new Exception("Ticket ID does not exist");
                    }
                    _context.Tickets.Remove(ticket);
                   
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
          
        }

        public async Task<bool> DeleteAsync(int showingId, int seatId)
        {
            var ticket = await _context.Tickets
                                       .FirstOrDefaultAsync(t => t.ShowingId == showingId && t.SeatId == seatId);
            using var transaction = await _context.Database.BeginTransactionAsync();
            {
                try
                {
                    if (ticket == null)
                    {
                        return false;
                    }
                    _context.Tickets.Remove(ticket);
                    bool result =  await _context.SaveChangesAsync() > 0;
                    if (result)
                    {
                        Seat seat = _context.Seats.Find(seatId);
                        seat.IsAvailable = true;
                        _context.Seats.Update(seat);
                    }
                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
               
            }
           
        }


        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task<IEnumerable<Ticket>> GetByShowingIdAsync(int showingId)
        {
            return await _context.Tickets
                .Where(t => t.ShowingId == showingId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Ticket entity)
        {
            _context.Tickets.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        private void UpdateSeatAvailability(int SeatId)
        {
            
            Seat seat = _context.Seats.Find(SeatId);
            seat.IsAvailable = false;
             _context.Seats.Update(seat);
            
        }
        

        public Movie? GetMovieFromTicket(int ticketId)
        {
            Ticket ticket = _context.Tickets.Find(ticketId);
            Showing showing = _context.Showings.Find(ticket.ShowingId);
            return _context.Movies.Find(showing.MovieId);
        }

        public Seat? GetSeatFromTicket(int seatId)
        {
            return _context.Seats.Find(seatId);
        }

        public Showing? GetShowingFromTicket(int ticketId)
        {
            Ticket ticket = _context.Tickets.Find(ticketId);
            return _context.Showings.Find(ticket.ShowingId);
        }
    }
}
