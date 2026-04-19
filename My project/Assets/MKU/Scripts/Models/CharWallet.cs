using System.Collections.Generic;

namespace MKU.Scripts.Models
{
    public class CharWallet
    {
        public CharWallet(){}
        public string id { get; set; }
        public Dictionary<string, Cst> csts { get; set; }

        public CharWallet(string id, Dictionary<string, Cst> csts)
        {
            this.id = id;
            this.csts = csts;
        }
    }
}