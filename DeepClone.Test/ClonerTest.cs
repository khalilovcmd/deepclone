using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DeepClone.Test
{
    [TestClass]
    public class ClonerTest
    {
        public class TestClass
        {
            public int a { set; get; }
            public int b { set; get; }
            public String c { set; get; }
            public DateTime time { set; get; }
            public TestClass testClass { set; get; }
            public TestStruct testStruct { set; get; }
            public List<TestSubClass> testSubClasses { set; get; }
            public List<TestStruct> testStructs { set; get; }
            public Dictionary<int, TestSubClass> dictionary { set; get; }
            public static String staticValue = "asdasd";
        }

        public class TestSubClass
        {
            public String a { set; get; }
            public ConsoleColor b { set; get; }
            public ConsoleColor c { set; get; }
        }

        public struct TestStruct
        {
            public int a { set; get; }
            public int b { set; get; }
        }

        [TestMethod]
        public void Can_CloneStruct()
        {
        }
    }
}
