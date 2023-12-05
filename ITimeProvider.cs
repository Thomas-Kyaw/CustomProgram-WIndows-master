using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace CustomProgram
{
    /// <summary>
    /// interface for getting current rime
    /// </summary>
    public interface ITimeProvider
    {
        double GetCurrentTime();
    }

    public class RealTimeProvider : ITimeProvider
    {
        /// <summary>
        /// This method returns the current time using Raylib's GetTime method.
        /// </summary>
        /// <returns></returns>
        public double GetCurrentTime()
        {
            return Raylib.GetTime();
        }
    }
}
