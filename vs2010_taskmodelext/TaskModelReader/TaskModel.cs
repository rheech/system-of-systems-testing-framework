﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TaskModelReader
{
    class TaskModel : XmlParser
    {
        public TaskModel(string file)
            : base(file, "/Diagram/node")
        {

        }

        public string test()
        {
            XmlNodeList nodes = RootNodes;

            /*foreach (XmlNode node in nodes)
            {
                foreach (XmlNode n in node.SelectNodes("node"))
                {
                    if (n.Attributes["type"].InnerText == "goal" && n.Attributes["name"].InnerText == "FindAvailableHospital")
                    {
                        return node.Attributes["name"].InnerText;
                    }
                }

                //temp = node.SelectSingleNode("attribute").Attributes["name"].InnerText;
                //temp = node.SelectSingleNode("arrow").InnerText;
            }

            return "[Find]";*/

            //return recFindNode(nodes[0]).Attributes["type"].InnerText;
            bool bFound = false;

            recFindNode(nodes[0], ref bFound);

            return bFound.ToString();
        }

        private void recFindNode(XmlNode xmlNode, ref bool bFound)
        {
            Console.WriteLine(xmlNode.Attributes["name"].InnerText);

            if (xmlNode.HasChildNodes)
            {
                /*if (myNode.Left != null)
                {
                    recTraverseInorder(myNode.Left, ref myString);
                }

                myString = String.Format("{0}\n{1}", myString, myNode.Key);

                if (myNode.Right != null)
                {
                    recTraverseInorder(myNode.Right, ref myString);
                }*/

                foreach (XmlNode x in xmlNode.ChildNodes)
                {
                    recFindNode(x, ref bFound);
                }
            }

            if (xmlNode.Attributes["name"].InnerText == "FindAvailableHospital")
            {
                bFound = true;
            }
        }

        public string GetAgentFromRole(string roleName)
        {
            XmlNodeList nodes = RootNodes;

            foreach (XmlNode node in nodes)
            {
                foreach (XmlNode n in node.SelectNodes("attribute"))
                {
                    if (n.Attributes["type"].InnerText == "plays" && n.Attributes["name"].InnerText == roleName)
                    {
                        return node.Attributes["name"].InnerText;
                    }
                }

                //temp = node.SelectSingleNode("attribute").Attributes["name"].InnerText;
                //temp = node.SelectSingleNode("arrow").InnerText;
            }

            return String.Format("[{0}]", roleName);
        }
    }
}
