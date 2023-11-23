using System.Collections.Generic;
using System.Config;
using System.Explorer;
using System.IO;
using System.Security;
using System.Text.RegularExpressions;
using UI.DynamicScrollView.Runtime.ScrollView;
using UnityEngine;

namespace System.WorkSpace
{
    public class TextWindowCtrl : MonoBehaviour
    {
        [SerializeField] private ScrollView _scrollView;

        private ExplorerNodeData curExplorerNodeData;

        private List<TextLineData> _textLineDatas = new List<TextLineData>();

        public void OpenFile(ExplorerNodeData explorerNodeData)
        {
            if (!File.Exists(explorerNodeData.FullPath))return;
            using (StreamReader reader = new StreamReader(explorerNodeData.FullPath))
            {
                string regexPattern = ConfigSystem.ProjectConfig.GetTextMatchRegex(explorerNodeData.FileName);
                MatchCollection  matches ;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    matches = Regex.Matches(line,regexPattern);
                    if (matches.Count > 0)
                    {
                        foreach (Match match in matches)
                        {
                            Console.WriteLine("Match found: " + match.Value);
                        }
                    }
                }
            }
            
            
        //序列化文本
        //根据设定规则筛选可翻译行
        //更新无限列表
            
        }
    }
}
