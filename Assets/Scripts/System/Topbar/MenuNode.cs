using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MenuNode
{
    public MenuNode(string name, string eventName = null, List<MenuNode> subMenus = null,bool interactable = true)
    {
        MenuName = name;
        EventName = string.IsNullOrEmpty(eventName) ? name : eventName;
        SubNode = new List<MenuNode>();
        if (subMenus != null)
        {
            SubNode = new List<MenuNode>();
            foreach (var node in subMenus)
            {
                SubNode.Add(node);
            }
        }

        Interactable = interactable;
    }

    public string MenuName;
    public string EventName;
    public List<MenuNode> SubNode;
    public bool Interactable;
}
