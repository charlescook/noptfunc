using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CookComputing;

[TestFixture]
public class LongOptions
{
  public static partial class Methods
  {
    public static object[] OneString(string str = "default")
    {
      return new object[] { str };
    }

    public static object[] OneBoolFalse(bool boolopt = false)
    {
      return new object[] { boolopt };
    }

    public static object[] OneBoolTrue(bool boolopt = true)
    {
      return new object[] { boolopt };
    }

    public static object[] ReplaceUnderscore(bool bool_opt = true)
    {
      return new object[] { bool_opt };
    }
  }

  [Test]
  public void OneLongString()
  {
    var args = new string[] { "--str", "teststring" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneString"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual("teststring", ret[0]);
  }

  [Test]
  public void OneLongStringTogether()
  {
    var args = new string[] { "--str=teststring" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneString"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual("teststring", ret[0]);
  }

  [Test]
  public void OneLongBool()
  {
    var args = new string[] { "--boolopt" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneBoolFalse"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(true, ret[0]);
  }

  [Test]
  [ExpectedException(typeof(Exception))]
  public void OneLongBoolTogether()
  {
    var args = new string[] { "--boolopt=true" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneBoolFalse"), args) as object[];
  }

  [Test]
  public void OneLongBoolTrueDefault()
  {
    var args = new string[0];
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneBoolTrue"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(true, ret[0]);
  }

  [Test]
  public void OneLongBoolTrue()
  {
    var args = new string[] { "--boolopt" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneBoolTrue"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(false, ret[0]);
  }

  [Test]
  public void ReplaceUnderscore()
  {
    var args = new string[] { "--bool-opt" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("ReplaceUnderscore"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(false, ret[0]);
  }
}
