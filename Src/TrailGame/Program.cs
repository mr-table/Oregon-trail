﻿using System;
using System.Threading;
using TrailEntities;

namespace TrailGame
{
    internal static class Program
    {
        private static void Main()
        {
            // Create console with title, no cursor, make CTRL-C act as input.
            Console.Title = "Oregon Trail Clone";
            Console.WriteLine("Starting...");
            Console.CursorVisible = false;
            Console.CancelKeyPress += Console_CancelKeyPress;

            // Create game simulation singleton instance, and start it.
            GameSimApp.Create();

            // Hook event to know when screen buffer wants to redraw the entire console screen.
            GameSimApp.Instance.ScreenBufferDirtyEvent += Simulation_ScreenBufferDirtyEvent;

            // Prevent console session from closing.
            while (GameSimApp.Instance != null)
            {
                // Simulation takes any numbers of pulses to determine seconds elapsed.
                GameSimApp.Instance.Tick();

                // Check if a key is being pressed, without blocking thread.
                if (Console.KeyAvailable)
                {
                    // Get the key that was pressed, without printing it to console.
                    var key = Console.ReadKey(true);

                    // If enter is pressed, pass whatever we have to simulation.
                    // ReSharper disable once SwitchStatementMissingSomeCases
                    switch (key.Key)
                    {
                        case ConsoleKey.Enter:
                            GameSimApp.Instance.SendInputBuffer();
                            break;
                        case ConsoleKey.Backspace:
                            GameSimApp.Instance.RemoteLastCharOfInputBuffer();
                            break;
                        default:
                            GameSimApp.Instance.SendKeyCharToInputBuffer(key.KeyChar);
                            break;
                    }
                }

                // Do not consume all of the CPU, allow other messages to occur.
                Thread.Sleep(1);
            }

            // Make user press any key to close out the simulation completely, this way they know it closed without error.
            Console.Clear();
            Console.WriteLine("Goodbye!");
            Console.WriteLine("Press ANY KEY to close this window...");
            Console.ReadKey();
        }

        /// <summary>
        ///     Write all text from objects to screen.
        /// </summary>
        private static void Simulation_ScreenBufferDirtyEvent(string tuiContent)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Write("{0}", tuiContent);
        }

        /// <summary>
        ///     Fired when the user presses CTRL-C on their keyboard, this is only relevant to operating system tick and this view
        ///     of simulation. If moved into another framework like game engine this statement would be removed and just destroy
        ///     the simulation when the engine is destroyed using its overrides.
        /// </summary>
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            // Destroy the simulation.
            GameSimApp.Instance.Destroy();

            // Stop the operating system from killing the entire process.
            e.Cancel = true;
        }
    }
}