using System;
using System.Collections.Generic;
using System.Text;

namespace ReestrBKS.BusinessLogic.Import
{
    public static class ImportStatus
    {
        public static ImportStatuses Status { get; set; } = ImportStatuses.Ready;
    }

    public enum ImportStatuses
    {
        Ready,
        Import
    }
}
