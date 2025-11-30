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
    public partial class AddShowing : Form
    {
        Movie movie;
        MonthCalendar calendar;
        DateTimePicker startTimePicker;
        DateTimePicker endTimePicker;
        ListBox infoListBox;

        private ShowingService showingService = new ShowingService(new ShowingRepository(new CinemaAppDbcontext()));
        public AddShowing(Movie movie)
        {
            InitializeComponent();
            this.movie = movie;
            SetupUI();
            
        }
        private void SetupUI()
        {
            
            this.ClientSize = new Size(800, 800);

            
            int padding = 20;
            int controlSpacing = 10;

            
            Label movieLabel = new Label();
            movieLabel.Text = $"Movie: {movie.Title}";
            movieLabel.Location = new Point(padding, padding);
            movieLabel.Size = new Size(400, 20);
            this.Controls.Add(movieLabel);

            
            calendar = new MonthCalendar();
            calendar.Location = new Point(padding, movieLabel.Bottom + controlSpacing);
            calendar.MaxSelectionCount = 1;
            calendar.MinDate = DateTime.Today;
            this.Controls.Add(calendar);

            
            Label startTimeLabel = new Label();
            startTimeLabel.Text = "Start Time:";
            startTimeLabel.Location = new Point(calendar.Right + 150, calendar.Top);
            this.Controls.Add(startTimeLabel);

            startTimePicker = new DateTimePicker();
            startTimePicker.Format = DateTimePickerFormat.Custom;
            startTimePicker.CustomFormat = "HH:mm";
            startTimePicker.ShowUpDown = true;
            startTimePicker.Location = new Point(startTimeLabel.Right + controlSpacing, startTimeLabel.Top);
            this.Controls.Add(startTimePicker);

           
            Label endTimeLabel = new Label();
            endTimeLabel.Text = "End Time:";
            endTimeLabel.Location = new Point(calendar.Right + 150, startTimePicker.Bottom + controlSpacing);
            this.Controls.Add(endTimeLabel);

            endTimePicker = new DateTimePicker();
            endTimePicker.Format = DateTimePickerFormat.Custom;
            endTimePicker.CustomFormat = "HH:mm";
            endTimePicker.ShowUpDown = true;
            endTimePicker.Location = new Point(endTimeLabel.Right + controlSpacing, endTimeLabel.Top);
            this.Controls.Add(endTimePicker);

            
            Label roomLabel = new Label();
            roomLabel.Text = "Room:";
            roomLabel.Location = new Point(calendar.Right + 150, endTimePicker.Bottom + controlSpacing);
            this.Controls.Add(roomLabel);

            ComboBox roomComboBox = new ComboBox();
            roomComboBox.Location = new Point(roomLabel.Right + controlSpacing, roomLabel.Top);
            roomComboBox.Size = new Size(100, 20);
            roomComboBox.Items.AddRange(new string[] { "Room1"});
            roomComboBox.SelectedIndex = 0;
            this.Controls.Add(roomComboBox);

            
            infoListBox = new ListBox();
            infoListBox.Location = new Point(padding, calendar.Bottom + 120); 
            infoListBox.Size = new Size(this.ClientSize.Width - padding * 2, 100);
            infoListBox.Tag = "infoListBox"; 
            ListShowings(movie);
            infoListBox.DisplayMember = "Value"; 
            infoListBox.ValueMember = "Key";     
            
            this.Controls.Add(infoListBox);
            ListShowings(movie);


            Button deleteButton = new Button();
            deleteButton.Text = "Delete";
            deleteButton.Size = new Size(100, 30);
            deleteButton.Location = new Point(roomLabel.Left, roomLabel.Bottom + 20);
            deleteButton.Click += DeleteButton_Click;
            this.Controls.Add(deleteButton);

            
            Button confirmButton = new Button();
            confirmButton.Text = "Confirm";
            confirmButton.Size = new Size(100, 30);
            confirmButton.Location = new Point(deleteButton.Right + controlSpacing / 4, deleteButton.Top); // 放置在删除按钮右侧
            confirmButton.Click += ConfirmButton_Click;
            this.Controls.Add(confirmButton);

            
            this.ClientSize = new Size(
                Math.Max(calendar.Right + controlSpacing + roomComboBox.Width + 50, infoListBox.Right + padding),
                infoListBox.Bottom + padding
            );
        }
        /// <summary>
        /// List all showings of the movie after today
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ListShowings(Movie movie)
        {
            
            infoListBox.Items.Clear();
            ICollection<Showing> showings = showingService.GetShowingsByMovieId(movie.MovieId);
            int counter = 1;
            if(showings.Count != 0)
            {
                foreach (Showing showing in showings)
                {
                    
                    string showingInfo = $"{counter}: {showing.StartTime} to {showing.EndTime}";
                    counter++;
                    infoListBox.Items.Add(new KeyValuePair<int, string>(showing.ShowingId, showingInfo));
                    
                }
            }
            else
            {
                infoListBox.Items.Add("No showings available");
            }
           
        }

        //catch showId and delete it
        private async void DeleteButton_Click(object sender, EventArgs e)
        {
            
            ListBox infoListBox = this.Controls.OfType<ListBox>().FirstOrDefault(c => c.Tag as string == "infoListBox");

            if (infoListBox?.SelectedItem is KeyValuePair<int, string> selectedItem)
            {
                int showingId = selectedItem.Key;

                
                infoListBox.Items.Remove(selectedItem);


                try
                {
                    await showingService.DeleteAsync(showingId);
                    ListShowings(movie);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a session to delete.");
            }
        }

        //if startdate is between the runtime cannot add
        private async void ConfirmButton_Click(object sender, EventArgs e)
        {
            ListBox infoListBox = this.Controls.OfType<ListBox>().FirstOrDefault(c => c.Tag as string == "infoListBox");
            DateTime StartTime = startTimePicker.Value; 
            DateTime EndTime = endTimePicker.Value; 
            DateTime movieStart = GetStartTime(StartTime);
            DateTime movieEnd = GetStartTime(EndTime);
            try
            {
                Showing showing = await showingService.CreateShowingAsync(movie.MovieId, 1, movieStart, movieEnd);
                ListShowings(movie);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
           
            MessageBox.Show("Showing created sucessfully!");
        }

        private DateTime GetStartTime(DateTime time)
        {
            DateTime selectedDate = calendar.SelectionRange.Start;
            DateTime combinedDateTime = new DateTime(
                 selectedDate.Year,
                 selectedDate.Month,
                 selectedDate.Day,
                 time.Hour,
                 time.Minute,
                 time.Second
            );
            return combinedDateTime; 
                                                                                                                    
        }










    }
}
