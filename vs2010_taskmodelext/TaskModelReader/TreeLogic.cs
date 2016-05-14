using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TaskModelReader
{
    class TreeLogic
    {
        TreeLogicNode _root;
        TreeLogicNode _current;

        public TreeLogic(XmlNode root)
        {
            _root = new TreeLogicNode();
            _root.node = root;
            _root.children = new List<TreeLogicNode>();
            _root.parents = null;

            _current = _root;
        }

        public void AddChild(XmlNode node)
        {
            TreeLogicNode newNode = new TreeLogicNode();
            newNode.node = node;
            newNode.children = new List<TreeLogicNode>();
            newNode.parents = new List<TreeLogicNode>();
            newNode.parents.Add(_current);

            _current.children.Add(newNode);
            _current = newNode;
        }

        public void combine(XmlNode node)
        {
        }

        public void AddSibling(XmlNode node)
        {
            TreeLogicNode newNode = new TreeLogicNode();
            newNode.node = node;
            newNode.children = new List<TreeLogicNode>();
            newNode.parents = _current.parents;

            foreach (TreeLogicNode tnode in _current.parents)
            {
                tnode.children.Add(newNode);
            }
        }
    }
}
