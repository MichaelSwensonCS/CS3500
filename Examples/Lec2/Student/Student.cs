using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lec2
{
  /// <summary>
  /// A simple student representation class.
  /// </summary>
  public class Student
  {
    public static int nextID = 0;
    public int ID;
    public string major;


    public Student(string _major)
    {
      ID = ++nextID;
      major = _major;
    }

  }
}

