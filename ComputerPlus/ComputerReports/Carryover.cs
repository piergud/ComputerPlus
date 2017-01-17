using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerPlus.Interfaces.ComputerReports
{
    class Carryover
    {
        public static PedData Data;
        public static bool continuecarry;
        public static void CarryOverData(PedData data)
        {
            GameFiber.StartNew(delegate
            {
                continuecarry = true;

                Data = data;

                while (continuecarry)
                {
                    if (!continuecarry)
                    {
                        Game.LogTrivial("Ending CarryOverData");
                        break;
                    }
                    GameFiber.Yield();
                }
            });
        }
    }
}
