namespace AppForSEII2526.LogViewer;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Start program");

        Subscriber subscriber = new Subscriber();

        bool run = true;
        string routingKey, input;

        while (run)
        {
            Console.Write("Choose a topic: (T)race, (D)ebug, (W)arning, (E)rror, (I)nformation, (C)ritical\n > ");

            input = Console.ReadLine();

            switch (input)
            {
                case "T":
                    routingKey = "log.trace";
                    break;

                case "D":
                    routingKey = "log.debug";
                    break;

                case "W":
                    routingKey = "log.warning";
                    break;

                case "E":
                    routingKey = "log.error";
                    break;

                case "I":
                    routingKey = "log.information";
                    break;

                case "C":
                    routingKey = "log.critical";
                    break;

                default:
                    routingKey = "Exit";
                    break;

            }

            if(routingKey == "Exit")
            {
                run = false;
                break;
            }


            subscriber.startConsuming(routingKey);
        }



        Console.WriteLine("End program");
    }
}
