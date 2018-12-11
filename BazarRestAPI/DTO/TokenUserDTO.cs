using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BazarRestAPI.DTO
{
    public class TokenUserDTO
    {
        public string Token { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
