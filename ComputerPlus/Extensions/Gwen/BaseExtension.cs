using Gwen;
using Gwen.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerPlus.Interfaces.Common;

namespace ComputerPlus.Extensions.Gwen
{
    static class BaseExtension
    {
        internal static Base LessHeightOf(this Base control, Base anchor, bool adjustY = true)
        {
            if (anchor != null)
            {          
                if (control.Height > 0) control.Height = control.Height - anchor.Height;
                if (adjustY) control.Y = control.Y - anchor.Height;
            }
            return control;
        }

        internal static Base LessWidthOf(this Base control, Base anchor, bool adjustX = true)
        {
            if (anchor != null)
            {
                if (control.Width > 0) control.Width = control.Width - anchor.Width;
                if (adjustX) control.X = control.X - anchor.Width;
            }
            return control;
        }

        internal static Base ShiftX(this Base control, Base swapWith, bool increase = true, int? Spacing = null)
        {
            Spacing = Spacing.HasValue ? Spacing : Configs.BaseFormControlSpacingHalf;
            var to = control.Width + Spacing.Value;
            control.SetPosition(swapWith.X, control.Y);
            if (increase)
                swapWith.MoveBy(increase ? to : to * -1, 0);
            return control;
        }

        internal static Base PlaceBelowOf(this Base control, Base anchor = null, int? Spacing = null)
        {
            anchor = anchor != null ? anchor : control.Parent;
            Spacing = Spacing.HasValue ? Spacing : Configs.BaseFormControlSpacing;
            //Function.Log(String.Format("PlaceBelowOf {0}, {1} + {2} + {3} + {4} = {5}", control.Name, control.X, anchor.Y, anchor.Height, control.Height, Spacing.Value, anchor.Y + anchor.Height + control.Height + Spacing.Value));
            control.SetPosition(control.X, anchor.Y + anchor.Height + Spacing.Value);
            //control.SetPosition(control.X, anchor.Y + anchor.Height + control.Height + Spacing.Value);
            return control;
        }

        internal static Base PlaceRightOf(this Base control, Base anchor = null, int? Spacing = null)
        {
            anchor = anchor != null ? anchor : control.Parent;
            Spacing = Spacing.HasValue ? Spacing : Configs.BaseFormControlSpacing;
            control.SetPosition(anchor.X + anchor.Width + Spacing.Value, control.Y);
            return control;
        }

        internal static Base PlaceLeftOf(this Base control, Base anchor = null, int? Spacing = null)
        {
            anchor = anchor != null ? anchor : control.Parent;
            Spacing = Spacing.HasValue ? Spacing : Configs.BaseFormControlSpacing;
            control.SetPosition(anchor.X + anchor.Width - control.Width - Spacing.Value, control.Y);
            return control;
        }

        internal static Base PlaceInsideRightOf(this Base control, Base anchor = null, int? Spacing = null)
        {
            anchor = anchor != null ? anchor : control.Parent;
            Spacing = Spacing.HasValue ? Spacing : Configs.BaseFormControlSpacing;
            control.SetPosition(anchor.Right - control.Width - Spacing.Value, control.Y);
            return control;
        }

        internal static Base SizeWidthWith(this Base control, Base anchor = null, int? Spacing = null, bool fullWidth = true)
        {
            anchor = anchor != null ? anchor : control.Parent;
            Spacing = Spacing.HasValue ? Spacing : fullWidth ? 0 : Configs.BaseFormControlSpacing;
            control.SetSize(anchor.Width - Spacing.Value, control.Height);
            return control;
        }

        internal static Base SizeChildrenWidth(this Base control, Base anchor = null, int? Spacing = null, bool fullWidth = true)
        {
            anchor = anchor != null ? anchor : control;
            control.Children.ForEach(x => x.SizeWidthWith(anchor, Spacing, fullWidth));
            return control;
        }

        internal static Base SizeChildrenHeight(this Base control, Base anchor = null, int? Spacing = null, bool fullWidth = true)
        {
            anchor = anchor != null ? anchor : control;
            control.Children.ForEach(x => x.SizeHeightWith(anchor, Spacing, fullWidth));
            return control;
        }

        internal static Base SizeHeightWith(this Base control, Base anchor = null, int? Spacing = null, bool fullHeight = true)
        {
            anchor = anchor != null ? anchor : control.Parent;
            Spacing = Spacing.HasValue ? Spacing : fullHeight ? 0 : Configs.BaseFormControlSpacing;
            control.SetSize(control.Width, anchor.Height - Spacing.Value);
            return control;
        }

        internal static Base SetHeight(this Base control, int height)
        {
            control.SetSize(control.Width, height);
            return control;
        }

        internal static Base SizeFull(this Base control, Base anchor = null, int? Spacing = null, bool fill = true)
        {
            anchor = anchor != null ? anchor : control.Parent;
            Spacing = Spacing.HasValue ? Spacing : fill ? 0 : Configs.BaseFormControlSpacing;
            control.SizeWidthWith(anchor, Spacing, fill).SizeHeightWith(anchor, Spacing, fill);
            return control;
        }

