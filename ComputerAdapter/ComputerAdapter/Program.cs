using System;
using System.Collections.Generic;

public class CPU
{
    public string Model { get; set; }
    public double SpeedInGhz { get; set; }
    public int Cores { get; set; }

    public override string ToString() => $"{Model} ({SpeedInGhz} GHz, {Cores} cores)";
}

public class Motherboard
{
    public string Model { get; set; }
    public string FormFactor { get; set; }

    public override string ToString() => $"{Model} ({FormFactor})";
}

public class RAM
{
    public int SizeInGB { get; set; }

    public override string ToString() => $"{SizeInGB} GB RAM";
}

public class Storage
{
    public int SizeInGB { get; set; }

    public override string ToString() => $"{SizeInGB} GB Storage";
}

public class GPU
{
    public string Model { get; set; }
    public int MemoryInGB { get; set; }

    public override string ToString() => $"{Model} ({MemoryInGB} GB GPU)";
}

public class Computer
{
    public CPU Cpu { get; set; }
    public Motherboard Motherboard { get; set; }
    public RAM Ram { get; set; }
    public Storage Storage { get; set; }
    public GPU Gpu { get; set; }

    public override string ToString()
    {
        return $"Computer: CPU={Cpu}, Motherboard={Motherboard}, RAM={Ram}, Storage={Storage}, GPU={Gpu}";
    }
}

public interface IComputerBuilder
{
    IComputerBuilder SetCPU(string model, double speedInGhz, int cores);
    IComputerBuilder SetMotherboard(string model, string formFactor);
    IComputerBuilder SetRAM(int sizeInGB);
    IComputerBuilder SetStorage(int sizeInGB);
    IComputerBuilder SetGPU(string model, int memoryInGB);
    Computer Build();
}

public class ComputerBuilder : IComputerBuilder
{
    private Computer computer;

    public ComputerBuilder()
    {
        computer = new Computer();
    }

    public IComputerBuilder SetCPU(string model, double speedInGhz, int cores)
    {
        computer.Cpu = new CPU { Model = model, SpeedInGhz = speedInGhz, Cores = cores };
        return this;
    }

    public IComputerBuilder SetMotherboard(string model, string formFactor)
    {
        computer.Motherboard = new Motherboard { Model = model, FormFactor = formFactor };
        return this;
    }

    public IComputerBuilder SetRAM(int sizeInGB)
    {
        computer.Ram = new RAM { SizeInGB = sizeInGB };
        return this;
    }

    public IComputerBuilder SetStorage(int sizeInGB)
    {
        computer.Storage = new Storage { SizeInGB = sizeInGB };
        return this;
    }

    public IComputerBuilder SetGPU(string model, int memoryInGB)
    {
        computer.Gpu = new GPU { Model = model, MemoryInGB = memoryInGB };
        return this;
    }

    public Computer Build()
    {
        return computer;
    }
}

public class ComputerAdapter
{
    private readonly ComputerBuilder computerBuilder;

    public ComputerAdapter()
    {
        computerBuilder = new ComputerBuilder();
    }

    public Computer BuildComputer(string cpuModel, double cpuSpeed, int cpuCores,
                                   string motherboardModel, string motherboardFormFactor,
                                   int ramSize, int storageSizeInGigabits,
                                   string gpuModel, int gpuMemoryInBits)
    {
        int gpuMemoryInGB = (int)Math.Ceiling((double)gpuMemoryInBits / (1024 * 8));
        int storageInGB = (int)Math.Ceiling((double)storageSizeInGigabits / 8);

        return computerBuilder.SetCPU(cpuModel, cpuSpeed, cpuCores)
                              .SetMotherboard(motherboardModel, motherboardFormFactor)
                              .SetRAM(ramSize)
                              .SetStorage(storageInGB)
                              .SetGPU(gpuModel, gpuMemoryInGB)
                              .Build();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        List<Computer> computers = new List<Computer>();
        ComputerAdapter adapter = new ComputerAdapter();

        while (true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Добавить новый компьютер");
            Console.WriteLine("2. Показать все компьютеры");
            Console.WriteLine("3. Выход");
            string choice = Console.ReadLine();

            if (choice == "3") break;

            switch (choice)
            {
                case "1":
                    computers.Add(AddNewComputer(adapter));
                    break;
                case "2":
                    DisplayComputers(computers);
                    break;
                default:
                    Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                    break;
            }
        }

        Console.WriteLine("Спасибо за использование менеджера компьютеров!");
    }

    private static Computer AddNewComputer(ComputerAdapter adapter)
    {
        Console.WriteLine("Введите модель процессора (например, Intel i7):");
        string cpuModel = Console.ReadLine();

        Console.WriteLine("Введите тактовую частоту процессора (в ГГц):");
        double cpuSpeed = GetPositiveDouble();

        Console.WriteLine("Введите количество ядер процессора:");
        int cpuCores = GetPositiveInt();

        Console.WriteLine("Введите модель материнской платы (например, ASUS ROG):");
        string motherboardModel = Console.ReadLine();

        Console.WriteLine("Введите форм-фактор материнской платы (например, ATX):");
        string motherboardFormFactor = Console.ReadLine();

        Console.WriteLine("Введите объем RAM (в ГБ):");
        int ramSize = GetPositiveInt();

        Console.WriteLine("Введите объем хранилища (в гигабитах):");
        int storageSizeInGigabits = GetPositiveInt();

        Console.WriteLine("Введите модель видеокарты (например, NVIDIA GeForce RTX 3080):");
        string gpuModel = Console.ReadLine();

        Console.WriteLine("Введите объем видеопамяти (в битах, например, 8192 для 8 ГБ):");
        int gpuMemoryInBits = GetPositiveInt();

        return adapter.BuildComputer(cpuModel, cpuSpeed, cpuCores,
                                     motherboardModel, motherboardFormFactor,
                                     ramSize, storageSizeInGigabits,
                                     gpuModel, gpuMemoryInBits);
    }

    private static void DisplayComputers(List<Computer> computers)
    {
        if (computers.Count == 0)
        {
            Console.WriteLine("Нет доступных компьютеров.");
            return;
        }

        Console.WriteLine("\nСписок доступных компьютеров:");
        foreach (var computer in computers)
        {
            Console.WriteLine(computer);
        }
    }

    private static double GetPositiveDouble()
    {
        double value;
        while (true)
        {
            if (double.TryParse(Console.ReadLine(), out value) && value > 0)
                return value;
            Console.WriteLine("Введите положительное число:");
        }
    }

    private static int GetPositiveInt()
    {
        int value;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out value) && value > 0)
                return value;
            Console.WriteLine("Введите положительное целое число:");
        }
    }
}
