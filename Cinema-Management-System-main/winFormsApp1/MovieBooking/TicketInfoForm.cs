using System;
using System.Windows.Forms;
using System.Drawing;
using WinFormsApp1.MovieBooking;
using WinFormsApp1.Models;
using WinFormsApp1.Service;
using WinFormsApp1.Repository;
using WinFormsApp1.Data;

public class TicketInfoForm : Form
{
    private List<Ticket> tickets;
    private List<Movie> movies = new List<Movie>();
    private List<Showing> showing = new List<Showing>();

    private readonly TicketService ticketService = new TicketService(new TicketRepository(new CinemaAppDbcontext()));
    private readonly ShowingRepository showingRepository = new ShowingRepository(new CinemaAppDbcontext());

    public TicketInfoForm(List<Ticket> tickets)
    {
        this.tickets = tickets;

        // Initialize form properties
        this.Text = "Ticket Information";
        this.AutoScroll = true;

        Initialize();

    }

    private void Initialize()
    {
        int baseX = 10; 
        int maxY = 10; 
        int ticketWidth = 300; 

        this.ClientSize = new Size(tickets.Count * ticketWidth, 300); //
        foreach (var ticket in tickets)
        {
            movies.Add(ticketService.GetMovieFromTicket(ticket.TicketId));
            showing.Add(showingRepository.GetById(ticket.ShowingId));
        }
        for (int i = 0; i < tickets.Count; i++)
        {
            int currentY = DisplayTicketInfo(tickets[i], movies[i], showing[i], baseX);
            maxY = Math.Max(maxY, currentY); 
            baseX += ticketWidth; 
        }

        
        Button closeButton = new Button();
        closeButton.Text = "Close";
        closeButton.Location = new Point(this.ClientSize.Width / 2 - 130, maxY + 10); // middle of window
        closeButton.Size = new Size(260, 30);
        closeButton.Click += (s, e) =>
        {
            this.Close();
        };
        this.Controls.Add(closeButton);


    }



    //ticket have showing and sear, trace movie info from showing
    private int DisplayTicketInfo(Ticket ticket, Movie movie, Showing showing, int baseX)
    {
        int x = baseX; 
        int y = 10;

        Label ticketIdLabel = new Label();
        ticketIdLabel.Text = "Ticket ID: " + ticket.TicketId;
        ticketIdLabel.Location = new Point(x, y);
        ticketIdLabel.Size = new Size(260, 30);
        this.Controls.Add(ticketIdLabel);
        y += 40;

        Label movieLabel = new Label();
        movieLabel.Text = "Movie: " + movie.Title;
        movieLabel.Location = new Point(x, y);
        movieLabel.Size = new Size(260, 30);
        this.Controls.Add(movieLabel);
        y += 40;

        Label dateLabel = new Label();
        dateLabel.Text = "Date: " + ticket.PurchaseTime.ToString("MM/dd/yyyy");
        dateLabel.Location = new Point(x, y);
        dateLabel.Size = new Size(260, 30);
        this.Controls.Add(dateLabel);
        y += 40;

        Label showtimeLabel = new Label();
        showtimeLabel.Text = "Showtime: " + showing.StartTime.ToString("HH:mm");
        showtimeLabel.Location = new Point(x, y);
        showtimeLabel.Size = new Size(260, 30);
        this.Controls.Add(showtimeLabel);
        y += 40;
        ///GetRoomHere need maxrow and maxcolumn
        Seat seat = ticketService.GetSeatFromTicket(ticket.SeatId);
       
        Label seatLabel = new Label();
        seatLabel.Text = $"Seat: {(char)('A' + seat.Row - 1)}{seat.Column}";
        seatLabel.Location = new Point(x, y);
        seatLabel.Size = new Size(260, 30);
        this.Controls.Add(seatLabel);
        y += 40;

        Label priceLabel = new Label(); 
        priceLabel.Text = "Price: " + ticket.Price.ToString("C");
        priceLabel.Location = new Point(x, y);
        priceLabel.Size = new Size(260, 30);
        this.Controls.Add(priceLabel);
        y += 40;

        return y; 
    }

}
