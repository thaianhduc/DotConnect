using System;
using System.Linq;

namespace DotConnect.LeakyAbstraction
{

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
                                        .Select(x => x.Name)));
            Console.WriteLine("> 10 years teachers are: {0}",
                string.Join("; ", school.TeacherCollection
                                        .WhereExperienced()
                                        .Select(x => x.Name)));
        }
    }
}
