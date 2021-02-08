using ReestrBKS.DataModel;

namespace ReestrBKS.DataAccess.Interfaces
{
    public interface IRepositoryStore
    {
        public IAmountTypeRepository AmountTypeRepository { get; }
        public IPersonRepository PersonRepository { get; }
        public IStreetRepository StreetRepository { get; }
        public ISubjectRepository SubjectRepository { get; }
        public ISubjectTypeRepository SubjectTypeRepository { get; }
    }
}
