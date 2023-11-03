using System.Collections.Generic;
using UnityEngine;

namespace System.TipWindows
{
    public class TipCtrl : MonoBehaviour
    {
        [SerializeField] private TipForm _tipFormP;

        private Dictionary<int, TipForm> tips = new Dictionary<int, TipForm>();

        private void Awake()
        {
            RegisterEvent();
        }

        private void RegisterEvent()
        {
            GlobalSubscribeSys.Subscribe("open_tips_window", (objects) =>
            {
                int id = 0;
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
                        if (objects.Length >= 4) cancleEvent = objects[3] as string;
                        if (objects.Length >= 5) percent = objects[4] as float? ?? -1;
                        
                        id = GetRandomId();
                        TipForm tipForm = Instantiate(_tipFormP, transform);
                        tips.Add(id,tipForm);
                        tipForm.Init(id,this,title,info,needMask,cancleEvent,percent);
                    }
                }
                catch (Exception e)
                {
                    string _parents = "";
                    foreach (var VARIABLE in objects)
                    {
                        _parents += $"{VARIABLE.ToString()} ";
                    }
                    Debug.LogError($"Event \"open_tips_window\" invoked failed : {_parents}:{e}");
                }
                return id;
            });

            GlobalSubscribeSys.Subscribe("close_tips_window",(objects)=>
            {
                if (objects.Length > 0)
                {
                    TipClose(objects[0] as int? ?? 0);
                }
                
            });
            
            GlobalSubscribeSys.Subscribe("update_tip_info",(objects)=>
            {
                int tipId=0;
                string tiptitle = "";
                string tipinfo = "";
                float fill = -1;
                if (objects.Length >= 1) tipId= objects[0] as int? ?? 0;
                if (objects.Length >= 2) tiptitle= objects[1] as string;
                if (objects.Length >= 3) tipinfo= objects[2] as string;
                if (objects.Length >= 4) fill = objects[3] as float? ?? -1;
                UpdateTip(tipId,tiptitle,tipinfo,fill);
            });
            
        }

        internal void TipCancle(int id)
        {
            if (tips.TryGetValue(id,out TipForm tipForm))
            {
                if(!string.IsNullOrEmpty(tipForm.CancelEvent)) GlobalSubscribeSys.Invoke(tipForm.CancelEvent);
                Destroy(tipForm.gameObject);
                tips.Remove(id);
            }
        }

        internal void TipClose(int id)
        {
            if (tips.TryGetValue(id,out TipForm tipForm))
            {
                Destroy(tipForm.gameObject);
                tips.Remove(id);
            }
        }
        
        internal void UpdateTip(int id,string tipTitle,string tipinfo,float fill)
        {
            if (tips.TryGetValue(id,out TipForm tipForm))
            {
                tipForm.UpdateTipInfo(tipTitle, tipinfo, fill);
            }
        }
        
        private int GetRandomId()
        {
            int seed = Environment.TickCount;
            UnityEngine.Random.InitState(seed);
            int id = UnityEngine.Random.Range(0, 1000);
            while (tips.ContainsKey(id))
            {
                seed = Environment.TickCount;
                UnityEngine.Random.InitState(seed);
                id = UnityEngine.Random.Range(0, 1000);
            }

            return id;
        } 

        






    }
}
