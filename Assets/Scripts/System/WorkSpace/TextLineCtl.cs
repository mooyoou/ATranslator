using TMPro;
using UnityEngine;

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

        public void UpdateLine(TextLineData textLineData)
        {
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
