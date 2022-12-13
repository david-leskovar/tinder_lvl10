using Tinder_lvl10.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using API.Services;
using API.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Middleware.Example;
using API.Data;
using API.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Console.WriteLine(builder.Configuration["TokenKey"]);

//Middleware

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {


    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
        ValidateIssuer = false,
        ValidateAudience = false,
    };

});





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEntityFrameworkSqlite().AddDbContext<DataContext>();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {

        policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:4200");

    });

});

builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<ILikesRepository, LikesRepository>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped<LogUserActivity>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);



var app = builder.Build();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    app.UseSwaggerUI();
    app.UseExceptionHandler("/error-development");



}

app.UseMiddleware<RequestCultureMiddleware>();

app.UseHttpsRedirection();


app.UseRouting();

app.UseCors("CorsPolicy");


app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers().RequireAuthorization();
});


app.Run();
