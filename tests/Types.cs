using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CookComputing;

[TestFixture]
public class Types
{
  public enum TestEnum { one, two, three }

  public static partial class Methods
  {
    public static object[] StringType(string arg)
    {
      return new object[] { arg };
    }

    public static object[] IntType(int arg)
    {
      return new object[] { arg };
    }

    public static object[] NullableIntType(Nullable<int> arg)
    {
      return new object[] { arg };
    }

    public static object[] NullableIntOption(Nullable<int> arg = null)
    {
      return new object[] { arg };
    }

    public static object[] LongType(long arg)
    {
      return new object[] { arg };
    }

    public static object[] NullableLongType(Nullable<long> arg)
    {
      return new object[] { arg };
    }

    public static object[] NullableLongOption(Nullable<long> arg = null)
    {
      return new object[] { arg };
    }

    public static object[] DoubleType(double arg)
    {
      return new object[] { arg };
    }

    public static object[] NullableDoubleType(Nullable<double> arg)
    {
      return new object[] { arg };
    }

    public static object[] NullableDoubleOption(Nullable<double> arg = null)
    {
      return new object[] { arg };
    }

    public static object[] EnumType(TestEnum arg)
    {
      return new object[] { arg };
    }

    public static object[] NullableEnumType(Nullable<TestEnum> arg)
    {
      return new object[] { arg };
    }

    public static object[] NullableEnumOption(Nullable<TestEnum> arg = null)
    {
      return new object[] { arg };
    }
  }

  //string, int, long, choice, float and complex

  [Test]
  public void StringType()
  {
    string[] args = new string[] { "teststring" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("StringType"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual("teststring", ret[0]);
  }

  [Test]
  public void IntType()
  {
    string[] args = new string[] { "1234" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("IntType"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(1234, ret[0]);
  }

  [Test]
  public void NullableIntType()
  {
    string[] args = new string[] { "1234" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("NullableIntType"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(1234, ret[0]);
  }

  [Test]
  public void NullableIntTypeOption()
  {
    string[] args = new string[0];
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("NullableIntOption"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(null, ret[0]);
  }

  [Test]
  public void LongType()
  {
    string[] args = new string[] { "123456789012345" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("LongType"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(123456789012345, ret[0]);
  }

  [Test]
  public void NullableLongType()
  {
    string[] args = new string[] { "123456789012345" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("NullableLongType"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(123456789012345, ret[0]);
  }

  [Test]
  public void NullableLongTypeOption()
  {
    string[] args = new string[0];
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("NullableLongOption"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(null, ret[0]);
  }

  [Test]
  public void DoubleType()
  {
    string[] args = new string[] { "1.0" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("DoubleType"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(1.0, ret[0]);
  }

  [Test]
  public void NullableDoubleType()
  {
    string[] args = new string[] { "1.0" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("NullableDoubleType"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(1.0, ret[0]);
  }

  [Test]
  public void NullableDoubleTypeOption()
  {
    string[] args = new string[0];
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("NullableDoubleOption"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(null, ret[0]);
  }

  [Test]
  public void EnumType()
  {
    string[] args = new string[] { "one" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("EnumType"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(TestEnum.one, ret[0]);
  }

  [Test]
  public void NullableEnumType()
  {
    string[] args = new string[] { "one" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("NullableEnumType"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(TestEnum.one, ret[0]);
  }

  [Test]
  public void NullableEnumTypeOption()
  {
    string[] args = new string[0];
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("NullableEnumOption"), args) as object[];
    Assert.AreEqual(1, ret.Length);
    Assert.AreEqual(null, ret[0]);
  }

}
