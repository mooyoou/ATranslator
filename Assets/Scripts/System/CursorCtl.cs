using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCtl : MonoBehaviour
{
    public enum CursorType
    {
        normal,
        /// <summary>
        /// 水平
        /// </summary>
        ew,
        /// <summary>
        /// 垂直
        /// </summary>
        ns,
        /// <summary>
        /// 右上左下
        /// </summary>
        nesw,
        /// <summary>
        /// 左上右下
        /// </summary>
        nwse,
    }
    
    
    [SerializeField] private Texture2D cursor_ew;
    [SerializeField] private Texture2D cursor_ns;
    [SerializeField] private Texture2D cursor_nesw;
    [SerializeField] private Texture2D cursor_nwse;
    
    private CursorType curType = CursorType.normal;
    
    private object curLockObj = null;

    private void Awake()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        //TODO 切换成静态事件
        GlobalSubscribeSys.Subscribe("set_cursor", (objects) =>
        {
            try
            {
                CursorType cursorType = CursorType.normal;
                object CommandObj = null;
                if (objects.Length >= 2)
                {
                    cursorType = (CursorType)objects[0];
                    CommandObj = objects[1];
                }
                SetCursor(cursorType, CommandObj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });

        GlobalSubscribeSys.Subscribe("reset_cursor", (objects) =>
        {
            try
            {
                object CommandObj = null;
                if (objects.Length >= 1)
                {
                    CommandObj = objects[0];
                }

                ResetCursor(CommandObj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });


    }    
    public void SetCursor(CursorType cursorType,object CommandObj)
    {
        if (curLockObj != null)
        {
            if(curLockObj!= CommandObj)return;
        }
        
        if (curType != cursorType)
        {
            curLockObj = CommandObj;
            Vector2 cursorHotSpot;
            curType = cursorType;
            switch (cursorType)
            {
                case CursorType.ew:
                    cursorHotSpot= new Vector2(cursor_ew.width / 2, cursor_ew.height / 2);
                    Cursor.SetCursor(cursor_ew,cursorHotSpot, CursorMode.Auto);
                    break;
                case CursorType.ns:
                    cursorHotSpot= new Vector2(cursor_ns.width / 2, cursor_ns.height / 2);
                    Cursor.SetCursor(cursor_ns,cursorHotSpot, CursorMode.Auto);
                    break;
                case CursorType.nesw:
                    cursorHotSpot= new Vector2(cursor_nesw.width / 2, cursor_nesw.height / 2);
                    Cursor.SetCursor(cursor_nesw,cursorHotSpot, CursorMode.Auto);
                    break;
                case CursorType.nwse:
                    cursorHotSpot= new Vector2(cursor_nwse.width / 2, cursor_nwse.height / 2);
                    Cursor.SetCursor(cursor_nwse,cursorHotSpot, CursorMode.Auto);
                    break;
            }
            
        }

    }

    public void ResetCursor(object CommandObj)
    {
        if (curType != CursorType.normal && curLockObj == CommandObj)
        {
            Cursor.SetCursor(null,Vector2.zero, CursorMode.Auto);
            curType = CursorType.normal;
            curLockObj = null;
        }
    }
    
}
