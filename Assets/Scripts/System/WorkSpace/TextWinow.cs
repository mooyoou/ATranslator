using System.Collections;
using System.Collections.Generic;
using System.Config;
using System.Explorer;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.WorkSpace
{
    public class TextWinow : MonoBehaviour
    {
        [SerializeField] private UIDocument textVisualElement;
        private ExplorerNodeData _explorerNodeData;
        private List<TextLineData> _textLineDatas;
        /// <summary>
        /// 当前打开文件的UI根节点
        /// </summary>
        private ScrollView _textAreaScrollView;

        public ScrollView TextAreaScrollView
        {
            get
            {
                if(_textAreaScrollView == null) _textAreaScrollView = textVisualElement.rootVisualElement.Q<ScrollView>("TextAreaContainer");
                return _textAreaScrollView;
            }
        }
            
        private int _tipId = -1;


        /// <summary>
        /// 生成窗口
        /// </summary>
        public bool InitWindow(ExplorerNodeData explorerNodeData)
        {
            if (!File.Exists(explorerNodeData.FullPath)) return false;
            
            OpenOrUpdateTip("File Opening",$"{explorerNodeData.FileName} analyzing...");
            StartCoroutine(OpenFileAsync(explorerNodeData));
            return true;
        }
        
        /// <summary>
        /// 序列化保存
        /// </summary>
        public void CheckAndSaveFile()
        {
            
        }

        public void SetDisplay(bool display)
        {
            TextAreaScrollView.style.display = display ? DisplayStyle.Flex : DisplayStyle.None;
        }
        
        
        /// <summary>
        /// 初始化界面
        /// </summary>
        /// <param name="explorerNodeData"></param>
        /// <returns></returns>
        private IEnumerator OpenFileAsync(ExplorerNodeData explorerNodeData)
        {
            _textLineDatas = new List<TextLineData>();
            _textLineDatas = InitFileData(explorerNodeData);
            Task initFileTask = Task.Run(() =>
            {
                _textLineDatas = InitFileData(explorerNodeData);
            });
            yield return new WaitUntil(() => initFileTask.IsCompleted);
            CloseTip();
            StartCoroutine(InitTextWindow(_textLineDatas));
        }
        
        
        
        /// <summary>
        /// 分析文件获取翻译数据
        /// </summary>
        /// <param name="explorerNodeData"></param>
        /// <returns></returns>
        private List<TextLineData> InitFileData(ExplorerNodeData explorerNodeData)
        {
            List<TextLineData> textLineDatas = new List<TextLineData>();
            string fileContent = File.ReadAllText(explorerNodeData.FullPath);
            string regexPattern = ConfigSystem.ProjectConfig.GetTextMatchRegex(explorerNodeData.FileName);
            MatchCollection matches = null;
            try
            {
                matches = Regex.Matches(fileContent, regexPattern,ConfigSystem.ProjectRegexOptions);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error message: {e.Message}\nStack trace: {e.StackTrace}");
            }

            int curMatchIndex = 0;
            int matchShowNumber = 0;
            int lineNum = 0;
            int preLength = 0;
            string storageText = "";
            
            string[] lines = fileContent.Split(ConfigSystem.ProjectConfig.SplitChar); // 按行分割文本
            for (int i = 0; i < lines.Length; i++)
            {
                if (matches==null || curMatchIndex > matches.Count - 1)
                {
                    //已处理所有匹配，剩余行数直接合并
                    for(int index = i; index < lines.Length; index++)
                    {
                        storageText += (!string.IsNullOrEmpty(storageText)?ConfigSystem.ProjectConfig.SplitChar:"") + lines[index];
                    }
                    TextLineData textLineData = new TextLineData(++lineNum,storageText);
                    textLineDatas.Add(textLineData);
                    break;
                }
                int matchStartIndex = matches[curMatchIndex].Index;
                int matchEndIndex = math.max(matchStartIndex,matchStartIndex + matches[curMatchIndex].Length-1);
                int curLineStartIndex = preLength;
                int curLineEndIndex = math.max(curLineStartIndex,(preLength += lines[i].Length+ConfigSystem.ProjectConfig.SplitChar.Length)-1);
                
                if (curLineEndIndex < matchStartIndex)
                {
                    //当前检索行不存在匹配字符串
                    // ^------$  m
                    storageText +=  (!string.IsNullOrEmpty(storageText)?ConfigSystem.ProjectConfig.SplitChar:"") + lines[i];
                    continue;
                }
                else
                {
                    //开始出现匹配行
                    List<TextLineData.MatchPos> matchPosList = new List<TextLineData.MatchPos>();

                    if (!string.IsNullOrEmpty(storageText))
                    {
                        TextLineData textLineData = new TextLineData(++lineNum,storageText);
                        textLineDatas.Add(textLineData);
                        storageText = "";
                    } //归档原文类数据

                    int matchStartLineIndex = curLineStartIndex;
                    while (matchStartIndex <= curLineEndIndex)
                    {
                        if (matches[curMatchIndex].Groups.Count > 1)
                        {
                            for (int groupIndex = 1; groupIndex <  matches[curMatchIndex].Groups.Count; groupIndex++)
                            {
                                Group group = matches[curMatchIndex].Groups[groupIndex];
                                bool isIgnoreGroup = group.Name == ConfigSystem.IgnoreGroupName ||
                                                     Regex.IsMatch(group.Name,
                                                         @$"^{ConfigSystem.IgnoreGroupName}_\d+$");
                                if (group.Success && !isIgnoreGroup)
                                {
                                    matchPosList.Add(new TextLineData.MatchPos(group.Index - matchStartLineIndex,group.Length));
                                }
                            }
                        }
                        else
                        {
                            matchPosList.Add(new TextLineData.MatchPos(matches[curMatchIndex].Index - matchStartLineIndex,matches[curMatchIndex].Length));
                        }

                        
                        //逐行检索直至匹配文本尾部，将最后一行之外全部存储
                        while (matchEndIndex > curLineEndIndex)
                        {
                            storageText += (!string.IsNullOrEmpty(storageText)?ConfigSystem.ProjectConfig.SplitChar:"") + lines[i];
                            i++;
                            curLineStartIndex  = preLength;
                            curLineEndIndex = math.max(curLineStartIndex, (preLength += lines[i].Length+ConfigSystem.ProjectConfig.SplitChar.Length) - 1);
                        }
                        

                        curMatchIndex++;
                        if (curMatchIndex > matches.Count - 1) break;
                        matchStartIndex = matches[curMatchIndex].Index;
                        matchEndIndex = math.max(matchStartIndex, matchStartIndex + matches[curMatchIndex].Length);
                    }
                    storageText +=  (!string.IsNullOrEmpty(storageText)?ConfigSystem.ProjectConfig.SplitChar:"") +lines[i];//存储最后一行
                    if (!string.IsNullOrEmpty(storageText))
                    {
                        TextLineData textLineData = new TextLineData(++lineNum,++matchShowNumber, storageText,matchPosList);
                        textLineDatas.Add(textLineData);
                        storageText = "";
                    } //归档匹配数据
                }

            }
            return textLineDatas;
        }
        
        /// <summary>
        /// 生成界面元素
        /// </summary>
        /// <param name="fileData"></param>
        private IEnumerator InitTextWindow(List<TextLineData> fileData)
        {
            textVisualElement.gameObject.SetActive(true);
            //TODO根据匹配条数设置页码列宽度
            TextAreaScrollView.Clear();
            // _textLines.Clear();
            int refreshCount = 0;
            int hasInitNum = 0;
            foreach (var data in fileData)
            {
                hasInitNum++;
                var textLine = new TextLine(data);
                // _textLines.Add(textLine);
                TextAreaScrollView.Add(textLine);
                if (refreshCount -- < 0)
                {
                    refreshCount = (int)(1 / Time.deltaTime);
                    OpenOrUpdateTip("File Opening",$"{hasInitNum} lines inited...");
                    yield return new WaitForEndOfFrame();
                }
            }
            CloseTip();
        }
    
                
        /// <summary>
        /// 打开提示窗口
        /// TODOTip更新和打开集成
        /// </summary>
        /// <param name="folderPath"></param>
        private void OpenOrUpdateTip(string title,string tipinfo)
        {
            if (_tipId >= 0)
            {
                UpdateTip(title, tipinfo);
                return;
            }
            GlobalSubscribeSys.Invoke("open_tips_window",out List<object> ids,new System.Object[]
            {
                title,
                tipinfo,
                true
            });
            _tipId = ids[0] as int? ?? 0;
        }

                
        /// <summary>
        /// 关闭提示窗口
        /// </summary>
        private void CloseTip()
        {
            if (_tipId >= 0) GlobalSubscribeSys.Invoke("close_tips_window",_tipId);
            _tipId = -1;
        }
        
        
        private void UpdateTip(string title,string tipinfo)
        {
            GlobalSubscribeSys.Invoke("update_tip_info",new System.Object[]
            {
                _tipId,
                title,
                tipinfo
            });
        }
        
    }
}
