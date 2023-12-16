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
        private List<MulUnitData> _ruleList = new List<MulUnitData>();
        protected List<string> RuleList
        {
            get
            {
                List<string> keys = _ruleList.Select(data => data.RuleName).ToList();
                return keys;
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

            _ruleList = new List<MulUnitData>();
            foreach (var rule in ruleList)
            {
                _ruleList.Add(new MulUnitData(rule,this));
            }
            infiniteListScrollRect.ResetData(_ruleList);
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
            
            MulUnitData ruleData = _ruleList.FirstOrDefault(data => data.RuleName == newRule);

            if (ruleData!=null)
            {
                infiniteListScrollRect.JumpToElement(ruleData,out InfiniteListElement infiniteListElement);
                (infiniteListElement as MulListBtn)?.PlayErrorAni();
            }
            else
            {
                MulUnitData newRuleData = new MulUnitData(newRule, this);
                _ruleList.Add(newRuleData);
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
            MulUnitData ruleData = _ruleList.FirstOrDefault(data => data.RuleName == CurChooseRule);
            if (ruleData!=null)
            {
                _ruleList.Remove(ruleData);
                infiniteListScrollRect.RemoveData(ruleData);
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
            
            ruleData = _ruleList.FirstOrDefault(data => data.RuleName == CurChooseRule);
            //取消原按钮
            if ( !string.IsNullOrEmpty(CurChooseRule)&&ruleData!=null)
            {
                ruleData.IsChoose = false;
                if (infiniteListScrollRect.GetDisplayElement(ruleData, out InfiniteListElement infiniteListElement))
                {
                    (infiniteListElement as MulListBtn)?.RefreshBtn();
                }
            }
            //选中新按钮
            ruleData  = _ruleList.FirstOrDefault(data => data.RuleName == ruleKey);
            if (ruleData!=null)
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
