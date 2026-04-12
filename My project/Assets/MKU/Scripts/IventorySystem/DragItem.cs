using System;
using System.Collections.Generic;
using MKU.Scripts.Interfaces;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.Singletons;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MKU.Scripts.IventorySystem
{
    public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler where T : class
    {
        Vector3 startposition;
        Transform originalPerent;
        IDragSource<T> source;
        int quantity;
        public Pickup pickup;

        Canvas parentCanvas;

        void Awake()
        {
            parentCanvas = GetComponentInParent<Canvas>();
            source = GetComponentInParent<IDragSource<T>>();
            
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            startposition = transform.position;
            originalPerent = transform.parent;

            GetComponent<CanvasGroup>().blocksRaycasts = false;
            transform.SetParent(parentCanvas.transform, true);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            transform.position = startposition;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.SetParent(originalPerent, true);

            IDragDestination<T> container = null;

            // Verifica se o mouse está sobre um CanvasGroup
            if (!IsPointerOverCanvasGroup(eventData))
            {
                // Se não estiver sobre um CanvasGroup, dropa o item no mundo
                var removeSourceNumber = source.GetNumber();
                var removeSourceItem = source.GetItem() as _Item;

                source.RemoveItems(removeSourceNumber);
                Vector3 spawnPosition = Singleton.Instance._charController.transform.position 
                                        + Singleton.Instance._charController.transform.right * -5.0f;

                var _obj = Instantiate(pickup, spawnPosition, quaternion.identity);
                _obj.AddDropPickup(removeSourceItem.itemID, removeSourceNumber);
                Singleton.Instance._charController.UpdateInventory();
                return;
            }
    
            // Caso contrário, tenta pegar um destino válido para o item
            container = GetContainer(eventData);

            if (container != null)
            {
                DropItemIntoContainer(container);
            }
        }
        private bool IsPointerOverCanvasGroup(PointerEventData eventData)
        {
            // Obtém o GraphicRaycaster do Canvas
            GraphicRaycaster raycaster = parentCanvas.GetComponent<GraphicRaycaster>();
            if (raycaster == null) return false;
            
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(eventData, results);

            // Verifica se algum resultado contém um CanvasGroup
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.GetComponentInParent<CanvasGroup>() != null)
                {
                    return true; // O mouse está sobre um CanvasGroup
                }
            }

            return false; // O mouse não está sobre um CanvasGroup
        }

        private IDragDestination<T> GetContainer(PointerEventData eventData)
        {
            if(eventData.pointerEnter)
            {
                var container = eventData.pointerEnter.GetComponent<IDragDestination<T>>();
                if(container != null )
                {
                    return container;
                }
            }
            return null;
        }

        private void DropItemIntoContainer(IDragDestination<T> destination)
        {
            if (object.ReferenceEquals(destination, source)) return;
            var destinationContainer = destination as IDragContainer<T>;
            var sourceContainer = source as IDragContainer<T>;

            if(destinationContainer == null || sourceContainer == null || destinationContainer.GetItem() == null || object.ReferenceEquals(destinationContainer.GetItem(), sourceContainer.GetItem()))
            {
                AttemptSimpleTransfer(destination);
                return;
            }
            AttemptSwap(destinationContainer, sourceContainer);
        }
        
        private void AttemptSwap(IDragContainer<T> destination, IDragContainer<T> source)
        {
            var removeSourceNumber = source.GetNumber();
            var removeSourceItem = source.GetItem();
            var removeDestinationNumber = destination.GetNumber();
            var removeDestinationItem = destination.GetItem();

            source.RemoveItems(removeSourceNumber);
            destination.RemoveItems(removeDestinationNumber);

            var sourceTakeBackNumber = CalculateTakeBack(removeSourceItem, removeSourceNumber, source, destination);
            var destinationTakeBackNumber = CalculateTakeBack(removeDestinationItem, removeDestinationNumber, destination, source);

            if(sourceTakeBackNumber > 0)
            {
                source.AddItems(removeSourceItem, sourceTakeBackNumber);
                removeSourceNumber -= sourceTakeBackNumber;
            }

            if(destinationTakeBackNumber > 0)
            {
                destination.AddItems(removeDestinationItem, destinationTakeBackNumber);
                removeDestinationNumber -= destinationTakeBackNumber;
            }

            if(source.MaxAccetable(removeDestinationItem) < removeDestinationNumber || destination.MaxAccetable(removeSourceItem) < removeSourceNumber)
            {
                destination.AddItems(removeDestinationItem, removeDestinationNumber);
                source.AddItems(removeSourceItem, removeSourceNumber);
                return;
            }

            if(removeDestinationNumber > 0)
            {
                source.AddItems(removeDestinationItem, removeDestinationNumber);
            }
            if(removeSourceNumber > 0)
            {
                destination.AddItems(removeSourceItem, removeDestinationNumber);
            }
            

        }



        private bool AttemptSimpleTransfer(IDragDestination<T> destination)
        {
            var draggingItem = source.GetItem();
            var draggingNumber = source.GetNumber();

            var acceptable = destination.MaxAccetable(draggingItem);
            var toTransfer = Mathf.Min(acceptable, draggingNumber);

            if(toTransfer > 0)
            {
                source.RemoveItems(toTransfer);
                destination.AddItems(draggingItem, toTransfer);
                return false;
            }
            return true;
        }


        private int CalculateTakeBack(T removeItem, int removeNumber, IDragContainer<T> removeSource, IDragContainer<T> destination)
        {
            var takeBackNumber = 0;
            var destinationMaxAcceptable = destination.MaxAccetable(removeItem);

            if(destinationMaxAcceptable < removeNumber)
            {
                takeBackNumber = removeNumber - destinationMaxAcceptable;

                var sourceTakeBackAccetable = removeSource.MaxAccetable(removeItem);

                if(sourceTakeBackAccetable < takeBackNumber)
                {
                    return 0;
                }
            }
            return takeBackNumber;
        }
    }
}