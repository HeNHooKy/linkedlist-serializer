using LinkedListSerializer.Tests.Tools;
using SerializerTests.Implementations;
using SerializerTests.Interfaces;
using SerializerTests.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using XCWorker.Tests;
using Xunit;
using Xunit.Abstractions;

namespace LinkedListSerialization.Tests
{
    /// <summary>
    /// Serialize - deserialize blackbox test.
    /// </summary>
    public class BlackBoxTest
    {
        private readonly ITestOutputHelper output;

        public object YourImplementation { get; private set; }

        public BlackBoxTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// <summary>
        /// Generates new list of nodes, makes serialize and deserialize, and then asserts that original and copy are equals.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetTestKit))]
        public void TestCore(TestData testData)
        {
            var head = testData.TurnOnRandom
                ? ListNodeGenerator.GenerateRandomList(testData.CountOfNodes)
                : ListNodeGenerator.GenerateList(testData.CountOfNodes);

            IListSerializer serializer = testData.Serializer;

            using var memory = new MemoryStream();

            var serializeTask = serializer.Serialize(head, memory);

            var start = DateTime.Now;
            serializeTask.Start();
            serializeTask.Wait();
            var end = DateTime.Now;

            var avgTimePerNode = (end - start) / testData.CountOfNodes;

            output.WriteLine($"Serialize spent {avgTimePerNode.Ticks} ticks per node on " +
                $"average for execution with {testData.CountOfNodes} nodes.");

            memory.Position = 0;

            var deserializeTask = serializer.Deserialize(memory);

            start = DateTime.Now;
            deserializeTask.Start();
            var newHead = deserializeTask.Result;
            end = DateTime.Now;

            avgTimePerNode = (end - start) / testData.CountOfNodes;

            output.WriteLine($"Deserialize spent {avgTimePerNode.Ticks} ticks per node on " +
                $"average for execution with {testData.CountOfNodes} nodes.");

            Assert.True(ListNodeComparer.Compare(head, newHead));
        }

        public static IEnumerable<object[]> GetTestKit()
        {
            //random ref tests
            yield return SimpleRandomTest();
            yield return HardRandomTest();
            yield return KilohardRandomTest();
            yield return MegahardRandomTest();

            //null random ref tests
            yield return SimpleTest();
            yield return HardTest();
            yield return KilohardTest();
            yield return MegahardTest();
        }

        /// <summary>
        /// #1 case: list with 10 nodes with Random refs.
        /// </summary>
        private static object[] SimpleRandomTest()
        {
            return new object[]
            {
                new TestData
                {
                    Serializer = new JohnSmithSerializer(),
                    CountOfNodes = 10,
                    TurnOnRandom = true
                }
            };
        }

        /// <summary>
        /// #2 case: list with 100 nodes with Random refs.
        /// </summary>
        private static object[] HardRandomTest()
        {
            return new object[]
            {
                new TestData
                {
                    Serializer = new JohnSmithSerializer(),
                    CountOfNodes = 100,
                    TurnOnRandom = true
                }
            };
        }

        /// <summary>
        /// #3 case: list with 1.000 nodes with Random refs.
        /// </summary>
        private static object[] KilohardRandomTest()
        {
            return new object[]
            {
                new TestData
                {
                    Serializer = new JohnSmithSerializer(),
                    CountOfNodes = 1000,
                    TurnOnRandom = true
                }
            };
        }

        /// <summary>
        /// #4 case: list with 10.000 nodes with Random refs.
        /// </summary>
        private static object[] MegahardRandomTest()
        {
            return new object[]
            {
                new TestData
                {
                    Serializer = new JohnSmithSerializer(),
                    CountOfNodes = 10000,
                    TurnOnRandom = true
                }
            };
        }

        /// <summary>
        /// #5 case: list with 10.000 nodes without Random refs.
        /// </summary>
        private static object[] SimpleTest()
        {
            return new object[]
            {
                new TestData
                {
                    Serializer = new JohnSmithSerializer(),
                    CountOfNodes = 10000,
                    TurnOnRandom = false
                }
            };
        }

        /// <summary>
        /// #6 case: list with 100.000 nodes without Random refs.
        /// </summary>
        private static object[] HardTest()
        {
            return new object[]
            {
                new TestData
                {
                    Serializer = new JohnSmithSerializer(),
                    CountOfNodes = 100000,
                    TurnOnRandom = false
                }
            };
        }

        /// <summary>
        /// #7 case: list with 1.000.000 nodes without Random refs.
        /// </summary>
        private static object[] KilohardTest()
        {
            return new object[]
            {
                new TestData
                {
                    Serializer = new JohnSmithSerializer(),
                    CountOfNodes = 1000000,
                    TurnOnRandom = false
                }
            };
        }

        /// <summary>
        /// #8 case: list with 10.000.000 nodes without Random refs.
        /// </summary>
        private static object[] MegahardTest()
        {
            return new object[]
            {
                new TestData
                {
                    Serializer = new JohnSmithSerializer(),
                    CountOfNodes = 10000000,
                    TurnOnRandom = false
                }
            };
        }


        public class TestData : TestSource
        {
            public TestData([CallerMemberName] string testName = null)
                : base(testName)
            {
            }
            /// <summary>
            /// Count of nodes in list.
            /// </summary>
            public int CountOfNodes { get; set; }
            /// <summary>
            /// Random ref generation switch.
            /// </summary>
            public bool TurnOnRandom { get; set; }
            /// <summary>
            /// Serialize implementation.
            /// </summary>
            public IListSerializer Serializer {get;set;}
        }

    }
}