        internal static Base SizeHalfWidthWith(this Base control, Base anchor = null)
        {
            anchor = anchor != null ? anchor : control.Parent;
            control.SetSize(anchor.Width / 2 - control.Width / 2, control.Height);
            return control;
        }

        internal static Base AlignTopWith(this Base control, Base anchor = null)
        {
            anchor = anchor != null ? anchor : control.Parent;
            control.SetPosition(control.X, anchor.Y);
            return control;
        }

        internal static Base AlignLeftWith(this Base control, Base anchor = null)
        {
            anchor = anchor != null ? anchor : control.Parent;
            control.SetPosition(anchor.X, control.Y);
            return control;
        }

        internal static Base Align(this Base control, Base left = null, Base top = null)
        {
            left = left != null ? left : control.Parent;
            top = left != null ? top : control.Parent;
            control.SetPosition(left.X, top.Y);
            return control;
        }

        internal static Base AlignRightWith(this Base control, Base anchor = null)
        {
            anchor = anchor != null ? anchor : control.Parent;
            control.SetPosition((anchor.X + anchor.Width) - control.Width, control.Y);
            return control;
        }

        internal static Base AlignCenterWith(this Base control, Base anchor = null)
        {
            anchor = anchor != null ? anchor : control.Parent;
            control.SetPosition(control.X, (((anchor.Height / 2) + anchor.Y) - (control.Height / 2)));
            return control;
        }

        internal static Base AlignMiddleWith(this Base control, Base anchor = null)
        {
            anchor = anchor != null ? anchor : control.Parent;
            control.SetPosition((((anchor.Width / 2) + anchor.X) - (control.Width / 2)), control.Y);
            return control;
        }

        internal static Base AlignMiddleTopWith(this Base control, Base anchor = null)
        {
            anchor = anchor != null ? anchor : control.Parent;
            return control.AlignMiddleWith(anchor).AlignTopWith(anchor);
        }

        internal static Base AlignCenterMiddleWith(this Base control, Base anchor = null)
        {
            anchor = anchor != null ? anchor : control.Parent;
            control.SetPosition(((anchor.Width / 2) + anchor.X) - (control.Width / 2), (((anchor.Height / 2) + anchor.Y) - (control.Height / 2)));
            return control;
        } 

        internal static Base DefaultPadding(this Base control, Padding padding)
        {
            control.Padding = padding;
            return control;
        }

        internal static Base DefaultPadding(this Base control, bool xAxis = true, bool yAxis = true)
        {
            return DefaultPadding(control, 
                new Padding(
                    xAxis ? Configs.BaseFormControlSpacing : 0,
                    yAxis ? Configs.BaseFormControlSpacing : 0,
                    xAxis ? Configs.BaseFormControlSpacing : 0,
                    yAxis ? Configs.BaseFormControlSpacing : 0
                ));
        }

        internal static Base DefaultPaddingDouble(this Base control, bool xAxis = true, bool yAxis = true)
        {
            return DefaultPadding(control,
                new Padding(
                    xAxis ? Configs.BaseFormControlSpacingDouble : 0,
                    yAxis ? Configs.BaseFormControlSpacingDouble : 0,
                    xAxis ? Configs.BaseFormControlSpacingDouble : 0,
                    yAxis ? Configs.BaseFormControlSpacingDouble : 0
                ));
        }

        internal static Base DefaultMargin(this Base control, Margin margin)
        {
            control.Margin = margin;
            return control;
        }

        internal static Base DefaultMargin(this Base control, bool xAxis = true, bool yAxis = true)
        {
            return DefaultMargin(control,
                new Margin(
                    xAxis ? Configs.BaseFormControlSpacing : 0,
                    yAxis ? Configs.BaseFormControlSpacing : 0,
                    xAxis ? Configs.BaseFormControlSpacing : 0,
                    yAxis ? Configs.BaseFormControlSpacing : 0
                ));
        }

        internal static Base DefaultMarginDouble(this Base control, bool xAxis = true, bool yAxis = true)
        {
            return DefaultMargin(control,
                new Margin(
                    xAxis ? Configs.BaseFormControlSpacingDouble : 0,
                    yAxis ? Configs.BaseFormControlSpacingDouble : 0,
                    xAxis ? Configs.BaseFormControlSpacingDouble : 0,
                    yAxis ? Configs.BaseFormControlSpacingDouble : 0
                ));
        }

        internal static Base LogPositionAndSize(this Base control)
        {
            Function.Log(String.Format("LogPositionAndSize {0}: W {1} H {2} X {3} Y {4}", control.Name, control.Width, control.Height, control.X, control.Y));
            return control;
        }

        internal static Base SizeToChildrenBlock(this Base control)
        {
            control.SizeToChildren(false, true);
            return control;
        }
    }
}
