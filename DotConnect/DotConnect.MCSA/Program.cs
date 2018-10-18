using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotConnect.MCSA
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var fl = new ForLoop();
            foreach (var i in fl.ReturnNewCollection)
            {
                Console.Write(i);
            }
            Console.Read();

        }

        static void TestReference(User user)
        {
            Console.WriteLine("Set user to null");
            user.Group = Group.Administrators;
            user = null;
        }
    }

    class ForLoop
    {
        public IList<int> ReturnNewCollection
        {
            get
            {
                Console.WriteLine("Hello");
                return  Enumerable.Range(1, new Random(1).Next(20)).ToList();
            }
        }
    }

    [Flags]
    public enum Group
    {
        Users = 1,
        Supervisors = 2,
        Managers = 4,
        Administrators = 8
    }

    public class User
    {
        public Group Group { get; set; }
    }

    interface IHome
    {
        void Start();
    }
    interface IOffice
    {
        void Start();
    }

    class Test1 : IHome, IOffice
    {
        void IHome.Start()
        {
            throw new NotImplementedException();
        }

        void IOffice.Start()
        {
            throw new NotImplementedException();
        }
    }
  
}
