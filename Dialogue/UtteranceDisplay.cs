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

                    var content = $"[[{utterance.speakerMarkup}]] {utterance.text.Substring(0, i)}";
                    ctx.UpdateTarget(new Markup(content));
                    Thread.Sleep(20);
                }
            });
    }

    public static void DisplayWithInterrupt(this Utterance utterance, int interval_ms)
    {
        var speakerMarkup = $"[{utterance.speakerColor}]{utterance.speaker}[/]";
        int length = utterance.text.Length;

        // 用于线程同步
        var finishedEvent = new ManualResetEvent(false);
        var interrupted = false;

        // 打字机线程
        Thread typingThread = new Thread(() =>
        {
            AnsiConsole.Live(new Markup(""))
                .Start(ctx =>
                {
                    for (int i = 1; i <= length; i++)
                    {
                        var content = "";
                        if (interrupted)
                        {
                            content = $"{speakerMarkup}: {utterance.text}";
                            ctx.UpdateTarget(new Markup(content));
                            finishedEvent.Set(); // 通知主线程
                            return;
                        }
                        content = $"{speakerMarkup}: {utterance.text.Substring(0, i)}";
                        ctx.UpdateTarget(new Markup(content));
                        Thread.Sleep(interval_ms);
                    }
                    // 动画自然结束
                    finishedEvent.Set(); // 通知主线程
                });
        });

        typingThread.Start();

        // 输入监听线程
        Thread inputThread = new Thread(() =>
        {
            // 如果动画已经结束，则不再等待输入
            while (!finishedEvent.WaitOne(0))
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true); // 消耗按键
                    interrupted = true;
                    break;
                }
                Thread.Sleep(10); // 避免CPU占用过高
            }
        });

        inputThread.Start();

        // 等待动画自然结束或被中断
        finishedEvent.WaitOne();

        // 等待两个线程都结束
        typingThread.Join();
        inputThread.Join();
    }
}
