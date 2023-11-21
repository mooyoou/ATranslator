using System.Collections.Generic;
using UI.InfiniteListScrollRect.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace System.WorkSpace
{
    public class InfiniteTextLineScrollRect : ScrollRect
    {
        //     // /// <summary>
        //     // /// 元素模板
        //     // /// </summary>
        //     // public InfiniteListElement ElementTemplate;
        //     // /// <summary>
        //     // /// 元素排列方向
        //     // /// </summary>
        //     // public Direction ListingDirection = Direction.Vertical;
        //
        //
        //     /// <summary>
        //     /// 元素排列组
        //     /// </summary>
        //     public HorizontalOrVerticalLayoutGroup LayoutGroup;
        //     private List<InfiniteListData> _datas  = new List<InfiniteListData>();
        //
        //     public List<InfiniteListData> Datas
        //     {
        //         get
        //         {
        //             return _datas;
        //         }
        //     }
        //
        //     // private HashSet<InfiniteListData> _dataIndexs = new HashSet<InfiniteListData>();
        //     private Dictionary<InfiniteListData, InfiniteListElement> _displayElements = new Dictionary<InfiniteListData, InfiniteListElement>();
        //     private HashSet<InfiniteListData> _invisibleList = new HashSet<InfiniteListData>();
        //     private Queue<InfiniteListElement> _elementsPool = new Queue<InfiniteListElement>();
        //
        //     /// <summary>
        //     /// 初始边缘间隔
        //     /// </summary>
        //     private RectOffset _orignRectOffset = new RectOffset();
        //     
        //     /// <summary>
        //     /// 元素之间的间隔
        //     /// </summary>
        //     private float _interval = 5;
        //     
        //     /// <summary>
        //     /// 元素高度
        //     /// </summary>
        //     private float _height = 20;
        //     
        //     /// <summary>
        //     /// 当前数据数量
        //     /// </summary>
        //     public int DataCount
        //     {
        //         get
        //         {
        //             return _datas.Count;
        //         }
        //     }
        //
        //     /// <summary>
        //     /// 是否需要检查view框范围变化
        //     /// </summary>
        //     [SerializeField] private bool needCheckViewLength = true;
        //     
        //     /// <summary>
        //     /// 是否执行节点刷新标记
        //     /// </summary>
        //     private bool _needRefresh = false;
        //
        //     private float currentViewLength;
        //     
        //     
        //     protected override void LateUpdate()
        //     {
        //         base.LateUpdate();
        //         if (needCheckViewLength)
        //         {
        //             CheckViewRectChange();
        //         }
        //         
        //         if (_needRefresh)
        //         {
        //             RefreshScrollContent(); 
        //             _needRefresh = false;
        //         }
        //     }
        //
        //     /// <summary>
        //     /// 显示范围发生变化
        //     /// </summary>
        //     /// <returns></returns>
        //     private void CheckViewRectChange()
        //     {
        //         if (Math.Abs(viewport.rect.size.y - currentViewLength) > 1.0f)
        //         {
        //             currentViewLength = viewport.rect.size.y;
        //             _needRefresh = true;
        //         }
        //     }
        //     
        //     public override void OnDrag(PointerEventData eventData)
        //     {
        //         return;
        //     }
        //
        //     public override void OnBeginDrag(PointerEventData eventData)
        //     {
        //         return;
        //     }
        //     
        //     public override void OnEndDrag(PointerEventData eventData)
        //     {
        //         return;
        //     }
        //
        //     protected override void OnEnable()
        //     {
        //         base.OnEnable();
        //             
        //
        //         currentViewLength = viewport.rect.size.y;
        //         onValueChanged.AddListener((value) => { _needRefresh = true; });
        //
        //
        //         
        //         _interval = LayoutGroup.spacing;
        //         // if (ElementTemplate != null)
        //         // {
        //         //     _height = ((RectTransform)ElementTemplate.transform).rect.height;
        //         // }
        //         //
        //         // if (ListingDirection == Direction.Vertical)
        //         // {
        //         //     LayoutGroup.padding.top = (int)_interval;//首部填充与间隔保持一致方便计算
        //         // }else if (ListingDirection == Direction.Horizontal)
        //         // {
        //         //     LayoutGroup.padding.left = (int)_interval;//首部填充与间隔保持一致方便计算
        //         // }
        //         _orignRectOffset = new RectOffset(LayoutGroup.padding.left,LayoutGroup.padding.right,LayoutGroup.padding.top,LayoutGroup.padding.bottom);
        //     }
        //
        //     protected override void Awake()
        //     {
        //         base.Awake();
        //     
        //     }
        //
        //     /// <summary>
        //     /// 添加一条新的数据到无限列表尾部
        //     /// </summary>
        //     /// <param name="data">无限列表数据</param>
        //     public void AddData(InfiniteListData data,int insertIndex=-1)
        //     {
        //         if(insertIndex>=0 && insertIndex<= _datas.Count)
        //         {
        //             _datas.Insert(insertIndex,data);
        //         }
        //         else
        //         {
        //             _datas.Add(data);
        //         }
        //
        //         _needRefresh = true;
        //     }
        //     
        //     /// <summary>
        //     /// 添加多条新的数据到无限列表尾部
        //     /// </summary>
        //     /// <typeparam name="T">无限列表数据类型</typeparam>
        //     /// <param name="datas">无限列表数据</param>
        //     public void AddData<T>(List<T> datas,int insertIndex=-1) where T : InfiniteListData
        //     {
        //         if(insertIndex>=0 && insertIndex<= _datas.Count)
        //         {
        //             _datas.InsertRange(insertIndex,datas);
        //         }
        //         else
        //         {
        //             for (int i = 0; i < datas.Count; i++)
        //             {
        //                 _datas.Add(datas[i]);
        //             }
        //         }
        //
        //         _needRefresh = true;
        //     }
        //     
        //     /// <summary>
        //     /// 清除/重设所有的无限列表数据
        //     /// </summary>
        //     /// <param name="data"></param>
        //     public void ResetData(InfiniteListData data = null)
        //     {
        //         _datas.Clear();
        //         foreach (var element in _displayElements)
        //         {
        //             RecycleElement(element.Value);
        //         }
        //
        //         _displayElements.Clear();
        //         if (data != null)
        //         {
        //             _datas.Add(data);
        //         }
        //
        //         _needRefresh = true;
        //     }
        //     /// <summary>
        //     /// 清除/重设所有的无限列表数据
        //     /// </summary>
        //     /// <param name="datas"></param>
        //     /// <typeparam name="T"></typeparam>
        //     public void ResetData<T>(List<T> datas= null) where T : InfiniteListData
        //     {
        //         _datas.Clear();
        //         foreach (var element in _displayElements)
        //         {
        //             RecycleElement(element.Value);
        //         }
        //         _displayElements.Clear();
        //         
        //         if (datas != null )
        //         {
        //             for (int i = 0; i < datas.Count; i++)
        //             {
        //                 _datas.Add(datas[i]);
        //             }
        //         }
        //
        //         _needRefresh = true;
        //     }
        //
        //     /// <summary>
        //     /// 替换指定列表数据
        //     /// </summary>
        //     /// <param name="data"></param>
        //     /// <param name="replaceIndex"></param>
        //     public void ReplaceData(InfiniteListData data, int replaceIndex)
        //     {
        //         if (0 <= replaceIndex && replaceIndex < _datas.Count)
        //         {
        //             if (_displayElements.ContainsKey(_datas[replaceIndex]))
        //             {
        //                 RecycleElement(_displayElements[_datas[replaceIndex]]);
        //                 _displayElements.Remove(_datas[replaceIndex]);
        //             }
        //             _datas.RemoveAt(replaceIndex);
        //             _datas.Insert(replaceIndex,data);
        //             _needRefresh = true;
        //         }
        //     }
        //
        //     /// <summary>
        //     /// 替换指定列表数据
        //     /// </summary>
        //     /// <param name="datas"></param>
        //     /// <param name="replaceIndex"></param>
        //     /// <typeparam name="T"></typeparam>
        //     public void ReplaceData<T>(List<T> datas,int replaceIndex) where T : InfiniteListData
        //     {
        //         if (0 <= replaceIndex && replaceIndex < _datas.Count)
        //         {
        //             if (_displayElements.ContainsKey(_datas[replaceIndex]))
        //             {
        //                 RecycleElement(_displayElements[_datas[replaceIndex]]);
        //                 _displayElements.Remove(_datas[replaceIndex]);
        //             }
        //             
        //             _datas.RemoveAt(replaceIndex);
        //             _datas.InsertRange(replaceIndex,datas);
        //             _needRefresh = true;
        //         }
        //     }
        //     
        //     /// <summary>
        //     /// 批量替换指定列表数据 
        //     /// </summary>
        //     /// <param name="data"></param>
        //     /// <param name="replaceIndex"></param>
        //     public void RangeReplaceData(InfiniteListData data, int startIndex ,int count)
        //     {
        //         if (0 <= startIndex && startIndex+count <=_datas.Count)
        //         {
        //             for (int i  = startIndex; i  < startIndex+count; i ++)
        //             {
        //                                 
        //                 if (_displayElements.ContainsKey(_datas[i]))
        //                 {
        //                     RecycleElement(_displayElements[_datas[i]]);
        //                     _displayElements.Remove(_datas[i]);
        //                 }
        //             }
        //             _datas.RemoveRange(startIndex,count);
        //             _datas.Insert(startIndex,data);
        //             _needRefresh = true;
        //         }
        //     }
        //     /// <summary>
        //     /// 批量替换指定列表数据
        //     /// </summary>
        //     /// <param name="datas"></param>
        //     /// <param name="startIndex"></param>
        //     /// <param name="count"></param>
        //     /// <typeparam name="T"></typeparam>
        //     public void RangeReplaceData<T>(List<T> datas, int startIndex ,int count) where T : InfiniteListData
        //     {
        //         if (0 <= startIndex && startIndex+count <_datas.Count)
        //         {
        //             for (int i  = startIndex; i  < startIndex+count; i ++)
        //             {
        //                                 
        //                 if (_displayElements.ContainsKey(_datas[i]))
        //                 {
        //                     RecycleElement(_displayElements[_datas[i]]);
        //                     _displayElements.Remove(_datas[i]);
        //                 }
        //             }
        //             _datas.RemoveRange(startIndex,count);
        //             _datas.InsertRange(startIndex,datas);
        //             _needRefresh = true;
        //         }
        //     }
        //
        //     /// <summary>
        //     /// 移除一条无限列表数据
        //     /// </summary>
        //     /// <param name="data">无限列表数据</param>
        //     public void RemoveData(InfiniteListData data)
        //     {
        //         _datas.Remove(data);
        //         if (_displayElements.ContainsKey(data))
        //         {
        //             RecycleElement(_displayElements[data]);
        //             _displayElements.Remove(data);
        //         }
        //
        //         _needRefresh = true;
        //     }
        //     
        //     /// <summary>
        //     /// 移除一条无限列表数据
        //     /// </summary>
        //     /// <param name="data">无限列表数据</param>
        //     public void RemoveData<T>(List<T> datas) where T : InfiniteListData
        //     {
        //         foreach (var data in datas)
        //         {
        //             _datas.Remove(data);
        //             if (_displayElements.ContainsKey(data))
        //             {
        //                 RecycleElement(_displayElements[data]);
        //                 _displayElements.Remove(data);
        //             }
        //         }
        //
        //         _needRefresh = true;
        //     }
        //     
        //     /// <summary>
        //     /// 刷新滚动列表内容
        //     /// </summary>
        //     protected void RefreshScrollContent()
        //     {
        //
        //         content.sizeDelta = new Vector2(content.sizeDelta.x, _datas.Count * (_height + _interval)+_orignRectOffset.bottom);
        //
        //
        //         RefreshScrollView();
        //     }
        //
        //     /// <summary>
        //     /// 返回指定列表中的Obj
        //     /// </summary>
        //     /// <param name="infiniteListData"></param>
        //     /// <param name="infiniteListElement"></param>
        //     /// <returns></returns>
        //     public bool GetDisplayElement(InfiniteListData infiniteListData ,out  InfiniteListElement infiniteListElement)
        //     {
        //         if (_displayElements.TryGetValue(infiniteListData, out infiniteListElement))
        //         {
        //
        //             return true;
        //         }
        //         else
        //         {
        //             return false;
        //         }
        //     }
        //
        //     /// <summary>
        //     /// 跳转到指定元素位置
        //     /// </summary>
        //     /// <param name="infiniteListData"></param>
        //     /// <param name="infiniteListElement"></param>
        //     /// <returns></returns>
        //     public bool JumpToElement(InfiniteListData infiniteListData, out InfiniteListElement infiniteListElement)
        //     {
        //         if (_displayElements.TryGetValue(infiniteListData, out infiniteListElement))
        //         {
        //             return true;
        //         }
        //         else
        //         {
        //             if (!_datas.Contains(infiniteListData)) return false;
        //             int indexOf = _datas.IndexOf(infiniteListData);
        //             int maxNum = _datas.Count;
        //             int showNum = _displayElements.Count;
        //             float value = 1-Mathf.Clamp01((float)indexOf / (maxNum - showNum));  // 计算value值
        //             normalizedPosition = new Vector2(normalizedPosition.x, value);
        //             
        //
        //             RefreshScrollView();
        //             if (_displayElements.TryGetValue(infiniteListData, out infiniteListElement))
        //             {
        //                 return true;
        //             }
        //             else
        //             {
        //                 return false;
        //             }
        //         }
        //     }
        //     
        //     
        //     
        //     
        //      /// <summary>
        //     /// 刷新滚动视图
        //     /// </summary>
        //     protected void RefreshScrollView() 
        //     {
        //         float originLength = 0;
        //         float viewLength = 0;
        //
        //             originLength = content.anchoredPosition.y;
        //             viewLength = viewport.rect.size.y;
        //        
        //
        //         int originIndex = Mathf.Max((int)(originLength/ (_height + _interval)),0);
        //         ClearInvisibleElement(originLength, viewLength);
        //         LayoutGroup.padding.top = (int)(_height + _interval) * originIndex + _orignRectOffset.top;
        //
        //         int index;
        //         for (index = originIndex; index < _datas.Count; index++)
        //         {
        //             InfiniteListData data = _datas[index];
        //
        //             float viewTopPos = -(LayoutGroup.padding.top + (_height + _interval) * (index - originIndex));                 
        //             float realTopPos = viewTopPos + originLength;
        //             //显示范围判定
        //             if (realTopPos > -viewLength)
        //             {
        //                 if (_displayElements.ContainsKey(data))
        //                 {
        //                     _displayElements[data].transform.SetSiblingIndex(index - originIndex);
        //                     // _displayElements[data].OnUpdateData(this, index, data);
        //                     continue;
        //                 }
        //
        //                 InfiniteListElement element = ExtractIdleElement();
        //
        //
        //                 element.transform.SetSiblingIndex(index - originIndex);
        //                 // element.OnUpdateData(this, index, data);
        //                 _displayElements.Add(data, element);
        //             }
        //             else
        //             {
        //                 break;
        //             }
        //         }
        //         
        //
        //             LayoutGroup.SetLayoutVertical();
        //
        //     }
        //     
        //     
        //      /// <summary>
        //      /// 清理并回收看不见的元素（垂直模式）
        //      /// </summary>
        //      /// <param name="contentP">滚动视图内容位置y</param>
        //      /// <param name="viewLength">滚动视图高度</param>
        //      private void ClearInvisibleElement(float contentP, float viewLength)
        //      {
        //          foreach (var element in _displayElements)
        //          {
        //
        //                  float realY = element.Value.UITransform.anchoredPosition.y + contentP;
        //                  float realTopY = realY + (1 - element.Value.UITransform.pivot.y) *
        //                      element.Value.UITransform.rect.height;
        //                  float realBottomY = realY - element.Value.UITransform.pivot.y *
        //                      element.Value.UITransform.rect.height;
        //
        //                  if (realBottomY < 0 && realTopY > -viewLength)
        //                  {
        //                      continue;
        //                  }
        //                  else
        //                  {
        //                      _invisibleList.Add(element.Key);
        //                  }
        //              
        //          }
        //
        //          if (_invisibleList.Count != 0)
        //          {
        //              foreach (var item in _invisibleList)
        //              {
        //                  RecycleElement(_displayElements[item]);
        //                  _displayElements.Remove(item);
        //              }
        //              _invisibleList.Clear();
        //          }
        //      }
        //
        //      /// <summary>
        //      /// 回收一个无用的无限列表元素
        //      /// </summary>
        //      /// <param name="element">无限列表元素</param>
        //      private void RecycleElement(InfiniteListElement element)
        //      {
        //          element.OnClearData();
        //          element.gameObject.SetActive(false);
        //          _elementsPool.Enqueue(element);
        //      }
        //
        //      
        //     /// <summary>
        //     /// 提取一个空闲的无限列表元素
        //     /// </summary>
        //     /// <returns>无限列表元素</returns>
        //     private InfiniteListElement ExtractIdleElement()
        //     {
        //         InfiniteListElement element;
        //         if (_elementsPool.Count > 0)
        //         {
        //             element = _elementsPool.Dequeue();
        //         }
        //         else
        //         {
        //             // element = Instantiate(ElementTemplate, content);
        //             return null;
        //         }
        //         element.gameObject.SetActive(true);
        //         return element;
        //     }
        //      
        //      
        // }
        //
        // //无限列表的类型
        // public enum Direction
        // {
        //     /// <summary>
        //     /// 水平
        //     /// </summary>
        //     Horizontal,
        //     /// <summary>
        //     /// 垂直
        //     /// </summary>
        //     Vertical
        // }
    }
}