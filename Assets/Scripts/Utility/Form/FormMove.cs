using UnityEngine;
using UnityEngine.EventSystems;

public class FormMove : MonoBehaviour
{
    [SerializeField] private RectTransform TargetTransform;
    
    private RectTransform canvasTransform;
    private Vector2 _btnDownTransformPos;
    private float TOLERANCE = 1f;
    private void Awake()
    {
        if (canvasTransform == null)
        {
            canvasTransform = GameObject.FindWithTag("MainCanvas").GetComponent<RectTransform>();
        }
    }
    public void OnFormMoveDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        if (pointerEventData != null)
        {
            if (!Utility.UI.InRectRange(canvasTransform, pointerEventData.position,out Vector2 fixPoint))
            {
                if(Mathf.Abs(pointerEventData.position.x - fixPoint.x)<TOLERANCE && Mathf.Abs(pointerEventData.position.y - fixPoint.y)<TOLERANCE) return;
            }
            TargetTransform.position = _btnDownTransformPos+fixPoint-pointerEventData.pressPosition;
        }
    }
    
    /// <summary>
    /// 按钮按下触发
    /// </summary>
    public void OnPointDown(BaseEventData baseEventData)
    {
        PointerEventData pointEvnetData = baseEventData as PointerEventData ;
        if (pointEvnetData != null) _btnDownTransformPos = TargetTransform.position;
    }
    
}
