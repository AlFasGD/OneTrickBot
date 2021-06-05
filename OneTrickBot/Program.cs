﻿using OneTrickBot.DiscordFunctionality;
using System;
using System.Threading.Tasks;
using static System.Console;

namespace OneTrickBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Task.WaitAll(BotClientManager.Instance.InitializeLogin());

            WriteLine("Press the ESC key to log out at any time.\n");
            while (ReadKey(true).Key != ConsoleKey.Escape) ;

            Task.WaitAll(BotClientManager.Instance.Logout());
        }
    }
}
