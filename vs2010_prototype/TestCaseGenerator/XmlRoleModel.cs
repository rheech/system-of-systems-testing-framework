using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestCaseGenerator
{
    class XmlRoleModel
    {
        // XML Parsing Reference: http://www.doublecloud.org/2013/08/parsing-xml-in-c-a-quick-working-sample/

        public XmlRoleModel(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/Diagram/Role");

            string temp;

            foreach (XmlNode node in nodes)
            {
                temp = node.SelectSingleNode("attribute").Attributes["name"].InnerText;
                temp = node.SelectSingleNode("arrow").InnerText;
            }
        }
    }
}
