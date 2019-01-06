using System.Collections.Generic;

namespace DotConnect.LeakyAbstraction
{
    public interface ITeacherCollection : IEnumerable<Teacher>
    {
        ITeacherCollection WhereTeachMathematics();
        ITeacherCollection WhereExperienced();
        int Count{get;}
        IIndexedTeacherCollection BuildIndex();
    }
}
