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
    public class MulListCtl : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Animation flashAnimation;
        [SerializeField] private InfiniteListScrollRect.Runtime.InfiniteListScrollRect infiniteListScrollRect;
        [SerializeField] private Button delButton;
        [SerializeField] private Button addButton;
        private Dictionary<string,MulUnitData> _ruleList = new Dictionary<string, MulUnitData>();
        protected List<string> RuleList
        {
            get
            {
                return _ruleList.Keys.ToList();
            }
        }

        protected string CurChooseRule;

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnRegisterEvents();
            CurChooseRule = null;
        }

        private void RegisterEvents()
        {
            delButton.onClick.AddListener(OnDelBtnClick);
            addButton.onClick.AddListener(OnAddBtnClick);
        } 
        
        private void UnRegisterEvents()
        {
            delButton.onClick.RemoveAllListeners();
            addButton.onClick.RemoveAllListeners();
        }         
        
        internal void InitRuleList(List<string> ruleList)
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
            delButton.interactable = false;
            CurChooseRule = null;
            inputField.text = null;
        }
        
        /// <summary>
        /// 增加按钮
        /// </summary>
        public virtual  void OnAddBtnClick()
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
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <returns></returns>
        public virtual void OnDelBtnClick()
        {
            if (_ruleList.TryGetValue(CurChooseRule, out MulUnitData mulUnitData))
            {
                _ruleList.Remove(CurChooseRule);
                infiniteListScrollRect.RemoveData(mulUnitData);
                delButton.interactable = false;
                CurChooseRule = null;
            }

        }

        /// <summary>
        /// 列表选择
        /// </summary>
        /// <param name="ruleKey"></param>
        public virtual void OnMulBtnClick(string ruleKey)
        {
            MulUnitData ruleData;
            if (CurChooseRule == ruleKey)
            {
                return;
            }
            //取消原按钮
            if ( !string.IsNullOrEmpty(CurChooseRule)&&_ruleList.TryGetValue(CurChooseRule, out MulUnitData curRuleData))
            {
                curRuleData.IsChoose = false;
                if (infiniteListScrollRect.GetDisplayElement(curRuleData, out InfiniteListElement infiniteListElement))
                {
                    (infiniteListElement as MulListBtn)?.RefreshBtn();
                }
            }
            //选中新按钮
            if (_ruleList.TryGetValue(ruleKey,out ruleData))
            {
                ruleData.IsChoose = true;
                if (infiniteListScrollRect.GetDisplayElement(ruleData, out InfiniteListElement newInfiniteListElement))
                {
                    (newInfiniteListElement as MulListBtn)?.RefreshBtn();
                }
            }

            CurChooseRule = ruleKey;
            //删除按钮激活
            delButton.interactable = true;
        }
        
        private bool IsInputHasText()
        {
            if (string.IsNullOrEmpty(inputField.text))
            {
                flashAnimation.Play();
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
