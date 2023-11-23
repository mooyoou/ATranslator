using System.Explorer;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace System.WorkSpace
{
    public class FileTab : MonoBehaviour
    {
        [SerializeField] private RawImage fileIcon;
        [SerializeField] private TMP_Text fileName;
        [SerializeField] private RawImage chooseLine;
        [SerializeField] private RawImage backImg;

        private ExplorerNodeData _explorerNodeData;
        private Action<ExplorerNodeData,RectTransform> _onChooseCallBack;
        private Action<ExplorerNodeData,RectTransform> _onCloseCallBack;
        public void Init(ExplorerNodeData explorerNodeData,Action<ExplorerNodeData,RectTransform> onChooseCallBack,Action<ExplorerNodeData,RectTransform> onCloseCallBack)
        {
            _explorerNodeData = explorerNodeData;
            fileName.text = explorerNodeData.FileName;
            _onChooseCallBack = onChooseCallBack;
            _onCloseCallBack = onCloseCallBack;
        }
        
        public void OnCloseBtnClick()
        {
            _onCloseCallBack?.Invoke(_explorerNodeData,transform as RectTransform);
        }

        public void OnTabClick()
        {
            chooseLine.gameObject.SetActive(true);
            backImg.color = new Color(0.19f, 0.19f, 0.19f, 1);
            _onChooseCallBack?.Invoke(_explorerNodeData,transform as RectTransform);
        }

        public void CancleChooseState()
        {
            backImg.color = new Color(0.23f, 0.23f, 0.23f, 1);
            chooseLine.gameObject.SetActive(false);
        }
    }
}
