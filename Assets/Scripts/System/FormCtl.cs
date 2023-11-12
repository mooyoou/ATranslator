using DebugSystem;
using UnityEngine;
using UnityEngine.UI;

namespace System
{
    public class FormSystem : MonoBehaviour
    {

        [SerializeField] private DebugConsole debugConsole;
        [SerializeField] private Image Mask;
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
                debugConsole.gameObject.SetActive(true);
            });
        }

    }
}

