using AutoMapper;
using Blog.DAL;
using Blog.DAL.Models;
using Blog.DAL.Repositories;
using Blog.DAL.Repositories.Interfaces;
using Blog.Services;
using Blog.Services.Services;
using Blog.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddRazorPages();




var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Blog.DAL"))).AddIdentity<User, Role>(opts =>
{
	opts.Password.RequiredLength = 5;
	opts.Password.RequireNonAlphanumeric = false;
	opts.Password.RequireLowercase = false;
	opts.Password.RequireUppercase = false;
	opts.Password.RequireDigit = false;
}).AddEntityFrameworkStores<DBContext>();

var mapperConfig = new MapperConfiguration((v) =>
{
	v.AddProfile(new MapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.Configure<SecurityStampValidatorOptions>(opt => { opt.ValidationInterval = TimeSpan.Zero; });
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

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

/*builder.Services.ConfigureApplicationCookie(opt =>
{
	opt.LoginPath = "/Login";
	opt.AccessDeniedPath = "/AccessDenied";
});*/

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

//app.MapRazorPages();
app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
