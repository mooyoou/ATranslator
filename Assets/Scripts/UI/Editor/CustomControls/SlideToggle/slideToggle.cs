using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomControls
{
    public class slideToggle : BaseField<bool>
    {
        internal new class UxmlFactory : UxmlFactory<slideToggle, UxmlTraits>{}
        public new class UxmlTraits : BaseFieldTraits<bool, UxmlBoolAttributeDescription> { }
        
        public static readonly new string ussClassName = "slide-toggle";
        public static readonly new string inputUssClassName = "slide-toggle__input";
        public static readonly string inputKnobUssClassName = "slide-toggle__input-knob";
        public static readonly string inputCheckedUssClassName = "slide-toggle__input--checked";
        VisualElement m_Input;
        VisualElement m_Knob;

        public slideToggle() : this(null) { }
         public slideToggle(string label) : base(label, null)
        {
            AddToClassList(ussClassName);
            
            m_Input = this.Q(className: BaseField<bool>.inputUssClassName);
            m_Input.AddToClassList(inputUssClassName);
            //Add(m_Input);
            
            m_Knob = new VisualElement();
            m_Knob.AddToClassList(inputKnobUssClassName);
            m_Input.Add(m_Knob);
            


            RegisterCallback<ClickEvent>(evt => OnClick(evt));
            RegisterCallback<KeyDownEvent>(evt => OnKeydownEvent(evt));
            RegisterCallback<NavigationSubmitEvent>(evt => OnSubmit(evt));
        }
         
         static void OnClick(ClickEvent evt)
         {
             var slideToggle = evt.currentTarget as slideToggle ;
             slideToggle.ToggleValue();

             evt.StopPropagation();
         }

         static void OnSubmit(NavigationSubmitEvent evt)
         {
             var slideToggle = evt.currentTarget as slideToggle;
             slideToggle.ToggleValue();

             evt.StopPropagation();
         }

         static void OnKeydownEvent(KeyDownEvent evt)
         {
             var slideToggle = evt.currentTarget as slideToggle;

             // NavigationSubmitEvent event already covers keydown events at runtime, so this method shouldn't handle
             // them.
             if (slideToggle.panel?.contextType == ContextType.Player)
                 return;

             // Toggle the value only when the user presses Enter, Return, or Space.
             if (evt.keyCode == KeyCode.KeypadEnter || evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.Space)
             {
                 slideToggle.ToggleValue();
                 evt.StopPropagation();
             }
         }
        
         void ToggleValue()
         {
             value = !value;
         }
         
         public override void SetValueWithoutNotify(bool newValue)
         {
             base.SetValueWithoutNotify(newValue);

             //This line of code styles the input element to look enabled or disabled.
             m_Input.EnableInClassList(inputCheckedUssClassName, newValue);
         }
         
         
    }
}