using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace System.Explorer
{
    public class ExplorerNodeBtn : MonoBehaviour
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
        [SerializeField]
        private Transform subNodeRoot;
        private ExplorerNode _explorerNode;

        private bool _isSubNodeGen;

        private ExplorerNodeBtn _explorerNodeBtnP;
        //是否展开状态
        private bool _isExpand;

        private List<ExplorerNodeBtn> _explorerNodeBtns=new List<ExplorerNodeBtn>();

        internal void Init(ExplorerNode explorerNode,ExplorerNodeBtn explorerNodeP)
        {
            _explorerNode = explorerNode;
            _explorerNodeBtnP = explorerNodeP;
            tmpText.text = _explorerNode.FileName;
            horizontalLayoutGroup.padding.left = explorerNode.Depth * 25;
            if (_explorerNode.IsFolder)
            {
                extraBtn.gameObject.SetActive(true);
                fileIcon.sprite = explorerIcons.GetSprite("folder");
            }
            else
            {
                fileIcon.sprite = explorerIcons.GetSprite("file");
                extraBtn.gameObject.SetActive(false);
            }
            
            
            
        }
        /// <summary>
        /// 展开按钮点击
        /// </summary>
        public void OnExpandClick()
        {
            if (_explorerNode.IsFolder)
            {
                //状态处理
                if (_isExpand)
                {
                    extraBtn.transform.rotation=Quaternion.Euler(0,0,0);
                    foreach (var explorerNodeBtn in _explorerNodeBtns)
                    {
                        explorerNodeBtn.gameObject.SetActive(false);
                    }
                }
                else
                {
                    extraBtn.transform.rotation=Quaternion.Euler(0,0,-90);
                    if (!_isSubNodeGen)
                    {
                        foreach (var node in _explorerNode.SubExplorerNodes)
                        {
                            ExplorerNodeBtn explorerNodeBtn = Instantiate(_explorerNodeBtnP, subNodeRoot);
                            explorerNodeBtn.Init(node,_explorerNodeBtnP);
                            _explorerNodeBtns.Add(explorerNodeBtn);
                        }

                        _isSubNodeGen = true;
                    }
                    else
                    {
                        foreach (var explorerNodeBtn in _explorerNodeBtns)
                        {
                            explorerNodeBtn.gameObject.SetActive(true);
                        }
                    }
                }
                
                //转变状态
                _isExpand = !_isExpand;
            }
        }
        public void OnNodeClick()
        {

        }

    }
}