using System.Collections;
using System.Collections.Generic;
using System.TipWindows;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TipForm : MonoBehaviour
{
    [SerializeField] private Image mask;
    [SerializeField] private Animation maskAni;
    [SerializeField] private TMP_Text tipTitle;
    [SerializeField] private TMP_Text tipInfo;
    [SerializeField] private Image progressFill;
    [SerializeField] private Animation progressRun;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private TMP_Text btnText;
    [SerializeField] private Transform tipWindow;
    
    internal string CancelEvent;
    private int _tipId;
    private TipCtrl _tipCtrl;
    public void Init(int id,TipCtrl tipCtrl,string title,string info,bool needMask,string cancelEvent="",float percent = -1)
    {
        _tipId = id;
        _tipCtrl = tipCtrl;
        gameObject.SetActive(true);
        tipTitle.text = title;
        tipInfo.text = info;
        if (string.IsNullOrEmpty(cancelEvent))
        {
            cancelBtn.interactable = false;
            btnText.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
        }
        else
        {
            cancelBtn.interactable = true;
            CancelEvent = cancelEvent;
            btnText.color = new Color(0.8f, 0.8f, 0.8f, 1);
        }
            
        mask.gameObject.SetActive(needMask);
        if (needMask)
        {
            maskAni.Play();
        }

        if (percent >= 0)
        {
            progressFill.gameObject.SetActive(true);
            progressRun.gameObject.SetActive(false);
            progressFill.fillAmount = percent;
        }
        else
        {
            progressFill.gameObject.SetActive(false);
            progressRun.gameObject.SetActive(true);
            progressRun.Play();
        }
            
    }
    
    internal void UpdateTipInfo(string tiptitle="", string tipinfo = "",float fill = -1)
    {
        if (!string.IsNullOrEmpty(tiptitle)) tipTitle.text = tiptitle;
        if (!string.IsNullOrEmpty(tiptitle)) tipInfo.text = tipinfo;
        if (fill >= 0)
        {
            progressFill.gameObject.SetActive(true);
            progressRun.gameObject.SetActive(false);
            progressFill.fillAmount = fill;
        }
    }
    
    /// <summary>
    /// 取消按钮事件
    /// </summary>
    public void OnCancelClick()
    {
        if (!string.IsNullOrEmpty(CancelEvent))
        {
            GlobalSubscribeSys.Invoke(CancelEvent);
        }
        _tipCtrl.TipCancle(_tipId);
    }
    
    
    public void OnTipDrag(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        Vector2 dragDelta = pointerEventData.delta;
        Vector3 targetPosition = tipWindow.position + new Vector3(dragDelta.x, dragDelta.y, 0);
        Rect screenBounds = new Rect(0, 0, Screen.width, Screen.height);
        if (screenBounds.Contains(targetPosition))
        {
            tipWindow.position = targetPosition;
        }
    }

    
    private void ResetTip()
    {
        gameObject.SetActive(false);
        tipTitle.text = "";
        tipInfo.text = "";
    }
    
    
}
