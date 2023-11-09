using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PullSlider : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public enum PullType
    {
        Left,
        Right,
        Up,
        Down,
    }
    public LayoutElement controlLayoutElement;
    public RectTransform canvasTransform;
    public PullType pullType;
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
                    controlLayoutElement.preferredWidth = Screen.width-Input.mousePosition.x*scalingFactorX;;
                    controlLayoutElement.preferredHeight = -1;
                    break;
                case PullType.Up:
                    controlLayoutElement.preferredWidth = -1;
                    controlLayoutElement.preferredHeight = Screen.height-Input.mousePosition.y*scalingFactorY;;
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
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDrag = false;
    }
}
