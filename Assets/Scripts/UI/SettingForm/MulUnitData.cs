using UI.InfiniteListScrollRect.Runtime;

namespace UI.SettingForm
{
    public class MulUnitData : InfiniteListData
    {
        public string RuleName ;
        public MulListCtl MulListCtl;
        public bool IsChoose;
        public MulUnitData (string text ,MulListCtl mulListCtl)
        {
            IsChoose = false;
            RuleName = text;
            MulListCtl = mulListCtl;
        }
    }
}
