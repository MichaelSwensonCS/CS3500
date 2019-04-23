using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace LabSQL
{
  public class Demo
  {
    /// <summary>
    /// The connection string.
    /// Your uID login name serves as both your database name and your uid
    /// </summary>
    public const string connectionString = "server=atr.eng.utah.edu;" +
      "database=Library;" +
      "uid=LabTestUser;" +
      "password=TestPassword";


    /// <summary>
    ///  Test several connections and print the output to the console
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
      AllPatrons();
      Console.WriteLine();

      AllPhones();
      Console.WriteLine();

      PatronsPhones();
      Console.ReadLine();
    }


    public static void AllPatrons()
    {
      // Connect to the DB
      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        try
        {
          // Open a connection
          conn.Open();

          // Create a command
          MySqlCommand command = conn.CreateCommand();
          command.CommandText = "select CardNum, Name from Patrons";

          // Execute the command and cycle through the DataReader object
          using (MySqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              Console.WriteLine(reader["CardNum"] + " " + reader["Name"]);
            }
          }

        }
        catch (Exception e)
        {
          Console.WriteLine(e.Message);
        }
      }
    }


    public static void AllPhones()
    {
      // Connect to the DB
      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        try
        {
          // Open a connection
          conn.Open();

          // Create a command
          MySqlCommand command = conn.CreateCommand();
          command.CommandText = "select CardNum, Phone from Phones";

          // Execute the command and cycle through the DataReader object
          using (MySqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              Console.WriteLine(reader["CardNum"] + " " + reader["Phone"]);
            }
          }
        }
        catch (Exception e)
        {
          Console.WriteLine(e.Message);
        }
      }
    }


    public static void PatronsPhones()
    {
      // Connect to the DB
      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        try
        {
          // Open a connection
          conn.Open();

          // Create a command
          MySqlCommand command = conn.CreateCommand();
          command.CommandText = "select Name, Phone from Patrons join Phones on Patrons.CardNum = Phones.CardNum";

          // Execute the command and cycle through the DataReader object
          using (MySqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              Console.WriteLine(reader["Name"] + " " + reader["Phone"]);
            }
          }
        }
        catch (Exception e)
        {
          Console.WriteLine(e.Message);
        }
      }
    }
  }
}