using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerPlus.Extensions;
using ComputerPlus.Extensions.Gwen;
using GwenSkin = Gwen.Skin;
using Gwen;
using Gwen.Control;
using SystemDrawing = System.Drawing;

namespace ComputerPlus.Interfaces.Common
{
    public enum RelationalPosition { LEFT = 0, TOP, RIGHT, BOTTOM }
    public enum RelationalSize { NONE = 0, SMALL = 1, MEDIUM = 2, LARGE = 3 }

    class RichLabeledComponent<T> : Base where T : Base
    {


        private RelationalPosition mLabelPosition;
        private RelationalSize mLabelFontScaling;
        private String mLabelText;
        private T mComponent;
        private RichLabel mLabel;
        private Font mLabelFont;
        private SystemDrawing.Color? mLabelColor;
        private bool mWordWrapLabel;


        public T Component
        {
            get { return mComponent; }
            private set
            {
                if (value.Parent != null)
                {
                    value.Parent.RemoveChild(value, false);

                }
                value.Parent = this;
                mComponent = value;
                mLayoutRequired = true;
            }
        }
        public RelationalPosition LabelPosition
        {
            get { return mLabelPosition; }
            set
            {
                mLabelPosition = value;
                mLayoutRequired = true;
                Invalidate();
            }
        }

        public RelationalSize LabelFontScaling
        {
            get { return mLabelFontScaling; }
            set
            {
                mLabelFontScaling = value;
                mLayoutRequired = true;
                Invalidate();
            }
        }

        public String LabelText
        {
            get { return mLabelText; }
            set
            {
                mLabelText = value;
                mLayoutRequired = true;
                Invalidate();
            }
        }

        public Font LabelFont
        {
            get { return mLabelFont; }
            set
            {
                mLabelFont = value.Copy();
                if (LabelFontScaling != RelationalSize.NONE)
                {
                    switch (LabelFontScaling)
                    {
                        case RelationalSize.LARGE: mLabelFont.Size = mLabelFont.Size * 2; break;
                        case RelationalSize.MEDIUM:
                            {
                                //Calculate the delta between large and small values and just subtract it to our current LabelFont size
                                var delta = ((mLabelFont.Size * 2 - mLabelFont.Size / 2) / (mLabelFont.Size * 2));
                                mLabelFont.Size = mLabelFont.Size - delta;
                                break;

                            }
                        case RelationalSize.SMALL: mLabelFont.Size = mLabelFont.Size / 2; break;
                    }
                }
                mLayoutRequired = true;
                Invalidate();
            }
        }

        public SystemDrawing.Color? LabelColor
        {
            get { return mLabelColor; }
            set
            {
                mLabelColor = value;
                mLayoutRequired = true;
                Invalidate();
            }
        }

        public RichLabel Label
        {
            get { return mLabel; }
        }

        public bool WordWrapLabel
        {
            get { return mWordWrapLabel; }
            set
            {
                mWordWrapLabel = value;
                mLayoutRequired = true;
                Invalidate();
            }

        }

        private int? mSpaceBetweenLabelAndComponent;
        private bool mLayoutRequired;

        public RichLabeledComponent(Base parent, String label, T component, Font labelFont, SystemDrawing.Color labelColor, RelationalPosition labelPosition = RelationalPosition.LEFT, RelationalSize labelScale = RelationalSize.NONE, int? Spacing = null) : base(parent)
        {
            if (component == null) throw new ArgumentNullException("LabelComponent cannot recieve a null component");
            mLabelText = label;
            Component = component;
            mLabelFontScaling = labelScale;
            mLabelPosition = labelPosition;
            mSpaceBetweenLabelAndComponent = Spacing;

            mLabel = new RichLabel(this);
            LabelFont = labelFont;
            LabelColor = labelColor;
            mLayoutRequired = true;

        }

        public RichLabeledComponent(Base parent, String label, T component, Font font, RelationalPosition labelPosition = RelationalPosition.LEFT, RelationalSize labelScale = RelationalSize.NONE, int? Spacing = null) : this(parent, label, component, font, SystemDrawing.Color.Black, labelPosition, labelScale, Spacing)
        {
            LabelFont = font == default(Font) || font == null ? this.Skin.DefaultFont : font;

        }

