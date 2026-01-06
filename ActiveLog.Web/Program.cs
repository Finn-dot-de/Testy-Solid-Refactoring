using ActiveLog.Web.Data;
using ActiveLog.Web.Services;
using ActiveLog.Web.Services.Strategies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Core Services
builder.Services.AddScoped<ITrainingRepository, TrainingRepository>();
builder.Services.AddScoped<TrainingFactory>();
builder.Services.AddScoped<TrainingValidator>();
builder.Services.AddScoped<ITrainingService, TrainingService>();
builder.Services.AddScoped<TrainingExporter>();
builder.Services.AddScoped<TrainingStatisticsService>();

// Strategies Registration
builder.Services.AddScoped<ITrainingCreationStrategy, CardioCreationStrategy>();
builder.Services.AddScoped<ITrainingCreationStrategy, KraftCreationStrategy>();
builder.Services.AddScoped<ITrainingCreationStrategy, TeamCreationStrategy>();
builder.Services.AddScoped<ITrainingCreationStrategy, YogaCreationStrategy>();

builder.Services.AddScoped<TrainingRepository>();
builder.Services.AddScoped<ITrainingRepository>(_ => _.GetRequiredService<TrainingRepository>());
builder.Services.AddScoped<ITrainingSearchRepository>(_ => _.GetRequiredService<TrainingRepository>());
builder.Services.AddScoped<ITrainingStatsRepository>(_ => _.GetRequiredService<TrainingRepository>());

var app = builder.Build();

DatabaseHelper.InitializeDatabase();

if (app.Environment.IsDevelopment())
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
