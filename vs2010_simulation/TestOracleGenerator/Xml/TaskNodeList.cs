using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using TestOracleGenerator.Oracle;

namespace TestOracleGenerator.Xml
{
    public class TaskNodeList : List<TaskNode>
    {
        /*private List<TaskNode> _taskNodes;

        public TaskNodeList(TaskNode[] taskNodes)
        {
            _taskNodes = new List<TaskNode>(taskNodes);
        }

        public TaskNodeList(List<TaskNode> nodeList)
        {
            _taskNodes = nodeList;
        }

        // Summary:
        //     Gets the number of nodes in the XmlNodeList.
        //
        // Returns:
        //     The number of nodes.
        public int Count
        {
            get
            {
                return _taskNodes.Count;
            }
        }

        // Summary:
        //     Retrieves a node at the given index.
        //
        // Parameters:
        //   i:
        //     Zero-based index into the list of nodes.
        //
        // Returns:
        //     The System.Xml.XmlNode in the collection. If index is greater than or equal
        //     to the number of nodes in the list, this returns null.
        public TaskNode this[int i]
        {
            get
            {
                if (_taskNodes != null)
                {
                    return _taskNodes[i];
                }

                throw new ApplicationException("Invalid index");
            }
        }

        // Summary:
        //     Provides a simple "foreach" style iteration over the collection of nodes
        //     in the XmlNodeList.
        //
        // Returns:
        //     An System.Collections.IEnumerator.
        public IEnumerator<TaskNode> GetEnumerator()
        {
            return _taskNodes.GetEnumerator();
        }
        //
        // Summary:
        //     Retrieves a node at the given index.
        //
        // Parameters:
        //   index:
        //     Zero-based index into the list of nodes.
        //
        // Returns:
        //     The System.Xml.XmlNode in the collection. If index is greater than or equal
        //     to the number of nodes in the list, this returns null.
        /*public TaskNode Item(int index)
        {
            if (_taskNodes != null)
            {
                return _taskNodes[index];
            }

            throw new ApplicationException("Invalid index");
        }* /

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }*/
    }
}
