using ReestrBKS.DataAccess.Interfaces;
using ReestrBKS.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReestrBKS.DataAccess
{
    public class SubjectTypeRepository : Repository<SubjectType>, ISubjectTypeRepository
    {
        public SubjectTypeRepository(ReestrContext context) :base(context)
        {
        }
    }
}
