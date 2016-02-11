using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestCaseGenerator.Xml
{
    class XmlAgentModel : XmlParser
    {
        public XmlAgentModel(string file) : base(file, "/Diagram/Agent")
        {

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
