using BookStore.DBContext;
using BookStore.Migrations;
using BookStore.Repository;
using Hangfire;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
//adding serilog to log info into a file
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

//Add services to the container.
builder.Services.AddHangfire(x =>
           x.UseSqlServerStorage("Server = localhost; Database = Bookstore; User Id = sa; " +
"Password=Password@123;TrustServerCertificate=True;"));
builder.Services.AddHangfireServer();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
   
    setupAction.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,$"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    setupAction.AddSecurityDefinition("BookStoreApiAuthentication", new()
    {
         Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
         Scheme = "Bearer",
         Description = "Input a Valid Token To Access Api"
    });
});
builder.Services.AddDbContext<BookStoreDbContext>(options =>
options.UseSqlServer("Server=localhost;Database=Bookstore;User Id=sa;" +
"Password=Password@123;TrustServerCertificate=True;"));
builder.Services.AddScoped<IBookStoreRepository, BookStoreRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IEmailSettings, EmailSettings>();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"])),
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHangfireDashboard();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("corsapp");

app.MapControllers();

app.Run();
