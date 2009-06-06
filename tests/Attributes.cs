using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CookComputing;

[TestFixture]
public class Attributes
{
  public static partial class Methods
  {
    public static object[] OptionName(
      [Option(Long="bool", Short='b')] bool optbool = false)
    {
      return new object[] { optbool };
    }

    public static object[] LongNameConflict(bool abool = false,
      [Option(Long = "abool")] bool bbool = false)
    {
      return new object[] { abool, bbool };
    }

    public static object[] ShortNameConflict(bool bbool = false,
      [Option(Short = 'b')] bool cbool = false)
    {
      return new object[] { bbool, cbool };
    }
  }

  [Test]
  public void RenamedShortOpt()
  {
    string[] args = new string[] { "-b" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OptionName"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(true, ret[0]);
  }

  [Test]
  public void RenamedLongOpt()
  {
    string[] args = new string[] { "--bool" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OptionName"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(true, ret[0]);
  }

  [Test]
  [ExpectedException(typeof(Exception))]
  public void ShortNameConflict()
  {
    string[] args = new string[] { "-b" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("ShortNameConflict"), args) as object[];
  }

  [Test]
  [ExpectedException(typeof(Exception))]
  public void LongNameConflict()
  {
    string[] args = new string[] { "-b" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("LongNameConflict"), args) as object[];
  }
}
