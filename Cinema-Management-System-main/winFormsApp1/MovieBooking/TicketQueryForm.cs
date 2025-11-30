using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp1.Models;
using WinFormsApp1.Service;
using WinFormsApp1.Repository;
using WinFormsApp1.Data;

public class TicketQueryForm : Form
{
    private TicketService TicketService = new TicketService(new TicketRepository(new CinemaAppDbcontext()));
    
    public TicketQueryForm()
    {
        // Initialize form properties
        this.Text = "Ticket Query";
        this.Size = new Size(500, 450);

        DisplayQueryInterface();
    }

    private void DisplayQueryInterface()
    {
        Label ticketIdLabel = new Label();
        ticketIdLabel.Text = "Enter Ticket ID:";
        ticketIdLabel.Location = new Point(10, 10);
        this.Controls.Add(ticketIdLabel);

        TextBox ticketIdTextBox = new TextBox();
        ticketIdTextBox.Location = new Point(ticketIdLabel.Right + 10, 10);
        ticketIdTextBox.Width = 200;
        this.Controls.Add(ticketIdTextBox);

        Button queryButton = new Button();
        queryButton.Text = "Search";
        queryButton.Location = new Point(ticketIdTextBox.Right + 30, 10);
        queryButton.Size = new Size(80, 30);
        queryButton.Click += async (s, e) =>
        {
            string ticketId = ticketIdTextBox.Text;
            try {
                Ticket ticket = await TicketService.GetTicketByIdAsync(int.Parse(ticketId));
                if (ticket == null)
                {
                    MessageBox.Show("Ticket not found.");
                    return;
                }
                DisplayTicketInfo(ticket);
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        };
        this.Controls.Add(queryButton);
    }

    private void DisplayTicketInfo(Ticket ticket)
    {
        Movie movie = TicketService.GetMovieFromTicket(ticket.TicketId);
        Showing showing = TicketService.GetShowingFromTicket(ticket.TicketId);
        Seat seat = TicketService.GetSeatFromTicket(ticket.TicketId);

        
        this.Controls.Clear();

        Label movieLabel = new Label();
        movieLabel.Text = "Movie: " + movie.Title;
        movieLabel.Location = new Point(10, 20);
        movieLabel.AutoSize = true;
        this.Controls.Add(movieLabel);

        Label dateLabel = new Label();
        dateLabel.Text = "Date: " + ticket.PurchaseTime.ToString("MM/dd/yyyy");
        dateLabel.Location = new Point(10, 50);
        dateLabel.AutoSize = true;
        this.Controls.Add(dateLabel);

        Label showtimeLabel = new Label();
        showtimeLabel.Text = "Showtime: " + showing.StartTime.ToString("HH:mm") +" - "+ showing.EndTime.ToString("HH:mm");
        showtimeLabel.Location = new Point(10, 80);
        showtimeLabel.AutoSize = true;
        this.Controls.Add(showtimeLabel);

        Label seatLabel = new Label();
        seatLabel.Text = $"Seat: {(char)('A' + seat.Row - 1)}{seat.Column}";
        seatLabel.Location = new Point(10, 110);
        seatLabel.AutoSize = true;
        this.Controls.Add(seatLabel);

        Label priceLabel = new Label();
        priceLabel.Text = "Price: " + ticket.Price;
        priceLabel.Location = new Point(10, 140);
        priceLabel.AutoSize = true;
        this.Controls.Add(priceLabel);

        Button cancelButton = new Button();
        cancelButton.Text = "Cancel Ticket";
        cancelButton.Location = new Point(10, 170);
        cancelButton.Size = new Size(80, 30);
        cancelButton.Click += async (s, e) =>
        {
            try
            {
                await TicketService.DeleteAsync(ticket.TicketId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            MessageBox.Show("Ticket has been canceled.");
            this.Controls.Clear();
            DisplayQueryInterface();
        };

        this.Controls.Add(cancelButton);
        this.ClientSize = new Size(400, 300);

       
    }
}

