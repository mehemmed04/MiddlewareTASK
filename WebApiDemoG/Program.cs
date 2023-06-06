using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApiDemoG.Data;
using WebApiDemoG.Formatters;
using WebApiDemoG.MiddleWares;
using WebApiDemoG.Repositories.Abstract;
using WebApiDemoG.Repositories.Concrete;
using WebApiDemoG.Services.Abstract;
using WebApiDemoG.Services.Asbtract;
using WebApiDemoG.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.OutputFormatters.Add(new VCardOutputFormatter());
    options.OutputFormatters.Add(new TextCsvOutputFormatter());
    options.InputFormatters.Add(new TextCsvInputFormatter());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var connection = builder.Configuration.GetConnectionString("myconn");
builder.Services.AddDbContext<StudentDBContext>(opt =>
{
    opt.UseSqlServer(connection);
});
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
 options => builder.Configuration.Bind("JwtSettings", options))
 .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
 options => builder.Configuration.Bind("CookieSettings", options));

//var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(key),
//            ValidateIssuer = false,
//            ValidateAudience = false
//        };
//    });
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<AuthenticationMiddleware>();
app.UseAuthentication();
app.UseCors("corsapp");
app.UseAuthorization();

app.MapControllers();

app.Run();
