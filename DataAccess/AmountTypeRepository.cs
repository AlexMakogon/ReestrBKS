using ReestrBKS.DataAccess.Interfaces;
using ReestrBKS.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReestrBKS.DataAccess
{
    public class AmountTypeRepository : Repository<AmountType>, IAmountTypeRepository
    {
        public AmountTypeRepository(ReestrContext context) :base(context)
        {
        }
    }
}
