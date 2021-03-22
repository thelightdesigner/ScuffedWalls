using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScuffedWalls.Functions
{
    [ScuffedFunction("OMG")]
    class OMG_OMG_OMGOMG : SFunction
    {
        public void Run() => RunAsync().GetAwaiter().GetResult();
        public async Task RunAsync()
        {
            Task<string> StringGetter = GiveStringAsync();
            Console.WriteLine(GiveString()); //takes 10 secodns to print
            Console.WriteLine(await StringGetter); //takes zero secodns to print
        }
        public async Task<string> GiveStringAsync()
        {
            await Task.Delay(1000);
            return "async hi waited 10 seconds for this";
        }
        public string GiveString()
        {
            Task.Delay(1000);
            return "normal hi waited 10 seconds for this";
        }
    }
}
