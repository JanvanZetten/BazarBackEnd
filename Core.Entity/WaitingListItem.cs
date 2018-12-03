using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity
{
    public class WaitingListItem
    {
        public int Id { get; set; }
        public User Booker { get; set; }
        public DateTime Date { get; set; }
    }
}
