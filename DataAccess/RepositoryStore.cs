using ReestrBKS.DataAccess.Interfaces;
using ReestrBKS.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReestrBKS.DataAccess
{
    public class RepositoryStore : IRepositoryStore
    {
        private IAmountTypeRepository amountTypeRepository;
        public IAmountTypeRepository AmountTypeRepository => amountTypeRepository;

        private IPersonRepository personRepository;
        public IPersonRepository PersonRepository => personRepository;

        private IStreetRepository streetRepository;
        public IStreetRepository StreetRepository => streetRepository;

        private ISubjectRepository subjectRepository;
        public ISubjectRepository SubjectRepository => subjectRepository;

        private ISubjectTypeRepository subjectTypeRepository;
        public ISubjectTypeRepository SubjectTypeRepository => subjectTypeRepository;

        public RepositoryStore(ReestrContext context, 
            IAmountTypeRepository amountTypeRepository, IPersonRepository personRepository,
            IStreetRepository streetRepository, ISubjectRepository subjectRepository,
            ISubjectTypeRepository subjectTypeRepository)
        {
            this.amountTypeRepository = amountTypeRepository;
            this.personRepository = personRepository;
            this.streetRepository = streetRepository;
            this.subjectRepository = subjectRepository;
            this.subjectTypeRepository = subjectTypeRepository;
        }
    }
}
