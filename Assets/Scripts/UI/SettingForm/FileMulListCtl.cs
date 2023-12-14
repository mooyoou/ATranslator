using System;
using System.Collections.Generic;
using System.Config;
using System.Linq;
using TMPro;
using UI.InfiniteListScrollRect.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingForm
{
    public class FileMulListCtl : MulListCtl
    {
        [SerializeField] private TextMulListCtl _textMulListCtl;
        public override void OnAddBtnClick()
        {
            base.OnAddBtnClick();
            ProjectConfig newProjectConfig = new ProjectConfig(ConfigSystem.ProjectConfig);
            foreach (string rule in RuleList)
            {
                if(!newProjectConfig.SpecialTextMatchRules.ContainsKey(rule))
                {
                    newProjectConfig.SpecialTextMatchRules.Add(rule,new List<string>());
                }
            }
            //增加新文件规则
            SettingEvent.SettingPanelChange(newProjectConfig);
        }
     

        public override void OnDelBtnClick()
        {
            base.OnDelBtnClick();
            
            ProjectConfig newProjectConfig = new ProjectConfig(ConfigSystem.ProjectConfig);

            List<string> configFileRules = newProjectConfig.SpecialTextMatchRules.Keys.ToList();
            
            foreach (string rule in configFileRules)
            {
                if (!RuleList.Contains(rule))
                {
                    newProjectConfig.SpecialTextMatchRules.Remove(rule);
                }
            }
            SettingEvent.SettingPanelChange(newProjectConfig);
        }

        public override void OnMulBtnClick(string ruleKey)
        {
            base.OnMulBtnClick(ruleKey);
            _textMulListCtl.ShowTextRulesOfFile(ruleKey);
        }

    }
}
