using ReestrBKS.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReestrBKS.WebClient.Models
{
    public class AccountNumberInfo
    {
        public List<IAbstractLine> Lines;
        public Subject Subject;
        public Person Person;
    }
}
