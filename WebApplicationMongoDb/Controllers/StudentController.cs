using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using WebApplicationMongoDb.Models;

namespace WebApplicationMongoDb.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index(string searchValue, string searchAge)
        {
            Models.MongoHelper.ConnectToMongoService();
            Models.MongoHelper.student_collection = Models.MongoHelper.database.GetCollection<Models.Student>("Students");

            if (String.IsNullOrEmpty(searchValue) && String.IsNullOrEmpty(searchAge))
            {
                var filter = Builders<Models.Student>.Filter.Ne("_id", "");  //return all documents with non-empty id
                var result = MongoHelper.student_collection.Find(filter).ToList();

                return View(result);
            }
            else
            {
                var collection = Models.MongoHelper.student_collection;
                var query = collection.AsQueryable<Student>()
                            .Where(e => e.firstName.Contains(searchValue) || e.lastName.Contains(searchValue) || e.emailAddress.Contains(searchValue))
                            .Select(e => e).ToList();

                if (String.IsNullOrEmpty(searchAge))
                {
                    return View(query);
                }
                else
                {
                    var query2 = query.AsQueryable<Student>()
                        .Where(e => e.Age >= Convert.ToInt32(searchAge)).Select(e => e).ToList();
                    return View(query2);
                }
            }  
        }

        // GET: Student/Details/5
        public ActionResult Details(string id)
        {
            Models.MongoHelper.ConnectToMongoService();
            Models.MongoHelper.student_collection = Models.MongoHelper.database.GetCollection<Models.Student>("Students");
            var filter = Builders<Models.Student>.Filter.Eq("_id", id);
            var result = MongoHelper.student_collection.Find(filter).FirstOrDefault();
            return View(result);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Models.MongoHelper.ConnectToMongoService();
                Models.MongoHelper.student_collection = Models.MongoHelper.database.GetCollection<Models.Student>("Students");
                //create random id
                Object id = GenerateRandomId(24);

                Models.MongoHelper.student_collection.InsertOneAsync(new Models.Student {
                    _id = id,
                    firstName = collection["firstname"],
                    lastName = collection["lastName"],
                    emailAddress = collection["emailAddress"],
                    Age = Convert.ToInt32 (collection["Age"])
                });

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(string id)
        {
            Models.MongoHelper.ConnectToMongoService();
            Models.MongoHelper.student_collection = Models.MongoHelper.database.GetCollection<Models.Student>("Students");
            var filter = Builders<Models.Student>.Filter.Eq("_id", id);
            var result = MongoHelper.student_collection.Find(filter).FirstOrDefault();
            return View(result);
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                Models.MongoHelper.ConnectToMongoService();
                Models.MongoHelper.student_collection = Models.MongoHelper.database.GetCollection<Models.Student>("Students");
                var filter = Builders<Models.Student>.Filter.Eq("_id", id);

                var update = Builders<Models.Student>.Update
                    .Set("firstName", collection["firstName"])
                    .Set("lastName", collection["lastName"])
                    .Set("emailAddress", collection["emailAddress"])
                    .Set("Age", Convert.ToInt32(collection["Age"]));

                var result = Models.MongoHelper.student_collection.UpdateOneAsync(filter, update);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Delete/5
        public ActionResult Delete(string id)
        {
            Models.MongoHelper.ConnectToMongoService();
            Models.MongoHelper.student_collection = Models.MongoHelper.database.GetCollection<Models.Student>("Students");
            var filter = Builders<Models.Student>.Filter.Eq("_id", id);
            var result = MongoHelper.student_collection.Find(filter).FirstOrDefault();

            return View(result);
        }

        // POST: Student/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                Models.MongoHelper.ConnectToMongoService();
                Models.MongoHelper.student_collection = Models.MongoHelper.database.GetCollection<Models.Student>("Students");
                var filter = Builders<Models.Student>.Filter.Eq("_id", id);

                var result = MongoHelper.student_collection.DeleteOneAsync(filter);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private static Random random = new Random();
        private object GenerateRandomId (int v)
        {
            string temparray = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(temparray, v).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
