using TMPro;
using UI.InfiniteListScrollRect.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingForm
{
    public class MulListBtn : InfiniteListElement 
    {
        [SerializeField]private TMP_Text btnText;
        [SerializeField]private Button button;
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
            var buttonColors = button.colors;
            buttonColors.normalColor = isChoose ? new Color(1f, 1f, 1f, 0.1f): new Color(1f, 1f, 1f, 0);
            button.colors = buttonColors;
        }

        public void OnBtnClick()
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
