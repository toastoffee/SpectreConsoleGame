using Spectre.Console;
using static System.Net.Mime.MediaTypeNames;

namespace SadRobot;

public static class UtteranceDisplay
{
    public class Options
    {
        public bool enableTypeWritter { get; set; } = true;
        public bool enableTypeWritterSkip { get; set; } = true;
        public int typeWritterInterval_ms { get; set; } = 0;
        public bool enableAutoContinue { get; set; } = true;
        public int startDelay_ms { get; set; } = 0; // delay before starting the display
    }

    public static void display_impl(Utterance utterance, Options options)
    {
        int sentenceLength = utterance.sentence.Length;
        var finishedEvent = new ManualResetEvent(false);
        var keyBoardInterrupted = false;

        // input listener thread
        Thread inputListenerThread = null;
        if (options.enableTypeWritter)
        {
            inputListenerThread = new Thread(() =>
            {
                while (!finishedEvent.WaitOne(0))
                {
                    if (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                        keyBoardInterrupted = true;
                        break;
                    }
                    Thread.Sleep(10);
                }
            });
            inputListenerThread.Start();
        }

        Thread.Sleep(options.startDelay_ms);

        if (options.enableTypeWritter)
        {
            AnsiConsole.Live(new Markup(String.Empty)).Start(ctx =>
            {
                for (int i = 1; i <= sentenceLength; i++)
                {
                    if (keyBoardInterrupted && options.enableTypeWritterSkip)
                    {
                        ctx.UpdateTarget(new Markup(utterance.GetContent()));
                        break;
                    }
                    ctx.UpdateTarget(new Markup(utterance.GetTypeWritterContent(i)));
                    Thread.Sleep(options.typeWritterInterval_ms);
                }
                finishedEvent.Set();
            });

            finishedEvent.WaitOne();
            inputListenerThread?.Join();

            // 关键：加一个分隔符或清理，帮助 Spectre.Console 恢复状态
            AnsiConsole.WriteLine();
            // 或者 AnsiConsole.MarkupLine("[grey]---[/]");
        }
        else
        {
            AnsiConsole.Markup(utterance.GetContent());
            AnsiConsole.WriteLine();
        }

        if (!options.enableAutoContinue)
        {
            Console.ReadKey(true);
        }
    }

    //public static void display_impl(Utterance utterance, Options options)
    //{
    //    int sentenceLength = utterance.sentence.Length;

    //    // thread synchronization
    //    var finishedEvent = new ManualResetEvent(false);
    //    var keyBoardInterrupted = false;

    //    Thread typeWritterThread = new Thread(() =>
    //    {
    //        AnsiConsole.Live(new Markup(String.Empty))
    //            .Start(ctx =>
    //            {
    //                // ENABLE type writter
    //                for (int i = 1; i <= sentenceLength; i++)
    //                {
    //                    var content = String.Empty;
    //                    if (keyBoardInterrupted && options.enableTypeWritterSkip)
    //                    {
    //                        content = utterance.GetContent();
    //                        ctx.UpdateTarget(new Markup(content));

    //                        // call the main thread to finish
    //                        finishedEvent.Set();
    //                        return;
    //                    }
    //                    content = utterance.GetTypeWritterContent(i);
    //                    ctx.UpdateTarget(new Markup(content));

    //                    Thread.Sleep(options.typeWritterInterval_ms);
    //                }


    //                finishedEvent.Set();
    //                return;

    //            });
    //    });

    //    // input listener thread
    //    Thread inputListenerThread = new Thread(() =>
    //    {
    //        // if animation has already finished, no need to wait for input
    //        while (!finishedEvent.WaitOne(0))
    //        {
    //            if (Console.KeyAvailable)
    //            {
    //                Console.ReadKey(true);
    //                keyBoardInterrupted = true;
    //                break;
    //            }
    //            Thread.Sleep(10); // Avoid high CPU usage
    //        }
    //    });

    //    // start delay
    //    Thread.Sleep(options.startDelay_ms);


    //    if(options.enableTypeWritter)
    //    {
    //        typeWritterThread.Start();
    //        inputListenerThread.Start();

    //        // Wait for the animation to finish or be interrupted
    //        finishedEvent.WaitOne();

    //        // wait for both threads to finish
    //        typeWritterThread.Join();
    //        inputListenerThread.Join();

    //        AnsiConsole.WriteLine();
    //    }
    //    else
    //    {
    //        AnsiConsole.Markup(utterance.GetContent());

    //        AnsiConsole.WriteLine(); 
    //    }


    //    // auto continue
    //    if (!options.enableAutoContinue)
    //    {
    //        Console.ReadKey(true); // Wait for user input to continue
    //    }
    //}


    private static string GetContent(this Utterance utterance)
    {
        return GetTypeWritterContent(utterance, utterance.sentence.Length);
    }

    private static string GetTypeWritterContent(this Utterance utterance, int sliceIdx)
    {
        var content = $"[[{utterance.speakerMarkup}]] {utterance.sentence.Substring(0, sliceIdx)}";

        return content;
    }
}
