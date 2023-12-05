using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace CustomProgram
{
    public class RaylibTimeProvider: ITimeProvider
    {
        public double GetCurrentTime()
        {
            return Raylib.GetTime();
        }
    }

}
