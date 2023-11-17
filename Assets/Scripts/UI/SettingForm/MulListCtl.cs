using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI.InfiniteListScrollRect.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingForm
{
    public class MulListCtl : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Animation flushAnimation;
        [SerializeField] private InfiniteListScrollRect.Runtime.InfiniteListScrollRect infiniteListScrollRect;
        [SerializeField] private Button delButton;
        
        [SerializeField] private SettingForm settingForm;
        
        
        private Dictionary<string,MulUnitData> _ruleList = new Dictionary<string, MulUnitData>();

        public List<string> RuleList
        {
            get
            {
                return _ruleList.Keys.ToList();
            }
        }

        private string _curChooseRule;


        public void InitRuleList(List<string> ruleList)
        {
            if (ruleList==null)
            {
                return;
            }
            _ruleList = new Dictionary<string, MulUnitData>();
            foreach (var rule in ruleList)
            {
                _ruleList.Add(rule,new MulUnitData(rule,this));
            }
            infiniteListScrollRect.ResetData(_ruleList.Values.ToList());
        }
        
        /// <summary>
        /// 增加按钮
        /// </summary>
        public void OnAddBtnClick()
        {
            if(!IsInputHasText())return;
            string newRule = inputField.text;
            if (_ruleList.TryGetValue(newRule,out MulUnitData ruleData))
            {
                infiniteListScrollRect.JumpToElement(ruleData,out InfiniteListElement infiniteListElement);
                (infiniteListElement as MulListBtn)?.PlayErrorAni();
            }
            else
            {
                MulUnitData newRuleData = new MulUnitData(newRule, this);
                _ruleList.Add(newRule,newRuleData);
                infiniteListScrollRect.AddData(newRuleData);
                infiniteListScrollRect.JumpToElement(newRuleData,out InfiniteListElement infiniteListElement);
                (infiniteListElement as MulListBtn)?.PlayTipAni();
            }
            inputField.text = null;
            settingForm.SetSaveBtn(true);
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <returns></returns>
        public void OnDelBtnClick()
        {
            if (_ruleList.TryGetValue(_curChooseRule, out MulUnitData mulUnitData))
            {
                _ruleList.Remove(_curChooseRule);
                infiniteListScrollRect.RemoveData(mulUnitData);
                delButton.interactable = false;
                _curChooseRule = null;
            }
            settingForm.SetSaveBtn(true);
        }

        /// <summary>
        /// 列表选择
        /// </summary>
        /// <param name="ruleKey"></param>
        public void OnMulBtnClick(string ruleKey)
        {
            if (_curChooseRule == ruleKey) return;
            //取消原按钮
            if ( !string.IsNullOrEmpty(_curChooseRule)&&_ruleList.TryGetValue(_curChooseRule, out MulUnitData curRuleData))
            {
                curRuleData.IsChoose = false;
                if (infiniteListScrollRect.GetDisplayElement(curRuleData, out InfiniteListElement infiniteListElement))
                {
                    (infiniteListElement as MulListBtn)?.RefreshBtn();
                }
            }
            //选中新按钮
            if (_ruleList.TryGetValue(ruleKey,out MulUnitData ruleData))
            {
                ruleData.IsChoose = true;
                if (infiniteListScrollRect.GetDisplayElement(ruleData, out InfiniteListElement newInfiniteListElement))
                {
                    (newInfiniteListElement as MulListBtn)?.RefreshBtn();
                }
            }

            _curChooseRule = ruleKey;
            //删除按钮激活
            delButton.interactable = true;
        }
        
        private bool IsInputHasText()
        {
            if (string.IsNullOrEmpty(inputField.text))
            {
                flushAnimation.Play();
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
