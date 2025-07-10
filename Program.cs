using Spectre.Console;
using Spectre.Console.Extensions;

public static class Program
{
    public static async Task Main(string[] args) // Changed return type to Task and added async modifier  
    {
        int counter = 0;

        // 创建布局
        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Left"),
                new Layout("Right")
                    .SplitRows(
                        new Layout("Top"),
                        new Layout("Bottom")));

        // 用 Live 实现动态刷新
        AnsiConsole.Live(layout)
            .Start(ctx =>
            {
                while (counter < 100)
                {
                    // 更新左侧内容
                    layout["Left"].Update(
                        new Panel(
                            Align.Center(
                                new Markup($"Hello [blue]World![/] [yellow]{counter}[/]"),
                                VerticalAlignment.Middle))
                            .Expand());

                    // 刷新显示
                    ctx.Refresh();

                    counter++;
                    Thread.Sleep(100); // 0.1秒
                }
            });
    }
}