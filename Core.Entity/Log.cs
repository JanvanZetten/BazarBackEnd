using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity
{
    public class Log
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
    }
}
