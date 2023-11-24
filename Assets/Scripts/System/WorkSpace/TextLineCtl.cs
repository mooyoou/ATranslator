using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

namespace System.WorkSpace
{
    public class TextLineCtl : MonoBehaviour
    {
        [SerializeField] private TMP_InputField rawText;
        [SerializeField] private TMP_InputField translationText;
        [SerializeField] private Transform mask;
        [SerializeField] private Transform translationArea;
        [SerializeField] private TMP_Text marchNum;
    
        private TextLineData _textLineData;
        private Action RefreshLines;
        
        private float previousPreferredHeight;
        private void OnEnable()
        {
            previousPreferredHeight = LayoutUtility.GetPreferredHeight((RectTransform)transform);
        }
        
        private void Update()
        {
            var newhight = LayoutUtility.GetPreferredHeight((RectTransform)transform);
            if (Mathf.Abs(newhight - previousPreferredHeight) > 1)
            {
                previousPreferredHeight = newhight;
                if (RefreshLines != null)
                {
                    RefreshLines();
                }
            };
        }

        public void UpdateLine(TextLineData textLineData,Action refresh = null)
        {
            RefreshLines = refresh;
            _textLineData = textLineData;
            
            if (textLineData.IsRawText)
            {
                translationArea.gameObject.SetActive(false);
                mask.gameObject.SetActive(true);
                rawText.text = textLineData.RawText;
                translationText.text = "";
                marchNum.text = "";
            }
            else
            {
                translationArea.gameObject.SetActive(true);
                mask.gameObject.SetActive(false);
                rawText.text = textLineData.RawText;
                translationText.text = textLineData.TranslationText;
                marchNum.text = textLineData.MatchNum.ToString();
            }
        }
    
    }
}
