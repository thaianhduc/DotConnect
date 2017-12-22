using System;
using System.Collections.Generic;
using System.Linq;

namespace DotConnect.LeakyAbstraction
{

    public class Teacher
    {
        public string Name { get; set; }
        public string Specialty { get; set; }
        public DateTime StartedOn { get; set; }
        public bool IsStillAtWork { get; set; }
    }

    public class TeacherCollection
    {
        private readonly IList<Teacher> _teachers = new List<Teacher>();

        public TeacherCollection(IEnumerable<Teacher> teachers)
        {
            if (teachers != null)
                _teachers = teachers.Where(x => x.IsStillAtWork).ToList();
        }

        public TeacherCollection WhereTeachMathematics()
        {
            return new TeacherCollection(_teachers.Where(x => x.Specialty == "Mathematics"));
        }

        public TeacherCollection WhereExperienced()
        {
            return new TeacherCollection(_teachers.Where(x => DateTime.Now >= x.StartedOn.AddYears(10)));
        }

        public int Count
        {
            get { return _teachers.Count; }
        }

        public IEnumerable<Teacher> AsEnumerable
        {
            get { return _teachers.AsEnumerable(); }
        }
    }

    public class School
    {
     
        public string Name { get; set; }
        public IList<Teacher> Teachers { get; set; }

        public TeacherCollection TeacherCollection
        {
            get
            {
                return new TeacherCollection(Teachers);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var school = new School
            {
                Name = "Gotham City"
            };
            school.Teachers.Add(new Teacher
            {
                Name = "Batman",
                Specialty = "Mathematics",
                StartedOn = DateTime.Now.AddYears(-11),
                IsStillAtWork = true
            });
            school.Teachers.Add(new Teacher
            {
                Name = "Joker",
                Specialty = "Chemical",
                StartedOn = DateTime.Now.AddYears(-6),
                IsStillAtWork = false
            });
            Console.WriteLine("Total teachers: {0}", school.TeacherCollection.Count);
            Console.WriteLine("Mathematics teachers are: {0}", 
                string.Join("; ", school.TeacherCollection
                                        .WhereTeachMathematics()
                                        .AsEnumerable
                                        .Select(x => x.Name)));
            Console.WriteLine("> 10 years teachers are: {0}",
                string.Join("; ", school.TeacherCollection
                                        .WhereExperienced()
                                        .AsEnumerable
                                        .Select(x => x.Name)));
        }
    }
}
