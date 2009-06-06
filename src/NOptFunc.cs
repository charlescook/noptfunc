// Copyright 2009 (c) Charles Cook <charlescook@cookcomputing.com>
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CookComputing
{
  class NOptFunc
  {
    //-------------------------------------------------------------------------/
    public static object Run(MethodInfo mi, string[] args)
    {
      return Run(null, mi, args);
    }

    //-------------------------------------------------------------------------/
    public static object Run(object instance, MethodInfo mi, string[] args)
    {
      var result = AnalyzeParameters(mi);
      ParseArgs(args, result.Item1, result.Item2);
      object[] parameters = result.Item1.Select(v => v.Value).Concat(
        result.Item2.Select(v => v.Value)).ToArray();
      return mi.Invoke(instance, parameters);
    }

    //-------------------------------------------------------------------------/
    private static Tuple<List<Positional>, List<Option>> AnalyzeParameters(
      MethodInfo mi)
    {
      var positionals = new List<Positional>();
      var options = new List<Option>();
      var pis = mi.GetParameters();
      bool gotPositionalArray = false;
      foreach (var pi in pis)
      {
        if (pi.RawDefaultValue is DBNull)
        {
          if (pi.ParameterType.IsArray)
            gotPositionalArray = true;
          else if (gotPositionalArray)
            throw new Exception("positional after positional array");
          positionals.Add(new Positional() { Type = pi.ParameterType });
        }
        else
        {
          var option = new Option();
          option.ArgType = pi.ParameterType;
          option.Default = pi.RawDefaultValue;
          option.Value = option.Default;
          string longName = pi.Name.Replace('_', '-');
          char shortName = MatchShortName(pi.Name, options);
          OptionAttribute attr = (OptionAttribute)Attribute.GetCustomAttribute(
            pi, typeof(OptionAttribute));
          if (attr != null)
          {
            if (attr.Long != null)
            {
              if (options.Select(opt => opt.Long).Contains(attr.Long))
                throw new Exception("attributed long name in use");
              else
                option.Long = attr.Long;
            }
            else
              option.Long = longName;
            if (attr.Short != default(char))
            {
              if (options.Select(opt => opt.Short).Contains(attr.Short))
                throw new Exception("attributed short name in use");
              else
                option.Short = attr.Short;
            }
            else
              option.Short = shortName;
          }
          else
          {
            option.Long = longName;
            option.Short = shortName;
          }
          if (shortName == default(char))
            throw new Exception("unable to find suitable short name");
          options.Add(option);
        }
      }
      return new Tuple<List<Positional>, List<Option>>(positionals, options);
    }

    //-------------------------------------------------------------------------/
    private static char MatchShortName(string name, List<Option> options)
    {
      return name.Except(options.Select(opt => opt.Short)).FirstOrDefault();
    }

    //-------------------------------------------------------------------------/
    private static void ParseArgs(string[] args, List<Positional> positionals,
      List<Option> options)
    {
      var argEnumerator = args.AsEnumerable().GetEnumerator();
      var posEnumerator = positionals.GetEnumerator() as IEnumerator<Positional>;
      posEnumerator.MoveNext();
      while (argEnumerator.MoveNext() && argEnumerator.Current != null)
      {
        string arg = argEnumerator.Current;
        if (arg.StartsWith("--"))
        {
          if (arg.Length == 2)
            throw new Exception("Invalid option");
          ProcessLongOption(arg.Substring(2), argEnumerator, options);
        }
        else if (arg.StartsWith("-"))
        {
          if (arg.Length == 1)
            throw new Exception("Invalid option");
          ProcessShortOption(arg.Substring(1), argEnumerator, options);
        }
        else
          ProcessPositional(arg, posEnumerator);
      }
      if (posEnumerator.Current != null && !posEnumerator.Current.Type.IsArray)
        throw new Exception("missing positional argument");
      Positional last;
      if (positionals.Count > 0 && (last = positionals.Last()).Value is ArrayList)
      {
        Type elemType = last.Type.GetElementType();
        last.Value = (last.Value as ArrayList).ToArray(elemType);
      }
    }

    //-------------------------------------------------------------------------/
    private static void ProcessLongOption(string arg,
      IEnumerator<string> argEnumerator, List<Option> options)
    {
      int idx = arg.IndexOf("=");
      string name = idx > 0 ? arg.Substring(0, idx) : arg;
      var optional = options.Where(opt => opt.Long == name).FirstOrDefault();
      if (optional == null)
        throw new Exception("invalid long option");
      if (idx >= 0)
      {
        if (optional.ArgType == typeof(bool))
          throw new Exception("long option boolean has a value");
        optional.Value = arg.Substring(idx + 1);
      }
      else
      {
        if (optional.ArgType == typeof(bool))
          optional.Value = !(bool)optional.Default;
        else
        {
          argEnumerator.MoveNext();
          if (argEnumerator.Current == null)
            throw new Exception("missing value");
          optional.Value = Convert(argEnumerator.Current, optional.ArgType);
        }
      }
    }

    //-------------------------------------------------------------------------/
    private static void ProcessShortOption(string arg,
      IEnumerator<string> argEnumerator, List<Option> options)
    {
      var optional = options.Where(
        opt => opt.Short == arg[0]).FirstOrDefault();
      if (optional == null)
        throw new Exception("invalid short option");
      if (optional.ArgType == typeof(bool))
      {
        optional.Value = !(bool)optional.Default;
        if (arg.Length > 1)
          ProcessShortOption(arg.Substring(1), argEnumerator, options);
        return;
      }
      arg = arg.Substring(1);
      if (arg != "")
        optional.Value = Convert(arg, optional.ArgType);
      else
      {
        argEnumerator.MoveNext();
        if (argEnumerator.Current == null)
          throw new Exception("missing value");
        optional.Value = Convert(argEnumerator.Current, optional.ArgType);
      }
    }

    //-------------------------------------------------------------------------/
    private static void ProcessPositional(string arg,
      IEnumerator<Positional> posEnumerator)
    {
      Positional positional = posEnumerator.Current;
      if (positional == null)
        throw new Exception("unexpected positional");
      if (positional.Type.IsArray)
      {
        Type elemType = positional.Type.GetElementType();
        object value = Convert(arg, elemType);
        if (positional.Value == null)
          positional.Value = new ArrayList();
        (positional.Value as ArrayList).Add(value);
      }
      else
      {
        positional.Value = Convert(arg, positional.Type);
        posEnumerator.MoveNext();
      }
    }

    //-------------------------------------------------------------------------/
    private static object Convert(string arg, Type type)
    {
      if (type.IsGenericType
        && type.GetGenericTypeDefinition() == typeof(Nullable<>))
      {
        Type nullableType = type.GetGenericArguments()[0];
        object obj = Convert(arg, nullableType);
        object ret = Activator.CreateInstance(type, new object[] { obj });
        return ret;
      }
      else if (type.IsEnum)
      {
        object ret = Enum.Parse(type, arg);
        return ret;
      }
      else
      {
        object ret = System.Convert.ChangeType(arg, type);
        return ret;
      }
    }

    //-------------------------------------------------------------------------/
    private class Positional
    {
      public Type Type { get; set; }
      public object Value { get; set; }
    }

    //-------------------------------------------------------------------------/
    private class Option
    {
      public Type ArgType { get; set; }
      public char Short { get; set; }
      public string Long { get; set; }
      public object Default { get; set; }
      public object Value { get; set; }
    }

    //-------------------------------------------------------------------------/
  }

  [AttributeUsage(AttributeTargets.Parameter)]
  public class OptionAttribute : Attribute
  {
    public OptionAttribute() { }
    public string Description { get; set; }
    public string Long { get; set; }
    public char Short { get; set; }
  }
}