using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SerializerTests.Interfaces;
using SerializerTests.Nodes;

namespace SerializerTests.Implementations
{
    //Specify your class\file name and complete implementation.
    public class JohnSmithSerializer : IListSerializer
    {
        //the constructor with no parameters is required and no other constructors can be used.
        public JohnSmithSerializer()
        {
            //...
        }

        public Task<ListNode> DeepCopy(ListNode head)
        {
            return new Task<ListNode>(() =>
            {
                if(head == null)
                {
                    return null;
                }

                //Dictionary with key - node in original list, and value - node in copy list
                var nodeDict = new Dictionary<ListNode, ListNode>();

                var node = head.Next;
                head = DeepCopyNode(nodeDict, head);

                while (node != null)
                {
                    DeepCopyNode(nodeDict, node);
                    node = node.Next;
                }

                return head;
            });
        }

        public Task<ListNode> Deserialize(Stream s)
        {
            return new Task<ListNode>(() =>
            {
                var nodeDict = new Dictionary<int, ListNode>();
                var head = DeserializeNode(s, nodeDict);

                while(s.Position < s.Length)
                {
                    DeserializeNode(s, nodeDict);
                }

                return head;
            });
        }

        

        public Task Serialize(ListNode head, Stream s)
        {
            return new Task(() =>
            {
                if(head == null)
                {
                    return;
                }

                var node = head;
                var nodeDict = new Dictionary<ListNode, int>(); 
                //we can create some specific dict realization for this task to make Serialize for unlimited list.

                while (node != null)
                {
                    var data = node.Data;
                    var nodeNumber = TryAddToNodeDict(nodeDict, node);
                    var previousNumber = TryAddToNodeDict(nodeDict, node.Previous);
                    var nextNumber = TryAddToNodeDict(nodeDict, node.Next);
                    var randomNumber = TryAddToNodeDict(nodeDict, node.Random);

                    SerializeNode(s, data, nodeNumber, previousNumber, nextNumber, randomNumber);

                    node = node.Next;
                }
            });
        }

        /// <summary>
        /// Creates copy of existing node.
        /// </summary>
        /// <param name="nodes">Dicationary of copied nodes, where key is original node, and value is copy</param>
        /// <param name="node">Node which will be copied</param>
        /// <returns>Ref to new node</returns>
        private static ListNode DeepCopyNode(Dictionary<ListNode, ListNode> nodeDict, ListNode node)
        {
            var copiedNode = TryGetFromNodeDict(nodeDict, node);

            copiedNode.Previous = TryGetFromNodeDict(nodeDict, node.Previous);
            copiedNode.Next = TryGetFromNodeDict(nodeDict, node.Next);
            copiedNode.Random = TryGetFromNodeDict(nodeDict, node.Random);
            copiedNode.Data = node.Data; //unmutable

            return copiedNode;
        }

        
        /// <summary>
        /// Deserializes node with fields in format: 
        /// number - 4 bytes, previous - 4 bytes, next - 4 bytes, random - 4 bytes, data size - 4 bytes, data - 4 bytes per symb (UTF-8).
        /// </summary>
        /// <param name="s">Input stream</param>
        /// <param name="nodeDict">Input node dictionary</param>
        /// <returns>One deserialized filled node</returns>
        private static ListNode DeserializeNode(Stream s, Dictionary<int, ListNode> nodeDict)
        {
            ReadNodeInfoFromStream(s, out var number, out var previous, out var next, out var random, out var data);

            var node = TryGetFromNodeDict(nodeDict, number);
            if (node != null)
            {
                node.Next = TryGetFromNodeDict(nodeDict, next);
                node.Previous = TryGetFromNodeDict(nodeDict, previous);
                node.Random = TryGetFromNodeDict(nodeDict, random);
                node.Data = data;
            }

            return node;
        }

