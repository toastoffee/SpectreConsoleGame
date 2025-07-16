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
            sentence = "Hello, I am a sad robot. I can only say sad things."
        };

        UtteranceDisplay.display_impl(utter, new UtteranceDisplay.Options
        {
            enableTypeWritter = true,
            enableTypeWritterSkip = true,
            typeWritterInterval_ms = 20,
            enableAutoContinue = true,
            startDelay_ms = 1000
        });

        UtteranceDisplay.display_impl(utter, new UtteranceDisplay.Options
        {
            enableTypeWritter = true,
            enableTypeWritterSkip = true,
            typeWritterInterval_ms = 20,
            enableAutoContinue = true,
            startDelay_ms = 1000
        });

        //UtteranceDisplay.display_impl(utter, new UtteranceDisplay.Options
        //{
        //    enableTypeWritter = false,
        //    enableTypeWritterSkip = true,
        //    typeWritterInterval_ms = 20,
        //    enableAutoContinue = true,
        //    startDelay_ms = 1000
        //});

        //UtteranceDisplay.display_impl(utter, new UtteranceDisplay.Options
        //{
        //    enableTypeWritter = false,
        //    enableTypeWritterSkip = true,
        //    typeWritterInterval_ms = 20,
        //    enableAutoContinue = true,
        //    startDelay_ms = 1000
        //});

        //UtteranceDisplay.display_impl(utter, new UtteranceDisplay.Options
        //{
        //    enableTypeWritter = false,
        //    enableTypeWritterSkip = true,
        //    typeWritterInterval_ms = 20,
        //    enableAutoContinue = true,
        //    startDelay_ms = 1000
        //});

        //var name = AnsiConsole.Ask<string>("[green]What's your name?[/]");
    }
}