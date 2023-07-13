using System;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Dragble : MonoBehaviour
{
    private bool _angleIsPress;
    private Vector2 _orginMidPoint;

    private void Awake()
    {
        _orginMidPoint = Camera.main!.WorldToScreenPoint(transform.position);
        _angleIsPress = false;
    }


    /// <summary>
    /// 拖动效果
    /// </summary>
    public void OnButtonDrag()
    {
        Vector2 mousePosition = Input.mousePosition;
        RectTransform canvasRectTransform = transform.parent as RectTransform;
        if (!Utility.UI.InRectRange(canvasRectTransform, mousePosition,out Vector2 vector2,10f,10f))
        {
            return;
        }
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, Camera.main, out Vector2 localPosition))
        {
            var rectTransform = transform as RectTransform;
            transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(localPosition.x,localPosition.y-rectTransform!.rect.height/2);
        }
    }



    /// <summary>
    /// 角拖动
    /// </summary>
    public void OnAngleDrag()
    {
        if (_angleIsPress)
        {
            Vector2 mousePosition = Input.mousePosition;
            
            
            var rectTransform = transform as RectTransform;
            if (rectTransform != null)
            {
                var newWidth = math.max(math.abs(_orginMidPoint.x - mousePosition.x) * 2, 200f);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,newWidth);
                var newHeight = math.max(math.abs(_orginMidPoint.y - mousePosition.y)*2, 200f);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
            }

        }
    }
    
    /// <summary>
    /// 角按下触发
    /// </summary>
    public void OnAngleDown()
    {
        
        _angleIsPress = true;
        _orginMidPoint = Camera.main!.WorldToScreenPoint(transform.position);
    }
    
    /// <summary>
    /// 角释放触发
    /// </summary>
    public void OnAngleRelease()
    {
        _angleIsPress = false;

    }
    
}
