using System.Collections.Generic;

namespace MKU.Scripts.Tasks
{

    public class CharQuest
    {
        public CharQuest(){}
        public string id { get; set; }
        public Dictionary< string,Quest> _quests { get; set; }

        public CharQuest(string id, Dictionary< string,Quest> _quests)
        {
            this.id = id;
            this._quests = _quests;
        }
    }
}