using System.Collections;
using System.Collections.Generic;
using System.Topbar;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class MenuConfig
{
    public static Dictionary<string, List<MenuNode>> TopMenus = new Dictionary<string, List<MenuNode>>()
    {
        {"<u>F</u>ile",new List<MenuNode>(){
            new MenuNode("OpenProject","open_new_project"),
            new MenuNode("Settings",null,new List<MenuNode>()
            {
                new MenuNode("ProjectSettings","open_project_settings",null,false),
                new MenuNode("GlobalSettings"),
                new MenuNode("Open2-test3",null,new List<MenuNode>()
                {
                    new MenuNode("Open2-test4"),
                    new MenuNode("Open2-test5")
                })
            }), 
            new MenuNode("Open3") }
        },
        {"<u>E</u>dit",new List<MenuNode>(){
            new MenuNode("Setting")}
        },
        {"<u>T</u>ests",new List<MenuNode>()
            {
                new MenuNode("Debug","open_debug_view"),
                new MenuNode("Debug2"),
                new MenuNode("Debug3")
            }
        }
    };
    
    
    
    
    
}
