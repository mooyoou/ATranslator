
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using UnityEngine;

public class TextLineData
{
    /// <summary>
    /// 是否为匹配数据（可翻译）
    /// </summary>
    public bool IsRawText
    {
        get
        {
            return _isRawText;
        }
    }
    private bool _isRawText;
    

     /// <summary>
     /// 原始文本
     /// </summary>
    private string _rawText;
    public string RawText
    {
        get
        {
            if (_matchPosList.Count > 0)
            {
                //TODO捕获文本突出处理
            }
            
            return _rawText;
        }
    }

    
    private List<MatchPos> _matchPosList = new List<MatchPos>();

    public List<MatchPos> MatchPosList
    {
        get
        {
            return _matchPosList;
        }
    }

    public struct MatchPos
    {
        private int _beginPos;

        public int BeginPos
        {
            get
            {
                return _beginPos;
            }
        }
        
        private int _endPos;

        public int EndPos
        {
            get
            {
                return _endPos;
            }
        }

        public MatchPos(int beginPos, int endPos)
        {
            beginPos = Mathf.Max(0, beginPos);
            endPos = Mathf.Max(beginPos, endPos);
            _beginPos = beginPos;
            _endPos = endPos;
        }
    }
    
    /// <summary>
    /// 翻译文本
    /// </summary>
    public string TranslationText
    {
        get
        {
            return _translationText;
        }
        set
        {
            _translationText = value;
        }
    }
    private string _translationText;
    
    /// <summary>
    /// 匹配序号
    /// </summary>
    private int _matchNum = -1;
    public int MatchNum
    {
        get { return _matchNum; }
    }


    /// <summary>
    /// 设置翻译行
    /// </summary>
    /// <param name="matchNum"></param>
    /// <param name="rawText"></param>
    /// <param name="translationText"></param>
    public TextLineData(int matchNum,string rawText,List<MatchPos> matchPosList, string translationText = "")
    {
        _isRawText = false;
        _matchNum = matchNum;
        _rawText = rawText;
        _matchPosList = matchPosList;
        TranslationText = translationText;

    }
    
    /// <summary>
    /// 设置原文行
    /// </summary>
    /// <param name="rawText"></param>
    public TextLineData(string rawText)
    {
        _isRawText = true;
        _rawText = rawText;
        _translationText = "";
    }
}