        public RichLabeledComponent(Base parent, String label, T component, SystemDrawing.Color fontColor, RelationalPosition labelPosition = RelationalPosition.LEFT, RelationalSize labelScale = RelationalSize.NONE, int? Spacing = null) : this(parent, label, component, default(Font), fontColor, labelPosition, labelScale, Spacing)
        {
            LabelColor = fontColor;
        }

        protected override void Layout(GwenSkin.Base skin)
        {
            base.Layout(skin);
            if (!mLayoutRequired) return;
            mLayoutRequired = false;
            if (LabelColor.HasValue)
                mLabel.AddText(LabelText, LabelColor.Value);
            else
                mLabel.AddText(LabelText, SystemDrawing.Color.Black);
            if (!WordWrapLabel)
            {

                var measure = mLabel.Skin.Renderer.MeasureText(LabelFont, LabelText);
                mLabel.SetSize(measure.X, measure.Y);
                //Function.Log(String.Format("WordWrap disabled {0} {1} {2} {3} {4} {5}", measure.X, measure.Y, LabelColor.HasValue ? LabelColor.Value.ToString() : "none", LabelFont.FaceName, LabelFont.Size, LabelText));
            }
            else
            {
                mLabel.SizeToChildren();
            }
            //if (WordWrapLabel)
            //{
            //    mLabel.SizeWidthWith(this);
            //    mLabel.SizeToChildren(false, true);
            //}
            //else
            //    mLabel.SizeToChildren(true, true);

            switch (LabelPosition)
            {
                case RelationalPosition.LEFT:
                    Component.PlaceRightOf(mLabel, mSpaceBetweenLabelAndComponent);
                    break;
                case RelationalPosition.RIGHT:
                    mLabel.PlaceRightOf(Component, mSpaceBetweenLabelAndComponent);
                    break;
                case RelationalPosition.TOP:
                    Component.PlaceBelowOf(mLabel, mSpaceBetweenLabelAndComponent);

                    break;
                case RelationalPosition.BOTTOM:
                    mLabel.PlaceBelowOf(Component, mSpaceBetweenLabelAndComponent);
                    break;
            }

            mLabel.LogPositionAndSize();
            Component.LogPositionAndSize();
            this.SizeToChildren();

        }


    }
    internal static class LabeledComponentExtensions
    {
        internal static void SetValueText(this LabeledComponent<Label> component, String text)
        {
            component.Component.Text = text;
        }

        internal static void SetValueText(this LabeledComponent<RichLabel> component, String text)
        {
            component.Component.ClearText();
            component.Component.Text(text, component.Skin.Colors.Label.Default);
        }

        internal static void SetValueText(this LabeledComponent<StateControlledTextbox> component, String text)
        {
            component.Component.SetText(text);
        }

        internal static void SetValueText(this LabeledComponent<StateControlledMultilineTextbox> component, String text)
        {
            component.Component.SetText(text);
        }
    }
    class LabeledComponent<T> : Base where T : Base
    {

       
        private RelationalPosition mLabelPosition;
        private RelationalSize mLabelFontScaling;
        private String mLabelText;
        private T mComponent;
        private Label mLabel;
        private Font mLabelFont;
        private SystemDrawing.Color mLabelColor;
        private bool mWordWrapLabel = false;


        public T Component
        {
            get { return mComponent; }
            private set
            {
                if (value.Parent != null)
                {
                    value.Parent.RemoveChild(value, false);

                }
                value.Parent = this;
                mComponent = value;
                mLayoutRequired = true;
            }
        }
        public RelationalPosition LabelPosition
        {
            get { return mLabelPosition; }
            set
            {
                mLabelPosition = value;
                mLayoutRequired = true;
                Invalidate();
            }
        }

        public RelationalSize LabelFontScaling
        {
            get { return mLabelFontScaling; }
            set
            {
                mLabelFontScaling = value;
                mLayoutRequired = true;
                Invalidate();
            }
        }

        public String LabelText
        {
            get { return mLabelText; }
            set
            {
                mLabelText = value;
                mLayoutRequired = true;
                Invalidate();
            }
        }

