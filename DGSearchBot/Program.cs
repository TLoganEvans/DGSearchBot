using System;

namespace DGSearchBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            bot.RunASync().GetAwaiter().GetResult();
        }
    }
}
