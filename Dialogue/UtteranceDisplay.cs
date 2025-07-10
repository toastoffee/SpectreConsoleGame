using Spectre.Console;
using static System.Net.Mime.MediaTypeNames;

namespace SadRobot;

public static class UtteranceDisplay
{
    public static void Display(this Utterance utterance)
    {
        var markup = new Markup($"{utterance.speakerMarkup}");

        AnsiConsole.Live(markup)
            .Start(ctx =>
            {
                for (int i = 1; i <= utterance.text.Length; i++)
                {
                    ctx.UpdateTarget(new Markup(utterance.speakerMarkup + utterance.text.Substring(0, i)));
                    Thread.Sleep(100);
                }
            });
    }
}
