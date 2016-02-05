using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestCaseGenerator
{
    class XmlRoleModel : XmlParser
    {
        // XML Parsing Reference: http://www.doublecloud.org/2013/08/parsing-xml-in-c-a-quick-working-sample/

        public XmlRoleModel(string file) : base(file, "/Diagram/Role")
        {

        }

        public string GetRoleFromGoal(string goalName)
        {
            XmlNodeList nodes = RootNodes;

            foreach (XmlNode node in nodes)
            {
                foreach (XmlNode n in node.SelectNodes("attribute"))
                {
                    if (n.Attributes["name"].InnerText == goalName)
                    {
                        return node.Attributes["name"].InnerText;
                    }
                }

                //temp = node.SelectSingleNode("attribute").Attributes["name"].InnerText;
                //temp = node.SelectSingleNode("arrow").InnerText;
            }

            return "";
        }

        public string[] GetCapabilityFromRole(string roleName)
        {
            XmlNodeList nodes = FindRole(roleName).ChildNodes;
            List<string> capabilities = new List<string>();

            foreach (XmlNode x in nodes)
            {
                if (x.Attributes["type"] != null && x.Attributes["type"].InnerText == "requires")
                {
                    capabilities.Add(x.Attributes["name"].InnerText);
                }

                //temp = node.SelectSingleNode("attribute").Attributes["name"].InnerText;
                //temp = node.SelectSingleNode("arrow").InnerText;
            }

            return capabilities.ToArray();
        }

        private XmlNode FindRole(string roleName)
        {
            foreach (XmlNode x in RootNodes)
            {
                if (x.Attributes["name"].InnerText == roleName)
                {
                    return x;
                }
                /*
                foreach (XmlNode y in x.SelectNodes("attribute"))
                {
                    Console.WriteLine(y.Attributes["name"].InnerText);

                    if (y.Attributes["name"].InnerText == roleName)
                    {
                        return y;
                    }
                }
                 */
            }

            return null;
        }
    }
}
