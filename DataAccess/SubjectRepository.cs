using ReestrBKS.DataAccess.Interfaces;
using ReestrBKS.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReestrBKS.DataAccess
{
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        public SubjectRepository(ReestrContext context) :base(context)
        {
        }
    }
}