        public Font LabelFont
        {
            get { return mLabelFont; }
            set
            {
                mLabelFont = value.Copy();
                if (LabelFontScaling != RelationalSize.NONE)
                {
                    switch (LabelFontScaling)
                    {
                        case RelationalSize.LARGE: mLabelFont.Size = mLabelFont.Size * 2; break;
                        case RelationalSize.MEDIUM:
                            {
                                //Calculate the delta between large and small values and just subtract it to our current LabelFont size
                                var delta = ((mLabelFont.Size * 2 - mLabelFont.Size / 2) / (mLabelFont.Size * 2));
                                mLabelFont.Size = mLabelFont.Size - delta;
                                break;

                            }
                        case RelationalSize.SMALL: mLabelFont.Size = mLabelFont.Size / 2; break;
                    }
                }
                mLayoutRequired = true;
                Invalidate();
            }
        }

        public SystemDrawing.Color LabelColor
        {
            get { return mLabelColor; }
            set
            {
                mLabelColor = value;
                mLayoutRequired = true;
                Invalidate();
            }
        }

        public Label Label
        {
            get { return mLabel; }
        }


        private int? mSpaceBetweenLabelAndComponent;
        private bool mLayoutRequired;

        public LabeledComponent(Base parent, String label, T component, RelationalPosition labelPosition = RelationalPosition.LEFT, RelationalSize labelScale = RelationalSize.NONE, int? Spacing = null, Font font = default(Font), SystemDrawing.Color fontColor = default(SystemDrawing.Color)) : base(parent)
        {
            if (component == null) throw new ArgumentNullException("LabelComponent cannot recieve a null component");
            mLabelText = label;
            Component = component;
            mLabelFontScaling = labelScale;
            mLabelPosition = labelPosition;
            LabelColor = fontColor == null || fontColor == default(SystemDrawing.Color) ? this.Skin.Colors.Label.Default : fontColor;
            mSpaceBetweenLabelAndComponent = Spacing;

            LabelFont = font == default(Font) || font == null ? this.Skin.DefaultFont : font;
            mLabel = new Label(this) { TextColorOverride = LabelColor, Text = LabelText, Font = LabelFont } ;
            mLayoutRequired = true;

        }

        protected override void Layout(GwenSkin.Base skin)
        {
            base.Layout(skin);
            if (!mLayoutRequired) return;
            mLayoutRequired = false;

            mLabel.TextColorOverride = LabelColor;
            mLabel.Font = LabelFont;
            mLabel.UpdateColors();
            mLabel.SizeToContents();

            //mLabel.FitChildrenToSize();

            switch (LabelPosition)
            {
                case RelationalPosition.LEFT:
                    Component.PlaceRightOf(mLabel, mSpaceBetweenLabelAndComponent);
                    break;
                case RelationalPosition.RIGHT:
                    mLabel.PlaceRightOf(Component, mSpaceBetweenLabelAndComponent);
                    break;
                case RelationalPosition.TOP:
                    Component.PlaceBelowOf(mLabel, mSpaceBetweenLabelAndComponent);

                    break;
                case RelationalPosition.BOTTOM:
                    mLabel.PlaceBelowOf(Component, mSpaceBetweenLabelAndComponent);
                    break;
            }


            this.SizeToChildren();

        }

    }


  

    static class LabeledComponent
    {
        public static LabeledComponent<Label> Label(Base parent, String labelText, RelationalPosition position = RelationalPosition.LEFT, int? spaceBetweenComponents = null, SystemDrawing.Color labelColor = default(SystemDrawing.Color), Font labelFont = default(Font))
        {
            return LabeledComponent.Label(parent, labelText, new Label(parent), position, spaceBetweenComponents, labelColor, labelFont);
        }

        public static LabeledComponent<Label> Label(Base parent, String labelText, Label component, RelationalPosition position = RelationalPosition.LEFT, int? spaceBetweenComponents = null, SystemDrawing.Color labelColor = default(SystemDrawing.Color), Font labelFont = default(Font))
        {
            return new LabeledComponent<Gwen.Control.Label>(parent, labelText, component, position, RelationalSize.NONE, spaceBetweenComponents, labelFont, labelColor);
        }

