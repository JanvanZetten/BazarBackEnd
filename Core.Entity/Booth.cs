using System;
namespace Core.Entity
{
    public class Booth
    {
        public int Id { get; set; }
        public User Booker { get; set; }
    }
}
