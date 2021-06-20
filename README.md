# Animulu
Animulu is video streaming service built on ASP.NET Core that is great starting point for creating your new video streaming service. With easy to customize code, a lot useful built-in functionalities and minimalistic great looking frontend, Animulu will save you a lot of time in your future projects.

## Functionalities
Animulu supports: 
* Responsive interface that works perfectly on any desktop or mobile device
* Streaming videos with adaptive bitrate (MPEG-DASH) and fully working video player 
* Fully working account system with lockdowns, email confirmation, avatars and basic administration functionality built into website.
* Reviewing shows, liking videos and posting comments without need to reload the page
* Sorting videos and filtering them by category and name 

## Demo
Check out online [DEMO](https://animulu.azurewebsites.net/).
(down at the moment)

## Technology
Animulu is built on ASP.NET Core 3.1 with use of:
* Blazor and jQuery for interactivity
* Bootstrap as a main css library
* Entity Framework
* SQL database
* Dash.js for video player

## Quick Start
1. Clone Animulu repository
2. Import databases from `Demo databases/`
3. Put necessary libraries into `wwwroot/lib/`
4. Run using Cake or manualy
5. Start creating your own streaming service with Animulu

## Necessary libraries
* Bootstrap
* css-element-queries (only Resize Sensor is needed)
* Dash.js
* Font Awesome
* jQuery
* malihu custom scrollbar plugin

### NuGet Packages:
* Microsoft.AspNetCore.Identity.EntityFrameworkCore
* Microsoft.AspNetCore.Identity.UI
* Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
* Microsoft.Azure.SignalR
* Microsoft.EntityFrameworkCore.Design
* Microsoft.EntityFrameworkCore.SqlServer
* Microsoft.EntityFrameworkCore.Tools
* SendGrid
