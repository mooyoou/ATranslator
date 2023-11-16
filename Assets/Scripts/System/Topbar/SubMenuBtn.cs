using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace System.Topbar
{
    public class SubMenuBtn : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
    {
        [SerializeField]
        private TMP_Text btnName;

        [SerializeField]
        private Image backImg;
        public Image rightRow;
        [SerializeField]
        private Transform subMenuRoot;

        private SubMenuBtn _subMenuBtnP;
        

        private bool _isSubMenuRoot;
        private MenuNode _menuNode;
        public MenuNode MenuNode
        {
            get
            {
                return _menuNode;
            }
        }
        
        
        private List<SubMenuBtn> _subMenuBtns = new List<SubMenuBtn>();

        public List<SubMenuBtn> SubMenuBtns
        {
            get
            {
                return _subMenuBtns;
            }
        }


        public void Init(MenuNode menuNode,SubMenuBtn subMenuBtnP)
        {
            _subMenuBtnP = subMenuBtnP;
            btnName.text = menuNode.MenuName;
            _menuNode = menuNode;

            btnName.color = _menuNode.Interactable
                ? new Color(0.8f, 0.8f, 0.8f, 1.0f)
                : new Color(0.2f, 0.2f, 0.2f, 1.0f); 
            
            if (menuNode.SubNode.Count!=0)
            {
                InitSubMenu(menuNode.SubNode);
            }

            RegisterEvent();

        }
        
        /// <summary>
        /// 顶部菜单栏选项的横向子菜单 
        /// </summary>
        /// <param name="subMenu"></param>
        private void InitSubMenu(List<MenuNode> subMenu)
        {
            _isSubMenuRoot = true;
            rightRow.transform.gameObject.SetActive(true);
            foreach (var sMenuNode in subMenu)
            {
                SubMenuBtn subMenuBtn = Instantiate(_subMenuBtnP, subMenuRoot);
                subMenuBtn.Init(sMenuNode,_subMenuBtnP);
                _subMenuBtns.Add(subMenuBtn);
            }
        }

        public void RegisterEvent()
        {
        }

        private void OnEnable()
        {
            UpdateBtnState();
        }
        
        public void UpdateBtnState()
        {
            btnName.color = _menuNode.Interactable
                ? new Color(0.8f, 0.8f, 0.8f, 1.0f)
                : new Color(0.4f, 0.4f, 0.4f, 1.0f);
        }
        
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!_menuNode.Interactable)return;
            backImg.color = new Color(0.3f, 0.4f, 0.5f, 1.0f);
            if(_isSubMenuRoot){
                subMenuRoot.gameObject.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(!_menuNode.Interactable)return;
            backImg.color = new Color(0.3f, 0.4f, 0.5f, 0.0f);
            if(_isSubMenuRoot){
                subMenuRoot.gameObject.SetActive(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(!_menuNode.Interactable)return;
            if (!_isSubMenuRoot)
            {
                GlobalSubscribeSys.Invoke(_menuNode.EventName);
                GlobalSubscribeSys.Invoke("topmenu_close");
            }
        }

        internal void CloseAllSubMenu()
        {
            subMenuRoot.gameObject.SetActive(false);
            backImg.color = new Color(0.3f, 0.4f, 0.5f, 0.0f);
            foreach (var subMenuBtn in _subMenuBtns)
            {
                subMenuBtn.CloseAllSubMenu();
            }
        }
        
    }
}