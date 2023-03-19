using TrainApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        CardType cardType = CardType.none;
        Console.WriteLine("Enter time: ");
        string time = Console.ReadLine();
        Console.WriteLine("Enter card type: ");
        string type = Console.ReadLine();
        if (type.Equals("oldPeople"))
        {
            cardType = CardType.oldPeople;
        }
        else if (type.Equals("family"))
        {
            cardType = CardType.family;
        }
        Console.WriteLine("Enter number of tickets: ");
        int numberOftickets = Int32.Parse(Console.ReadLine());
        Console.WriteLine("Enter destination: ");
        string destination = Console.ReadLine();
        Console.WriteLine("Enter true if you have a child: ");
        bool child = bool.Parse(Console.ReadLine());
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

    public static Train SearchTrain(string destination, TimeSpan departureTime)
    {
        foreach (Train train in trains)
        {
            if (train.destination == destination && train.departureTime == departureTime)
            {
                return train;
            }
        }
        return null;
    }

    public Double CalculateTicketPriceBasedOnTrip(bool isRoundTrip)
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

}