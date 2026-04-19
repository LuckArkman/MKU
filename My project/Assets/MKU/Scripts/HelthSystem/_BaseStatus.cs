using MKU.Scripts.HelthSystem;
using UnityEngine;

namespace MKU.Scripts.HelthSystem
{
    public class _BaseStatus
    {
        
            public Base Init(_Attributs _attributes, _Stats _status, int level)
            => new Base().StartCalc(_attributes, _status, level);
    }
}