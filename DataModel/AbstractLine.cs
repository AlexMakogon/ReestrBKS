using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrBKS.DataModel
{
    public interface IAbstractLine
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string AccountNumber { get; set; }

        public int? PersonId { get; set; }
        public Person Person { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int? AmountTypeId { get; set; }
        public AmountType AmountType { get; set; }
    }

    public class AbstractLineComparer<T> : IEqualityComparer<T> where T : IAbstractLine
    {
        public bool Equals([AllowNull] T x, [AllowNull] T y)
        {
            return x.PersonId == y.PersonId && x.SubjectId == y.SubjectId && x.AccountNumber == y.AccountNumber;

            //int? xPersonId = x.Person != null ? x.Person.Id : x.PersonId;
            //int? yPersonId = y.Person != null ? y.Person.Id : y.PersonId;
            //int xSubjectId = x.Subject != null ? x.Subject.Id : x.SubjectId;
            //int ySubjectId = y.Subject != null ? y.Subject.Id : y.SubjectId;

            //if (xPersonId == yPersonId && xSubjectId == ySubjectId && x.AccountNumber == y.AccountNumber)
            //    return true;
            //else
            //    return false;
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.GetHashCode();
        }
    }
}
