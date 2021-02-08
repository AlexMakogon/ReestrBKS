using ReestrBKS.DataAccess.Interfaces;
using ReestrBKS.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReestrBKS.DataAccess
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(ReestrContext context) :base(context)
        {
        }
    }
}
