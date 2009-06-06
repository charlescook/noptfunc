using System;
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
    Console.WriteLine("filename: {0}  verbose: {1}", filename, verbose);
  }
}