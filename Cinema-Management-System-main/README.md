#  Cinema Management System  

A Windows Forms-based Movie Ticket Ordering System built using C#, .NET, SQL Server, and OOP principles.

Project Duration: Oct 2024 – Nov 2024

Team Project: 3 members

##  Project Overview

The **Cinema Management System** is a Windows Forms application designed for movie ticket ordering, providing users with an intuitive self-service experience.  
The system allows users to browse movies, view details, select sessions, choose seats, book tickets, and query existing bookings.

This application was developed as part of a UTS software engineering assignment, demonstrating OOP design, GUI development, and database integration.

##  My Contribution — WinForms Front-End Interface

I was **primarily responsible for the entire Windows Forms (WinForms) front-end interface**, including:

###  UI Design  
- Designing and implementing all major user interfaces:
  - Movie List Page  
  - Movie Details Page  
  - Session Selection Page  
  - Seat Selection Page  
  - Ticket Query Page  
  - Add Session Page  

###  Event-Driven Form Logic  
- Implementing button events, form navigation, and page transitions  
- Ensuring each form responds correctly to user actions  
- Handling UI logic such as:
  - Opening new forms  
  - Passing selected movie/session data between screens  
  - Updating views after database changes

###  UI–Backend Integration  
- Connecting the UI with backend services and database operations  
- Binding movie/session data to UI components  
- Displaying dynamic seat maps and availability states  
- Rendering real-time results after ticket bookings or cancellations  

###  User Experience (UX) Design  
- Designing intuitive interfaces suitable for kiosk self-service use  
- Ensuring smooth interaction flow between browsing → selecting → booking  
- Maintaining consistent layout, fonts, colors, and button styles  

###  Form Component Implementation  
- DataGridView components  
- PictureBox movie poster rendering  
- ComboBoxes for date/time/hall selection  
- Seat map rendering (custom layout or grid controls)  
- Calendar/time pickers  
- Text fields and validation prompts

##  Key System Features

### 1.  Movie List Interface  
Displays all movies with title + poster, allowing users to click through to details.

### 2.  Movie Details Interface  
Shows plot, cast, duration, and allows users to proceed to booking.

### 3.  Session & Date Selection  
Users choose the showtime and view available sessions via calendar & dropdown UI.

### 4.  Seat Selection Interface  
Visual seat map where users pick seats, with each seat showing availability status.

### 5.  Add Session (Staff UI)  
Admin can add movie sessions (movie, hall, time, date) using intuitive controls.

### 6.  Ticket Query  
Users input ticket ID to view booking information or cancel a purchase.

##  Technical Stack

###  Front-End (My Responsibility)
- **C# WinForms (.NET)**  
- Event-driven programming  
- GUI component layouts  
- Form navigation and data passing  
- Input validation and error handling  

###  Back-End (Team)
- SQL Server database  
- OOP architecture (Models, Services)  
- LINQ & Lambda expressions  
- NUnit unit testing  
- Data persistence and seat logic  

