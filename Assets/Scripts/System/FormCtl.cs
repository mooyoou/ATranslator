using DebugSystem;
using UI.SettingForm;
using UnityEngine;
using UnityEngine.UI;

namespace System
{
    public class FormSystem : MonoBehaviour
    {

        [SerializeField] private DebugConsole debugForm;
        [SerializeField] private SettingForm settingForm;
        private void Awake()
        {
            ApplicationInit();
            RegisterEvent();
        }

        private void ApplicationInit()
        {
        }
    
        private void RegisterEvent()
        {
            GlobalSubscribeSys.Subscribe("open_debug_view", (objects) =>
            {
                debugForm.gameObject.SetActive(true);
            });
            GlobalSubscribeSys.Subscribe("open_project_settings", (objects) =>
            {
                settingForm.gameObject.SetActive(true);
            });
        }

    }
}

