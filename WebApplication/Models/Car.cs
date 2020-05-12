using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Enumerations;

namespace WebApplication.Models
{
    public class Car
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public short Year { get; set; }

        public string Model { get; set; }

        public short Capacity { get; set; }

        public string CarCode { get; set; }

        public decimal PricePerDay { get; set; }

        public decimal PricePerMonth { get; set; }

        public decimal PricePerYear { get; set; }

        public EnumCarType CarType { get; set; }

        public int ParkingId { get; set; }

        public Parking Parking { get; set; }
    }
}
