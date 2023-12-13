using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.WorkSpace
{
    public class TextLine : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TextLine,UxmlTraits>{}
        

        public static readonly string UssClassName = "textline";
        public static readonly string NumUssClassName = "textline__num";
        public static readonly string RawTextUssClassName = "textline__raw-text";
        public static readonly string TransTextUssClassName = "textline__trans-text";
        // public static readonly string rawTextMaskedUssClassName = "textline__raw-text--masked";
        
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            // UxmlIntAttributeDescription _mNum =
            //     new UxmlIntAttributeDescription { name = "number", defaultValue = 999 };
            // UxmlStringAttributeDescription _mRawText =
            //     new UxmlStringAttributeDescription { name = "raw-text", defaultValue = "未翻译文本Untranslatedtext未翻訳のテキスト번역되지 않은 텍스트" };
            // UxmlStringAttributeDescription _mTransText =
            //     new UxmlStringAttributeDescription { name = "trans-text", defaultValue = "" };
            // UxmlBoolAttributeDescription _mNeedTrans=
            //     new UxmlBoolAttributeDescription { name = "need-trans", defaultValue = true };
            // public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            // {
            //     base.Init(ve, bag, cc);
            //     var tl = ve as TextLine;
            //
            //     tl.Number = _mNum.GetValueFromBag(bag, cc);
            //     tl.RawText = _mRawText.GetValueFromBag(bag, cc);
            //     tl.TransText = _mTransText.GetValueFromBag(bag, cc);
            //     tl.NeedTranslation = _mNeedTrans.GetValueFromBag(bag, cc);
            //     
            // }
        }
        // public int Number { get; set; }
        // public string RawText { get; set; }
        // public string TransText { get; set; }
        // public bool NeedTranslation { get; set; }

        
        private Label _mNumLabel;
        private Label _mRawLabel;
        private TextField _mRawInputField;
        private TextField _mTransInputField;

        
        public TextLineData _textLineData; 
        
         public TextLine()
        {
            var textLineData = new TextLineData(99999, "Default Raw Text", null, "Default Translation Text");
            //var textLineData = new TextLineData("");
            Init(textLineData);
        } 

        public TextLine(TextLineData textLineData)
        {
            _textLineData = textLineData;
            Init(textLineData);
        }

        private void Init(TextLineData textLineData)
        {
            AddToClassList(UssClassName);
            
            _mNumLabel = new Label();
            _mNumLabel.name = "MatchNum";
            _mNumLabel.AddToClassList(NumUssClassName);
            if (textLineData.IsRawText)
            {
                _mNumLabel.text = "";
            }
            else
            {
                _mNumLabel.text = textLineData.MatchNum.ToString();
            }

            Add(_mNumLabel);
            
            _mRawLabel = new Label(textLineData.RawText);
            _mRawLabel.name = "RawText";
            _mRawLabel.AddToClassList(RawTextUssClassName);
            _mRawLabel.selection.isSelectable = true;
            Add(_mRawLabel);

            
            if (!textLineData.IsRawText)
            {
                _mTransInputField = new TextField();
                _mTransInputField.name = "TransText";
                _mTransInputField.AddToClassList(TransTextUssClassName);
                _mTransInputField.multiline = true;
                _mTransInputField.value = textLineData.TranslationText??"";
                _mTransInputField.RegisterValueChangedCallback(evt =>
                {
                    if (_textLineData != null) _textLineData.TranslationText = evt.newValue;
                });
                Add(_mTransInputField);
            }

        }

    }
    
    
}
