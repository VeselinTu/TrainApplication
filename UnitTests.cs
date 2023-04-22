using NUnit.Framework;

namespace TrainApplication
{
    class UnitTests
    {
        [Test]
        public void TestCalculatePrice_RushHourNoDiscount()
        {
            // Arrange
            string enteredTime = "08:00:00";
            CardType cardType = CardType.none;
            int numberOfTickets = 2;
            string destination = "New York";
            bool child = false;

            // Act
            double price = Program.calculatePrice(enteredTime, cardType, numberOfTickets, destination, child);

            // Assert
            Assert.AreEqual(100.00, price);
        }

        [Test]
        public void TestCalculatePrice_RushHourWithDiscount()
        {
            // Arrange
            string enteredTime = "08:00:00";
            CardType cardType = CardType.oldPeople;
            int numberOfTickets = 2;
            string destination = "New York";
            bool child = false;

            // Act
            double price = Program.calculatePrice(enteredTime, cardType, numberOfTickets, destination, child);

            // Assert
            Assert.AreEqual(66.00, price);
        }

        [Test]
        public void TestCalculatePrice_NonRushHourNoDiscount()
        {
            // Arrange
            string enteredTime = "11:00:00";
            CardType cardType = CardType.none;
            int numberOfTickets = 1;
            string destination = "Chicago";
            bool child = false;

            // Act
            double price = Program.calculatePrice(enteredTime, cardType, numberOfTickets, destination, child);

            // Assert
            Assert.AreEqual(47.50, price);
        }

        [Test]
        public void TestCalculatePrice_NonRushHourChildDiscount()
        {
            // Arrange
            string enteredTime = "15:00:00";
            CardType cardType = CardType.family;
            int numberOfTickets = 2;
            string destination = "Washington D.C.";
            bool child = true;

            // Act
            double price = Program.calculatePrice(enteredTime, cardType, numberOfTickets, destination, child);

            // Assert
            Assert.AreEqual(47.50, price);
        }

        [Test]
        public void TestCalculatePrice_NonRushHourSeniorDiscount()
        {
            // Arrange
            string enteredTime = "16:00:00";
            CardType cardType = CardType.oldPeople;
            int numberOfTickets = 1;
            string destination = "Boston";
            bool child = false;

            // Act
            double price = Program.calculatePrice(enteredTime, cardType, numberOfTickets, destination, child);

            // Assert
            Assert.AreEqual(28.05, price);
        }

        public Train[] trains;
        [SetUp]
        public void SetUp()
        {
            // Create some sample trains for testing
            trains = new Train[]
            {
            new Train("Seattle", new TimeSpan(8, 0, 0), 50),
            new Train("Portland", new TimeSpan(9, 30, 0), 20),
            new Train("San Francisco", new TimeSpan(10, 15, 0), 100),
            new Train("Los Angeles", new TimeSpan(12, 0, 0), 75)
            };
        }

        [Test]
        public void ReserveTrainTicket_ValidReservation_ReturnsTrue()
        {
            // Arrange
            TimeSpan desiredTime = new TimeSpan(9, 30, 0);
            int seatsNeeded = 2;

            // Act
            bool result = Program.ReserveTrainTicket(trains, desiredTime, seatsNeeded);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(18, trains[1].seatsAvailable); // Check that seats were reserved
        }

        [Test]
        public void ReserveTrainTicket_NotEnoughSeats_ReturnsFalse()
        {
            // Arrange
            TimeSpan desiredTime = new TimeSpan(12, 0, 0);
            int seatsNeeded = 100;

            // Act
            bool result = Program.ReserveTrainTicket(trains, desiredTime, seatsNeeded);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(75, trains[3].seatsAvailable); // Check that seats were not reserved
        }

        [Test]
        public void ReserveTrainTicket_NoTrainsAvailable_ReturnsFalse()
        {
            // Arrange
            TimeSpan desiredTime = new TimeSpan(13, 0, 0);
            int seatsNeeded = 1;

            // Act
            bool result = Program.ReserveTrainTicket(trains, desiredTime, seatsNeeded);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CalculateTicketPriceBasedOnTrip_ReturnsCorrectPrice_WhenIsRoundTripIsTrue()
        {
            // Arrange
            bool isRoundTrip = true;
            double expectedPrice = 180.00;

            // Act
            double result = Program.calculateTicketPriceBasedOnTrip(isRoundTrip);

            // Assert
            Assert.AreEqual(expectedPrice, result);
        }

        [Test]
        public void CalculateTicketPriceBasedOnTrip_ReturnsCorrectPrice_WhenIsRoundTripIsFalse()
        {
            // Arrange
            bool isRoundTrip = false;
            double expectedPrice = 100.00;

            // Act
            double result = Program.calculateTicketPriceBasedOnTrip(isRoundTrip);

            // Assert
            Assert.AreEqual(expectedPrice, result);
        }

        [Test]
        public void SearchTrain_ReturnsCorrectTrain_WhenDestinationAndDepartureTimeMatch()
        {
            // Arrange
            string destination = "New York";
            TimeSpan departureTime = new TimeSpan(10, 0, 0);
            string time = "10, 0, 0";
            Train expectedTrain = new Train("New York", departureTime, 100);

            // Act
            Train result = Program.SearchTrain(destination, time);

            // Assert
            Assert.AreEqual(expectedTrain, result);
        }

        [Test]
        public void SearchTrain_ReturnsNull_WhenNoTrainMatchesDestinationAndDepartureTime()
        {
            // Arrange
            string destination = "Los Angeles";
            TimeSpan departureTime = new TimeSpan(11, 0, 0);
            string time = "11, 0, 0";

            // Act
            Train result = Program.SearchTrain(destination, time);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void isRushHour_ReturnsTrue_WhenEnteredTimeIsBetween730And930()
        {
            // Arrange
            string enteredTime = "08:00:00";

            // Act
            bool result = Program.isRushHour(enteredTime);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void isRushHour_ReturnsFalse_WhenEnteredTimeIsBefore730()
        {
            // Arrange
            string enteredTime = "06:00:00";

            // Act
            bool result = Program.isRushHour(enteredTime);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void isRushHour_ReturnsTrue_WhenEnteredTimeIsBetween1600And1930()
        {
            // Arrange
            string enteredTime = "18:00:00";

            // Act
            bool result = Program.isRushHour(enteredTime);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void isRushHour_ReturnsFalse_WhenEnteredTimeIsAfter1930()
        {
            // Arrange
            string enteredTime = "20:00:00";

            // Act
            bool result = Program.isRushHour(enteredTime);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
