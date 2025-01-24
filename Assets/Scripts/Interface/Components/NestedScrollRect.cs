using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Interface.Components
{
    public class NestedScrollRect : ScrollRect
    {
        [SerializeField]
        private ScrollRect scrollRect;
        
        private bool _routeToParent;
        

        private IInitializePotentialDragHandler[] _initializePotentialDragHandlers;
        private IBeginDragHandler[] _beginDragHandlers;
        private IDragHandler[] _dragHandlers;
        private IEndDragHandler[] _endDragHandlers;


        protected override void Awake()
        {
            base.Awake();

            if (scrollRect == null)
                throw new NullReferenceException("Parent Scroll Rect is null");

            _initializePotentialDragHandlers = scrollRect.GetComponents<IInitializePotentialDragHandler>();
            _beginDragHandlers = scrollRect.GetComponents<IBeginDragHandler>();
            _dragHandlers = scrollRect.GetComponents<IDragHandler>();
            _endDragHandlers = scrollRect.GetComponents<IEndDragHandler>();
        }


        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            foreach (var handler in _initializePotentialDragHandlers)
                handler.OnInitializePotentialDrag(eventData);
            base.OnInitializePotentialDrag(eventData);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            _routeToParent = (!horizontal && Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y)) || (!vertical && Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y));
            
            if (_routeToParent)
            {
                foreach (var handler in _beginDragHandlers)
                    handler.OnBeginDrag(eventData);
            }
            else 
                base.OnBeginDrag(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (_routeToParent)
            {
                foreach (var handler in _dragHandlers)
                    handler.OnDrag(eventData);
            }
            else 
                base.OnDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (_routeToParent)
            {
                foreach (var handler in _endDragHandlers)
                    handler.OnEndDrag(eventData);
            }
            else 
                base.OnEndDrag(eventData);

            _routeToParent = false;
        }
    }
}
