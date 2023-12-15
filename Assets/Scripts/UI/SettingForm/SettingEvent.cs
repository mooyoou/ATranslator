using System;
using System.Collections;
using System.Collections.Generic;
using System.Config;
using UnityEngine;

public static class SettingEvent
{
    public static Action<ProjectConfig> SettingPanelChange;
    public static Func<ProjectConfig> GetPannelSetting;
}
