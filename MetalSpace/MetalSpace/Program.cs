using System;
using MetalSpace.Managers;

namespace MetalSpace
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (EngineManager game = new EngineManager())
            {
                game.Run();
            }
        }
    }
#endif
}

