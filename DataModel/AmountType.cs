using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrBKS.DataModel
{
    /// <summary>
    /// Вид суммы
    /// </summary>
    public class AmountType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<HotWaterLine> HotWaterLines { get; set; }
        public virtual List<CommonHouseLine> CommonHouseLines { get; set; }
    }
}
