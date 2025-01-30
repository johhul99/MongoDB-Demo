using Microsoft.OpenApi.Validations;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB_Demo.Models;

namespace MongoDB_Demo.Data
{
    public class MongoCRUD
    {
        private IMongoDatabase db;

        public MongoCRUD(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);
        }

        //Create
        public async Task<List<Courses>> AddCourse(string table, Courses course)
        {
            var collection = db.GetCollection<Courses>(table);
            await collection.InsertOneAsync(course);
            return collection.AsQueryable().ToList();
        }

        //Update
        public async Task<List<Courses>> UpdateCourse(string table, string name, string description, string link, string category, string id)
        {
            var collection = db.GetCollection<Courses>(table);
            var course = await collection.Find(c => c.Id == id).FirstOrDefaultAsync();
            if (course != null)
            {
                course.Name = name;
                course.Description = description;
                course.Link = link;
                course.Category = category;

                await collection.ReplaceOneAsync(c => c.Id == id, course);
            }

            return await collection.Find(_ => true).ToListAsync();
        }

        //Read
        public async Task<List<Courses>> GetAllCourses(string table)
        {
            var collection = db.GetCollection<Courses>(table);
            var courses = await collection.AsQueryable().ToListAsync();
            return courses;
        }

        //ReadById
        public async Task<Courses> GetCourseById(string table, string id)
        {
            var collection = db.GetCollection<Courses>(table);
            var course  = await collection.Find(c => c.Id == id).FirstOrDefaultAsync();
            return course;


        }      

        //Delete
        public async Task<string> DeleteCourse (string table, string id)
        {
            var collection = db.GetCollection<Courses>(table);
            var course = await collection.DeleteOneAsync(x => x.Id == id);
            return "Deleted course";
        }
    }
}

