using System;

namespace FibersManager
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isChoosed = false;
            int numberOfProcesses = 0;
            while (!isChoosed)
            {
                Console.WriteLine("Select dispatch algorithm: \nP for Priority\nN for Non-priority");
                Console.Write("Selected: ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "P":
                        {
                            ProcessManager.Priority = DispatchType.Priority;
                            isChoosed = true;
                            break;
                        }
                    case "N":
                        {
                            ProcessManager.Priority = DispatchType.NonPriority;
                            isChoosed = true;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Incorrect input. Type P or N please.");
                            break;
                        }
                }
            }
            isChoosed = false;
            while (!isChoosed)
            {
                Console.Write("Select number of processes: ");
                string input = Console.ReadLine();
                try
                {
                    numberOfProcesses = Convert.ToInt32(input);
                    isChoosed = true;
                    if (numberOfProcesses < 1)
                    {
                        isChoosed = false;
                        Console.WriteLine("Incorrect input value.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Incorrect input value.");
                }
            }
            for (int i = 0; i < numberOfProcesses; i++)
            {
                ProcessManager.AddProcess(new Process());
            }
            ProcessManager.Run();
            ProcessManager.Dispose();
            Console.WriteLine("The work is over. Press any key to Exit.");
            Console.ReadKey();
        }
    }
}