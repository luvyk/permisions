using permisionsApp.Entities;
using System.Text.Json;

namespace mensiTEst
{
    public class Program
    {
        //public static List<Permsion> AllPerms {  get; set; }
        public static List<Subject> AllSubjects { get; set; }
        public static void Main(string[] args)
        {
            try
            {
                AllSubjects = JsonSerializer.Deserialize<Subject[]>(File.ReadAllText(@"SecretStuff.txt")).ToList();
                //AllPerms = JsonSerializer.Deserialize<Permsion[]>(File.ReadAllText("\\SecretStuff.txt")).ToList();
            }
            catch { 
            
            }
            /* test data */
            AllSubjects = new List<Subject>();
            AllSubjects.Add(new Subject(new List<Permsion>(), "Toanir", "123456Ab", true, 0));
            AllSubjects[0].Permsions.Add(new Permsion("Hugs", true));
            AllSubjects[0].kodUzivatele = "1F4";

            AllSubjects[0].Permsions.Add(new Permsion("Mòaukání", false));
 



            AllSubjects.Add(new Subject(new List<Permsion>(), "Granya", "123456Ab", false, 1));
            AllSubjects[1].Permsions.Add(new Permsion("Hugs", false));
            AllSubjects[1].kodUzivatele = "FF0";

            AllSubjects[1].Permsions.Add(new Permsion("Drbání", true));


            /* test data */
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

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

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
