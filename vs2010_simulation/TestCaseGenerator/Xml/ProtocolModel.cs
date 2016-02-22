using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Debugger;

namespace TestCaseGenerator.Xml
{
    class ProtocolModel : XmlParser
    {
        public ProtocolModel(string file)
            : base(file, "/Diagram/Object")
        {
            GetArrowByIndex(1);
        }

        public Arrow[] TrackSequence(string roleName, AgentModel agentModel)
        {
            List<Arrow> aList = new List<Arrow>();
            Arrow[] arrows;

            int i = 0;

            foreach (XmlNode x in FindObject(roleName))
            {
                Arrow arrow = new Arrow();
                arrow.index = Int32.Parse(x.Attributes["index"].InnerText);
                arrow.message = x.Attributes["name"].InnerText;
                arrow.role_to = x.Attributes["to"].InnerText;

                // Find last index
                if (i == 0)
                {
                    aList.Add(GetPreviousArrow(arrow));
                    i++;
                }

                aList.Add(arrow);

                Arrow nextArrow = GetNextArrow(arrow);

                while (nextArrow.index != -1
                        && nextArrow.role_to == roleName)
                {
                    aList.Add(nextArrow);
                    nextArrow = GetNextArrow(nextArrow);
                }
            }

            // Track agents
            arrows = aList.ToArray();

            for (int k = 0; k < arrows.Length; k++)
            {
                arrows[k].agent_to = agentModel.GetAgentFromRole(arrows[k].role_to);
            }

            return arrows;
        }

        public Arrow[] TrackSequence_depreciated(string roleName, AgentModel agentModel)
        {
            List<Arrow> aList = new List<Arrow>();
            Arrow[] arrows;

            //int i = 0;
            int destIndex = 0;

            // Find last index
            foreach (XmlNode x in FindObject(roleName))
            {
                // Get found arrow
                Arrow arrow = new Arrow();
                arrow.index = Int32.Parse(x.Attributes["index"].InnerText);
                arrow.message = x.Attributes["name"].InnerText;
                arrow.role_to = x.Attributes["to"].InnerText;

                destIndex = arrow.index;
            }

            Arrow newArrow = GetArrowByIndex(1);

            for (int i = 1; i <= destIndex; i++)
            {
                aList.Add(newArrow);
                newArrow = GetNextArrow(newArrow);
            }

            // Track agents
            arrows = aList.ToArray();

            for (int k = 0; k < arrows.Length; k++)
            {
                arrows[k].agent_to = agentModel.GetAgentFromRole(arrows[k].role_to);
            }

            return arrows;
        }

        private Arrow GetArrowByIndex(int index)
        {
            Arrow arrow = new Arrow();
            arrow.index = -1;

            foreach (XmlNode x in RootNodes)
            {
                foreach (XmlNode y in x.ChildNodes)
                {
                    if (y.Attributes["index"] != null &&
                            (Int32.Parse(y.Attributes["index"].InnerText) == index))
                    {
                        arrow.index = Int32.Parse(y.Attributes["index"].InnerText);
                        arrow.message = y.Attributes["name"].InnerText;
                        arrow.role_to = y.Attributes["to"].InnerText;

                        return arrow; 
                    }
                }
            }

            return arrow;
        }

        private Arrow GetNextArrow(Arrow prevArrow)
        {
            Arrow arrow = new Arrow();
            arrow.index = -1;

            Debug.Print("GetNextArrow: {0}", prevArrow.message);

            // Find only next element
            foreach (XmlNode x in FindObject(prevArrow.role_to))
            {
                if (prevArrow.index == (Int32.Parse(x.Attributes["index"].InnerText) - 1))
                {
                    arrow.index = Int32.Parse(x.Attributes["index"].InnerText);
                    arrow.message = x.Attributes["name"].InnerText;
                    arrow.role_to = x.Attributes["to"].InnerText;

                    return arrow;
                }
            }

            return arrow;
        }

        private Arrow GetPreviousArrow(Arrow nextArrow)
        {
            Arrow arrow = new Arrow();
            arrow.index = -1;

            Debug.Print("GetPrevArrow: {0}", nextArrow.message);

            // Find everywhere
            foreach (XmlNode x in RootNodes)
            {
                foreach (XmlNode y in x.ChildNodes)
                {
                    if (nextArrow.index == (Int32.Parse(y.Attributes["index"].InnerText) + 1))
                    {
                        arrow.index = Int32.Parse(y.Attributes["index"].InnerText);
                        arrow.message = y.Attributes["name"].InnerText;
                        arrow.role_to = y.Attributes["to"].InnerText;

                        return arrow;
                    }
                }
            }

            return arrow;
        }

        private XmlNode FindObject(string objectName)
        {
            foreach (XmlNode x in RootNodes)
            {
                if (x.Attributes["name"].InnerText == objectName)
                {
                    Debug.Print("FindObject: {0}", objectName);
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
