
namespace SadRobot;

public class Utterance
{
    public string speaker { get; set; } = string.Empty;
    public string speakerColor { get; set; } = string.Empty;
    public string sentence { get; set; } = string.Empty;

    public string speakerMarkup => $"[{speakerColor}]{speaker}[/]";
}
