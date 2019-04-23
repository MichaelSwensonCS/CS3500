// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

// Implemented Logic: Michael Swenson, U0585863

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //

    //Has,Add,Remove methods should be O(1), this lead to choosing hashsets.

    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        //Two sets make thinking easier, and Hashset without collision
        //has an insert/Add/Remove/elementAt/GetEnumerator complexity of O(1)
        //Since we are using two data structures we must enforce the invariant that whenever one is changed the other is changed

        private Dictionary<String, HashSet<String>> dependents;
        private Dictionary<String, HashSet<String>> dependees;

        private int DependencyGraphSize { get; set; }

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
            DependencyGraphSize = 0;
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            //Use our own private int so that we always know that lookup for size will be constant
            get { return DependencyGraphSize; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                //Just see how many elements are in the keys hashset
                if (dependees.ContainsKey(s)) return dependees[s].Count;

                return 0;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            //Just remember the key is what all the list elements depends on
            return dependees.ContainsKey(s);
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            //The key is the dependee and the list are the dependents
            return dependents.ContainsKey(s);
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (dependees.ContainsKey(s)) return new HashSet<string>(dependees[s]);

            else return new HashSet<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (dependents.ContainsKey(s)) return new HashSet<string>(dependents[s]);

            else return new HashSet<string>();
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            //PS4 Circular Dependency
            //Only increment the size if the key does not exist because we will have to create a new "node"            
            if (!(dependees.ContainsKey(s) && dependents.ContainsKey(t))) DependencyGraphSize++;

            //If the Key already exists just add it to the list
            if (dependees.ContainsKey(s))
            {
                dependees[s].Add(t);
            }
            else
            {
                //Make a new entry with the key s and a collection filled with t
                dependees.Add(s, new HashSet<string>() { t });
            }


            //Do the same thing to enforce the invariant of the two data structures 
            if (dependents.ContainsKey(t))
            {
                dependents[t].Add(s);
            }
            else
            {
                dependents.Add(t, new HashSet<string>() { s });
            }
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            //If this is a key that already exists we must decrease the dependency size
            if (dependees.ContainsKey(s) && dependents.ContainsKey(t)) DependencyGraphSize--;

            if (dependees.ContainsKey(s))
            {
                dependees[s].Remove(t);
                //Getting rid of Keys with empty lists will not affect the size because we are tracking that independently
                if (dependees[s].Count == 0) dependees.Remove(s);
            }

            if (dependents.ContainsKey(t))
            {
                dependents[t].Remove(s);
                if (dependents[t].Count == 0) dependents.Remove(t);
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            //Cannot enumerate through something and change it, we need a copy
            //I tried it on the original and couldn't see anything wrong but this was a big topic in class
            IEnumerable<string> originalDependents = GetDependents(s);

                foreach (string dependent in originalDependents)
                {
                    RemoveDependency(s, dependent);
                }
                foreach (string value in newDependents)
                {
                    AddDependency(s, value);
                }
            

        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            //Cannot enumerate through something and change it, we need a copy
            //I tried it on the original and couldn't see anything wrong but this was a big topic in class
            IEnumerable<string> originalDependees = GetDependees(s);
            foreach (string dependee in originalDependees)
            {
                RemoveDependency(dependee, s);
            }
            foreach (string value in newDependees)
            {
                AddDependency(value, s);
            }
        }
    }
}


