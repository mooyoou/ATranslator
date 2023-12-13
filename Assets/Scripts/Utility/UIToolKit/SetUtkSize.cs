using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// 将UIElement元素与Ugui元素重合
/// </summary>
public class SetUtkSize : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [Tooltip("被控制大小的UiElement")]
    [SerializeField] private string controlElementName = "TextAreaContainer";
    [Tooltip("作为大小依据的组件")]
    [SerializeField] private RectTransform targetRectTransform;
    private VisualElement _rootElement;
    
    private Vector2 _lastSize;
    private Vector2 _lastPos;
    private Coroutine _refreshUIRect;
    [SerializeField] private bool delayUpdate = false ;
    private bool _isDraging = false;
    private void OnEnable()
    {
        RegisterEvents();
        _rootElement = uiDocument.rootVisualElement.Q(controlElementName);
        if (_rootElement == null)
        {
            _rootElement = uiDocument.rootVisualElement.Q();
        }
        if (uiDocument == null)
        {
            uiDocument = GetComponent<UIDocument>();
        }
        if (targetRectTransform == null)
        {
            targetRectTransform = (RectTransform)transform;
        }
        targetRectTransform.pivot = Vector2.up;
        _rootElement.style.display = DisplayStyle.None;
        StartCoroutine(StartRefresh());//延迟数帧以解决Screen Size获取错误
    }

    public void RegisterEvents()
    {
        PullSlider.EndDrag += RemoveDragFlag;
        PullSlider.BeginDrag += SetDragFlag;
    }
    
    public void UnRegisterEvents()
    {
        PullSlider.EndDrag -= RemoveDragFlag;
        PullSlider.BeginDrag -= SetDragFlag;
    }

    private void SetDragFlag()
    {
        _isDraging = true;
    }
    private void RemoveDragFlag()
    {
        _isDraging = false;
    }


    private void OnDisable()
    {
        UnRegisterEvents();
        if (_refreshUIRect != null)
        {
            StopCoroutine(_refreshUIRect);
        }

        _lastSize = Vector2.zero;
        _lastPos = Vector2.zero;
        
    }

    private IEnumerator StartRefresh()
    {
        yield return new WaitForSeconds(0.1f);
        _rootElement.style.display = DisplayStyle.Flex;
        ResetElementRect(); //先刷新一次
        _refreshUIRect = StartCoroutine(RefreshUIRect());
    }
    
    private IEnumerator RefreshUIRect()
    {
        while (true)
        {
            if (NeedRefresh() && uiDocument.gameObject.activeSelf)
            {
                ResetElementRect(); 
            }
            yield return new WaitForEndOfFrame();
        }
    }
    

    /// <summary>
    /// 重设UTK元素大小位置与所选transform一致
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="targetElement"></param>
    private void ResetElementRect()
    {
        Vector2 topLeftPos = targetRectTransform.position;
        var fixTopLeftPos = new Vector2(topLeftPos.x, Screen.height-topLeftPos.y);//UIToolKit y坐标相反
        var panelPos = RuntimePanelUtils.ScreenToPanel(_rootElement.panel, fixTopLeftPos);
        _rootElement.transform.position = panelPos;

        var rect = targetRectTransform.rect;
        var lossyScale = targetRectTransform.lossyScale;
        var worldSize = rect.size * lossyScale;
        _rootElement.style.height = worldSize.y;
        _rootElement.style.width = worldSize.x;

        _lastSize = rect.size;
        _lastPos = topLeftPos;
    }

    /// <summary>
    /// 判断是否需要更新UIElement
    /// </summary>
    protected virtual bool NeedRefresh()
    {
        if (!delayUpdate)
        {
            if (_lastSize != targetRectTransform.rect.size || _lastPos != (Vector2)targetRectTransform.position)
            {
                return true;
            }
        }
        else
        {
            if (!_isDraging&&(_lastSize != targetRectTransform.rect.size || _lastPos != (Vector2)targetRectTransform.position))
            {
                return true;
            }
        }


        return false;
    }
    
}
