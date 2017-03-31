using System;
using Gwen.Control;
using ComputerPlus.Controllers.Models;
using Rage;
using LSPD_First_Response.Engine.Scripting.Entities;
using System.Collections.Generic;
using System.Linq;
using LSPD_First_Response;

namespace ComputerPlus.Extensions.Gwen
{
    internal static class ListBoxExtensions
    {
        internal static ListBoxRow AddPed(this ListBox listBox, Tuple<Ped, Persona> ped)
        {
            if (listBox == null) return null;
            String rowId = String.Format("{0}_{1}",
               ped.Item2.Forename,
               ped.Item2.Surname
               );
            ListBoxRow previousRow = listBox.FindChildByName(rowId) as ListBoxRow;
            if (previousRow == null)
            {
                listBox.AddRow(
                String.Format("({0}) {1} | {2}", ped.Item2.Gender == Gender.Male ? "M" : "F", ped.Item2.FullName, ped.Item2.BirthDay.ToString("MMMM dd yyyy")),
                String.Format("{0}_{1}", ped.Item2.Forename, ped.Item2.Surname),
                ped);
            }

            return previousRow;

        }

        internal static ListBoxRow AddPed(this ListBox listBox, ComputerPlusEntity entry)
        {
            if (listBox == null) return null;
            String alert = entry.IsWanted ? "(ALERT) " : String.Empty;
            String rowId = String.Format("{0}_{1}_{2}",
               entry.FirstName,
               entry.LastName,
               entry.Ped.Model.Name
               );
            var previousRow = listBox.FindChildByName(rowId);
            if (previousRow == null)
            {
                var row = listBox.AddRow(
                    String.Format("{0}({1}) | {2}", alert, entry.GenderId, entry.FullName),
                    rowId,
                    entry);
                return row;
            }
            return previousRow as ListBoxRow;
        }

        internal static ListBoxRow AddVehicle(this ListBox listBox, ComputerPlusEntity entry)
        {
            if (listBox == null) return null;
            String alert = (entry.IsWanted
                || (!entry.HasInsurance)
                || (!entry.IsRegistered)) ?
                    "(ALERT) " : String.Empty;
            if (!String.IsNullOrWhiteSpace(alert))
            {
                //alert = String.Format(alert, String.IsNullOrWhiteSpace(entry.Item4.Alert) ? String.Empty : entry.Item4.Alert);
            }
            String rowId = String.Format("{0}_{1}_{2}",
                entry.FirstName,
                entry.LastName,
                entry.Ped.Model.Name
                );
            var previousRow = listBox.FindChildByName(rowId);
            if (previousRow == null)
            {
                var row = listBox.AddRow(
                    String.Format("{0}{1} | {2}", alert, entry.Vehicle.Model.Name, entry.Vehicle.LicensePlate),
                    rowId,
                    entry);
                return row;
            }
            return previousRow as ListBoxRow;
        }
    }
}
