using System;
using System.Collections.Generic;
using System.Config;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingForm
{
   public class SettingForm : MonoBehaviour
   {
      [SerializeField] private FileMulListCtl fileRuleList;
      [SerializeField] private Toggle skipHideFolderBtn;
      [SerializeField] private Toggle displaySpecificFileTypesBtn;
      [SerializeField] private Button saveBtn;
      [SerializeField] private TMP_Text saveBtnText; 
      [SerializeField] private Button cancelBtn;

      private ProjectConfig _projectConfig;
      
      public void OnSaveBtnClick()
      {
         if (_projectConfig != ConfigSystem.ProjectConfig)
         {
            ConfigSystem.ProjectConfig = _projectConfig;
         }
         gameObject.SetActive(false);
      }

      public void OnCancleBtnClick()
      {
         gameObject.SetActive(false);
      }

      public void OnSkipHideFolderBtnClick(bool value)
      {
         ProjectConfig newProjectConfig = new ProjectConfig(ConfigSystem.ProjectConfig);
         newProjectConfig.SkipHideFolder = value;
         SettingEvent.SettingPanelChange(newProjectConfig);
      }
      
      public void OnDisplaySpecificFileTypesBtnClick(bool value)
      {
         ProjectConfig newProjectConfig = new ProjectConfig(ConfigSystem.ProjectConfig);
         newProjectConfig.DisplaySpecificFileTypes = value;
         SettingEvent.SettingPanelChange(newProjectConfig);
      }
      
      
      private void OnEnable()
      {
         LoadSettings();
         SetSaveBtn(false);
         RegisterEvent();
      }

      private void OnDisable()
      {
         UnRegisterEvent();
      }

      private void RegisterEvent()
      {
         SettingEvent.SettingPanelChange += (configSystem) => { SettingChangeCheck(configSystem);} ;
         cancelBtn.onClick.AddListener(OnCancleBtnClick);
         saveBtn.onClick.AddListener(OnSaveBtnClick);
         skipHideFolderBtn.onValueChanged.AddListener(OnSkipHideFolderBtnClick);
         displaySpecificFileTypesBtn.onValueChanged.AddListener(OnDisplaySpecificFileTypesBtnClick);
      }
      
      private void UnRegisterEvent()
      {
         SettingEvent.SettingPanelChange = null ;
         cancelBtn.onClick.RemoveAllListeners();
         saveBtn.onClick.RemoveAllListeners();
         skipHideFolderBtn.onValueChanged.RemoveAllListeners();
         displaySpecificFileTypesBtn.onValueChanged.RemoveAllListeners();
      }


      private void SettingChangeCheck(ProjectConfig newConfig)
      {
         _projectConfig = newConfig;
         if (newConfig.Equals(ConfigSystem.ProjectConfig))
         {
            SetSaveBtn(false);
         }
         else
         {
            SetSaveBtn(true);
         }
      }
      
      private void SetSaveBtn(bool active)
      {
         saveBtn.interactable = active;
         saveBtnText.color = active ? new Color(0.8f, 0.8f, 0.8f, 1) : new Color(0.2f, 0.2f, 0.2f, 1);
      }
      
      private void LoadSettings()
      {
         _projectConfig = ConfigSystem.ProjectConfig;
         skipHideFolderBtn.isOn = _projectConfig.SkipHideFolder;
         displaySpecificFileTypesBtn.isOn = _projectConfig.DisplaySpecificFileTypes;
         List<string> fileMatchRules = _projectConfig.SpecialTextMatchRules.Keys.ToList();
         fileRuleList.InitRuleList(fileMatchRules);
         if(_projectConfig.SpecialTextMatchRules.Count>0)
         {
            fileRuleList.OnMulBtnClick(fileMatchRules[0]);
         }
      }
   }
}
