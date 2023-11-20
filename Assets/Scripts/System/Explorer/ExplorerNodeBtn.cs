using System.Collections.Generic;
using TMPro;
using UI.InfiniteListScrollRect;
using UI.InfiniteListScrollRect.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace System.Explorer
{
    public class ExplorerNodeBtn : InfiniteListElement
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

        internal ExplorerNodeData ExplorerNode;

        private int _btnIndex;
        private InfiniteListScrollRect _infiniteListScrollRect; 
        private float _lastClickTime = 0f;
        private readonly float _doubleClickDelay = 0.3f;
        
        
        /// <summary>
        /// 更新显示数据
        /// </summary>
        /// <param name="scrollRect">无限列表滚动视野</param>
        /// <param name="index"></param>
        /// <param name="data">无限列表数据</param>
        public override void OnUpdateData(InfiniteListScrollRect scrollRect,int index, InfiniteListData data)
        {
            _infiniteListScrollRect = scrollRect;
            _btnIndex = index;
            ExplorerNode = data as ExplorerNodeData ;
            
            if (ExplorerNode != null)
            {
                tmpText.text = ExplorerNode.FileName;
                horizontalLayoutGroup.padding.left = ExplorerNode.Depth * 13+3;
                if (ExplorerNode.IsFolder)
                {
                    extraBtn.gameObject.SetActive(true);
                    
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
                    extraBtn.gameObject.SetActive(false);
                    horizontalLayoutGroup.padding.left += 13;
                    // extraBtn.interactable = false;
                }
            }
        }
        
        /// <summary>
        /// 展开按钮点击
        /// </summary>
        public void OnExpandClick()
        {
            if (ExplorerNode.IsFolder)
            {
                List<ExplorerNodeData> explorerNodeDatas;


                //状态处理
                if (ExplorerNode.IsExpand)
                {
                    explorerNodeDatas = GetNodeList(ExplorerNode);//获取子节点
                    ExplorerNode.IsExpand = !ExplorerNode.IsExpand;
                    //->折叠
                    extraBtn.transform.rotation = Quaternion.Euler(0, 0, 0);
                    
                    
                    explorerNodeDatas.RemoveAt(0);
                    _infiniteListScrollRect.RemoveData(explorerNodeDatas);
                }
                else
                {
                    ExplorerNode.IsExpand = !ExplorerNode.IsExpand;
                    explorerNodeDatas = GetNodeList(ExplorerNode);
                    //->展开
                    extraBtn.transform.rotation = Quaternion.Euler(0, 0, -90);
                    
                    explorerNodeDatas.RemoveAt(0);
                    _infiniteListScrollRect.AddData(explorerNodeDatas,_btnIndex+1);
                }
            }
        }
        
        private List<ExplorerNodeData> GetNodeList(ExplorerNodeData explorerNode)
        {
            List<ExplorerNodeData> listDatas = new List<ExplorerNodeData>();
            Stack<ExplorerNodeData> nodeStack = new Stack<ExplorerNodeData>();
            nodeStack.Push(explorerNode);

            while (nodeStack.Count > 0)
            {
                ExplorerNodeData curNode = nodeStack.Pop();
            
                if (curNode.IsFolder && curNode.IsExpand)
                {
                    foreach (var nodeBtn in curNode.SubExplorerNodes)
                    {
                        nodeStack.Push(nodeBtn);
                    }
                }
                listDatas.Add(curNode);
            }

            
            
            return listDatas;
        }

        public void OnBtnClick()
        {
            if (!ExplorerNode.IsFolder)
            {
                if (Time.time - _lastClickTime < _doubleClickDelay)
                {
                   GlobalSubscribeSys.Invoke("choose_open_file",ExplorerNode);
                }
                _lastClickTime = Time.time;
            }
        }
    }
}
