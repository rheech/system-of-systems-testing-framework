using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TaskModelReader
{
    class TreeLogicNode
    {
        public XmlNode node;
        public List<TreeLogicNode> parents;
        public List<TreeLogicNode> children;
    }
}
