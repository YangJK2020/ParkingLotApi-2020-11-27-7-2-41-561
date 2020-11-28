﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.DTO
{
    public class ParkinglotDTO
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((ParkinglotDTO)obj);
        }

        private bool Equals(ParkinglotDTO other)
        {
            return Name == other.Name && Capacity == other.Capacity && Location == other.Location;
        }
    }
}
