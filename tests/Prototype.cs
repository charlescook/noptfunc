using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CookComputing;

[TestFixture] 
public class Prototype 
{
  public static partial class Methods
  {
    public static object[] Prototype(string str, bool optbool = false)
    {
      return new object[] { str, optbool };  
    }
  }

  [Test]   
  public void Test1()
  {
    string[] args = new string[] { "teststring", "-o" };
    var ret = NOptFunc.Run(typeof(Methods).GetMethod("Prototype"), args) as object[];
    Assert.AreEqual(2, ret.Length);
    Assert.AreEqual("teststring", ret[0]);
    Assert.AreEqual(true, ret[1]);
  }
}
