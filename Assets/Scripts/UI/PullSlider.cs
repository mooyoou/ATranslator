using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

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
            switch (pullType)
            {
                case PullType.Left:
                    controlLayoutElement.preferredWidth =Input.mousePosition.x;
                    controlLayoutElement.preferredHeight = -1;
                    break;
                case PullType.Right:
                    controlLayoutElement.preferredWidth = Screen.width-Input.mousePosition.x;
                    controlLayoutElement.preferredHeight = -1;
                    break;
                case PullType.Up:
                    controlLayoutElement.preferredWidth = -1;
                    controlLayoutElement.preferredHeight = Screen.height-Input.mousePosition.y;
                    break;
                case PullType.Down:
                    controlLayoutElement.preferredWidth = -1;
                    controlLayoutElement.preferredHeight =Input.mousePosition.y;
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
