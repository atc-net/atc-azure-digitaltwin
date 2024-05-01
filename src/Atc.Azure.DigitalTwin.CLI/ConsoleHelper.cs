namespace Atc.Azure.DigitalTwin.CLI;

public static class ConsoleHelper
{
    public static void WriteHeader()
        => AnsiConsole.Write(new FigletText("DTDL CLI").Color(Color.CornflowerBlue));
}