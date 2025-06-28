using permisionsApp.Entities;

namespace mensiTEst
{
    public class Program
    {
        //public static List<Permsion> AllPerms {  get; set; }
        public static List<Subject> AllSubjects { get; set; }
        public static void Main(string[] args)
        {
            //AllSubjects = JsonSerializer.Deserialize<Subject[]>(File.ReadAllText("\\SecretStuff.txt")).ToList();
            //AllPerms = JsonSerializer.Deserialize<Permsion[]>(File.ReadAllText("\\SecretStuff.txt")).ToList();


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