        public static LabeledComponent<RichLabel> RichLabel(Base parent, String labelText, RelationalPosition position = RelationalPosition.LEFT, int? spaceBetweenComponents = null, SystemDrawing.Color labelColor = default(SystemDrawing.Color), Font labelFont = default(Font))
        {
            return LabeledComponent.RichLabel(parent, labelText, new RichLabel(parent), position, spaceBetweenComponents, labelColor, labelFont);
        }

        public static LabeledComponent<RichLabel> RichLabel(Base parent, String labelText, RichLabel component, RelationalPosition position = RelationalPosition.LEFT, int? spaceBetweenComponents = null, SystemDrawing.Color labelColor = default(SystemDrawing.Color), Font labelFont = default(Font))
        {
            return new LabeledComponent<Gwen.Control.RichLabel>(parent, labelText, component, position, RelationalSize.NONE, spaceBetweenComponents, labelFont, labelColor);
        }

        public static LabeledComponent<TextBox> TextBox(Base parent, String labelText, TextBox component, RelationalPosition position = RelationalPosition.LEFT, int? spaceBetweenComponents = null, SystemDrawing.Color labelColor = default(SystemDrawing.Color), Font labelFont = default(Font))
        {
            return new LabeledComponent<Gwen.Control.TextBox>(parent, labelText, component, position, RelationalSize.NONE, spaceBetweenComponents, labelFont, labelColor);
        }

        public static LabeledComponent<StateControlledTextbox> StatefulTextbox(Base parent, String labelText, StateControlledTextbox component, RelationalPosition position = RelationalPosition.LEFT, int? spaceBetweenComponents = null, SystemDrawing.Color labelColor = default(SystemDrawing.Color), Font labelFont = default(Font))
        {
            return new LabeledComponent<StateControlledTextbox>(parent, labelText, component, position, RelationalSize.NONE, spaceBetweenComponents, labelFont, labelColor);
        }

        public static LabeledComponent<StateControlledTextbox> StatefulTextbox(Base parent, String labelText, RelationalPosition position = RelationalPosition.LEFT, int? spaceBetweenComponents = null, SystemDrawing.Color labelColor = default(SystemDrawing.Color), Font labelFont = default(Font))
        {
            return new LabeledComponent<StateControlledTextbox>(parent, labelText, new StateControlledTextbox(parent), position, RelationalSize.NONE, spaceBetweenComponents, labelFont, labelColor);
        }

        public static LabeledComponent<StateControlledMultilineTextbox> StatefulMultilineTextBox(Base parent, String labelText, StateControlledMultilineTextbox component, RelationalPosition position = RelationalPosition.LEFT, int? spaceBetweenComponents = null, SystemDrawing.Color labelColor = default(SystemDrawing.Color), Font labelFont = default(Font))
        {
            return new LabeledComponent<StateControlledMultilineTextbox>(parent, labelText, component, position, RelationalSize.NONE, spaceBetweenComponents, labelFont, labelColor);
        }

        public static LabeledComponent<StateControlledMultilineTextbox> StatefulMultilineTextBox(Base parent, String labelText, RelationalPosition position = RelationalPosition.LEFT, int? spaceBetweenComponents = null, SystemDrawing.Color labelColor = default(SystemDrawing.Color), Font labelFont = default(Font))
        {
            return new LabeledComponent<StateControlledMultilineTextbox>(parent, labelText, new StateControlledMultilineTextbox(parent), position, RelationalSize.NONE, spaceBetweenComponents, labelFont, labelColor);
        }


        
        public static LabeledComponent<Button> Button(Base parent, String labelText, Button button, RelationalPosition position = RelationalPosition.LEFT, int? spaceBetweenComponents = null, SystemDrawing.Color labelColor = default(SystemDrawing.Color), Font labelFont = default(Font))
        {
            return new LabeledComponent<Button>(parent, labelText, button, position, RelationalSize.NONE, spaceBetweenComponents, labelFont, labelColor);
        }

        public static LabeledComponent<Button> Button(Base parent, String labelText, RelationalPosition position = RelationalPosition.LEFT, int? spaceBetweenComponents = null, SystemDrawing.Color labelColor = default(SystemDrawing.Color), Font labelFont = default(Font))
        {
            return new LabeledComponent<Button>(parent, labelText, new Button(parent), position, RelationalSize.NONE, spaceBetweenComponents, labelFont, labelColor);
        }
    }
}
