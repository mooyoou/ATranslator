using System;
using System.Config;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingForm
{
   public class SettingForm : MonoBehaviour
   {
      [SerializeField] private MulListCtl ruleList;
      [SerializeField] private Toggle skipHideFolderBtn;
      [SerializeField] private Button saveBtn;
      [SerializeField] private Button cancelBtn;
      
      
      private ProjectConfig _projectConfig;
      
      private void Awake()
      {
         // ConfigSystem.ConfigUpdate += (sender, args) =>
         // {
         //    LoadSettings();
         // };
      }

      public void OnSaveBtnClick()
      {
         var loadRules = ruleList.RuleList.Keys.ToList();
         var skipHideFolder = skipHideFolderBtn.isOn;
         _projectConfig = new ProjectConfig(skipHideFolder, loadRules);
         ConfigSystem.ProjectConfig = _projectConfig;
      }

      public void OnCancleBtnClick()
      {
         gameObject.SetActive(false);
      }

      private void OnEnable()
      {
         LoadSettings();
      }

      private void LoadSettings()
      {
         _projectConfig = ConfigSystem.ProjectConfig;
         skipHideFolderBtn.isOn = _projectConfig.SkipHideFolder;
         ruleList.InitRuleList(_projectConfig.LoadRules.ToList());
      }
   
   }
}
