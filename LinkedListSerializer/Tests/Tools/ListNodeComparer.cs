using SerializerTests.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListSerializer.Tests.Tools
{
    /// <summary>
    /// Contains methods for comparing two lists or two streams
    /// </summary>
    public static class ListNodeComparer
    {
        /// <summary>
        /// Compares two listNode lists.
        /// </summary>
        /// <returns>true if first has the same structure as second and all Data fields equals, false in other case</returns>
        public static bool Compare(ListNode first, ListNode second)
        {
            while(first != null || second != null)
            {
                if(first == null || second == null)
                {
                    return false;
                }

                if(first.Data != second.Data)
                {
                    return false;
                }

                if(first.Previous?.Data != second.Previous?.Data)
                {
                    return false;
                }

                if(first.Next?.Data != second.Next?.Data)
                {
                    return false;
                }

                if(first.Random?.Data != second.Random?.Data)
                {
                    return false;
                }

                first = first.Next;
                second = second.Next;
            }

            return true;
        }
    }
}
