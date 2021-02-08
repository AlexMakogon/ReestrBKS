using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReestrBKS.WebClient.Models
{
    public class AbstractLineFilter
    {
        public string Street { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
        public string Room { get; set; }
        public string AccountNumber { get; set; }
        public string Person { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Street) && 
                string.IsNullOrEmpty(House) &&
                string.IsNullOrEmpty(Apartment) &&
                string.IsNullOrEmpty(Room) &&
                string.IsNullOrEmpty(AccountNumber) &&
                string.IsNullOrEmpty(Person);
        }
    }
}
