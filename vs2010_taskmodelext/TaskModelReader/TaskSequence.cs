using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TaskModelReader
{
    class TaskSequence
    {
        //public void AddTask(
        private Queue<XmlNode> _primaryQueue;
        private List<Queue<XmlNode>> _queueList;

        public TaskSequence()
        {
            _primaryQueue = new Queue<XmlNode>();
            _queueList = new List<Queue<XmlNode>>();
        }

        public void Enqueue(XmlNode node)
        {
            _primaryQueue.Enqueue(node);
        }

        public void Duplicate()
        {
            _queueList.Add(_primaryQueue);

        }
    }
}
