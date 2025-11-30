using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Data;
using WinFormsApp1.Models;
using WinFormsApp1.Repository;

namespace WinFormsApp1.Service
{
    /// <summary>
    /// pricing ticket based on seat location and showing time
    /// </summary>
    public class TicketService
    {
        private readonly TicketRepository _ticketRepository;
        private readonly ShowingRepository _showingRepository = new ShowingRepository(new CinemaAppDbcontext());
        private readonly RoomRepository _roomRepository = new RoomRepository(new CinemaAppDbcontext());
        private readonly CinemaAppDbcontext _context = new CinemaAppDbcontext();
        

        public TicketService(TicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;

        }
        /// <summary>
        /// produce a ticket new showing id and seat id
        /// puchaseTime is now
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TicketException"></exception>
        public async Task<Ticket> CreatTicket(int showId, int SeatId)
        {
            
            //calculate price based on seat location and showing time
            Ticket ticket = new Ticket
            {
                ShowingId = showId,
                SeatId = SeatId,
                PurchaseTime = DateTime.Now,
                
            };
            ticket.Price = CalculateTicketPrice(ticket);
            if (!await _ticketRepository.AddAsync(ticket))
            {
                throw new TicketException("Failed to add ticket");
            }
            
            return ticket;
            
        }
       

        /// <summary>
        /// start time in showing object and pricing by seat logic in seatSerice class
        /// Showing -> ticket <- seat (one to Many relationship)
        /// </summary>
        public decimal CalculateTicketPrice(Ticket ticket)
        {
            decimal basePrice = 0;
            Showing showing = _context.Showings.Find(ticket.ShowingId);
            Seat seat = _context.Seats.Find(ticket.SeatId);
            Room room = _context.Rooms.Find(showing.RoomId);
            SeatService seatServcie = new SeatService();
            basePrice = seatServcie.CalculatePriceBySeatLocation(seat.Row, seat.Column, room.MaximunRow, room.MaximunCol);

            if (showing.StartTime.Hour < 10)
                return basePrice * 0.9m; 
            else if (showing.StartTime.Hour >= 22)
                return basePrice * 0.8m;
            return Math.Round(basePrice, 2); 
           
        }

        public async Task DeleteByShowingAndSeatAsync(int showingId, int seatId)
        {
            if(!await _ticketRepository.DeleteAsync(showingId, seatId))
            {
                throw new TicketException("Failed to delete ticket");
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _ticketRepository.DeleteAsync(id))
            {
                throw new TicketException("Failed to delete ticket");
            }
        }
        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await _ticketRepository.GetByIdAsync(id);
        }

        public Movie? GetMovieFromTicket(int ticketId)
        {
            return _ticketRepository.GetMovieFromTicket(ticketId);
        }

        public Seat? GetSeatFromTicket(int seatId)
        {
            return _ticketRepository.GetSeatFromTicket(seatId);
        }

        public Showing? GetShowingFromTicket(int ticketId)
        {
            return _ticketRepository.GetShowingFromTicket(ticketId);
        }
    }

    [Serializable]
    internal class TicketException : Exception
    {
        public TicketException()
        {
        }

        public TicketException(string? message) : base(message)
        {
        }

        public TicketException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TicketException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
