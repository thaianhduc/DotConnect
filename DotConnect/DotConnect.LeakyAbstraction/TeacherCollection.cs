using System;
using System.Collections.Generic;
using System.Linq;

namespace DotConnect.LeakyAbstraction
{
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
}
