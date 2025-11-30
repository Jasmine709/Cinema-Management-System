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
    public class RoomRepository : IRepository<Room>
    {
        private readonly CinemaAppDbcontext _context;

        public RoomRepository(CinemaAppDbcontext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Room entity)
        {
            await _context.Rooms.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Room> CreateRoomWithSeats(string roomName, int capacity, int maximumRow, int maximumCol)
        {
            var room = new Room
            {
                Name = roomName,
                Capacity = capacity,
                MaximunRow = maximumRow,
                MaximunCol = maximumCol,
                Seats = new List<Seat>()
            };

            
            for (int row = 1; row <= maximumRow; row++)
            {
                for (int col = 1; col <= maximumCol; col++)
                {
                    room.Seats.Add(new Seat
                    {
                        Row = row,
                        Column = col,
                        IsAvailable = true
                    });
                }
            }

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync(); // Save Room and other Seat.
            return room;
        }
        /// <summary>
        /// Return seats by room id
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns>list </returns>
        public IList<SeatDto> GetSeatsByRoom(int roomId)
        {
            var seats = _context.Rooms
                                .Where(r => r.RoomId == roomId)
                                .SelectMany(r => r.Seats)
                                .Select(s => new SeatDto
                                {
                                    SeatId = s.SeatId,
                                    Row = s.Row,
                                    Column = s.Column,
                                    IsAvailable = s.IsAvailable
                                })
                                .ToList();

            return seats;
        }

        public Seat? GetSeat(int seatId)
        {
            return  _context.Seats.Find(seatId);
        }
        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms.ToListAsync();
        }
        public async Task<bool> UpdateAsync(Room entity)
        {
            _context.Rooms.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var room = await GetByIdAsync(id);
            if (room == null)
            {
                return false;
            }
            _context.Rooms.Remove(room);
            return await _context.SaveChangesAsync() > 0;
        }
    }

    public class SeatDto
    {
        public int SeatId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsAvailable { get; set; }
    }
}
