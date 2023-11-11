using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.LookDev;

namespace System.Topbar
{
    public class MenuNode
    {
        public MenuNode(string name,string eventName = null ,List<MenuNode> subMenus=null)
        {
            MenuName = name;
            EventName = string.IsNullOrEmpty(eventName)?name:eventName ;
            SubNode = new List<MenuNode>();
            if (subMenus != null)
            {
                SubNode = new List<MenuNode>();
                foreach (var node in subMenus)
                {
                    SubNode.Add(node);
                }
            }

        }
        public string MenuName;
        public string EventName;
        public List<MenuNode> SubNode;
    }
    public class MenuCtrl : MonoBehaviour ,IPointerClickHandler,IPointerMoveHandler
    {
        [SerializeField]
        private TopMenuBtn TopMenuBtn_P;
        private bool MenuChoosing;
        /// <summary>
        /// 菜单数据树
        /// </summary>
        private Dictionary<string, List<MenuNode>> _topMenus;
        /// <summary>
        /// 实例化菜单Obj
        /// </summary>
        private List<TopMenuBtn> _topMenuObjs;
        private void Awake()
        {
            InitMenus();
            GlobalSubscribeSys.Subscribe("topmenu_close", (object[] objects) =>
            {
                MenuChoosing = false;
                foreach (var topMenuBtn in _topMenuObjs)
                {
                    topMenuBtn.CloseAllSubMenu();
                }
                ResetMenu();
            });
        }
        private void InitMenus()
        {

            RegeisterMenus();
            _topMenuObjs = new List<TopMenuBtn>();
            foreach (var topMenu in _topMenus)
            {
                TopMenuBtn topMenuBtn = Instantiate(TopMenuBtn_P, transform);
                topMenuBtn.Init(topMenu.Key,topMenu.Value);
                _topMenuObjs.Add(topMenuBtn);
            }
        }

        /// <summary>
        /// 可改为读取配置
        /// </summary>
        private void RegeisterMenus()
        {
            _topMenus= new Dictionary<string, List<MenuNode>>();
        
            _topMenus.Add("<u>F</u>ile",new List<MenuNode>()
            {
                new MenuNode("OpenProject","open_new_project"),
                new MenuNode("Settings",null,new List<MenuNode>()
                {
                    new MenuNode("ProjectSettings","open_project_settings"),
                    new MenuNode("GlobalSettings"),
                    new MenuNode("Open2-test3",null,new List<MenuNode>()
                    {
                        new MenuNode("Open2-test4"),
                        new MenuNode("Open2-test5")
                    })
                }), 
                new MenuNode("Open3")
            });
        
            _topMenus.Add("<u>E</u>dit",new List<MenuNode>()
            {
                new MenuNode("Setting")
            });
        
            _topMenus.Add("<u>T</u>ests",new List<MenuNode>()
            {
                new MenuNode("Debug","open_debug_view"),
                new MenuNode("Debug2"),
                new MenuNode("Debug3")
            });
        }
        private void TryOpenMenu(Transform transform)
        {  
            if (transform.TryGetComponent<TopMenuBtn>(out TopMenuBtn topMenuBtn))
            {
                ResetMenu(topMenuBtn);
                topMenuBtn.ActiveBtn();
            }
        }
        
        private void ResetMenu(TopMenuBtn chooseMenuBtn = null)
        {
            foreach (TopMenuBtn topMenuBtn in _topMenuObjs)
            {
                if (chooseMenuBtn != topMenuBtn)
                {
                    
                    topMenuBtn.DisactiveBtn();
                }  
            }
        }
        
        

        public void OnPointerClick(PointerEventData eventData)
        {
            MenuChoosing = true;
            TryOpenMenu(eventData.pointerEnter.gameObject.transform);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (MenuChoosing)
            {
                TryOpenMenu(eventData.pointerEnter.gameObject.transform);
            }
        }

        public void Update()
        {
            if (MenuChoosing)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    PointerEventData eventData = new PointerEventData(EventSystem.current);
                    eventData.position = Input.mousePosition;

                    List<RaycastResult> RaycastResults = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(eventData, RaycastResults);
                    //点击到非menu层时关闭menu
                    if (RaycastResults.Count != 0 && RaycastResults[0].gameObject.layer != 7)
                    {
                        MenuChoosing = false;
                        ResetMenu();
                    }
                    
                }
            }
        }
    }
}
