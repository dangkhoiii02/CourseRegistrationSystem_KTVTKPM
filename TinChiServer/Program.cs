using Microsoft.EntityFrameworkCore;
using QuanLyTinChi.Data;
using QuanLyTinChi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<QuanLyTinChi.TinChiComp>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
var clientPath = Path.Combine(builder.Environment.ContentRootPath, "..", "Vanilla_Client");

app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(clientPath)
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(clientPath),
    RequestPath = ""
});
app.MapControllers();

app.Run();
