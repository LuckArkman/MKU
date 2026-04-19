using System;

namespace MKU.Scripts.InputSystem
{
    [Serializable]
    public class InputController : GenericInput
    {
        static InputController _instance;

        public static InputController instance
        {
            get
            {
                _instance = new InputController();
                return _instance;
            }
        }
    }
}