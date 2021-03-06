using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotConnect.LeakyAbstraction
{
    public class TeacherCollection : ITeacherCollection, IIndexedTeacherCollection
    {
        private readonly IList<Teacher> _teachers = new List<Teacher>();

        public TeacherCollection(IEnumerable<Teacher> teachers)
        {
            if (teachers != null)
                _teachers = teachers.Where(x => x.IsStillAtWork).ToList();
        }

        public ITeacherCollection WhereTeachMathematics()
        {
            return new TeacherCollection(_teachers.Where(x => x.Specialty == "Mathematics"));
        }

        public ITeacherCollection WhereExperienced()
        {
            return new TeacherCollection(_teachers.Where(x => DateTime.Now >= x.StartedOn.AddYears(10)));
        }

        public IEnumerator<Teacher> GetEnumerator()
        {
            return _teachers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _teachers.GetEnumerator();
        }

        public int Count
        {
            get { return _teachers.Count; }
        }

        public int GetIndex(string teacherName)
        {
            for(var index = 0; index < _teachers.Count; index ++)
            {
                if(_teachers[index].Name == teacherName)
                {
                    return index;
                }
            }
            return -1;
        }

        public IIndexedTeacherCollection BuildIndex()
        {
            return new TeacherCollection(_teachers.OrderBy(x => x.StartedOn));
        }
    }
}
