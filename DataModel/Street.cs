using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrBKS.DataModel
{
    /// <summary>
    /// Улица
    /// </summary>
    public class Street
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Subject> Subjects { get; set; }
    }
}
