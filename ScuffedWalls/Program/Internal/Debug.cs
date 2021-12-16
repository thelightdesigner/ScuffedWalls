using System;
using System.Collections.Generic;
using System.Text;

namespace ScuffedWalls
{
    public static class Debug
    {
        public static void TryAction(Action action, Action<Exception> onError)
        {
             /*
            if (Utils.ScuffedConfig.Debug) 
            {
                try
                {
                    action();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
                return;
            }

            try
            { 
                action(); 
            } 
            catch(Exception e)
            {
                onError(e);
            }
             */ action(); 
        }
    }
}
