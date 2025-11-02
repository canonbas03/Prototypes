// README - Minimal Railway Audio Guide Prototype (ASP.NET Core MVC)
// -----------------------------------------------------------------
// Purpose: lightweight prototype to test time-based "audio points" on a train route.
// Stack: ASP.NET Core (Minimal hosting) + MVC + in-memory seed data.
// Files included below. Create a new project and paste files or copy snippets.

/*
Project layout (as provided in this single file):
- Program.cs
- Models/RoutePoint.cs
- Services/RouteService.cs
- Controllers/GuideController.cs
- Views/Guide/Index.cshtml

How to create project quickly:
1. dotnet new web -n RailGuide
2. Replace Program.cs with the Program.cs code below.
3. Add folders Models, Services, Controllers, Views/Guide and add the respective files.
4. dotnet run
5. Open https://localhost:5001/Guide (or the URL printed in the console)

Design choices (kept simple):
- RoutePoint uses TimeSpan offsets (StartOffset, EndOffset) relative to "journey start".
- The system can be driven by a simulation parameter ?offsetSeconds=123 to test different moments.
- Optional query params: ?side=left|right|both, ?direction=forward|backward, ?gps=true&gpsOffsetSeconds=...
- No real audio files: there are buttons that simulate "play" and the UI highlights active point.
*/

// -------------------- Program.cs --------------------
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<RouteService>(); // in-memory seed


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
name: "default",
pattern: "{controller=Guide}/{action=Index}/{id?}");


app.Run();

// -------------------- Models/RoutePoint.cs --------------------


// -------------------- Services/RouteService.cs --------------------


// -------------------- Controllers/GuideController.cs --------------------


// -------------------- Controllers/ViewModels (inline) --------------------


// -------------------- Views/Guide/Index.cshtml --------------------

// -------------------- End of file --------------------
