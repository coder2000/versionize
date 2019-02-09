﻿using Colorful;
using System.Drawing;

namespace Versionize.CommandLine
{
    public static class CommandLineUI
    {
        public static IPlatformAbstractions Platform { get; set; } = new PlatformAbstractions() { Verbosity = LogLevel.All };

        public static void Exit(string message, int code)
        {
            Platform.WriteLine(message, Color.Red);
            Platform.Exit(code);
        }

        public static void Information(string message)
        {
            Platform.WriteLine(message, Color.LightGray);
        }

        public static void Step(string message)
        {
            string stepMessage = "{0} {1}";
            var messageFormatters = new Formatter[]
            {
                new Formatter("√", Color.Green),
                new Formatter(message, Color.LightGray),
            };

            Platform.WriteLineFormatted(stepMessage, Color.White, messageFormatters);
        }

        public static LogLevel Verbosity { get => Platform.Verbosity; set => Platform.Verbosity = value; }
    }
}
