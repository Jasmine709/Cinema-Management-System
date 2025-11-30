using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Models;
using WinFormsApp1.Repository;

namespace WinFormsApp1.Service
{
    public class ShowingService
    {
        private readonly ShowingRepository _showingRepository;
        

        public ShowingService(ShowingRepository showingRepository)
        {
            _showingRepository = showingRepository;
           
        }
        /// <summary>
        /// Using price of tickets per showing to calculate profit
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
       public async Task<decimal> ProfitInDate(DateTime date)
        {
            return await _showingRepository.ProfitInDate(date);
        }
        /// <summary>
        /// Make sure room available before creating a showing
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="roomId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>

        public async Task<Showing> CreateShowingAsync(int movieId, int roomId, DateTime startTime, DateTime endTime)
        {
            if (!await _showingRepository.IsRoomAvailableAsync(roomId, startTime, endTime))
            {
                throw new ShowingException("Room is not available");
            }
            if (startTime >= endTime)
            {
                throw new ShowingException("Start time must be before end time");
            }
            Showing showing = new Showing
            {
                MovieId = movieId,
                RoomId = roomId,
                StartTime = startTime,
                EndTime = endTime
            };
            if (!await _showingRepository.AddAsync(showing))
            {
                throw new ShowingException("Failed to add showing");
            }
            return showing;
        }

        

        public async Task DeleteAsync(int id)
        {
            if (!await _showingRepository.DeleteAsync(id))
            {
                throw new ShowingException("Failed to delete showing");
            }
        }

        public ICollection<Showing> GetShowingByMovieAndDate(int movieId, DateTime date)
        {
            var showings = _showingRepository.GetShowingsByDateAndMovie(date, movieId);
            
            return showings;
        }

        public Room? GetRoomByShowingId(int showingId)
        {
            Room room = _showingRepository.GetRoomByShowingId(showingId);
            return room;
           
        }
      

        public ICollection<Ticket> GetTicketsByShowingId(int showingId)
        {
            return _showingRepository.GetTicketsByShowingId(showingId);
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime startTime, DateTime endTime)
        {
            return await _showingRepository.IsRoomAvailableAsync(roomId, startTime, endTime);
        }

        public ICollection<Showing> GetShowingsByMovieId(int movieId)
        {
            return _showingRepository.GetShowingsByFK(movieId, ShowingRepository.ShowingFilter.Movie);
        }





    }

    [Serializable]
    internal class ShowingException : Exception
    {
        public ShowingException()
        {
        }

        public ShowingException(string? message) : base(message)
        {
        }

        public ShowingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ShowingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
