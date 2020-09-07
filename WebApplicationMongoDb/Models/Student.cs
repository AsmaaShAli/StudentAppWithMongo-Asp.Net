using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationMongoDb.Models
{
    public class Student
    {
        public object _id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailAddress { get; set; }
        public int Age { get; set; }
    }
}