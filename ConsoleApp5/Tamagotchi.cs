using System;
using System.Threading;

public class Tamagotchi
{
    private static readonly string[] Requests = { "покормить", "погулять", "уложить спать", "полечить", "поиграть" };
    private static Random random = new Random();
    private string lastRequest = "";
    private int unfulfilledRequests = 0;
    private bool isSick = false;
    private bool isAlive = true;
    private int lifetimeSeconds = 60;

    // События
    public event Action<string> OnRequest;
    public event Action OnSick;
    public event Action OnHealed;
    public event Action<string> OnRequestFulfilled;
    public event Action OnLifeEnd;

    public void StartLife()
    {
        DateTime endTime = DateTime.Now.AddSeconds(lifetimeSeconds);
        Console.WriteLine("Ваш Тамагочи начал свою жизнь!\n");

        while (isAlive && DateTime.Now < endTime)
        {
            string request = GenerateRequest();
            DisplayTamagotchi(request);
            OnRequest?.Invoke(request);

            Console.WriteLine($"Тамагочи просит: {request}. Удовлетворить? (да/нет)");
            string response = Console.ReadLine()?.ToLower();

            if (response == "да")
            {
                ProcessRequest(request, true);
            }
            else
            {
                ProcessRequest(request, false);
            }

            if (!isAlive) break;

            Thread.Sleep(2000);
        }

        if (isAlive)
        {
            OnLifeEnd?.Invoke();
            Console.WriteLine("Жизненный цикл Тамагочи завершён.");
        }
    }

    private string GenerateRequest()
    {
        string request;
        do
        {
            request = Requests[random.Next(Requests.Length)];
        } while (request == lastRequest);

        lastRequest = request;
        return request;
    }

    private void DisplayTamagotchi(string request)
    {
        Console.Clear();
        Console.WriteLine(" (°-°) ");
        Console.WriteLine("/|_|\\ ");
        Console.WriteLine("  |  ");
        Console.WriteLine(" / \\ ");
        Console.WriteLine($"\nТамагочи: {(isSick ? "болеет" : "здоров")}");
        Console.WriteLine($"Просьба: {request}");
    }

    private void ProcessRequest(string request, bool accepted)
    {
        if (accepted)
        {
            Console.WriteLine($"Вы выполнили просьбу: {request}.");
            OnRequestFulfilled?.Invoke(request);

            if (isSick && request == "полечить")
            {
                isSick = false;
                unfulfilledRequests = 0;
                Console.WriteLine("Тамагочи выздоровел!");
                OnHealed?.Invoke();
            }
            else if (!isSick)
            {
                unfulfilledRequests = 0;
            }
        }
        else
        {
            Console.WriteLine($"Вы отказали в просьбе: {request}.");
            unfulfilledRequests++;

            if (unfulfilledRequests >= 3)
            {
                if (!isSick)
                {
                    isSick = true;
                    Console.WriteLine("Тамагочи заболел!");
                    OnSick?.Invoke();
                }
                else if (request == "полечить")
                {
                    isAlive = false;
                    Console.WriteLine("Тамагочи умер от болезни...");
                }
            }
        }
    }
}
