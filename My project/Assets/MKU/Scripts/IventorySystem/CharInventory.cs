using System.Collections.Generic;

namespace MKU.Scripts.IventorySystem
{
    public class CharInventory
    {
        public CharInventory(){}
        public string id { get; set; }
        public Dictionary<string, Bag> _bag { get; set; }

        public CharInventory(string id, Dictionary<string, Bag> bag)
        {
            this.id = id;
            _bag = bag;
        }
    }
}