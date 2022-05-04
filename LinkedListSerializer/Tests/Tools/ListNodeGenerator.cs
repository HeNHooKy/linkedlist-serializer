using SerializerTests.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListSerializer.Tests.Tools
{
    /// <summary>
    /// Contains methods for generating different types of list node.
    /// </summary>
    public static class ListNodeGenerator
    {
        private static readonly Random random = new();

        /// <summary>
        /// Creates list of nodes with Random ref to random element of list.
        /// </summary>
        /// <param name="countOfElements">Count of elements in result list</param>
        /// <returns>Ref to head of list</returns>
        public static ListNode GenerateRandomList(int countOfElements)
        {
            var list = GenerateUnlinkedList(countOfElements);

            for (int number = 0; number < countOfElements; number++)
            {
                var node = list[number];

                node.Previous = number > 0 ? list[number - 1] : null;
                node.Next = number < countOfElements - 1 ? list[number + 1] : null;
                node.Random = list[Random(0, countOfElements, number)];
            }

            return list[0];
        }

        /// <summary>
        /// Generates list of unlinked nodes.
        /// </summary>
        /// <param name="countOfElements">count of node in list</param>
        /// <returns>List of nodes</returns>
        private static List<ListNode> GenerateUnlinkedList(int countOfElements)
        {
            var list = new List<ListNode>();

            for (int number = 0; number < countOfElements; number++)
            {
                list.Add(new ListNode
                {
                    Data = number.ToString()
                });
            }

            return list;
        }

        /// <summary>
        /// Generates radnom number from startRange to endRange exclude some number.
        /// </summary>
        /// <returns>Random integer number in range [startRange, endRange)</returns>
        private static int Random(int startRange, int endRange, int exclude)
        {
            var range = Enumerable.Range(startRange, endRange).Where(i => i != exclude);
            int index = random.Next(startRange, endRange - 1);
            return range.ElementAt(index);
        }

        /// <summary>
        /// Creates list of nodes with Random ref to next element.
        /// </summary>
        /// <param name="countOfElements">Count of elements in result list</param>
        /// <returns>Ref to head of list</returns>
        public static ListNode GenerateList(int countOfElements)
        {
            ListNode head = new ListNode();
            head.Data = "first";

            var node = head;
            for(int number = 0; number < countOfElements - 1; number++)
            {
                var newNode = new ListNode
                {
                    Data = number.ToString(),
                    Previous = node
                };

                node.Next = newNode;
                node.Random = newNode;

                node = node.Next;
            }

            return head;
        }
    }
}
