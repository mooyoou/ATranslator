using System.Config;
using System.Windows.Forms;
using UnityEngine;

namespace System
{
    public class MainCtl : MonoBehaviour
    {
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
        }

    }
}
