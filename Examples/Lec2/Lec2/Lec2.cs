using System;
using System.Collections.Generic;
using System.Linq;
//using Namespace1;
using Namespace2;
//using University;

namespace Lec2
{

  public delegate int StudentComparer(Student s1, Student s2);


  class Lec2
  {


    static void Main(string[] args)
    {
     

      /*
            // Part 1 - references
            Student s1 = new Student("cs", 20);
            Student s2 = s1;
            //s2.ID = 50;
            changeStudent(s2);
            Console.WriteLine(s1.major);
            Console.WriteLine(s2.major);

            int x = 5;
            changeInt(x);
            Console.WriteLine(x);
            */

      Student s = new Student("cs");
      Student.nextID = 5;
      

      // Part 2 - Namespaces
      Thing t;
      Namespace2.Thing t2;

      University.Teacher teach = new University.Teacher();



      // Part 3 - Generic containers, IEnumerable

      List<Student> students = new List<Student>();
      students.Add(s);

      foreach (Student st in students)
      {
      }




      // Part 4 - Delegates

      Student min_s = MinStudent(students, CompareByID);


      // Cause the terminal to wait
      Console.Read();
    }

    
    public static int Foo(Student s1, Student s2)
    {
      return 4;
    }


    public static void changeStudent(Student s)
    {
      s.major = "Biology";
    }


    public static void changeInt(int i)
    {
      i++;
    }

    public static int CompareByID(Student s1, Student s2)
    {
      return s1.ID - s2.ID;
    }


    
    public static Student MinStudent(List<Student> students, StudentComparer cmp)
    {
      Student min = students[0];
      foreach (Student s in students)
      {
        if (cmp(s, min) < 0)
          min = s;
      }
      return min;
    }
    




  }
}


