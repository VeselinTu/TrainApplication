using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainApplication
{
    public class Train
    {
        public string destination;
        public TimeSpan departureTime;
        public int seatsAvailable;

        public Train(string destination, TimeSpan departureTime, int seatsAvailable)
        {
            this.destination = destination;
            this.departureTime = departureTime;
            this.seatsAvailable = seatsAvailable;
        }
    }
}
