using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;
using GwenSkin = Gwen.Skin;

namespace ComputerPlus.Interfaces.Common
{
    internal class StateControlledMultilineTextbox : MultilineTextBox
    {
        public StateControlledMultilineTextbox(Base parent) : base(parent)
        {
            //this.IsTabable = true;
            this.AcceptTabs = false;            
        }
        public void InsertAtCursor(String message, bool newLine = false)
        {
            if (this.Text.Length == 0)
                this.InsertText(message);
            else
                this.InsertText(GetAppendText(message, newLine));
        }
        public bool ForceWordWrap;
        //bool IgnoreNextReturn;
        public void InsertNewLineAtCursor()
        {
          //  IgnoreNextReturn = true;
            OnKeyReturn(false);
            //this.SetTextLine(this.TotalLines + 1, "fffff");
        }

        public String LastCharacter()
        {
            return this.Text.Substring(this.Text.Length - 1);
        }

        public void ClearText()
        {
            do
            {
                OnSelectAll(this, null);
                EraseSelection();
            }
            while (this.TotalLines > 1);
        }

        public bool IsEmpty()
        {
            return this.Text.Length == 0;
        }

        public bool LastCharacterIsSpaceNewLineOrEmpty()
        {
            if (IsEmpty()) return false;
            var last = LastCharacter();
            return last == String.Empty || last == " " || last == Environment.NewLine;
        }

        public bool LastCharacterIsSpace()
        {
            return this.IsEmpty() ? false : LastCharacter().Equals(" ");
        }

        public bool LastCharacterIsNewLine()
        {
            return this.IsEmpty() ? false : LastCharacter().Equals(Environment.NewLine);
        }
        protected override void OnTextChanged()
        {
            try
            {                
                if (ForceWordWrap && !this.IsEmpty() && this.Skin.Renderer.MeasureText(Font, this.GetTextLine(this.TotalLines - 1)).X > this.Width - 20)
                {
                    //Function.Log(String.Format("{0} > {1} - 15 : {2} {3}", this.TextWidth, this.Width, this.TextWidth > this.Width - 15, this.TotalLines));
                    InsertNewLineAtCursor();
                }
                else
                    base.OnTextChanged();
            }
            catch
            { }
        }

        protected override void PostLayout(GwenSkin.Base skin)
        {
            base.PostLayout(skin);
        }


        //protected override bool OnKeyReturn(bool down)
        //{
        //    //if (IgnoreNextReturn)
        //    //{
        //    //    IgnoreNextReturn = false;
        //    //}
        //    var r = base.OnKeyReturn(down);
        //    //this.InsertText(String.Empty);//Force the cursor to move
        //    return r;
        //}

        //protected override bool OnKeyHome(bool down)
        //{
        //    if (IgnoreNextReturn)
        //        return true;
        //    return base.OnKeyHome(down);
        //}

        //public override void Invalidate()
        //{
        //    if (!IgnoreNextReturn)
        //        base.Invalidate();
        //}

        //public override void Redraw()
        //{
        //    if (!IgnoreNextReturn)
        //        base.Redraw();
        //}

        private String GetAppendText(String message, bool appendNewLine = true)
        {
            if (this.Text.Length == 0)
            {
                return message;
            }
            var hasText = !String.IsNullOrEmpty(this.Text);
            var prevText = hasText ? this.Text : String.Empty;
            var lastString = hasText ? prevText.Substring(prevText.Length - 1) : null;
            var prevTextEndsInSpace = !hasText || lastString == null ? false : lastString.Equals(" ");
            var prevTextEndsInNewLine = !hasText ? false : lastString == null ? true : lastString == Environment.NewLine || lastString == "";
            var newText = String.Format("{0}{1}", hasText ? String.Empty : appendNewLine ? Environment.NewLine : prevTextEndsInSpace ? "" : prevTextEndsInNewLine ? "" : " ", message);
            return newText;
        }

    }
}
