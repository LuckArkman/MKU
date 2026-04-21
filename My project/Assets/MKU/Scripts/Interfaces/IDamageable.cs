using MKU.Scripts.HelthSystem;
using UnityEngine;

namespace MKU.Scripts.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(int damage);
        Transform GetTransform();
        Base GetBaseStats();
    }
}
