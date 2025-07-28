using Bloggie.Web.Data;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the DbContext with dependency injection
builder.Services.AddDbContext<BloggieDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString")));
// Register the AuthDbContext with dependency injection
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieAuthDbConnectionString")));


// Configure Identity options
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();
// Register the TagRepository as a service
builder.Services.AddScoped<ITagRepository, TagRepository>();
// Register the BlogPostRepository as a service
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
// Register the ImageRepository as a service
builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

//using Microsoft.AspNetCore.Identity;

//namespace PasswordHashGenerator
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var user = new IdentityUser
//            {
//                UserName = "superadmin@bloggie.com",
//                Email = "superadmin@bloggie.com"
//            };

//            var hasher = new PasswordHasher<IdentityUser>();
//            var hashedPassword = hasher.HashPassword(user, "Superadmin@123");

//            Console.WriteLine("Generated Password Hash:");
//            Console.WriteLine(hashedPassword);

//            Console.WriteLine("\nPress any key to exit...");
//            Console.ReadKey(); // Waits for a key press before closing
//        }
//    }
//}
