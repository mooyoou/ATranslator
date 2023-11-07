using System.Collections.Generic;
using System.IO;
using UI.InfiniteListScrollRect;
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
                SubExplorerNodes.Add(new ExplorerNodeData(subdirectory,true,Depth+1,this));
            }
            foreach (string fileEntry in fileEntries)
            {
                SubExplorerNodes.Add(new ExplorerNodeData(fileEntry,false,Depth+1,this));
            }
        
        }
    }
}
