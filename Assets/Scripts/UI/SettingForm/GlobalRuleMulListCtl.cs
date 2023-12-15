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
    public class GlobalRuleMulListCtl : MulListCtl
    {
        public override void OnAddBtnClick()
        {
            base.OnAddBtnClick();
            ProjectConfig newProjectConfig = SettingEvent.GetPannelSetting();
            foreach (string rule in RuleList)
            {
                if(!newProjectConfig.GlobalTextMatchRules.Contains(rule))
                {
                    newProjectConfig.GlobalTextMatchRules.Add(rule);
                }
            }
            //增加新文件规则
            SettingEvent.SettingPanelChange(newProjectConfig);
        }
     

        public override void OnDelBtnClick()
        {
            base.OnDelBtnClick();
            
            ProjectConfig newProjectConfig = SettingEvent.GetPannelSetting();

            List<string> configFileRules = new List<string>(newProjectConfig.GlobalTextMatchRules) ;
            
            foreach (string rule in configFileRules)
            {
                if (!RuleList.Contains(rule))
                {
                    newProjectConfig.GlobalTextMatchRules.Remove(rule);
                }
            }
            SettingEvent.SettingPanelChange(newProjectConfig);
        }
        
    }
}
