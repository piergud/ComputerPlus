using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;
using Gwen;

namespace ComputerPlus.Extensions.Gwen
{
    internal static class ButtonExtensions
    {
        internal static void Icon(this Button button, String iconName, bool clearText = true, String backupText = "", bool resize = true, bool center = true)
        {
            if (button == null) return;
            try
            {
                var iconSize = Function.GetIconSize();
                var path = Function.GetIconPath(iconName);                
                if (resize)
                {
                    button.SetSize(44, 35);
                    //button.Padding = new Padding(6, 6, 6, 6); //Attempt to get button to register inner gap
                    //button.Margin = new Margin(3, 3, 3, 3);                       
                }
                button.SetImage(path, center);
                var imagePanel = button.Children.Where(x => x is ImagePanel).FirstOrDefault();
                imagePanel.Clicked += new Base.GwenEventHandler<ClickedEventArgs>((sender, args) => {
                    button.Press(); //Hack to pass image panel clicks to button
                });

                if (clearText && !String.IsNullOrWhiteSpace(button.Text))
                    button.SetText(String.Empty);
            }
            catch
            {
                button.SetText(backupText);
            }
           

        }
        internal static void DeleteIcon(this Button button, bool clearText = true)
        {
            button.Icon("ic_delete_forever_black_24dp_1x.png", clearText, "Delete");
        }

        internal static void AddIcon(this Button button, bool clearText = true)
        {
            button.Icon("ic_add_black_24dp_1x.png", clearText, "Add");
        }

        internal static void NavigateIcon(this Button button, bool clearText = true)
        {
            button.Icon("ic_navigation_black_24dp_1x.png", clearText, "Navigate");
        }

        internal static void LocationIcon(this Button button, bool clearText = true)
        {
            button.Icon("ic_my_location_black_24dp_1x.png", clearText, "My Location");
        }

        internal static void CopyContentIcon(this Button button, bool clearText = true)
        {
            button.Icon("ic_content_copy_black_24dp_1x.png", clearText, "Copy Content");
        }
    }
}
