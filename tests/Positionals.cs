using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CookComputing;

[TestFixture]
public class Positionals
{
  public static partial class Methods
  {
    public static object[] NoArgs()
    {
      return new object[0];
    }

    public static object[] OnePositionalString(string str)
    {
      return new object[] { str };
    }

    public static object[] TwoPositionalString(string str1, string str2)
    {
      return new object[] { str1, str2 };
    }

    public static object[] PositionalArray(string[] strs)
    {
      return new object[] { strs };
    }

    public static object[] IntPositionalArray(int number, string[] strs)
    {
      return new object[] { number, strs };
    }

    public static object[] PositionalAfterArray(string[] strs, int number)
    {
      return new object[] { strs, number };
    }
  }

  [Test]
  public void NoArgs()
  {
    var args = new string[0];
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("NoArgs"), args) as object[];
    Assert.AreEqual(0, args.Length);
  }

  [Test]
  [ExpectedException(typeof(Exception))]
  public void PositionalButNoArgs()
  {
    var args = new string[] { "teststring" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("NoArgs"), args) as object[];
    Assert.AreEqual(0, args.Length);
  }

  [Test]
  public void OnePositionalString()
  {
    var args = new string[] { "teststring" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OnePositionalString"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual("teststring", ret[0]);
  }

  [Test]
  [ExpectedException(typeof(Exception))]
  public void MissingOnePositionalString()
  {
    var args = new string[0];
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OnePositionalString"), args) as object[];
  }

  [Test]
  public void TwoPositionalString()
  {
    var args = new string[] { "teststring1", "teststring2" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("TwoPositionalString"), args) as object[];
    Assert.AreEqual(2, ret.Length);
    Assert.AreEqual("teststring1", ret[0]);
    Assert.AreEqual("teststring2", ret[1]);
  }

  [Test]
  public void PositionalArray()
  {
    var args = new string[] { "teststring1", "teststring2" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("PositionalArray"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.IsInstanceOf<string[]>(ret[0]);
    string[] retstrs = (string[])ret[0];
    Assert.AreEqual("teststring1", retstrs[0]);
    Assert.AreEqual("teststring2", retstrs[1]);
  }

  [Test]
  public void IntPositionalArray()
  {
    var args = new string[] { "1234", "teststring1", "teststring2" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("IntPositionalArray"), args) as object[];
    Assert.AreEqual(2, ret.Length);
    Assert.IsInstanceOf<int>(ret[0]);
    Assert.AreEqual(1234, ret[0]);
    Assert.IsInstanceOf<string[]>(ret[1]);
    string[] retstrs = (string[])ret[1];
    Assert.AreEqual("teststring1", retstrs[0]);
    Assert.AreEqual("teststring2", retstrs[1]);
  }

  [Test]
  [ExpectedException(typeof(Exception))]
  public void PositionalAfterArray()
  {
    var args = new string[0];
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("PositionalAfterArray"), args) as object[];
  }
}
