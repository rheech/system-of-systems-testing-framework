// CISK351: Program 4 - Binary Tree & Recursion
// BinaryTree.cs, version 1.1
// Author: Cheong H Lee; March 23, 2013
//
// Contains binary tree class
//
// Maintenance Log
// v1.1 2013-04-12
//   Modified Traversing methods.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TaskModelReader
{
    class BinaryTree<T>
    {
        // Root node of the tree
        private BinaryTreeNode<T> root;
        // Number of nodes in the tree
        private int iCount;
        // Saved search log
        private string sSearchLog;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BinaryTree()
        {
            Clear();
        }

        #region Public Methods
        /// <summary>
        /// Count of the nodes in the tree
        /// </summary>
        public int Count
        {
            get
            {
                return iCount;
            }
        }

        /// <summary>
        /// Insert value in tree
        /// </summary>
        /// <param name="value">valut to add</param>
        public void Insert(T value)
        {
            if (root == null)
            {
                root = new BinaryTreeNode<T>(value);
                iCount++;
            }
            else
            {
                recInsert(root, value);
            }
        }

        /// <summary>
        /// Find value
        /// </summary>
        /// <param name="value">value to find & reference value to return</param>
        /// <returns>true if found</returns>
        public bool Find(ref T value)
        {
            sSearchLog += String.Format("Searching {0}", value);

            if (root != null)
            {
                //return recFind(root, ref value);
            }

            sSearchLog += " not-found.\n";

            return false;
        }

        /// <summary>
        /// Clear list
        /// </summary>
        public void Clear()
        {
            root = null;
            iCount = 0;
            sSearchLog = "";
        }

        /// <summary>
        /// Traverse tree in order
        /// </summary>
        /// <returns>Result in string</returns>
        public string TraverseInorder()
        {
            string myString = "";

            if (root != null)
            {
                //recTraverseInorder(root, ref myString);
            }

            return myString.Trim();
        }

        /// <summary>
        /// Traverse tree preorder
        /// </summary>
        /// <returns>Result in string</returns>
        public string TraversePreorder()
        {
            string myString = "";

            if (root != null)
            {
                //recTraversePreorder(root, ref myString);
            }

            return myString.Trim();
        }

        /// <summary>
        /// Traverse tree postorder
        /// </summary>
        /// <returns>Result in string</returns>
        public string TraversePostorder()
        {
            string myString = "";

            if (root != null)
            {
                //recTraversePostorder(root, ref myString);
            }

            return myString.Trim();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Recursive function to insert value
        /// </summary>
        /// <param name="myNode">current node position</param>
        /// <param name="value">value to insert</param>
        private void recInsert(BinaryTreeNode<T> myNode, T value)
        {
            myNode.Children.Add(new BinaryTreeNode<T>(value));
        }

        /*/// <summary>
        /// Recursive function to find value
        /// </summary>
        /// <param name="myNode">current node position</param>
        /// <param name="value">value to find</param>
        private bool recFind(BinaryTreeNode<T> myNode, ref T value)
        {
            if (myNode.Key.CompareTo(value) == 0)
            {
                sSearchLog += " found.\n";
                value = (T)myNode.Key;
                return true;
            }
            else if (myNode.Key.CompareTo(value) > 0)
            {
                if (myNode.Left == null)
                {
                    sSearchLog += " not-found.\n";
                    return false;
                }
                else
                {
                    sSearchLog += " left";
                    return recFind(myNode.Left, ref value);
                }
            }
            else
            {
                if (myNode.Right == null)
                {
                    sSearchLog += " not-found.\n";
                    return false;
                }
                else
                {
                    sSearchLog += " right";
                    return recFind(myNode.Right, ref value);
                }
            }
        }

        /// <summary>
        /// Recursive function to print list
        /// </summary>
        /// <param name="listView">listview to print nodes</param>
        /// <param name="myNode">current node position</param>
        /// <param name="level">current level of the node</param>
        /// <param name="side">Indicates Left or Right side</param>
        /// <param name="parent">Value of parent</param>
        private void recPrintList(ListView listView, BinaryTreeNode<T> myNode,
                    int level, string side, string parent)
        {
            string childLeft, childRight;
            childLeft = "";
            childRight = "";

            if (myNode.Left != null)
            {
                childLeft = myNode.Left.Key.ToString();
            }

            if (myNode.Right != null)
            {
                childRight = myNode.Right.Key.ToString();
            }

            if (myNode.Left != null)
            {
                recPrintList(listView, myNode.Left,
                    level + 1, "Left", myNode.Key.ToString());
            }

            ListViewItem lvItem = new ListViewItem(myNode.Key.ToString());
            lvItem.SubItems.Add(childLeft);
            lvItem.SubItems.Add(childRight);
            lvItem.SubItems.Add(parent);
            lvItem.SubItems.Add(level.ToString());

            listView.Items.Add(lvItem);

            if (myNode.Right != null)
            {
                recPrintList(listView, myNode.Right,
                    level + 1, "Right", myNode.Key.ToString());
            }
        }

        /// <summary>
        /// Recursive function to traverse inorder
        /// </summary>
        /// <param name="myNode">current node position</param>
        /// <param name="myString">String to print values</param>
        private void recTraverseInorder(BinaryTreeNode<T> myNode, ref String myString)
        {
            if (myNode.Left != null)
            {
                recTraverseInorder(myNode.Left, ref myString);
            }

            myString = String.Format("{0}\n{1}", myString, myNode.Key);

            if (myNode.Right != null)
            {
                recTraverseInorder(myNode.Right, ref myString);
            }
        }

        /// <summary>
        /// Recursive function to traverse preorder
        /// </summary>
        /// <param name="myNode">current node position</param>
        /// <param name="myString">String to print values</param>
        private void recTraversePreorder(BinaryTreeNode<T> myNode, ref String myString)
        {
            myString = String.Format("{0}\n{1}", myString, myNode.Key);

            if (myNode.Left != null)
            {
                recTraversePreorder(myNode.Left, ref myString);
            }

            if (myNode.Right != null)
            {
                recTraversePreorder(myNode.Right, ref myString);
            }
        }

        /// <summary>
        /// Recursive function to traverse postorder
        /// </summary>
        /// <param name="myNode">current node position</param>
        /// <param name="myString">String to print values</param>
        private void recTraversePostorder(BinaryTreeNode<T> myNode, ref String myString)
        {
            if (myNode.Left != null)
            {
                recTraversePostorder(myNode.Left, ref myString);
            }

            if (myNode.Right != null)
            {
                recTraversePostorder(myNode.Right, ref myString);
            }

            myString = String.Format("{0}\n{1}", myString, myNode.Key);
        }*/
        #endregion
    }
}