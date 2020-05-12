﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Parking
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
