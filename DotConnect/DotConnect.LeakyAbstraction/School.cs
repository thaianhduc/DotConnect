using System.Collections.Generic;

namespace DotConnect.LeakyAbstraction
{
    public class School
    {
     
        public string Name { get; set; }
        public IList<Teacher> Teachers { get; set; }

        public ITeacherCollection TeacherCollection
        {
            get
            {
                return new TeacherCollection(Teachers);
            }
        }
    }
}
