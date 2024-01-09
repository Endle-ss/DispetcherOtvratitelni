using System;
using System.Collections.Generic;
using System.Diagnostics;
public static class TaskManager
{
    public static void Main()
    {
        Start:
        Process[] processes = Process.GetProcesses();
        foreach (Process process in processes)
        {
            Console.WriteLine($"Имя: {process.ProcessName}, Занятость физической памяти: {process.WorkingSet64}, Занятость оперативной памяти: {process.PrivateMemorySize64}");
        }
        Console.WriteLine("\nВыберите процесс:");
        int selectedProcessIndex = SelectProcessMenu(processes);
        if (selectedProcessIndex >= 0 && selectedProcessIndex < processes.Length)
        {
            Process selectedProcess = processes[selectedProcessIndex];
            Console.WriteLine($"\nПодробная информация о процессе \"{selectedProcess.ProcessName}\":");
            ShowProcessDetails(selectedProcess);
            int keyPressed = ProcessDetailsMenu();
            switch (keyPressed)
            {
                case 0: //backspace
                    goto Start;
                case -1: //d
                    TerminateProcess(selectedProcess);
                    break;
                case -2: //del
                    TerminateProcessesByName(selectedProcess.ProcessName);
                    break;
                default:
                    Console.WriteLine("Неверный ввод!");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Неверный выбор процесса!");
        }
    }
    private static int SelectProcessMenu(Process[] processes)
    {
        int selectedIndex = 0;
        ConsoleKey key;
        while (true)
        {
            for (int i = 0; i < processes.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("->");
                }
                Console.WriteLine($"{i + 1}. {processes[i].ProcessName}");
                Console.ResetColor();
            }
            key = Console.ReadKey().Key;
            Console.Clear();
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex == 0) ? processes.Length - 1 : selectedIndex - 1;
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex == processes.Length - 1) ? 0 : selectedIndex + 1;
                    break;
                case ConsoleKey.Enter:
                    return selectedIndex;
            }
        }
    }
    private static void ShowProcessDetails(Process process)
    {
        try
        {
            Console.WriteLine($"Id: {process.Id}, Имя: {process.ProcessName}, Занятость физической памяти: {process.WorkingSet64}, Занятость оперативной памяти: {process.PrivateMemorySize64}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка доступа: {e.Message}");
        }
    }
    private static int ProcessDetailsMenu()
    {
        Console.WriteLine("\nВыберите действие:");
        Console.WriteLine("  d  - Завершить выбранный процесс");
        Console.WriteLine("  del - Завершить все процессы с выбранным именем");
        Console.WriteLine("  backspace - чтоб вернуться в список");
        while (true)
        {
            ConsoleKey key = Console.ReadKey().Key;
            Console.Clear();
            switch (key)
            {
                case ConsoleKey.D:
                    return -1;
                case ConsoleKey.Delete:
                    return -2;
                case ConsoleKey.Backspace:
                    return 0;
            }
        }
    }
    private static void TerminateProcess(Process process)
    {
        try
        {
            process.Kill();
            Console.WriteLine("Процесс успешно завершен.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка доступа: {e.Message}");
        }
    }
    private static void TerminateProcessesByName(string processName)
    {
        try
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                process.Kill();
            }
            Console.WriteLine("Все процессы с выбранным именем успешно завершены.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка доступа: {e.Message}");
        }
    }
}