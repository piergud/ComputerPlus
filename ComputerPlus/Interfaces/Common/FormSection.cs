using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen;
using Gwen.Control;
using ComputerPlus.Extensions.Gwen;
using GwenSkin = Gwen.Skin;

namespace ComputerPlus.Interfaces.Common
{
    class FormSection : Base
    {
        
        Base content;
        internal Base Content
        {
            get { return content;  }            
        }
        Label lbl_header;

        internal FormSection(Base parent, String headerText) : base (parent)
        {
            lbl_header = new Label(this) { Text = headerText };
            content = new Base(this);
            

        }
        internal FormSection AddContentChild(Base child)
        {
            //child.Parent = content;            
            if (Content != child)
            {
                child.Parent.RemoveChild(child, false);                
                content = child;
                content.SizeToChildren(false, true);            
                content.Parent = this;
                this.Invalidate();
            }
            return this;
        }

        internal void SetContainerSize(int width, int height)
        {
            this.SetSize(width, height);
        }

        private void PositionControls()
        {
            content
                .SizeFull()
                .PlaceBelowOf(lbl_header);
                //.SizeToChildren(false, true);

        }

        protected override void Layout(GwenSkin.Base skin)
        {
            base.Layout(skin);
            this.SizeFull();
            PositionControls();
        }
    }
}
