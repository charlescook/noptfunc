NOptFunc
========

Overview
--------

NOptFunc is a command line parser for C# which uses reflection to generate a specification of command line positional and option arguments. It is inspired by Simon Willison's optfunc written in Python. 

NOptFunc uses the default parameter value feature of C# 4.0 to specify optional command line arguments. The parameters of a function are used to specify both positional and optional arguments, and at runtime the arguments supplied on the command line are parsed and then passed as parameters to the function. In this example filename specifies a required argument and verbose specifies an option. 


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
        // application code here
      }
    }
    

Run() specifies that there is one required argument &ndash filename &ndash and one option which is used on the command line as either -v or --verbose. 

NOptFunc is maintained at the noptfunc Google Code project under the MIT License. Currently it is not built into as a separate assembly. To use it simply include NOptFunc.cs in your project (plain source file here). 

Positional and Option Arguments
-------------------------------

Arguments passed on a command can be divided into the arguments the program always requires and those that optionally alter the way the program runs. 

The standard parameters in the function passed to NOptFunc are treated as required arguments, known as positional arguments. NOptFunc.Run verifies that the correct number of positional arguments have been supplied. 

The parameters with default values in the function passed to NOptFunc are treated as option arguments, the default value specified in the function declaration being the default value for the optional argument. 

The following types are supported: 

* bool
* string
* int
* long
* double

Value types, those other than string in this list, can be specified as Nullable<T> with null being the default value of the argument, which allows the default value of the type to be a valid value of the option (for example use Nullable<int> to allow the user to specify zero as the value of the option).

Consider this example: 

    public static void Run(string s, string apiKey = null,
      string geocoder="google", bool list_geocoders = false)
    {
      // ...
    }

's' is a required positional argument. api_key, geocoder and list_geocoders are all options with defaults provided. 

The command line options generated from the parameter names look like this: 

    Options:
      -h, --help            shows this help message and exit
      -l, --list-geocoders
      -a API_KEY, --api-key=API_KEY
      -g GEOCODER, --geocoder=GEOCODER

Note that the boolean --list-geocoders is a flag and does not have an associated value on the command line. 

The short option is derived from the first letter of the parameter. If that character is already in use, the second character will be used and so on. 

The long option is the full name of the parameter with underscores converted to hyphens. 

The Option attribute is used to customize the long and short names, for example: 

    public static void Run(
      [Option(Long="filename", Short='f')] string s)
    {
      // ...
    }

This results in a short option of -f and a long option of --filename. 

A variable number of positional parameters can be specified by using an array type, for example: 

    public static void Run(string[] filenames)
    {
      // ...
    }

There can only be one array type positional argument and any other positional arguments must precede it.

ToDo
----

* Improve error handling using custom exceptions
* Support for option descriptions
* Support for --help
* Support for --version
* Support for other argument types (DateTime)
* Sub-commands












