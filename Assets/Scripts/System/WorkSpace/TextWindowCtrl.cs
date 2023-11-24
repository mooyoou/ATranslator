using System.Collections.Generic;
using System.Config;
using System.Explorer;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UI.DynamicScrollView.Runtime.ScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace System.WorkSpace
{
    public class TextWindowCtrl : MonoBehaviour
    {
        [SerializeField] private ScrollView _scrollView;
        [SerializeField] private TextLineCtl textLinePrefab;
        private ExplorerNodeData curExplorerNodeData;

        private List<TextLineData> _textLineDatas = new List<TextLineData>();
        
        private float previousViewHeight;
        public void Awake()
        {
            _scrollView.SetUpdateFunc(UpdateTextLine);
            _scrollView.SetItemSizeFunc(GetTexttLinSize);
            _scrollView.SetItemCountFunc(GetLineCount);
            previousViewHeight = _scrollView.viewport.rect.height;
        }

        private void Update()
        {
            var newhight = _scrollView.viewport.rect.height;
            if (Mathf.Abs(newhight - previousViewHeight) > 1)
            {
                previousViewHeight = newhight;
                _scrollView.UpdateData(false);
            };
        }
        
        void UpdateTextLine(int index, RectTransform item)
        {
            item.gameObject.SetActive(true);
            item.transform.GetComponent<TextLineCtl>().UpdateLine(_textLineDatas[index],(() => {_scrollView.UpdateData(false);}));
        }
        
        Vector2 GetTexttLinSize(int index)
        {
            textLinePrefab.gameObject.SetActive(true);
            textLinePrefab.UpdateLine(_textLineDatas[index]);
            RectTransform rectTransform = textLinePrefab.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            float preferredHeight = 0;
            if (rectTransform != null)
            { 
                preferredHeight = LayoutUtility.GetPreferredHeight(rectTransform);
            }
            textLinePrefab.gameObject.SetActive(false);
            return new Vector2(0, preferredHeight);
        }
        
        int GetLineCount()
        {
            return _textLineDatas.Count;
        }
        
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="explorerNodeData"></param>
        public void OpenFile(ExplorerNodeData explorerNodeData)
        {
            if (!File.Exists(explorerNodeData.FullPath))return;
            using (StreamReader reader = new StreamReader(explorerNodeData.FullPath))
            {
                _textLineDatas.Clear();
                string regexPattern = ConfigSystem.ProjectConfig.GetTextMatchRegex(explorerNodeData.FileName);
                MatchCollection  matches ;
                int matchNum = 0;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    matches = Regex.Matches(line,regexPattern);
                    TextLineData textLineData;
                    if (matches.Count > 0)
                    {
                        
                        List<TextLineData.MatchPos> matchPosList = new List<TextLineData.MatchPos>();
                        
                        foreach (Match match in matches)
                        {
                            TextLineData.MatchPos matchPos = new TextLineData.MatchPos(match.Index, match.Length);
                            matchPosList.Add(matchPos);
                        }
                        textLineData = new TextLineData(matchNum++, line, matchPosList);
                    }
                    else
                    {
                        textLineData = new TextLineData(line);
                    }
                    _textLineDatas.Add(textLineData);
                }
            }
            _scrollView.UpdateData(false);
        }
        
        
        
        
    }
}
