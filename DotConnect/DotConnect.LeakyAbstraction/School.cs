using System.Collections.Generic;

namespace DotConnect.LeakyAbstraction
{
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
}
