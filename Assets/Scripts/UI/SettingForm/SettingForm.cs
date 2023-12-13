using System.Config;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingForm
{
   public class SettingForm : MonoBehaviour
   {
      [SerializeField] private MulListCtl ruleList;
      [SerializeField] private Toggle skipHideFolderBtn;
      [SerializeField] private Toggle displaySpecificFileTypesBtn;
      [SerializeField] private Button saveBtn;
      [SerializeField] private TMP_Text saveBtnText; 
      [SerializeField] private Button cancelBtn;

      private ProjectConfig _projectConfig;

      public void OnSaveBtnClick()
      {
         var loadRules = ruleList.RuleList;
         var skipHideFolder = skipHideFolderBtn.isOn;
         var onlyShowFileSpecify = displaySpecificFileTypesBtn.isOn;
         _projectConfig = new ProjectConfig(skipHideFolder,onlyShowFileSpecify, loadRules );
         ConfigSystem.ProjectConfig = _projectConfig;
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
         displaySpecificFileTypesBtn.isOn = _projectConfig.DisplaySpecificFileTypes;
         ruleList.InitRuleList(_projectConfig.FileMatchRules.ToList());
      }


      
   }
}
