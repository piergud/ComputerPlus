using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;

namespace ComputerPlus.Interfaces.Common
{
    class StateControlledTextbox : TextBox
    {
        public StateControlledTextbox(Base parent) : base(parent)
        {

        }

        protected override bool IsTextAllowed(string text, int position)
        {
            return IsDisabled ? false : base.IsTextAllowed(text, position);
        }

        public override void DeleteText(int startPos, int length)
        {
           if (!IsDisabled)
                base.DeleteText(startPos, length);
        }

        //public override void Disable()
        //{
        //    base.Disable();
        //    IsDisabled = true;
        //}

        protected override void OnMouseClickedLeft(int x, int y, bool down)
        {
            if (IsDisabled) return;
            base.OnMouseClickedLeft(x, y, down);
        }

        protected override void MakeCaretVisible()
        {
            if (IsDisabled) return;
            base.MakeCaretVisible();
        }
    }
}
