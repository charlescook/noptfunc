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

Positional and Option Arguments
-------------------------------

Arguments passed on a command can be divided into the
arguments the program always requires and those that 
optionally alter the way the program runs.

The non-optional parameters in the function passed to
NOptFunc are treated as required arguments, known as 
positional arguments. NOptFunc.Run verifies that the
correct number of positional arguments have been supplied. 

The optional parameters in the function passed to NOptFunc
are treated as option arguments, the default value 
specified in the function declaration being the default
value for the optional argument. 

The following types are supported:
- bool
- string
- int
- long
- double
- enum

Value types, those other than string in this list, can be 
specified as Nullable<T> to allow the default value of the 
type to be a valid value of the option.

Consider the following:

  public static void Run(string s, string apiKey = null,
    string geocoder="google", bool list_geocoders = false)
  {
    // ...
  }

's' is a required positional argument. api_key, geocoder 
and list_geocoders are all options, with defaults provided. 

The command line options are derived from the parameter 
names like this:

    Options:
      -h, --help            shows this help message and exit
      -l, --list-geocoders
      -a API_KEY, --api-key=API_KEY
      -g GEOCODER, --geocoder=GEOCODER

Note that the boolean --list-geocoders is a flag, not an 
option that requires a value on the command line.

The short option is derived from the first letter of the 
parameter. If that character is already in use, the second 
character will be used and so on.

The long option is the full name of the parameter with 
underscores converted to hyphens.

The Option attribute is used to customize the long and
short names, for example:

  public static void Run(
    [Option(Long="filename", Short="f")] string s)
  {
    // ...
  }

This results in a short option of -f and a long option 
of --filename.

A variable number of positional parameters can be specified
by using an array type, for example:

  public static void Run(string[] filenames)
  {
    // ...
  }

There can only be one array type positional argument and
any other positional arguments must precede it.

Usage
-----

NOptFunc is supplied as a single source file which is
be added to a project.  

NOptFunc requires a C# 4.0 compiler.

TODO
----

* Improve error handling using custom exceptions
* Support for option descriptions
* Support for --help
* Support for --version
* Support for other argument types (DateTime)
* Sub-commands
