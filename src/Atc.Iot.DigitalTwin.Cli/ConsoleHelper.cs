namespace Atc.Iot.DigitalTwin.Cli;

public static class ConsoleHelper
{
    public static void WriteHeader()
    {
        AnsiConsole.Write(new FigletText("DTDL CLI").Color(Color.CornflowerBlue));
    }
}