using System;
using System.Collections.Generic;
using Rage;
using System.Drawing;

namespace ComputerPlus.Extensions.Rage
{
    internal static class BlipExtensions
    {
        internal static Blip AddBlipSafe(this Entity entity, Color color, List<Blip> addToList = null)
        {
            if (entity != null && entity.Exists())
            {
                if (entity.GetAttachedBlip() != null) return entity.GetAttachedBlip();
                var b = new Blip(entity);
                b.Color = color;
                if (addToList != null)
                    addToList.Add(b);
                return b;
            }
            return null;
        }
    }
}
