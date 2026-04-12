using MKU.Scripts.Enums;

namespace MKU.Scripts.Models
{
    public class Message
    {
        public string userId { get; set; }
        public ActionCode actionCode { get; set; }
        public int quantity { get; set; }
        public string otherUserId { get; set; }

        public Message(string userId, ActionCode actionCode, int quantity, string otherUserId)
        {
            this.userId = userId;
            this.actionCode = actionCode;
            this.quantity = quantity;
            this.otherUserId = otherUserId;
        }
    }
}