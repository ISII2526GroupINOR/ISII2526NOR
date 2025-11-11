namespace AppForSEII2526.LogViewer;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Start program");

        Subscriber subscriber = new Subscriber();

        subscriber.startConsuming();


        Console.WriteLine("Start consuming");
    }
}
