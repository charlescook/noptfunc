NOptFunc
=======

Parse command line options in C# using reflection.

Inspired by Simon Willison's optfunc written in Python:

http://github.com/simonw/optfunc

NOptFunc uses the optional parameter feature of C# 4.0 to 
specify optional command line arguments. The parameters 
of a function are used to specify both positional and 
optional arguments, and at runtime the arguments supplied 
on the command line are parsed and then passed as parameters 
to the function. For example:

using System;
using CookComputing;

class Program
{
  static void Main(string[] args)
  {
    NOptFunc.Run(typeof(Program).GetMethod("Run"), args);
  }

  public static void Run(string filename, bool verbose = false)
  {
    Console.WriteLine("filename: {0}  verbose: {1}", 
      filename, verbose);
    // application code here
  }
}

For full details see:
http://www.cookcomputing.com/blog/noptfunc.html

Requirements: Visual Studio 2010.