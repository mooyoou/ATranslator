using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace System.Topbar
{
    public class TopMenuBtn :  MonoBehaviour  
    {
        [SerializeField]
        private GameObject dropdownMenuRoot;
        [SerializeField]
        private TMP_Text btnName;
        [SerializeField]
        private Image backImg;
        [SerializeField]
        private SubMenuBtn subMenuBtnP;
        
        private List<SubMenuBtn> _subMenuBtns;

        public List<SubMenuBtn> SubMenuBtns
        {
            get
            {
                return _subMenuBtns;
            }
        }

        public void Init(string menuName,List<MenuNode>  subMenu)
        {
            btnName.text = menuName;
            InitSubMenu(subMenu);
        }
        private void InitSubMenu(List<MenuNode>  subMenu)
        {
            _subMenuBtns = new List<SubMenuBtn>();
            foreach (var sMenuNode in subMenu)
            {
                SubMenuBtn firstSubMenuBtn = Instantiate(subMenuBtnP, dropdownMenuRoot.transform);
                firstSubMenuBtn.Init(sMenuNode,subMenuBtnP);
                _subMenuBtns.Add(firstSubMenuBtn);
            }
        }
        
        public void DisactiveBtn()
        {
            dropdownMenuRoot.SetActive(false);
            backImg.color = new Color(0.3f, 0.4f, 0.5f, 0.0f);
        }
        
        public void ActiveBtn()
        {
            dropdownMenuRoot.SetActive(true);

            backImg.color = new Color(0.3f, 0.4f, 0.5f, 1.0f);
        }

        internal void CloseAllSubMenu()
        {
            foreach (var subMenuBtn in _subMenuBtns)
            {
                subMenuBtn.CloseAllSubMenu();
            }
        }
    }
}
