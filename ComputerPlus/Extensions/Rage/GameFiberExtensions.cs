using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;

namespace ComputerPlus.Extensions.Rage
{
    static class GameFiberExtensions
    {
        internal static bool IsRunning(this GameFiber fiber)
        {
            return fiber.IsAlive && !fiber.IsHibernating;
        }
    }
}
