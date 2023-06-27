using Blog.Services.Services.Interfaces;
using Blog.Services.Services;
using Blog.DAL;
using Microsoft.EntityFrameworkCore;
using Blog.DAL.Models;
using AutoMapper;
using Blog.DAL.Repositories.Interfaces;
using Blog.DAL.Repositories;
using Blog.Services;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
var filepath = Path.Combine(AppContext.BaseDirectory, "API.xml");
c.IncludeXmlComments(filepath);
});
;
var mapperConfig = new MapperConfiguration((v) =>
{
    v.AddProfile(new MapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(connectionString))
    .AddIdentity<User, Role>(opts =>
    {
        opts.Password.RequiredLength = 5;
        opts.Password.RequireNonAlphanumeric = false;
        opts.Password.RequireLowercase = false;
        opts.Password.RequireUppercase = false;
        opts.Password.RequireDigit = false;
    }).AddEntityFrameworkStores<DBContext>();
builder.Services
    .AddSingleton(mapper)
    .AddTransient<IUserService, UserService>()
    .AddTransient<IPostService, PostService>()
    .AddTransient<IPostRepository, PostRepository>()
    .AddTransient<ITagRepository, TagRepository>()
    .AddTransient<ITagService, TagService>()
    .AddTransient<ICommentsRepository, CommentRepository>()
    .AddTransient<ICommentService, CommentSevice>()
    .AddTransient<IHomeService, HomeService>()
    .AddTransient<ICrutch, Crutch>()
    .AddTransient<IRoleService, RoleService>();

builder.Logging
    .ClearProviders()
    .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace)
    .AddConsole()
    .AddNLog("nlog");

builder.Services.AddAuthentication(optionts => optionts.DefaultScheme = "Cookies")
           .AddCookie("Cookies", options =>
           {
               options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
               {
                   OnRedirectToLogin = redirectContext =>
                   {
                       redirectContext.HttpContext.Response.StatusCode = 401;
                       return Task.CompletedTask;
                   }
               };
           });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