        /// <summary>
        /// Reads node information from input stream.
        /// </summary>
        /// <param name="s">Some input stream</param>
        /// <param name="number">Current node number</param>
        /// <param name="previous">Previous node number</param>
        /// <param name="next">Next node number</param>
        /// <param name="random">Random node number</param>
        /// <param name="data">Payload of node</param>
        private static void ReadNodeInfoFromStream(Stream s, out int number, out int previous, out int next, out int random, out string data)
        {
            byte[] buffer = new byte[4];

            number = ReadInt();
            previous = ReadInt();
            next = ReadInt();
            random = ReadInt();
            var dataLength = ReadInt();

            byte[] dataBuffer = new byte[dataLength];
            s.Read(dataBuffer);

            data = Encoding.UTF8.GetString(dataBuffer);

            int ReadInt()
            {
                s.Read(buffer);
                int integer32 = BitConverter.ToInt32(buffer);
                return integer32;
            }
        }

        /// <summary>
        /// Serializes node with fields in format: 
        /// number - 4 bytes, previous - 4 bytes, next - 4 bytes, random - 4 bytes, data size - 4 bytes, data - 4 bytes per symb (UTF-8).
        /// </summary>
        /// <param name="s">Output stream</param>
        /// <param name="data">Payload</param>
        /// <param name="number">Number of current node</param>
        /// <param name="previous">Ref to the previous node in the list, 0 for head</param>
        /// <param name="next">Ref to the next node in the list, 0 for tail</param>
        /// <param name="random">Ref to the random node in the list, could be 0</param>
        private static void SerializeNode(Stream s, string data, int number, int previous, int next, int random)
        {
            //here we don't configure encoding, but we can.
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var dataLength = dataBytes.Length;

            s.Write(BitConverter.GetBytes(number));
            s.Write(BitConverter.GetBytes(previous));
            s.Write(BitConverter.GetBytes(next));
            s.Write(BitConverter.GetBytes(random));
            s.Write(BitConverter.GetBytes(dataLength));
            s.Write(dataBytes);
        }

        /// <summary>
        /// Tries to get node from node set or create new empty node.
        /// </summary>
        /// <param name="nodeDict">Ref to node dictionary of copied nodes, where key is original node, and value is copy</param>
        /// <param name="node">Some </param>
        /// <returns>node in set or null if input node is null</returns>
        private static ListNode TryGetFromNodeDict(Dictionary<ListNode, ListNode> nodeDict, ListNode node)
        {
            if (node == null)
            {
                return null;
            }

            if (!nodeDict.TryGetValue(node, out var copiedNode))
            {
                copiedNode = new ListNode();
                nodeDict[node] = copiedNode;
            }

            return copiedNode;
        }


        /// <summary>
        /// Tries to get node from node dictionary or create new empty node.
        /// </summary>
        /// <param name="nodeDict">Ref to node dictionary, where key is number of node, and value is ref to node</param>
        /// <param name="number">Some node number you want to get</param>
        /// <returns>node in dictionary or null if number is -1</returns>
        private static ListNode TryGetFromNodeDict(Dictionary<int, ListNode> nodeDict, int number)
        {
            if(number == 0)
            {
                return null;
            }


            if(!nodeDict.TryGetValue(number, out var node))
            {
                node = new ListNode();
                nodeDict.Add(number, node);
            }

            return node;
        }

        /// <summary>
        /// Tries to add new node to node dictionary.
        /// </summary>
        /// <param name="nodeDict">Ref to node dictionary, where key is ref to node, and value is number of node</param>
        /// <param name="node">Some node you want to add</param>
        /// <returns>number of node in dictionary or -1 if node is null</returns>
        private static int TryAddToNodeDict(Dictionary<ListNode, int> nodeDict, ListNode node)
        {
            if (node == null)
            {
                return 0;
            }

            nodeDict.TryAdd(node, nodeDict.Count + 1);

            return nodeDict[node];
        }
    }
}
