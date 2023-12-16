using System.Collections;
using System.Collections.Generic;
using System.Config;
using System.Explorer;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.WorkSpace
{
    public class TextWindowCtrl : MonoBehaviour
    {
        [SerializeField] private UIDocument textArea;

        public bool InitFileDataTest;
        
        private ScrollView _textAreaScrollView;

        private Dictionary<ExplorerNodeData, List<TextLineData>> _projectFileDates = new Dictionary<ExplorerNodeData, List<TextLineData>>();
        private int _tipId = -1;
        private List<TextLine> _textLines = new List<TextLine>();
        
        private const string splitChar = "\n";
        private void Start()
        {
            textArea.gameObject.SetActive(false);
        }
        public void CloseWin()
        {
            _textAreaScrollView.Clear();
            _textAreaScrollView = null;
            textArea.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="explorerNodeData"></param>
        public void OpenFile(ExplorerNodeData explorerNodeData)
        {
            if (!File.Exists(explorerNodeData.FullPath)) return;
            OpenOrUpdateTip("File Opening",$"{explorerNodeData.FileName} analyzing...");
            StartCoroutine(OpenFileAsync(explorerNodeData));
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        /// <param name="explorerNodeData"></param>
        /// <returns></returns>
        private IEnumerator OpenFileAsync(ExplorerNodeData explorerNodeData)
        {
            List<TextLineData> fileDate = new List<TextLineData>();
            Task initFileTask = Task.Run(() =>
            {
                
                if (_projectFileDates.ContainsKey(explorerNodeData) && !InitFileDataTest )
                {
                    fileDate = _projectFileDates[explorerNodeData];
                }
                else
                {
                    fileDate = InitFileData(explorerNodeData);
                    _projectFileDates.Add(explorerNodeData,fileDate);
                }
                
            });
            yield return new WaitUntil(() => initFileTask.IsCompleted);
            CloseTip();
            StartCoroutine(InitTextWindow(fileDate));
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
            MatchCollection matches = Regex.Matches(fileContent, regexPattern, ConfigSystem.ProjectRegexOptions );
            int curMatchIndex = 0;
            int matchShowNumber = 0;
            int preLength = 0;
            string storageText = "";
            
            string[] lines = fileContent.Split(splitChar); // 按行分割文本
            for (int i = 0; i < lines.Length; i++)
            {
                if (curMatchIndex > matches.Count - 1)
                {
                    //已处理所有匹配，剩余行数直接合并
                    for(int index = i; index < lines.Length; index++)
                    {
                        storageText += (!string.IsNullOrEmpty(storageText)?splitChar:"") + lines[index];
                    }
                    TextLineData textLineData = new TextLineData(storageText);
                    textLineDatas.Add(textLineData);
                    break;
                }
                int matchStartIndex = matches[curMatchIndex].Index;
                int matchEndIndex = math.max(matchStartIndex,matchStartIndex + matches[curMatchIndex].Length-1);
                int curLineStartIndex = preLength;
                int curLineEndIndex = math.max(curLineStartIndex,(preLength += lines[i].Length+splitChar.Length)-1);
                
                if (curLineEndIndex < matchStartIndex)
                {
                    //当前检索行不存在匹配字符串
                    // ^------$  m
                    storageText +=  (!string.IsNullOrEmpty(storageText)?splitChar:"") + lines[i];
                    continue;
                }
                else
                {
                    //开始出现匹配行
                    List<TextLineData.MatchPos> matchPosList = new List<TextLineData.MatchPos>();

                    if (!string.IsNullOrEmpty(storageText))
                    {
                        TextLineData textLineData = new TextLineData(storageText);
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
                                if (group.Success)
                                {
                                    matchPosList.Add(new TextLineData.MatchPos(group.Index - matchStartLineIndex,group.Length));
                                    break;
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
                            storageText +=   (!string.IsNullOrEmpty(storageText)?splitChar:"") + lines[i];
                            i++;
                            curLineStartIndex  = preLength;
                            curLineEndIndex = math.max(curLineStartIndex, (preLength += lines[i].Length+splitChar.Length) - 1);
                        }
                        

                        curMatchIndex++;
                        if (curMatchIndex > matches.Count - 1) break;
                        matchStartIndex = matches[curMatchIndex].Index;
                        matchEndIndex = math.max(matchStartIndex, matchStartIndex + matches[curMatchIndex].Length);
                    }
                    storageText +=  (!string.IsNullOrEmpty(storageText)?splitChar:"") +lines[i];//存储最后一行
                    if (!string.IsNullOrEmpty(storageText))
                    {
                        TextLineData textLineData = new TextLineData(++matchShowNumber, storageText,matchPosList);
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

            
            textArea.gameObject.SetActive(true);
            if(_textAreaScrollView == null) _textAreaScrollView = textArea.rootVisualElement.Q<ScrollView>("TextAreaContainer");
            //TODO根据匹配条数设置页码列宽度
            _textAreaScrollView.Clear();
            _textLines.Clear();
            int refreshCount = 0;
            int hasInitNum = 0;
            foreach (var data in fileData)
            {
                hasInitNum++;
                var textLine = new TextLine(data);
                _textLines.Add(textLine);
                _textAreaScrollView.Add(textLine);
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
        /// 关闭提示窗口
        /// </summary>
        private void CloseTip()
        {
            if (_tipId >= 0) GlobalSubscribeSys.Invoke("close_tips_window",_tipId);
            _tipId = -1;
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

