using System;
using System.Collections.Generic;
using System.Linq;
using Rage.Forms;
using Gwen;
using Gwen.Control;
using Rage;
using LSPD_First_Response.Engine.Scripting.Entities;
using ComputerPlus.Extensions.Gwen;
namespace ComputerPlus.Interfaces.ComputerPedDB
{
    sealed class ComputerPedSearch : GwenForm
    {
        ListBox list_collected_ids;
        ListBox list_manual_results;

        TextBox text_manual_name;

        internal ComputerPedSearch() : base(typeof(ComputerPedSearchTemplate))
        {
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            text_manual_name.SetToolTipText("Name");
            this.Position = this.GetLaunchPosition();
            this.Window.DisableResizing();
            PopulateManualSearchPedsList();
            PopulateStoppedPedsList();
            list_manual_results.AllowMultiSelect = false;
            list_collected_ids.AllowMultiSelect = false;
            list_manual_results.RowSelected += onListItemSelected;
            list_collected_ids.RowSelected += onListItemSelected;
            text_manual_name.SubmitPressed += onSearchSubmit;
        }

        private void onSearchSubmit(Base sender, EventArgs arguments)
        {
            String name = text_manual_name.Text.ToLower();
            if (String.IsNullOrWhiteSpace(name)) return;
            ComputerPedController controller = ComputerPedController.Instance;
            var ped = controller.LookupPersona(name);
            if (ped != null)
            {
                list_manual_results.AddPed(ped);
            } else
            {
                text_manual_name.BoundsOutlineColor = System.Drawing.Color.Red;
                text_manual_name.SetToolTipText("No persons found");
            }
        }

        private void AddPedPersonaToList(List<dynamic> list)
        {

        }

        public void PopulateManualSearchPedsList()
        {
            ComputerPedController controller = ComputerPedController.Instance;
            list_collected_ids.Clear();
            var results = controller.GetRecentSearches();
            if (results != null && results.Count > 0)
                results.ForEach(x => list_manual_results.AddPed(x));
        }

        public void PopulateStoppedPedsList()
        {
            ComputerPedController controller = ComputerPedController.Instance;
            list_collected_ids.Clear();
            var results = controller.PedsCurrentlyStoppedByPlayer.ToArray();
            if (results != null && results.Length > 0)
                results
                .Select(x => controller.LookupPersona(x))
                .ToList()
                .ForEach(x => list_collected_ids.AddPed(x));
        }

        private void ClearSelections()
        {
            list_collected_ids.UnselectAll();
            list_manual_results.UnselectAll();
        }

        private void onListItemSelected(Base sender, ItemSelectedEventArgs arguments)
        {
            if (arguments.SelectedItem.UserData is Tuple<Ped, Persona>)
            {
                ComputerPedController.LastSelected = arguments.SelectedItem.UserData as Tuple<Ped, Persona>;
                Game.LogVerboseDebug(String.Format("ComputerPedSearch.onListItemSelected updated ComputerPedController.LastSelected {0}", ComputerPedController.LastSelected.Item2.FullName));
                ClearSelections();
                var fiber = ComputerPedController.PedViewGameFiber;
                if (fiber.IsHibernating) fiber.Wake();
                else fiber.Start();
                Game.LogVerboseDebug("ComputerPedSearch.onListItemSelected successful");
            }
            else
            {
                Game.LogVerboseDebug("ComputerPedSearch.onListItemSelected arguments were not valid");
            }         
        }      
    }
}
