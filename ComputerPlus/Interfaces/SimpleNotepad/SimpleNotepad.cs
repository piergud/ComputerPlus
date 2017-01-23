using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage.Forms;
using Gwen.Control;

namespace ComputerPlus.Interfaces.SimpleNotepad
{
    class SimpleNotepad : GwenForm
    {
        private MultilineTextBox tb_notes;
        private Button btn_erase;
        private CheckBox cb_toggle_pause;

        internal SimpleNotepad() : base(typeof(SimpleNotepadTemplate))
        {

        }
        //~SimpleNotepad()
        //{
        //    if (tb_notes != null)
        //    tb_notes.TextChanged -= NotesChanged;
        //}
        public override void InitializeLayout()
        {
            tb_notes.TextChanged += NotesChanged;
            tb_notes.Text = Globals.SimpleNotepadText;
            tb_notes.RightClicked += NotesRightClickHandler;
            cb_toggle_pause.IsChecked = Globals.PauseGameWhenOpen;
            cb_toggle_pause.Toggled += PauseToggleHandler;            
            btn_erase.Clicked += EraseTextHandler;
            this.Position = this.GetLaunchPosition();
            cb_toggle_pause.IsHidden = true;
        }

        private void PauseToggleHandler(Base sender, EventArgs arguments)
        {
            if (sender == cb_toggle_pause)
            {
                EntryPoint.TogglePause();
               //var caller = sender as CheckBox;
                //if (caller.IsChecked != Globals.PauseGameWhenOpen)
                   
            }
        }

        internal void ShowPause(bool visible)
        {
            if (visible == cb_toggle_pause.IsVisible) return;
            else cb_toggle_pause.IsHidden = !visible;
        }

        internal void SetPauseState(bool paused)
        {
            if (cb_toggle_pause.IsChecked != paused)            
                cb_toggle_pause.IsChecked = paused;
        }

        private void EraseTextHandler(Base sender, ClickedEventArgs arguments)
        {
            tb_notes.Text = String.Empty;
        }

        private void NotesRightClickHandler(Base sender, ClickedEventArgs arguments)
        {
            if (tb_notes.HasSelection)
            {
                try {
                    Globals.SetClipboard(tb_notes.GetSelection());
                } catch(IndexOutOfRangeException e)
                {
                    Globals.SetClipboard(null);
                }
            }
            else if (!String.IsNullOrEmpty(Globals.GetClipboard()))
            {
                tb_notes.Text = Globals.GetClipboard();
            }
        }

        private void NotesChanged(Base sender, EventArgs arguments)
        {
           if (sender != null && sender == tb_notes)
            {
                Globals.SimpleNotepadText = tb_notes.Text;
            }
        }
    }
}
