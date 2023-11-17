using System;
using System.Config;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.SettingForm
{
   public class SettingForm : MonoBehaviour
   {
      [SerializeField] private MulListCtl ruleList;
      [SerializeField] private Toggle skipHideFolderBtn;
      [SerializeField] private Button saveBtn;
      [SerializeField] private TMP_Text saveBtnText; 
      [SerializeField] private Button cancelBtn;

      private ProjectConfig _projectConfig;

      public void OnSaveBtnClick()
      {
         var loadRules = ruleList.RuleList;
         var skipHideFolder = skipHideFolderBtn.isOn;
         _projectConfig = new ProjectConfig(skipHideFolder, loadRules);
         ConfigSystem.ProjectConfig = _projectConfig;
         GlobalSubscribeSys.Invoke("refresh_explorer_list");
         gameObject.SetActive(false);
      }

      public void OnCancleBtnClick()
      {
         gameObject.SetActive(false);
      }

      private void OnEnable()
      {
         LoadSettings();
         SetSaveBtn(false);
      }

      public void SetSaveBtn(bool active)
      {
         saveBtn.interactable = active;
         saveBtnText.color = active ? new Color(0.8f, 0.8f, 0.8f, 1) : new Color(0.2f, 0.2f, 0.2f, 1);
      }
      
      private void LoadSettings()
      {
         _projectConfig = ConfigSystem.ProjectConfig;
         skipHideFolderBtn.isOn = _projectConfig.SkipHideFolder;
         ruleList.InitRuleList(_projectConfig.LoadRules.ToList());
      }


      
   }
}
