using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class SetImgSize : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private RawImage rawImage;
    [Tooltip("作为大小依据的组件")]
    [SerializeField] private RectTransform targetRectTransform;


    
    private void OnEnable()
    {
        if (uiDocument == null)
        {
            uiDocument = GetComponent<UIDocument>();
        }
        if (targetRectTransform == null)
        {
            targetRectTransform = (RectTransform)transform;
        }


        RefreshUIRect();


    }

    private void Update()
    {
    }
    
    /// <summary>
    /// 重设UTK元素大小位置与所选transform一致
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="targetElement"></param>
    private void RefreshUIRect()
    {
            var lossyScale = targetRectTransform.lossyScale;
            var size = targetRectTransform.rect.size * lossyScale;
            var texture = new RenderTexture( (int)size.x,(int)size.y,0);
            rawImage.texture = texture;
            uiDocument.panelSettings.targetTexture = texture;
            
            // uiDocument.panelSettings.SetScreenToPanelSpaceFunction((Vector2 screenPosition)=>
            // {
            //     var panelPos = RuntimePanelUtils.ScreenToPanel(uiDocument.rootVisualElement.panel, screenPosition);
            //     return panelPos;
            // }
            // );
            
    }
    
    
}
