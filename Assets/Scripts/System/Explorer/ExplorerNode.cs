using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ExplorerNode : MonoBehaviour
{
    [SerializeField]
    private Image RightRow;
    [SerializeField]
    private Image FileIcom;
    [SerializeField]
    private TMP_Text TMPText;

    internal void Init(string FilePath,bool isFolder)
    {
        TMPText.text = Path.GetFileName(FilePath);
        if (isFolder)
        {
            RightRow.enabled = true;
        }
        else
        {
            RightRow.enabled = false;
        }
        
    } 
    
}
