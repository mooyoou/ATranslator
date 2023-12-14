using System;
using System.Collections.Generic;
using System.Config;
using System.Data;
using System.Linq;
using TMPro;
using UI.InfiniteListScrollRect.Runtime;
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
            ProjectConfig newProjectConfig = new ProjectConfig(ConfigSystem.ProjectConfig);
            if (newProjectConfig.SpecialTextMatchRules.TryGetValue(_curFileRule, out List<string> configTextRule))
            {
                foreach (string rule in RuleList)
                {
                    if(!configTextRule.Contains(rule))
                    {
                        newProjectConfig.SpecialTextMatchRules[_curFileRule].Add(rule);
                    }
                }
            }
            SettingEvent.SettingPanelChange(newProjectConfig);
        }
     

        public override void OnDelBtnClick()
        {
            base.OnDelBtnClick();
            if(string.IsNullOrEmpty(_curFileRule))return;
            ProjectConfig newProjectConfig = new ProjectConfig(ConfigSystem.ProjectConfig);
            if (newProjectConfig.SpecialTextMatchRules.TryGetValue(_curFileRule, out List<string> configTextRule))
            {
                for (int i = configTextRule.Count-1; i >=0 ; i--)
                {
                    if(!RuleList.Contains(configTextRule[i]))
                    {
                        int delIndex = newProjectConfig.SpecialTextMatchRules[_curFileRule].IndexOf(configTextRule[i]);
                        newProjectConfig.SpecialTextMatchRules[_curFileRule].RemoveAt(delIndex);
                    }
                }
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
            if (ConfigSystem.ProjectConfig.SpecialTextMatchRules.TryGetValue(fileRule, out List<string> textRules))
            {
                InitRuleList(textRules);
                _curFileRule = fileRule;
            }
        }
        
    }
}
