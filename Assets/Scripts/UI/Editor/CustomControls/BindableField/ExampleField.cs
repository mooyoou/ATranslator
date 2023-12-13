using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomControls
{

    public class ExampleField : BaseField<double>
    {
        public new class
            UxmlFactory : UxmlFactory<ExampleField, BaseFieldTraits<double, UxmlDoubleAttributeDescription>>
        {
        }

        Label m_Input;

        public ExampleField() : this(null)
        {

        }

        public ExampleField(string label) : base(label, new Label() { })
        {
            m_Input = this.Q<Label>(className: inputUssClassName);
        }

        public override void SetValueWithoutNotify(double newValue)
        {
            base.SetValueWithoutNotify(newValue);

            m_Input.text = value.ToString("N");
        }
    }
}