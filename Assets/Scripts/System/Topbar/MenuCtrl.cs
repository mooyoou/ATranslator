using System.Collections.Generic;
using System.Config;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.LookDev;

namespace System.Topbar
{

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
            RegisterEvents();
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

        private void RegisterEvents()
        {
            
            //项目设置激活
            ConfigSystem.ConfigUpdate += ((sender, args) =>
            {
                if(!string.IsNullOrEmpty(ConfigSystem.CurConfigFolderPath))
                {
                    var TargetMenuNode = GetSubMenuNode("ProjectSettings");
                    if (TargetMenuNode != null)
                    {
                        TargetMenuNode.Interactable = true;
                    }
                }
            });


        }

        private MenuNode GetSubMenuNode(string menuName)
        {
            foreach (var topMenu in _topMenuObjs)
            {
                foreach (var subBtn in topMenu.SubMenuBtns)
                {
                    var targetBtn = SearchSubMenu(subBtn.MenuNode, menuName);
                    if (targetBtn != null) return targetBtn;
                }
            }

            return null;
        }

        private MenuNode SearchSubMenu(MenuNode subMenuNode,string menuName)
        {
            if (subMenuNode.MenuName == menuName) return subMenuNode;
            foreach (var btn in subMenuNode.SubNode)
            {
                var tagetNode = SearchSubMenu(btn, menuName);
                if (tagetNode != null) return tagetNode;
            }
            return null;
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
            _topMenus = MenuConfig.TopMenus;
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
