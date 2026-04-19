using System;

namespace MKU.Scripts.Models
{
    public class Cst
    {
        public Cst(){}
        public string id { get; set; }
        public string token { get; set; }
        public DateTime dateTime { get; set; }

        public Cst(string id, string token, DateTime dateTime)
        {
            this.id = id;
            this.token = token;
            this.dateTime = dateTime;
        }
    }
}