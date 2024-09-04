using System;
using System.Threading;

public class Terminal
{
    private static Terminal _instance;
    private bool _isLoading = true;
    private readonly object _lock = new object();

    public int SizeX { get; set; } = 100;
    public int SizeY { get; set; } = 20;

    private Terminal() { }

    public static Terminal gI()
    {
        if (_instance == null)
        {
            _instance = new Terminal();
        }
        return _instance;
    }

    public void SetTitle(string title, int delay = 75)
    {
        foreach (char c in title)
        {
            Console.Title += c;
            Thread.Sleep(delay);
        }
    }

    public void EffectPrint(string text, int x, int y, ConsoleColor color = ConsoleColor.White, int delay = 100)
    {
        for (int i = 1; i <= text.Length; i++)
        {
            Print(text.Substring(0, i), x, y, color);
            Thread.Sleep(delay);
        }
    }

    public void Print(string text, int x, int y, ConsoleColor color = ConsoleColor.White)
    {
        lock (_lock)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
            Console.SetCursorPosition(0, SizeY);
        }
    }

    public void StartLoading()
    {
        new Thread(() =>
        {
            for (int i = SizeX; i >= 4; i--)
            {
                Print("-", i, 0, ConsoleColor.Green);
                Thread.Sleep(2);
            }

            while (_isLoading)
            {
                for (int i = 4; i < SizeX - 1; i++)
                {
                    Print("=", i, 0, ConsoleColor.Yellow);
                    if (i > 0) Print("-", i - 1, 0, ConsoleColor.Green);
                    Thread.Sleep(1);
                }

                for (int i = SizeX; i >= 4; i--)
                {
                    Print("##", i, 0, ConsoleColor.Red);
                    if (i < SizeX) Print("-", i + 2, 0, ConsoleColor.Green);
                    Thread.Sleep(1);
                }
            }
        }).Start();
    }

    public void StopLoading() => _isLoading = false;
}
