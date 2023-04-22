using TrainApplication;

class Program
{
    private static Double price = 100.0;
    private static Train[]? trains = null;

    static TimeSpan sevenThirty = new TimeSpan(7, 30, 00);
    static TimeSpan nineThirty = new TimeSpan(9, 30, 00);
    static TimeSpan sixteen = new TimeSpan(16, 00, 00);
    static TimeSpan nineteenThirty = new TimeSpan(19, 30, 00);

    static void Main(string[] args)
    {
        trains = setupTrains();
        DisplayMenu();
    }
public static void DisplayMenu()
{
    bool exit = false;
    CardType cardType = CardType.none;
    string time;
    string type;
    int numberOftickets;
    string destination;
    bool child;

    while (!exit)
    {
        Console.WriteLine("Please select an option:");
        Console.WriteLine("1. Calculate ticket price");
        Console.WriteLine("2. Search trains");
        Console.WriteLine("3. Calculate ticket price based on trip");
        Console.WriteLine("4. Reserve train ticket");
        Console.WriteLine("5. Exit");

        string userInput = Console.ReadLine();

        switch (userInput)
        {
            case "1":
                    Console.Write("Enter time: ");
                    time = Console.ReadLine();
                    Console.Write("Enter card type: ");
                    type = Console.ReadLine();
                    if (type.Equals("oldPeople"))
                    {
                        cardType = CardType.oldPeople;
                    }
                    else if (type.Equals("family"))
                    {
                        cardType = CardType.family;
                    }
                    Console.Write("Enter number of tickets: ");
                    numberOftickets = Int32.Parse(Console.ReadLine());
                    Console.Write("Enter destination: ");
                    destination = Console.ReadLine();
                    Console.Write("Enter true if you have a child: ");
                    child = bool.Parse(Console.ReadLine());
                    Console.WriteLine(calculatePrice(time, cardType, numberOftickets, destination, child));
                    break;
            case "2":
                    Console.Write("Enter destination: ");
                    destination = Console.ReadLine();
                    Console.Write("Enter time: ");
                    time = Console.ReadLine();
                    SearchTrain(destination, time);
                break;
            case "3":
                    Console.Write("Enter isRoundTrip: ");
                    bool isRoundTrip = bool.Parse(Console.ReadLine());
                    Console.WriteLine(calculateTicketPriceBasedOnTrip(isRoundTrip));
                break;
            case "4":
                    Console.Write("Enter time of leaving: ");
                    time = Console.ReadLine();
                    Console.Write("Enter number of seats needed: ");
                    int numberOfSeatsNeeded = Int32.Parse(Console.ReadLine());
                    ReserveTrainTicket(trains, TimeSpan.Parse(time), numberOfSeatsNeeded);
                break;
            case "5":
                Console.WriteLine("Exiting...");
                exit = true;
                break;
            default:
                Console.WriteLine("Invalid input. Please try again.");
                break;
        }

        Console.WriteLine();
    }
}


public static Double calculatePrice(String enteredTime, CardType cardType, int numberOfTickets, String destination, bool child)
    {
        DateTime time = DateTime.Parse(enteredTime);
        Double finalPrice = 0;
        Double currentPrice = 0;
        bool flag = false;

        for (int i = 0; i < numberOfTickets; i++)
        {

            if (isRushHour(enteredTime))
            {
                currentPrice += price;
            }
            else
            {
                currentPrice = price;
                currentPrice -= price * 0.05;
                if (cardType == CardType.oldPeople && !flag)
                {
                    flag = true;
                    currentPrice -= currentPrice * 0.34;
                }
                else if (cardType == CardType.family)
                {
                    if (child)
                    {
                        currentPrice -= currentPrice * 0.5;
                    }
                    else
                    {
                        currentPrice -= currentPrice * 0.1;
                    }
                }
            }
            finalPrice += currentPrice;
            currentPrice = 0;
        }
        return finalPrice;
    }

    public static bool isRushHour(String enteredTime)
    {
        TimeSpan time = TimeSpan.Parse(enteredTime);

        if (time.Ticks > sevenThirty.Ticks && time.Ticks < nineThirty.Ticks
            || time.Ticks > sixteen.Ticks && time.Ticks < nineteenThirty.Ticks)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static Train[] setupTrains()
    {
        Train[] trains = new Train[3];

        trains[0] = new Train("New York", new TimeSpan(8, 30, 0), 50);
        trains[1] = new Train("Boston", new TimeSpan(10, 15, 0), 35);
        trains[2] = new Train("Washington D.C.", new TimeSpan(13, 45, 0), 70);

        return trains;
    }

    public static Train SearchTrain(string destination, string time)
    {
        TimeSpan departureTime = TimeSpan.Parse(time);
        foreach (Train train in trains)
        {
            if (train.destination == destination && train.departureTime == departureTime)
            {
                Console.WriteLine("There is a train with the desired destination and departure time");
                return train;
            }
        }
        Console.WriteLine("No train found with the desired destination and departure time");
        return null;
    }

    public static Double calculateTicketPriceBasedOnTrip(bool isRoundTrip)
    {
        Double total = price;
        if (isRoundTrip)
        {
            total *= 2;
            total *= 0.9; // 10% discount for round-trip
        }
        return total;
    }

    public static bool ReserveTrainTicket(Train[] trains, TimeSpan desiredTimeOfLeaving, int numOfSeatsNeeded)
    {
        string message = "";
        bool reservationMade = false;

        foreach (Train train in trains)
        {
            // check if the train is available at the desired time
            if (train.departureTime == desiredTimeOfLeaving)
            {
                // check if the train has enough seats
                if (train.seatsAvailable >= numOfSeatsNeeded)
                {
                    train.seatsAvailable -= numOfSeatsNeeded;
                    message = $"Reservation successful for {numOfSeatsNeeded} seat(s) on train to {train.destination} leaving at {train.departureTime}";
                    Console.WriteLine(message);
                    reservationMade = true;
                    SaveDataToFile(train.destination, train.departureTime, reservationMade);
                    break;
                }
                else
                {
                    message = $"Train to {train.destination} leaving at {train.departureTime} doesn't have enough seats.";
                    Console.WriteLine(message);
                    break;
                }
            }
        }

        if (!reservationMade)
        {
            message = "No trains available at the desired time.";
            Console.WriteLine(message);
        }

        return reservationMade;
    }

    public static void SaveDataToFile(string destination, TimeSpan departureTime, bool isReservationMade)
    {
        using StreamWriter writer = new("data.txt", true);
        writer.WriteLine("Destination: " + destination + ", Departure Time: " + departureTime + ", Reserved: " + isReservationMade + ", Reservation was made on: " + DateTime.Now.ToString());
    }
}
