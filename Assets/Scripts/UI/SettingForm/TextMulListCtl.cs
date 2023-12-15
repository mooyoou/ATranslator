using System;
using System.Collections.Generic;
using System.Config;
using System.Data;
using System.Linq;
using TMPro;
using UI.InfiniteListScrollRect.Runtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingForm
{
    public class TextMulListCtl : MulListCtl
    {

        private string _curFileRule;

        public override void OnAddBtnClick()
        {
            base.OnAddBtnClick();
            if(string.IsNullOrEmpty(_curFileRule))return;
            ProjectConfig newProjectConfig = SettingEvent.GetPannelSetting();
            if (newProjectConfig.SpecialTextMatchRules.TryGetValue(_curFileRule, out List<string> configTextRule))
            {
                newProjectConfig.SpecialTextMatchRules[_curFileRule] = new List<string>(RuleList);
            }
            SettingEvent.SettingPanelChange(newProjectConfig);
        }
     

        public override void OnDelBtnClick()
        {
            base.OnDelBtnClick();
            if(string.IsNullOrEmpty(_curFileRule))return;
            ProjectConfig newProjectConfig = SettingEvent.GetPannelSetting();
            if (newProjectConfig.SpecialTextMatchRules.TryGetValue(_curFileRule, out List<string> configTextRule))
            {
                newProjectConfig.SpecialTextMatchRules[_curFileRule] = new List<string>(RuleList);
            }
            SettingEvent.SettingPanelChange(newProjectConfig);
        }

        public override void OnMulBtnClick(string ruleKey)
        {
            base.OnMulBtnClick(ruleKey);
        }

        /// <summary>
        /// 显示指定文件类型的独特pattern
        /// </summary>
        /// <param name="fileRule"></param>
        public void ShowTextRulesOfFile(string fileRule)
        {
            if (_curFileRule == fileRule)
            {
                return;
            }
            ProjectConfig newProjectConfig = SettingEvent.GetPannelSetting();
            if (newProjectConfig.SpecialTextMatchRules.TryGetValue(fileRule, out List<string> textRules))
            {
                InitRuleList(textRules);
                _curFileRule = fileRule;
            }
        }
        
    }
}
