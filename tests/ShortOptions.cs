using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CookComputing;

[TestFixture]
public class ShortOptions
{
  public static partial class Methods
  {
    public static object[] OneString(string str = "default")
    {
      return new object[] { str };
    }

    public static object[] OneStringNull(string str = null)
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

    public static object[] TwoStringTwoBool(string first = null, string second = null, 
      bool aboolopt=false, bool bboolopt=false)
    {
      return new object[] { first, second, aboolopt, bboolopt };
    }

    public static object[] OneNullableInt(Nullable<int> nint = null)
    {
      return new object[] { nint };
    }

    public static object[] MultipleMatches(bool a = false, bool b = false, bool abc = false)
    {
      return new object[] { a, b, abc };
    }

    public static object[] MultipleAllInUse(bool a = false, bool b = false, bool ab = false)
    {
      return new object[] { a, b, ab };
    }
  }

  [Test]
  public void OneShortStringDefault()
  {
    var args = new string[0];
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneString"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual("default", ret[0]);
  }

  [Test]
  public void OneStringNullDefault()
  {
    var args = new string[0];
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneStringNull"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(null, ret[0]);
  }

  [Test]
  public void OneShortString()
  {
    var args = new string[] { "-s", "teststring" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneString"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual("teststring", ret[0]);
  }

  [Test]
  public void OneShortStringTogether()
  {
    var args = new string[] { "-steststring" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneString"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual("teststring", ret[0]);
  }

  [Test]
  public void OneShortBoolDefault()
  {
    var args = new string[0];
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneBoolFalse"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(false, ret[0]);
  }

  [Test]
  public void OneShortBool()
  {
    var args = new string[] { "-b" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneBoolFalse"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(true, ret[0]);
  }

  [Test]
  [ExpectedException(typeof(Exception))]
  public void OneShortBoolTogether()
  {
    var args = new string[] { "-btrue" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneBoolFalse"), args) as object[];
  }

  [Test]
  public void OneShortBoolTrueDefault()
  {
    var args = new string[0];
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneBoolTrue"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(true, ret[0]);
  }

  [Test]
  public void OneShortBoolTrue()
  {
    var args = new string[] { "-b" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("OneBoolTrue"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(false, ret[0]);
  }

  [Test]
  public void TwoBool()
  {
    var args = new string[] { "-ab" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("TwoStringTwoBool"), args) as object[];
    Assert.AreEqual(4, ret.Length);
    Assert.AreEqual(null, ret[0]);
    Assert.AreEqual(null, ret[1]);
    Assert.AreEqual(true, ret[2]);
    Assert.AreEqual(true, ret[3]);
  }

  [Test]
  public void BoolString()
  {
    var args = new string[] { "-af", "teststring" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("TwoStringTwoBool"), args) as object[];
    Assert.AreEqual(4, ret.Length);
    Assert.AreEqual("teststring", ret[0]);
    Assert.AreEqual(null, ret[1]);
    Assert.AreEqual(true, ret[2]);
    Assert.AreEqual(false, ret[3]);
  }

  [Test]
  public void BoolStringTogether()
  {
    var args = new string[] { "-afteststring" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("TwoStringTwoBool"), args) as object[];
    Assert.AreEqual(4, ret.Length);
    Assert.AreEqual("teststring", ret[0]);
    Assert.AreEqual(null, ret[1]);
    Assert.AreEqual(true, ret[2]);
    Assert.AreEqual(false, ret[3]);
  }

  [Test]
  public void MultipleMatches()
  {
    var args = new string[] { "-c" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("MultipleMatches"), args) as object[];
    Assert.AreEqual(3, ret.Length);
    Assert.AreEqual(false, ret[0]);
    Assert.AreEqual(false, ret[1]);
    Assert.AreEqual(true, ret[2]);
  }

  [Test]
  [ExpectedException(typeof(Exception))]
  public void MultipleAllInUse()
  {
    var args = new string[] { "-c" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("MultipleAllInUse"), args) as object[];
  }
}
