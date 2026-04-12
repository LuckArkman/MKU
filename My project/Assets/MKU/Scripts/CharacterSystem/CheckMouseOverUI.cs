using System.Collections.Generic;
using MKU.Scripts.CharacterSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CursedStone.CharacterSystem
{
    public class CheckMouseOverUI : MonoBehaviour
    {
        // Referência ao Canvas que contém os elementos de UI
        public Canvas canvas;

        public GraphicRaycaster raycaster;
        public PointerEventData pointerEventData;
        public EventSystem eventSystem;

        private void Update()
        {
            //controller.notcanvas = IsMouseOverUIElement();
        }

        private bool IsMouseOverUIElement()
        {
            if (raycaster == null || eventSystem == null)
                return false;
            pointerEventData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, results);
            return results.Count > 0;
        }
    }
}