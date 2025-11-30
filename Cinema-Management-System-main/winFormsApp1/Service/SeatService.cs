using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Data;
using WinFormsApp1.Models;
using WinFormsApp1.Repository;

namespace WinFormsApp1.Service
{
    /// <summary>
    /// pricing logic for seats
    /// </summary>
    public class SeatService
    {
        public decimal basePrice = 10.00m;
        private readonly RoomRepository roomRepository = new RoomRepository(new CinemaAppDbcontext());

       
        public void SetBasePrice(decimal price)
        {
            basePrice = price;
        }
        /// <summary>
        /// 1/3 is the boundary of the middle area
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="totalRows"></param>
        /// <param name="totalCol"></param>
        /// <returns></returns>
        public decimal CalculatePriceBySeatLocation(int row, int col, int totalRows, int totalCol)
        {
            
            int startMiddleRow = totalRows / 3;
            int endMiddleRow = totalRows * 2 / 3;
            int startMiddleCol = totalCol / 3;
            int endMiddleCol = totalCol * 2 / 3;

            
            decimal priceFactor = 1.0m;  

            
            if (row >= startMiddleRow && row <= endMiddleRow && col >= startMiddleCol && col <= endMiddleCol)
            {
                
                if (row >= startMiddleRow + totalRows / 9 && row <= endMiddleRow - totalRows / 9 &&
                    col >= startMiddleCol + totalCol / 9 && col <= endMiddleCol - totalCol / 9)
                {
                    priceFactor = 1.5m;  
                }
                else
                {
                    priceFactor = 1.2m;  
                }
            }

           
            decimal finalPrice = basePrice * priceFactor;
            
            return Math.Round(finalPrice, 2);
        }
        /// <summary>
        /// loading all seats in a room
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ICollection<SeatDto> GetSeatByRoom(int roomId)
        {
           
           List<SeatDto> seats = (List<SeatDto>)roomRepository.GetSeatsByRoom(roomId);
            return seats.Count == 0? throw new Exception("Seat not found") : seats;

        }

        public bool IsSeatAvailable(int seatId)
        {
            Seat seat = roomRepository.GetSeat(seatId);
            return seat == null ? throw new Exception("Seat not found") : seat.IsAvailable;
        }
    }
}
