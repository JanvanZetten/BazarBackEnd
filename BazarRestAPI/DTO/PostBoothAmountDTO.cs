using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BazarRestAPI.DTO
{
    public class PostBoothAmountDTO
    {
        public int Amount { get; set; }
        public Booth Booth { get; set; }
    }
}
