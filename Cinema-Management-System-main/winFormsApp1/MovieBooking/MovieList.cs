using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.Models;
using WinFormsApp1.Repository;
using WinFormsApp1.Service;
using WinFormsApp1.Data;

namespace WinFormsApp1.MovieBooking
{
    
    public partial class MovieList : Form
    {
        private const string API_KEY = "ecfcd92abcf0735492247a8eae3c742a";
        private const string IMAGE_BASE_URL = "https://image.tmdb.org/t/p/w500";
        private FlowLayoutPanel flowLayoutPanel;
        private MovieRepository movieRepository = new MovieRepository(new CinemaAppDbcontext());
        List<Movie> movies = new List<Movie>();
        private readonly int MoviePerPage = 16;

        public MovieList()
        {
            InitializeComponent();
            this.Load += async (sender, e) => await InitializeMovieList();
        }

        private async Task InitializeMovieList()
        {
            // Initialize form properties
            this.Text = "Movie Ticketing System";
            this.Size = new Size(800, 700);

            // Initialize FlowLayoutPanel
            flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.Dock = DockStyle.Fill;
            flowLayoutPanel.AutoScroll = true;
            flowLayoutPanel.WrapContents = true;
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            this.Controls.Add(flowLayoutPanel);

            this.movies = (List<Movie>)await movieRepository.GetMoviesPageAsync(1, MoviePerPage);

            
            DisplayMovies();

        }

        private async Task NextPageLoading(int page)
        {
            this.movies = (List<Movie>)await movieRepository.GetMoviesPageAsync(page, MoviePerPage);
            DisplayMovies();
        }

        private void DisplayMovies()
        {
            int posterWidth = 150;
            int posterHeight = 225;

            foreach (var movie in movies)
            {
                // Create a Panel container that includes the poster and title
                Panel moviePanel = new Panel();
                moviePanel.Width = posterWidth;
                moviePanel.Height = posterHeight + 50;
                moviePanel.Margin = new Padding(10);

                PictureBox pb = new PictureBox();
                pb.LoadAsync(IMAGE_BASE_URL + movie.PosterPath);
                pb.Size = new Size(posterWidth, posterHeight);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Cursor = Cursors.Hand;
                pb.Click += (s, e) =>
                {
                    MovieDetailsForm detailsForm = new MovieDetailsForm(movie);
                    detailsForm.ShowDialog();
                };

                Label lbl = new Label();
                lbl.Text = movie.Title;
                lbl.Size = new Size(posterWidth, 30);
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.Location = new Point(0, posterHeight + 5);

                moviePanel.Controls.Add(pb);
                moviePanel.Controls.Add(lbl);

                flowLayoutPanel.Controls.Add(moviePanel);
            }

            // Add "My Tickets" button
            Button myTicketButton = new Button();
            myTicketButton.Text = "Ticket";
            myTicketButton.Size = new Size(100, 30);
            myTicketButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            myTicketButton.Click += (s, e) =>
            {
                TicketQueryForm queryForm = new TicketQueryForm();
                queryForm.ShowDialog();
            };

            // Add the button to the bottom of the form
            this.Controls.Add(myTicketButton);
            myTicketButton.Location = new Point(10, this.ClientSize.Height - myTicketButton.Height - 10);
            myTicketButton.BringToFront();
        }

        
    }
}
