using System;

public class Program
{
    public static void Main()
    {
        Tamagotchi tamagotchi = new Tamagotchi();

        // Создание наблюдателей
        var requestObserver = new RequestObserver();
        var healthObserver = new HealthObserver();
        var lifeCycleObserver = new LifeCycleObserver();

        // Подписка наблюдателей на события
        tamagotchi.OnRequest += requestObserver.OnRequestReceived;
        tamagotchi.OnSick += healthObserver.OnSick;
        tamagotchi.OnHealed += healthObserver.OnHealed;
        tamagotchi.OnLifeEnd += lifeCycleObserver.OnLifeEnd;

        tamagotchi.StartLife();
    }
}

// Наблюдатели
public class RequestObserver
{
    public void OnRequestReceived(string request)
    {
        Console.WriteLine($"Наблюдатель: Тамагочи запросил {request}.");
    }
}

public class HealthObserver
{
    public void OnSick()
    {
        Console.WriteLine("Наблюдатель: Тамагочи заболел!");
    }

    public void OnHealed()
    {
        Console.WriteLine("Наблюдатель: Тамагочи выздоровел!");
    }
}

public class LifeCycleObserver
{
    public void OnLifeEnd()
    {
        Console.WriteLine("Наблюдатель: Жизненный цикл Тамагочи завершён.");
    }
}
