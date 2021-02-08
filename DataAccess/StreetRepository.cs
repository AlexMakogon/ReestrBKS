using ReestrBKS.DataAccess.Interfaces;
using ReestrBKS.DataModel;

namespace ReestrBKS.DataAccess
{
    public class StreetRepository : Repository<Street>, IStreetRepository
    {
        public StreetRepository(ReestrContext context) :base(context)
        {
        }
    }
}
