using ActiveLog.Web.Data;
using ActiveLog.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Custom Services registrieren
builder.Services.AddScoped<ITrainingRepository, TrainingRepository>();
builder.Services.AddScoped<TrainingFactory>();
builder.Services.AddScoped<TrainingValidator>();
builder.Services.AddScoped<ITrainingService, TrainingService>();
builder.Services.AddScoped<TrainingExporter>();
builder.Services.AddScoped<TrainingStatisticsService>();

var app = builder.Build();

DatabaseHelper.InitializeDatabase();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Training}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
