using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestOracleGenerator.Xml
{
    public class AgentModel : XmlParser
    {
        public AgentModel(string file) : base(file, "/Diagram/Agent")
        {

        }

        public string GetAgentFromRole(string roleName)
        {
            XmlNodeList nodes = RootNodes;

            foreach (XmlNode node in nodes)
            {
                foreach (XmlNode n in node.SelectNodes("Role"))
                {
                    if (n.Attributes["name"].InnerText == roleName)
                    {
                        //Console.WriteLine(n.ParentNode.Attributes["name"].InnerText);
                        return n.ParentNode.Attributes["name"].InnerText;
                    }
                }

                //temp = node.SelectSingleNode("attribute").Attributes["name"].InnerText;
                //temp = node.SelectSingleNode("arrow").InnerText;
            }

            return String.Format("[{0}]", roleName);
        }
    }
}
