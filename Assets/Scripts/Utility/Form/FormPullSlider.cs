using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utility.Form
{
    public class FormPullSlider : MonoBehaviour
    {
        public enum PullType
        {
            Left,
            Right,
            Top,
            Bottom,
            LeftTop,
            LeftBottom,
            RightTop,
            RightBottom
        }

        [SerializeField] private PullType pullType;
        [SerializeField] private RectTransform TargetTransform;
    
        private RectTransform canvasTransform;
        private Boolean _isDrag;
        private Rect _originRect;
        private Vector2 _originAniPos;
        public int minHeight = 100;
        public int minWidth = 100;
        private float TOLERANCE = 1f;
    
        private void Awake()
        {
            _isDrag = false;
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.FindWithTag("MainCanvas").GetComponent<RectTransform>();
            }
        }

        public void OnMouseDrag(BaseEventData baseEventData)
        {
            PointerEventData pointerEventData = baseEventData as PointerEventData;
            if (pointerEventData != null)
            {
                if (!Utility.UI.InRectRange(canvasTransform, pointerEventData.position, out Vector2 fixPoint, 5f, 5f))
                {
                    switch (pullType)
                    {
                        case PullType.Left:
                        case PullType.Right:
                            if(Math.Abs(pointerEventData.position.x - fixPoint.x) < TOLERANCE)return;
                            break;
                        case PullType.Top:
                        case PullType.Bottom:
                            if(Math.Abs(pointerEventData.position.y - fixPoint.y) < TOLERANCE)return;
                            break;
                        case PullType.LeftBottom: 
                        case PullType.RightBottom:
                        case PullType.RightTop:
                        case PullType.LeftTop:
                            if(Math.Abs(pointerEventData.position.y - fixPoint.y) < TOLERANCE || Math.Abs(pointerEventData.position.x - fixPoint.x) < TOLERANCE)return;
                            break;
                    }
                }
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, fixPoint, null, out Vector2 canvasPosition);
                var offsetMousePos = canvasPosition - _originAniPos;
                switch (pullType)
                {
                    case PullType.Left:
                        if (_originRect.xMax - offsetMousePos.x <= minWidth) return;
                        TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                            _originRect.xMax - offsetMousePos.x);
                        break;
                    case PullType.Right:
                        if (offsetMousePos.x - _originRect.xMin <= minWidth) return;
                        TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                            offsetMousePos.x - _originRect.xMin);
                        break;
                    case PullType.Top:
                        if (offsetMousePos.y - _originRect.yMin <= minHeight) return;
                        TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                            offsetMousePos.y - _originRect.yMin);
                        break;
                    case PullType.Bottom:
                        if (_originRect.yMax - offsetMousePos.y <= minHeight) return;
                        TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                            _originRect.yMax - offsetMousePos.y);
                        break;

                    case PullType.LeftTop:
                        if (_originRect.xMax - offsetMousePos.x > minWidth)
                        {
                            TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                                _originRect.xMax - offsetMousePos.x);
                        }

                        if (offsetMousePos.y - _originRect.yMin > minHeight)
                        {
                            TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                                offsetMousePos.y - _originRect.yMin);
                        }

                        break;
                    case PullType.RightTop:
                        if (offsetMousePos.x - _originRect.xMin > minWidth)
                        {
                            TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                                offsetMousePos.x - _originRect.xMin);
                        }

                        if (offsetMousePos.y - _originRect.yMin > minHeight)
                        {
                            TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                                offsetMousePos.y - _originRect.yMin);
                        }



                        break;
                    case PullType.LeftBottom:
                        if (_originRect.xMax - offsetMousePos.x > minWidth)
                        {
                            TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                                _originRect.xMax - offsetMousePos.x);
                        }

                        if (_originRect.yMax - offsetMousePos.y > minHeight)
                        {
                            TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                                _originRect.yMax - offsetMousePos.y);
                        }


                        break;
                    case PullType.RightBottom:
                        if (offsetMousePos.x - _originRect.xMin > minWidth)
                        {
                            TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                                offsetMousePos.x - _originRect.xMin);
                        }

                        if (_originRect.yMax - offsetMousePos.y > minHeight)
                        {
                            TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                                _originRect.yMax - offsetMousePos.y);
                        }
                        break;
                }

                TargetTransform.ForceUpdateRectTransforms();
            }
        }

        public void OnBeginDrag(BaseEventData eventData)
        {
            _isDrag = true;
            _originRect = new Rect(TargetTransform.rect);
            _originAniPos = TargetTransform.anchoredPosition;
            switch (pullType)
            {
                case PullType.Left:
                    Utility.UI.ChangePivotWithoutMovingPosition(TargetTransform, new Vector2(1, 0.5f));
                    break;
                case PullType.Right:
                    Utility.UI.ChangePivotWithoutMovingPosition(TargetTransform, new Vector2(0, 0.5f));
                    break;
                case PullType.Top:
                    Utility.UI.ChangePivotWithoutMovingPosition(TargetTransform, new Vector2(0.5f, 0));
                    break;
                case PullType.Bottom:
                    Utility.UI.ChangePivotWithoutMovingPosition(TargetTransform, new Vector2(0.5f, 1));
                    break;
                case PullType.LeftTop:
                    Utility.UI.ChangePivotWithoutMovingPosition(TargetTransform, new Vector2(1, 0));
                    break;
                case PullType.RightTop:
                    Utility.UI.ChangePivotWithoutMovingPosition(TargetTransform, new Vector2(0, 0));
                    break;
                case PullType.LeftBottom:
                    Utility.UI.ChangePivotWithoutMovingPosition(TargetTransform, new Vector2(1, 1));
                    break;
                case PullType.RightBottom:
                    Utility.UI.ChangePivotWithoutMovingPosition(TargetTransform, new Vector2(0, 1));
                    break;
            }
        }

        public void OnEndDrag(BaseEventData eventData)
        {
            _isDrag = false;
            GlobalSubscribeSys.Invoke("reset_cursor", this);
        }


        public void OnPointerEnter(BaseEventData eventData)
        {
            switch (pullType)
            {
                case PullType.Top:
                case PullType.Bottom:
                    GlobalSubscribeSys.Invoke("set_cursor", CursorCtl.CursorType.ns, this);
                    break;
                case PullType.Left:
                case PullType.Right:
                    GlobalSubscribeSys.Invoke("set_cursor", CursorCtl.CursorType.ew, this);
                    break;
                case PullType.LeftTop:
                case PullType.RightBottom:
                    GlobalSubscribeSys.Invoke("set_cursor", CursorCtl.CursorType.nwse, this);
                    break;
                case PullType.LeftBottom:
                case PullType.RightTop:
                    GlobalSubscribeSys.Invoke("set_cursor", CursorCtl.CursorType.nesw, this);
                    break;
            }

        }

        public void OnPointerExit(BaseEventData eventData)
        {
            if (!_isDrag)
            {
                GlobalSubscribeSys.Invoke("reset_cursor", this);
            }
        }
    }
}
