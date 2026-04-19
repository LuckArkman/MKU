using System;
using UnityEngine;

namespace MKU.Scripts.Strucs
{
    public class InteractionBase : MonoBehaviour
    {
        [Header("Interact")]
        [SerializeField] protected float interactDistance = 1;

        public float InteractDistance => interactDistance;

        public event Action<InteractionBase> OnExitInteraction;

        public virtual void Interact()
        {

        }

        public virtual void ExitInteract()
        {
            OnExitInteraction?.Invoke(this);
        }
    }
}