using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using WinFormsApp1.MovieBooking;
using System.Drawing;
using WinFormsApp1.Models;

public class MovieDetailsForm : Form
{
    private const string API_KEY = "ecfcd92abcf0735492247a8eae3c742a"; 
    private Movie movie;

    public MovieDetailsForm(Movie movie)
    {
        this.movie = movie;

        // Initialize form properties
        this.Text = "Movie Details";
        this.Size = new Size(600, 600);
        this.AutoScroll = true;

       DisplayMovieDetails();
    }

    /*private async Task LoadMovieDetailsAsync()
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"https://api.themoviedb.org/3/movie/{movie.Id}?api_key={API_KEY}&language=en-US&append_to_response=credits";
            var response = await client.GetStringAsync(url);
            JObject data = JObject.Parse(response);

            movie.Runtime = data["runtime"] != null ? (int)data["runtime"] : 0;
            JArray castArray = (JArray)data["credits"]["cast"];
            movie.Cast = new List<string>();
            foreach (var castMember in castArray)
            {
                movie.Cast.Add((string)castMember["name"]);
                if (movie.Cast.Count >= 5) break;
            }

            DisplayMovieDetails();
        }
    }*/

   private void DisplayMovieDetails()
    {
        // Title Label
        Label titleLabel = new Label();
        titleLabel.Text = "Title: " + movie.Title;
        titleLabel.Location = new Point(10, 10);
        titleLabel.Size = new Size(400, 30);
        this.Controls.Add(titleLabel);

        // Overview Label with Wrapping
        Label overviewLabel = new Label();
        overviewLabel.Text = "Overview: " + movie.Overview;
        overviewLabel.Location = new Point(10, 50); // Adjusted location to be below title
        overviewLabel.AutoSize = true;
        overviewLabel.MaximumSize = new Size(this.ClientSize.Width - 20, 0); // Set wrapping width
        this.Controls.Add(overviewLabel);

        // Genre Label
        var genres = movie.Genres;
        Label genreLabel = new Label();
        genreLabel.Text = "Genre: " + string.Join(", ", genres.Select(g => g.Name));
        genreLabel.Location = new Point(10, overviewLabel.Bottom + 10); // Position after overview
        genreLabel.Size = new Size(400, 30);
        this.Controls.Add(genreLabel);

        

        buttonComponent();
    }

    private void buttonComponent()
    {
        
        int formWidth = this.ClientSize.Width;
        int formHeight = this.ClientSize.Height;

        
        Label selectShowtimeLabel = new Label();
        selectShowtimeLabel.Text = "Select Showtime and Seats";
        selectShowtimeLabel.AutoSize = true;
        selectShowtimeLabel.MaximumSize = new Size(150, 0); 
        selectShowtimeLabel.BackColor = SystemColors.ControlLight;
        selectShowtimeLabel.BorderStyle = BorderStyle.FixedSingle;
        selectShowtimeLabel.TextAlign = ContentAlignment.MiddleCenter;
        selectShowtimeLabel.Padding = new Padding(5);
        selectShowtimeLabel.Cursor = Cursors.Hand;
        selectShowtimeLabel.Click += (s, e) =>
        {
            SeatSelection showtimeForm = new SeatSelection(movie);
            showtimeForm.ShowDialog();
        };

        
        selectShowtimeLabel.Location = new Point(10, (formHeight / 2) - (selectShowtimeLabel.Height / 2));
        this.Controls.Add(selectShowtimeLabel);

        
        Label anotherLabel = new Label();
        anotherLabel.Text = "Manage Show of this movie";
        anotherLabel.AutoSize = true;
        anotherLabel.MaximumSize = selectShowtimeLabel.MaximumSize;
        anotherLabel.BackColor = SystemColors.ControlLight;
        anotherLabel.BorderStyle = BorderStyle.FixedSingle;
        anotherLabel.TextAlign = ContentAlignment.MiddleCenter;
        anotherLabel.Padding = new Padding(5);
        anotherLabel.Click += (s, e) =>
        {
            AddShowing addShowingForm = new AddShowing(movie);
            addShowingForm.ShowDialog();
        };

        
        anotherLabel.Location = new Point(formWidth - anotherLabel.Width - 50, (formHeight / 2) - (anotherLabel.Height / 2));
        this.Controls.Add(anotherLabel);
    }
}
