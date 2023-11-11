using System.ProjectConfig;
using DebugSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace System
{
    public class MainSystem : MonoBehaviour
    {

        [SerializeField]
        private DebugConsole debugConsole;
        
        
        private void Awake()
        {
            ApplicationInit();
            RegisterEvent();
        }

        private void ApplicationInit()
        {
            ConfigSystem.RefreshGlobbalPlayerPrefs();
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
