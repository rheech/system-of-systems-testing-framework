// CISK351: Program 4 - Binary Tree & Recursion
// BinaryTreeNode.cs, version 1.0
// Author: Cheong H Lee; March 23, 2013
//
// Contains binary tree node class
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskModelReader
{
    class BinaryTreeNode<T>
    {
        // Key value of the node
        public IComparable Key;
        // children
        public List<BinaryTreeNode<T>> Children;

        /// <summary>
        /// Constructor gets value of the node
        /// </summary>
        /// <param name="value">Value of the node</param>
        public BinaryTreeNode(T value)
        {
            if (!(value is IComparable))
            {
                throw new Exception("Key must be comparable.");
            }

            Children = new List<BinaryTreeNode<T>>();

            Key = (IComparable)value;
        }
    }
}
