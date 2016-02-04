using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestCaseGenerator
{
    public struct Arrow
    {
        public string name;
        public string to;
    }
    class XmlProtocolModel : XmlParser
    {
        public XmlProtocolModel(string file)
            : base(file, "/Diagram/Object")
        {
        }

        public Arrow[] TrackSequence(string roleName)
        {
            List<Arrow> aList = new List<Arrow>();

            foreach (XmlNode x in FindRole(roleName))
            {
                Arrow arrow = new Arrow();
                arrow.name = x.Attributes["name"].InnerText;
                arrow.to = x.Attributes["to"].InnerText;

                aList.Add(arrow);
            }

            return aList.ToArray();
        }

        private XmlNode FindRole(string roleName)
        {
            foreach (XmlNode x in RootNodes)
            {
                Console.WriteLine("{0} {1}", x.Attributes["type"].InnerText, x.Attributes["name"].InnerText);

                if (x.Attributes["type"].InnerText == "Role" &&
                        x.Attributes["name"].InnerText == roleName)
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
