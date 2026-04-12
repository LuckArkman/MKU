using MKU.Scripts.Enums;

namespace MKU.Scripts.Interfaces
{
    public interface IController
    {
        void SetAction(ActionCode actionCode, string data);
    }
}