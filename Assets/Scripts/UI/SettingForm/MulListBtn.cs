using System;
using TMPro;
using UI.InfiniteListScrollRect.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingForm
{
    public class MulListBtn : InfiniteListElement 
    {
        [SerializeField ]private TMP_InputField btnText;
        [SerializeField] private RawImage BackColor;
        [SerializeField] private Animation flashAni;
        [SerializeField] private AnimationClip singleFlashAniClip;
        [SerializeField] private AnimationClip doubleFlashAniClip;
        
        private MulUnitData _mulUnitData;
        private MulListCtl _mulListCtl;

        private void Awake()
        {
            flashAni.AddClip(singleFlashAniClip,"SingleFlashAniClip");
            flashAni.AddClip(doubleFlashAniClip,"DoubleFlashAniClip");
        }

        private void OnEnable()
        {
            btnText.onSelect.AddListener(OnBtnClick);
        }

        private void OnDisable()
        {
            btnText.onSelect.RemoveAllListeners();
        }

        public override void OnUpdateData(InfiniteListScrollRect.Runtime.InfiniteListScrollRect scrollRect, int index, InfiniteListData data)
        {
            base.OnUpdateData(scrollRect, index, data);
            _mulUnitData = (MulUnitData)data;
            _mulListCtl = _mulUnitData.MulListCtl;
            btnText.text = _mulUnitData.RuleName;
            SetBtnState(_mulUnitData.IsChoose);
        }

        public void RefreshBtn()
        {
            _mulListCtl = _mulUnitData.MulListCtl;
            btnText.text = _mulUnitData.RuleName;
            SetBtnState(_mulUnitData.IsChoose);
        }
        
        public void SetBtnState(bool isChoose)
        {
            _mulUnitData.IsChoose = isChoose;
            BackColor.enabled = isChoose;
        }

        public void OnBtnClick(string rule)
        {
            SetBtnState(true);
            _mulListCtl.OnMulBtnClick(_mulUnitData.RuleName);
        }
        
        /// <summary>
        /// 错误动画
        /// </summary>
        public void PlayErrorAni()
        {
            flashAni.Play("DoubleFlashAniClip");
        }
        
        public void PlayTipAni()
        {
            flashAni.Play("SingleFlashAniClip");
        }
    
    }
}
