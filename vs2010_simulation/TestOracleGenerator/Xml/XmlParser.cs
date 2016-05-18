using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestOracleGenerator.Xml
{
    public abstract class XmlParser
    {
        protected XmlDocument _doc;
        private string ROOT_PATH;
        //protected XmlNodeList _nodes;

        public XmlParser(string file, string rootPath)
        {
            _doc = new XmlDocument();
            _doc.Load(file);

            //XmlNodeList nodes = doc.DocumentElement.SelectNodes("/Diagram/Role");

            string temp;
            ROOT_PATH = rootPath;

            /*foreach (XmlNode node in nodes)
            {
                temp = node.SelectSingleNode("attribute").Attributes["name"].InnerText;
                temp = node.SelectSingleNode("arrow").InnerText;
            }*/
        }

        /// <summary>
        /// Selects a list of nodes matching the XPath expression.
        /// </summary>
        /// <param name="xpath">The XPath expression.</param>
        /// <returns></returns>
        public XmlNodeList SelectNodes(string xpath)
        {
            return _doc.DocumentElement.SelectNodes(xpath);
        }

        /// <summary>
        /// Selects a list of nodes matching the XPath expression. Any prefixes found in the XPath expression are resolved using the supplied System.Xml.XmlNamespaceManager.
        /// </summary>
        /// <param name="xpath">The XPath expression.</param>
        /// <param name="nsmgr">An System.Xml.XmlNamespaceManager to use for resolving namespaces for prefixes in the XPath expression.</param>
        /// <returns></returns>
        public XmlNodeList SelectNodes(string xpath, XmlNamespaceManager nsmgr)
        {
            return _doc.DocumentElement.SelectNodes(xpath, nsmgr);
        }

        private void aa()
        {
            XmlNodeList nodes = _doc.DocumentElement.SelectNodes("/Diagram/Role");

            string temp;

            foreach (XmlNode node in nodes)
            {
                temp = node.SelectSingleNode("attribute").Attributes["name"].InnerText;
                temp = node.SelectSingleNode("arrow").InnerText;
            }
        }

        protected XmlNodeList RootNodes
        {
            get
            {
                return SelectNodes(ROOT_PATH);
            }
        }
    }
}
