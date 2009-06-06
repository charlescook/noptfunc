using System;
using System.IO;
using CookComputing;

class Program
{
  static void Main(string[] args)
  {
    try
    {
      NOptFunc.Run(typeof(Program).GetMethod("Run"), args);
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine(ex.Message);
    }
  }

  public static void Run(string filename, bool verbose = false)
  {
    string s = File.ReadAllText(filename);
    if (verbose)
      Console.WriteLine("Processing {0} bytes...", s.Length);
    Console.WriteLine(s.ToUpper());
  }
}