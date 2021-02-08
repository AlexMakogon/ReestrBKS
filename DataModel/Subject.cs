using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReestrBKS.DataModel
{
    /// <summary>
    /// Объект
    /// </summary>
    public class Subject
    {
        public int Id { get; set; }
        public float TotalArea { get; set; }
        public float LivingArea { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
        public string Room { get; set; }

        public int StreetId { get; set; }
        public Street Street { get; set; }

        public int? SubjectTypeId { get; set; }
        public SubjectType SubjectType { get; set; }

        public virtual List<HotWaterLine> HotWaterLines { get; set; }
        public virtual List<CommonHouseLine> CommonHouseLines { get; set; }

        public string GetAddress()
        {
            string address = "";

            if (Street != null)
            {
                address += string.Format("{0} ул., {1} д.", Street.Name, House);

                if (!string.IsNullOrEmpty(Apartment))
                    address += string.Format(", {0} кв.", Apartment);

                if (!string.IsNullOrEmpty(Room))
                    address += string.Format(", {0} ком.", Apartment);
            }

            return address;
        }
    }
}
