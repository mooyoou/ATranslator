using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ExplorerCtrl : MonoBehaviour
{
    [SerializeField] 
    private Transform explorerNodeRoot;
    [SerializeField] 
    private ExplorerNode explorerNodeP;

    private List<ExplorerNode> _explorerNodes = new List<ExplorerNode>();
    private void Awake()
    {
        RegisterEvent();
    }

    public void RegisterEvent()
    {
        GlobalSubscribeSys.Subscribe("open_new_project", (objects) =>
        {
            OpenNewProject();
        });


    }
    
    private void OpenNewProject()
    {
        string folderPath = EditorUtility.OpenFolderPanel("Choose Folder", "", "");
        if(string.IsNullOrEmpty(folderPath)) return;
        ResetNodeList();
        ProcessDirectory(folderPath);

    }

    //可改为对象池模式
    private void ResetNodeList()
    {
        foreach (var explorerNode in _explorerNodes)
        {
            Destroy(explorerNode.gameObject);
        }

        _explorerNodes = new List<ExplorerNode>();
    }

    private void ProcessDirectory(string targetDirectory)
    {
                
        string[] fileEntries = Directory.GetFiles(targetDirectory);
        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
        {
            ExplorerNode explorerNode = Instantiate(explorerNodeP, explorerNodeRoot);
            explorerNode.Init(subdirectory,true);
            _explorerNodes.Add(explorerNode);
        }
        
        foreach (string fileName in fileEntries)
        {
            ExplorerNode explorerNode = Instantiate(explorerNodeP, explorerNodeRoot);
            explorerNode.Init(fileName,false);
            _explorerNodes.Add(explorerNode);
        }
        

    }
    
}
