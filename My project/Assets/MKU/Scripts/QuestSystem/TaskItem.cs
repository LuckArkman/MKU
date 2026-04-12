using System;

namespace MKU.Scripts.Tasks
{
    [Serializable]
    public class TaskItem : QItem {

        public QTask m_QTask;

        public TaskItem(int id, QTask qTask) {

            m_Id = id;
            m_QTask = qTask;

        }
    }
}