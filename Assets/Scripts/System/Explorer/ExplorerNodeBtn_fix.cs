using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace System.Explorer
{
    public class ExplorerNodeBtn_fix : MonoBehaviour
    {
        [SerializeField]
        private Button extraBtn;
        [SerializeField]
        private Image fileIcon;
        [SerializeField]
        private TMP_Text tmpText;
        [SerializeField] 
        private SpriteAtlas explorerIcons;
        [SerializeField] 
        private HorizontalLayoutGroup horizontalLayoutGroup;

        internal ExplorerNode ExplorerNode;
        private ExplorerCtrl _explorerCtrl;
        
        private List<ExplorerNodeBtn> _explorerNodeBtns=new List<ExplorerNodeBtn>();

        internal void Init(ExplorerNode explorerNode,ExplorerCtrl explorerCtrl)
        {
            ExplorerNode = explorerNode;
            _explorerCtrl = explorerCtrl;
            tmpText.text = ExplorerNode.FileName;
            horizontalLayoutGroup.padding.left = explorerNode.Depth * 23;
            if (ExplorerNode.IsFolder)
            {
                extraBtn.interactable = true;
                fileIcon.sprite = explorerIcons.GetSprite("folder");
                if (!ExplorerNode.IsExpand)
                {
                    extraBtn.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    extraBtn.transform.rotation = Quaternion.Euler(0, 0, -90);
                }

            }
            else
            {
                fileIcon.sprite = explorerIcons.GetSprite("file");
                extraBtn.interactable = false;
            }
        }
        /// <summary>
        /// 展开按钮点击
        /// </summary>
        public void OnExpandClick()
        {
            if (ExplorerNode.IsFolder)
            {
                //状态处理
                if (ExplorerNode.IsExpand)
                {
                    extraBtn.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    extraBtn.transform.rotation = Quaternion.Euler(0, 0, -90);
                }

                //转变状态
                ExplorerNode.IsExpand = !ExplorerNode.IsExpand;

                _explorerCtrl.ExpendBtn(ExplorerNode);
            }
        }
        public void OnNodeClick()
        {

        }

    }
}
