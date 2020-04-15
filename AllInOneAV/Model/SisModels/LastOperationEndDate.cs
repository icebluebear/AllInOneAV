using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SisModels
{
    public class LastOperationEndDate
    {
        public LastOperationEndDate()
        {
            LastOperationDate = DateTime.Today.AddDays(-10);
        }

        public DateTime LastOperationDate { get; set; }
    }
}
