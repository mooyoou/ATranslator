using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Dragble : MonoBehaviour
{
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
            transform.GetComponent<RectTransform>().anchoredPosition = localPosition;
        }
        
         
    }

    /// <summary>
    /// 角拖动
    /// </summary>
    public void OnAngleDrag()
    {
        
    }
    
}
