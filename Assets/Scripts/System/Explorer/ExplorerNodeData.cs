using System.Collections.Generic;
using System.Config;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UI.InfiniteListScrollRect.Runtime;
using UnityEngine;


namespace System.Explorer
{
    public class ExplorerNodeData : InfiniteListData
    {

        public String FileName;
        public String FullPath;
        public bool IsFolder;
        public int Depth;
        public List<ExplorerNodeData> SubExplorerNodes=new List<ExplorerNodeData>();
        public ExplorerNodeData FatherExplorerNode;
        public FileInfo FileInfo;

        public bool IsExpand;
        
        public ExplorerNodeData(string fullPath,bool isFolder, int depth = 0, ExplorerNodeData fnode = null)
        {
            if (!Directory.Exists(fullPath) && !File.Exists(fullPath))
            {
                return;
            }
            FullPath = fullPath;
            FileName = Path.GetFileName(fullPath);
            Depth = depth;
            FatherExplorerNode = fnode;
            IsFolder = isFolder;
            if (isFolder)
            {
                UpdateSubNodes();
            }
            else
            {
                FileInfo = new FileInfo(fullPath);
            }
        }

        public void UpdateSubNodes()
        {
            if(!IsFolder)return;
            string[] fileEntries = Directory.GetFiles(FullPath);
            string[] subdirectoryEntries = Directory.GetDirectories(FullPath);
            foreach (string subdirectory in subdirectoryEntries)
            {
                if (ConfigSystem.ProjectConfig.SkipHideFolder)
                {
                    
                    DirectoryInfo folderInfo = new DirectoryInfo(subdirectory);

                    if (HideCheck(folderInfo) //隐藏文件夹跳过
                        || subdirectory == ConfigSystem.CurConfigFolderPath)// 不读取配置文件夹
                    {
                        continue;
                    }

                    SubExplorerNodes.Add(new ExplorerNodeData(subdirectory,true,Depth+1,this));
                }
            }
            
            string regexRule = ConfigSystem.ProjectConfig.RuleRegex;
            
            
            foreach (string fileEntry in fileEntries)
            {
                FileInfo fileInfo = new FileInfo(fileEntry);


                if (CheckFileType(fileInfo.Name,regexRule))
                {
                    SubExplorerNodes.Add(new ExplorerNodeData(fileEntry,false,Depth+1,this));
                }
            }
        
        }

        private bool CheckFileType(string fileName,string regexRule)
        {
            Regex regex = new Regex(regexRule);
            Match match = regex.Match(fileName);
            if (match.Success) return true;
            return false;
        }

        private bool HideCheck(DirectoryInfo folderInfo)
        {
            bool isHidden = ((folderInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden);
            if (ConfigSystem.ProjectConfig.SkipHideFolder && isHidden) return true;

            return false;
        }
        
    }
}
