using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class PullSlider : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
    {
        public static event Action BeginDrag;
        public static event Action EndDrag;
        public static event Action<Vector2> IsDarging;
        
        public enum PullType
        {
            Left,
            Right,
            Up,
            Down,
        }
        [SerializeField]
        private LayoutElement controlLayoutElement;
        [SerializeField]
        private RectTransform canvasTransform;
        [SerializeField]
        private PullType pullType;
    
        private Boolean _isDrag;

    
        private void Awake()
        {
            _isDrag = false;
        
        
        }

        private void LateUpdate()
        {
            if (_isDrag)
            {
                var localScale = canvasTransform.localScale;
                float scalingFactorX = 1.0f/localScale.x;
                float scalingFactorY = 1.0f/localScale.y;
                switch (pullType)
                {
                    case PullType.Left:
                        controlLayoutElement.preferredWidth =Input.mousePosition.x*scalingFactorX;
                        controlLayoutElement.preferredHeight = -1;
                        break;
                    case PullType.Right:
                        controlLayoutElement.preferredWidth = (Screen.width-Input.mousePosition.x)*scalingFactorX;
                        controlLayoutElement.preferredHeight = -1;
                        break;
                    case PullType.Up:
                        controlLayoutElement.preferredWidth = -1;
                        controlLayoutElement.preferredHeight = (Screen.height-Input.mousePosition.y)*scalingFactorY;
                        break;
                    case PullType.Down:
                        controlLayoutElement.preferredWidth = -1;
                        controlLayoutElement.preferredHeight =Input.mousePosition.y*scalingFactorY;;
                        break;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isDrag = true;
            BeginDrag?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isDrag = false;
            EndDrag?.Invoke();
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            switch (pullType)
            {
                case PullType.Down:
                case PullType.Up:
                    GlobalSubscribeSys.Invoke("set_cursor", CursorCtl.CursorType.ns, this);
                    break;
                case PullType.Left:
                case PullType.Right:
                    GlobalSubscribeSys.Invoke("set_cursor", CursorCtl.CursorType.ew, this);
                    break;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isDrag)
            {
                GlobalSubscribeSys.Invoke("reset_cursor", this);
            }
        }
    }
}
