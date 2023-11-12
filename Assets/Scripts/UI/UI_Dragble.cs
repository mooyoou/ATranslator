using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
public class UI_Dragble : MonoBehaviour
{
    private bool _angleIsPress;
    private Vector2 _orginMidPoint;
    public RectTransform canvasRectTransform;
    private Vector2 _btnDownTransformPos;

    /// <summary>
    /// 按钮按下触发
    /// </summary>
    public void OnPointDown(BaseEventData baseEventData)
    {
        PointerEventData pointEvnetData = baseEventData as PointerEventData ;
        if (pointEvnetData != null) _btnDownTransformPos = transform.position;
    }

    /// <summary>
    /// 拖动效果
    /// </summary>
    public void OnButtonDrag(BaseEventData value)
    {
        PointerEventData  inputEventData = value as PointerEventData ;
        if (inputEventData != null)
        {
            if (!Utility.UI.InRectRange(canvasRectTransform, inputEventData.position,out Vector2 vector2,10f,10f))
            {
                return;
            }
            transform.position = _btnDownTransformPos+inputEventData.position-inputEventData.pressPosition;
        }
    }

    /// <summary>
    /// 角拖动
    /// </summary>
    public void OnAngleDrag(BaseEventData baseEventData)
    {
        PointerEventData pointEvnetData = baseEventData as PointerEventData ;

        if (pointEvnetData != null)
        {
            if (!Utility.UI.InRectRange(canvasRectTransform, pointEvnetData.position, out Vector2 vector2, 10f, 10f))
            {
                return;
            }
            
            if (pointEvnetData.dragging)
            {
                Vector2 mousePosition = pointEvnetData.position;
                var rectTransform = transform as RectTransform;
                if (rectTransform != null)
                {
                    var localScale = canvasRectTransform.localScale;
                    var newWidth = math.max(math.abs(_btnDownTransformPos.x - mousePosition.x) * 2 / localScale.x,
                        200f);
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
                    var newHeight = math.max(math.abs(_btnDownTransformPos.y - mousePosition.y) * 2 / localScale.y,
                        200f);
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
                }
            }
        }
    }

}
