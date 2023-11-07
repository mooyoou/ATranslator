using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InfiniteListScrollRect.Runtime
{
    public class InfiniteListScrollRect : ScrollRect
    {
        /// <summary>
        /// 元素模板
        /// </summary>
        public InfiniteListElement ElementTemplate;
        /// <summary>
        /// 元素排列方向
        /// </summary>
        public Direction ListingDirection = Direction.Vertical;


        /// <summary>
        /// 元素排列组
        /// </summary>
        public HorizontalOrVerticalLayoutGroup LayoutGroup;
        private List<InfiniteListData> _datas  = new List<InfiniteListData>();


        // private HashSet<InfiniteListData> _dataIndexs = new HashSet<InfiniteListData>();
        private Dictionary<InfiniteListData, InfiniteListElement> _displayElements = new Dictionary<InfiniteListData, InfiniteListElement>();
        private HashSet<InfiniteListData> _invisibleList = new HashSet<InfiniteListData>();
        private Queue<InfiniteListElement> _elementsPool = new Queue<InfiniteListElement>();

        /// <summary>
        /// 初始边缘间隔
        /// </summary>
        private RectOffset _orignRectOffset;
        
        /// <summary>
        /// 元素之间的间隔
        /// </summary>
        private float _interval = 5;
        
        /// <summary>
        /// 元素高度
        /// </summary>
        private float _height = 20;
        
        /// <summary>
        /// 当前数据数量
        /// </summary>
        public int DataCount
        {
            get
            {
                return _datas.Count;
            }
        }

        /// <summary>
        /// 是否执行节点刷新标记
        /// </summary>
        private bool _needRefresh = false;

        /// <summary>
        /// 是否执行节点刷新标记
        /// </summary>
        private bool _dataUpdate = false;
        
        /// <summary>
        /// 
        /// </summary>
        private int _preOriginIndex = -1;
        
        protected override void LateUpdate()
        {
            base.LateUpdate();
            if (_needRefresh)
            {
                RefreshScrollView(); 
                _needRefresh = false;
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            return;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            return;
        }
        
        public override void OnEndDrag(PointerEventData eventData)
        {
            return;
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            if (ListingDirection == Direction.Vertical)
            {
                onValueChanged.AddListener((value) => { _needRefresh = true; });
            }
            else if(ListingDirection == Direction.Horizontal)
            {
                horizontalScrollbar.onValueChanged.AddListener((value) => { _needRefresh = true; });
            }
            
            _interval = LayoutGroup.spacing;
            _height = ((RectTransform)ElementTemplate.transform).rect.height;
            if (ListingDirection == Direction.Vertical)
            {
                LayoutGroup.padding.top = (int)_interval;//首部填充与间隔保持一致方便计算
            }else if (ListingDirection == Direction.Horizontal)
            {
                LayoutGroup.padding.left = (int)_interval;//首部填充与间隔保持一致方便计算
            }
            _orignRectOffset = new RectOffset(LayoutGroup.padding.left,LayoutGroup.padding.right,LayoutGroup.padding.top,LayoutGroup.padding.bottom);
        }

        /// <summary>
        /// 添加一条新的数据到无限列表尾部
        /// </summary>
        /// <param name="data">无限列表数据</param>
        public void AddData(InfiniteListData data,int insertIndex=-1)
        {
            _dataUpdate = true;
            if(insertIndex>=0 && insertIndex<= _datas.Count)
            {
                _datas.Insert(insertIndex,data);
            }
            else
            {
                _datas.Add(data);
            }

            RefreshScrollContent();
        }
        
        /// <summary>
        /// 添加多条新的数据到无限列表尾部
        /// </summary>
        /// <typeparam name="T">无限列表数据类型</typeparam>
        /// <param name="datas">无限列表数据</param>
        public void AddData<T>(List<T> datas,int insertIndex=-1) where T : InfiniteListData
        {
            _dataUpdate = true;
            if(insertIndex>=0 && insertIndex<= _datas.Count)
            {
                _datas.InsertRange(insertIndex,datas);
            }
            else
            {
                for (int i = 0; i < datas.Count; i++)
                {
                    _datas.Add(datas[i]);
                }
            }

            RefreshScrollContent();
        }
        
        /// <summary>
        /// 清除/重设所有的无限列表数据
        /// </summary>
        /// <param name="data"></param>
        public void ResetData(InfiniteListData data = null)
        {
            _dataUpdate = true;
            _datas.Clear();
            if (data == null)
            {
                foreach (var element in _displayElements)
                {
                    RecycleElement(element.Value);
                }

                _displayElements.Clear();
            }
            else
            {
                _datas.Add(data);
            }

            RefreshScrollContent();
        }
        /// <summary>
        /// 清除/重设所有的无限列表数据
        /// </summary>
        /// <param name="datas"></param>
        /// <typeparam name="T"></typeparam>
        public void ResetData<T>(List<T> datas= null) where T : InfiniteListData
        {
            _dataUpdate = true;
            _datas.Clear();
            if (datas == null)
            {
                foreach (var element in _displayElements)
                {
                    RecycleElement(element.Value);
                }
                _displayElements.Clear();
            }
            else
            {
                for (int i = 0; i < datas.Count; i++)
                {
                    _datas.Add(datas[i]);
                }
            }
            RefreshScrollContent();
        }

        /// <summary>
        /// 替换指定列表数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="replaceIndex"></param>
        public void ReplaceData(InfiniteListData data, int replaceIndex)
        {
            _dataUpdate = true;
            if (0 <= replaceIndex && replaceIndex < _datas.Count)
            {
                if (_displayElements.ContainsKey(_datas[replaceIndex]))
                {
                    RecycleElement(_displayElements[_datas[replaceIndex]]);
                    _displayElements.Remove(_datas[replaceIndex]);
                }
                _datas.RemoveAt(replaceIndex);
                _datas.Insert(replaceIndex,data);
                RefreshScrollContent();
            }
        }

        public void ReplaceData<T>(List<T> datas,int replaceIndex) where T : InfiniteListData
        {
            _dataUpdate = true;
            if (0 <= replaceIndex && replaceIndex < _datas.Count)
            {
                if (_displayElements.ContainsKey(_datas[replaceIndex]))
                {
                    RecycleElement(_displayElements[_datas[replaceIndex]]);
                    _displayElements.Remove(_datas[replaceIndex]);
                }
                
                _datas.RemoveAt(replaceIndex);
                _datas.InsertRange(replaceIndex,datas);
                RefreshScrollContent();
            }
        }
        
        /// <summary>
        /// 批量替换指定列表数据 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="replaceIndex"></param>
        public void RangeReplaceData(InfiniteListData data, int startIndex ,int count)
        {
            _dataUpdate = true;
            if (0 <= startIndex && startIndex+count <=_datas.Count)
            {
                for (int i  = startIndex; i  < startIndex+count; i ++)
                {
                                    
                    if (_displayElements.ContainsKey(_datas[i]))
                    {
                        RecycleElement(_displayElements[_datas[i]]);
                        _displayElements.Remove(_datas[i]);
                    }
                }
                _datas.RemoveRange(startIndex,count);
                _datas.Insert(startIndex,data);
                RefreshScrollContent();
            }
        }
        public void RangeReplaceData<T>(List<T> datas, int startIndex ,int count) where T : InfiniteListData
        {
            _dataUpdate = true;
            if (0 <= startIndex && startIndex+count <_datas.Count)
            {
                for (int i  = startIndex; i  < startIndex+count; i ++)
                {
                                    
                    if (_displayElements.ContainsKey(_datas[i]))
                    {
                        RecycleElement(_displayElements[_datas[i]]);
                        _displayElements.Remove(_datas[i]);
                    }
                }
                _datas.RemoveRange(startIndex,count);
                _datas.InsertRange(startIndex,datas);
                RefreshScrollContent();
            }
        }

        /// <summary>
        /// 移除一条无限列表数据
        /// </summary>
        /// <param name="data">无限列表数据</param>
        public void RemoveData(InfiniteListData data)
        {
            _dataUpdate = true;
            _datas.Remove(data);
            if (_displayElements.ContainsKey(data))
            {
                RecycleElement(_displayElements[data]);
                _displayElements.Remove(data);
            }
            RefreshScrollContent();
        }
        
        /// <summary>
        /// 移除一条无限列表数据
        /// </summary>
        /// <param name="data">无限列表数据</param>
        public void RemoveData<T>(List<T> datas) where T : InfiniteListData
        {
            _dataUpdate = true;
            foreach (var data in datas)
            {
                _datas.Remove(data);
                if (_displayElements.ContainsKey(data))
                {
                    RecycleElement(_displayElements[data]);
                    _displayElements.Remove(data);
                }
            }
            RefreshScrollContent();
        }
        
        /// <summary>
        /// 刷新滚动列表内容
        /// </summary>
        protected void RefreshScrollContent()
        {
            if (ListingDirection == Direction.Vertical)
            {
                content.sizeDelta = new Vector2(content.sizeDelta.x, _datas.Count * (_height + _interval)+_orignRectOffset.bottom);
            }
            else
            {
                content.sizeDelta = new Vector2( _datas.Count * (_height + _interval)+_orignRectOffset.right,content.sizeDelta.y);
            }

            RefreshScrollView();
        }
        
         /// <summary>
        /// 刷新滚动视图
        /// </summary>
        protected void RefreshScrollView() 
        {
            float originLength = 0;
            float viewLength = 0;
            if (ListingDirection == Direction.Vertical)
            {
                originLength = content.anchoredPosition.y;
                viewLength = viewport.rect.size.y;
            }else if (ListingDirection == Direction.Horizontal)
            {
                originLength = content.anchoredPosition.x;
                viewLength = viewport.rect.size.x;
            }

            int originIndex = Mathf.Max((int)(originLength/ (_height + _interval)),0);

            if (_preOriginIndex != originIndex || _dataUpdate)
            {
                _preOriginIndex = originIndex;
                _dataUpdate = false;
                ClearInvisibleElement(originLength, viewLength);
                LayoutGroup.padding.top = (int)(_height + _interval) * originIndex + _orignRectOffset.top;

                int index;
                for (index = originIndex; index < _datas.Count; index++)
                {
                    InfiniteListData data = _datas[index];

                    float viewP = -(index * _height + (index + 1) * _interval);
                    float realP = viewP + originLength;
                    //显示范围判定
                    if (realP > -viewLength)
                    {
                        if (_displayElements.ContainsKey(data))
                        {
                            _displayElements[data].transform.SetSiblingIndex(index - originIndex);
                            _displayElements[data].OnUpdateData(this, index, data);
                            continue;
                        }

                        InfiniteListElement element = ExtractIdleElement();


                        element.transform.SetSiblingIndex(index - originIndex);
                        element.OnUpdateData(this, index, data);
                        _displayElements.Add(data, element);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (ListingDirection == Direction.Vertical)
            {
                LayoutGroup.SetLayoutVertical();
            }
            else if (ListingDirection == Direction.Horizontal)
            {
                LayoutGroup.SetLayoutHorizontal();
            }
        }
        
        
         /// <summary>
         /// 清理并回收看不见的元素（垂直模式）
         /// </summary>
         /// <param name="contentP">滚动视图内容位置y</param>
         /// <param name="viewLength">滚动视图高度</param>
         private void ClearInvisibleElement(float contentP, float viewLength)
         {
             foreach (var element in _displayElements)
             {
                 if (ListingDirection == Direction.Vertical)
                 {
                     float realY = element.Value.UITransform.anchoredPosition.y + contentP;
                     if (realY < _height && realY > -viewLength)
                     {
                         continue;
                     }
                     else
                     {
                         _invisibleList.Add(element.Key);
                     }
                 }else if (ListingDirection == Direction.Horizontal)
                 {
                     float realX = element.Value.UITransform.anchoredPosition.x + contentP;
                     if (realX > -_height && realX < viewLength)
                     {
                         continue;
                     }
                     else
                     {
                         _invisibleList.Add(element.Key);
                     }
                 }
             }

             if (_invisibleList.Count != 0)
             {
                 foreach (var item in _invisibleList)
                 {
                     RecycleElement(_displayElements[item]);
                     _displayElements.Remove(item);
                 }
                 _invisibleList.Clear();
             }
         }

         /// <summary>
         /// 回收一个无用的无限列表元素
         /// </summary>
         /// <param name="element">无限列表元素</param>
         private void RecycleElement(InfiniteListElement element)
         {
             element.OnClearData();
             element.gameObject.SetActive(false);
             _elementsPool.Enqueue(element);
         }

         
        /// <summary>
        /// 提取一个空闲的无限列表元素
        /// </summary>
        /// <returns>无限列表元素</returns>
        private InfiniteListElement ExtractIdleElement()
        {
            if (_elementsPool.Count > 0)
            {
                InfiniteListElement element = _elementsPool.Dequeue();
                // element.transform.SetParent(content);
                element.gameObject.SetActive(true);
                return element;
            }
            else
            {
                InfiniteListElement element = Instantiate(ElementTemplate, content);
                element.gameObject.SetActive(true);
                return element.GetComponent<InfiniteListElement>();
            }
        }
         
         
    }

    //无限列表的类型
    public enum Direction
    {
        /// <summary>
        /// 水平
        /// </summary>
        Horizontal,
        /// <summary>
        /// 垂直
        /// </summary>
        Vertical
    }
}