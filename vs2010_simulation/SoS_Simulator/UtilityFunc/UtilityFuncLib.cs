using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;
using System.IO;

namespace SoS_Simulator.UtilityFunc
{
    public abstract class UtilityFuncLib
    {
        protected Simulator _simulator;

        public UtilityFuncLib(Simulator simulator)
        {
            _simulator = simulator;
        }

        /// <summary>
        /// Retrieve utility function library path from xml
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetUtilityFuncLibrary(string file)
        {
            XmlDocument xmlDoc;
            XmlNodeList nodes;

            xmlDoc = new XmlDocument();
            xmlDoc.Load(file);

            nodes = xmlDoc.DocumentElement.SelectNodes("/SoS");

            foreach (XmlNode node in nodes)
            {
                foreach (XmlNode n in node.SelectNodes("Module"))
                {
                    if (n.Attributes["type"].InnerText == "Utility")
                    {
                        return Path.GetFullPath(Path.Combine(new FileInfo(file).Directory.FullName, n.Attributes["path"].InnerText));
                    }
                }
            }

            return null;
        }

        public static UtilityFuncLib LoadUtilityFuncLib(string file, Simulator sim)
        {
            Assembly utilFile;
            UtilityFuncLib lib;

            try
            {
                utilFile = Assembly.LoadFrom(file);
                Type mainType;

                foreach (Type t in utilFile.GetTypes())
                {
                    if (t.Name == "UtilityFuncMain")
                    {
                        mainType = t;

                        lib = (UtilityFuncLib)Activator.CreateInstance(mainType, sim);

                        return lib;
                    }
                }
            }
            catch (ApplicationException ex)
            {
                // Error loading utility func library
            }

            return null;
        }

        public abstract bool CheckGoalAccomplishment(string goalName);
    }
}
