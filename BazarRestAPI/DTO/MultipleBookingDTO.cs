using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BazarRestAPI.DTO
{
    public class MultipleBookingDTO
    {
        public string Token { get; set; }
        public List<Booth> Booths { get; set; }
    }
}
