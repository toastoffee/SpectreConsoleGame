using SadRobot;
using Spectre.Console;
using Spectre.Console.Extensions;
using System.Text.RegularExpressions;

public static class Program
{
    static void Main()
    {
        var utter = new Utterance
        {
            speakerColor = "bold red",
            speaker = "Robot",
            text = "Hello, I am a sad robot. I can only say sad things."
        };

        utter.Display();
    }
}