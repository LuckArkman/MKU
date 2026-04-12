using MKU.Scripts.Enums;
using UnityEngine;

namespace MKU.Scripts.Interfaces
{
    public interface IRaycastable
    {
        InteractCursor GetCursor();
        bool HandleRaycast(CharacterController control);
    }
}