
using MongoDB.Driver;
using MongoDB_Demo.Models;
using MongoDB_Demo.Data;

namespace MongoDB_Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            MongoCRUD db = new MongoCRUD("AzureCourses");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //Create
            app.MapPost("/course", async (Courses course) =>
            {

                var testDB = await db.AddCourse("Courses", course);
                return Results.Ok(testDB);
            });

            //Update
            app.MapPut("/course/{id}", async (string id, string name, string description, string link, string category) =>
            {
                var courses = await db.UpdateCourse("Courses", name, description, link, category, id);
                return Results.Ok(courses);

            });

            //Read
            app.MapGet("/courses", async () =>
            {
                var courses = await db.GetAllCourses("Courses");
                return Results.Ok(courses);
            });

            //ReadById
            app.MapGet("/courses/{id}", async (string id) =>
            {
                var course = await db.GetCourseById("Courses", id);
                return Results.Ok(course);
            });

            
            //Delete
            app.MapDelete("/course/{id}", async (string id) =>
            {
                var course = await db.DeleteCourse("Courses", id);
                return Results.Ok(course);
            });
            

            app.Run();
        }
    }
}
