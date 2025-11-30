using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Drawing;
using WinFormsApp1.Models;

/*namespace WinFormsApp1.MovieBooking
{ 
public class MainForm : Form
{
    private const string API_KEY = "ecfcd92abcf0735492247a8eae3c742a"; 
    private const string IMAGE_BASE_URL = "https://image.tmdb.org/t/p/w500";
    private List<Movie> movies;
    private FlowLayoutPanel flowLayoutPanel;

    public MainForm()
    {
        // Initialize form properties
        this.Text = "Movie Ticketing System";
        this.Size = new Size(800, 600);

        // Initialize FlowLayoutPanel
        flowLayoutPanel = new FlowLayoutPanel();
        flowLayoutPanel.Dock = DockStyle.Fill;
        flowLayoutPanel.AutoScroll = true;
        flowLayoutPanel.WrapContents = true;
        flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;

        this.Controls.Add(flowLayoutPanel);

        // Load movie data
        LoadMoviesAsync();
    }

    private async Task LoadMoviesAsync()
    {
        movies = await GetMoviesAsync();
        DisplayMovies();
    }

    private async Task<List<Movie>> GetMoviesAsync()
    {
        List<Movie> movieList = new List<Movie>();
        using (HttpClient client = new HttpClient())
        {
            string url = $"https://api.themoviedb.org/3/movie/now_playing?api_key={API_KEY}&language=en-US&page=1";
            var response = await client.GetStringAsync(url);
            JObject data = JObject.Parse(response);
            JArray results = (JArray)data["results"];

            foreach (var item in results)
            {
                Movie movie = new Movie
                {
                    Id = (int)item["id"],
                    Title = (string)item["title"],
                    Overview = (string)item["overview"],
                    PosterPath = (string)item["poster_path"]
                };
                movieList.Add(movie);
            }
        }
        return movieList;
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
        myTicketButton.Text = "My Tickets";
        myTicketButton.Size = new Size(100, 30);
        myTicketButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
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
}*/