using MKU.Scripts.Enums;
using System;

namespace MKU.Scripts.Tasks
{
    [Serializable]
    public class QuestChain
    {
        public QuestModel questModel = QuestModel.None;
        public string Name;
    }
}
