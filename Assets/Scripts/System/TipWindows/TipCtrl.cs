using System.Diagnostics.Tracing;
using System.Security.AccessControl;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace System.TipWindows
{
    public class TipCtrl : MonoBehaviour
    {
        [SerializeField] private Image mask;
        [SerializeField] private Animation maskAni;
        [SerializeField] private TMP_Text tipTitle;
        [SerializeField] private TMP_Text tipInfo;
        [SerializeField] private Image progressFill;
        [SerializeField] private Animation progressRun;
        [SerializeField] private Button cancelBtn;
        
        private string _cancelEvent;
        private void Awake()
        {
            RegisterEvent();
            ResetTip();
        }

        private void RegisterEvent()
        {
            GlobalSubscribeSys.Subscribe("open_tips_window", (objects) =>
            {
                try
                {
                    string title="";
                    string info="";
                    bool needMask=true;
                    string cancleEvent="";
                    float percent = -1;
                    if (objects.Length >= 3)
                    {
                        title = objects[0] as string;
                        info = objects[1] as string;
                        needMask = objects[2] as bool? ?? false;

                        if (objects.Length >= 4)
                        {
                            cancleEvent = objects[3] as string;
                        }

                        if (objects.Length >= 5)
                        {
                            percent = objects[4] as float? ?? -1;
                        }


                        InvokeTipWindows(title,info, needMask,cancleEvent,percent);
                        
                    }
                }
                catch (Exception e)
                {
                    string _parents = "";
                    foreach (var VARIABLE in objects)
                    {
                        _parents += $"{VARIABLE.ToString()} ";
                    }
                    Debug.LogError($"Event\"open_tips_window\" invoked failed with parents : {_parents}");
                }
            });

            GlobalSubscribeSys.Subscribe("close_tips_window",(objects)=>
            {
                ResetTip();
            });
            
            GlobalSubscribeSys.Subscribe("update_tip_info",(objects)=>
            {
                string tiptitle = "";
                string tipinfo = "";
                float fill = -1;
                if (objects.Length >= 1)
                {
                    tiptitle= objects[0] as string;
                }else if (objects.Length >= 2)
                {
                    tipinfo= objects[1] as string;
                }else if (objects.Length >= 3)
                {
                    fill = objects[1] as float? ?? -1;
                }
                UpdateTipInfo(tiptitle,tipinfo,fill);
            });
            
        }

        private void InvokeTipWindows(string title,string info,bool needMask,string cancelEvent="",float percent = -1)
        {
            gameObject.SetActive(true);
            tipTitle.text = title;
            tipInfo.text = info;
            if (string.IsNullOrEmpty(cancelEvent))
            {
                cancelBtn.interactable = false;
            }
            else
            {
                cancelBtn.interactable = true;
                _cancelEvent = cancelEvent;
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

        private void UpdateTipInfo(string tiptitle, string tipinfo = "",float fill = -1)
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


        public void OnCancelClick()
        {
            if (!string.IsNullOrEmpty(_cancelEvent))
            {
                GlobalSubscribeSys.Invoke(_cancelEvent);
            }

            ResetTip();
        }

        public void OnTipDrag(BaseEventData eventData)
        {
            PointerEventData pointerEventData = eventData as PointerEventData;

        }
        
        private void ResetTip()
        {
            gameObject.SetActive(false);
            tipTitle.text = "";
            tipInfo.text = "";
        }

    }
}
