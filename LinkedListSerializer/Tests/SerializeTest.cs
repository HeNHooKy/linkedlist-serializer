﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XCWorker.Tests;
using Xunit;
using Xunit.Abstractions;

namespace LinkedListSerialization.Tests
{
    /// <summary>
    /// Test cases for DeepCopy methods test
    /// </summary>
    public class SerializeTest
    {
        private readonly ITestOutputHelper output;
        public SerializeTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// <summary>
        /// 
        /// </summary>
        [Theory]
        [MemberData(nameof(GetTestKit))]
        public void TestCore()
        {

        }

        public static IEnumerable<object[]> GetTestKit()
        {
            yield return SimpleTest();
        }

        /// <summary>
        /// #1 case:
        /// </summary>
        private static object[] SimpleTest()
        {
            return new object[]
            {
                new TestData
                {

                }
            };
        }

        public class TestData : TestSource
        {
            public TestData([CallerMemberName] string testName = null)
                : base(testName)
            {
            }
        }

    }
}