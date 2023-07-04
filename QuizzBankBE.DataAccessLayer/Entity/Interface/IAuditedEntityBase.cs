using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzBankBE.DataAccessLayer.Entity.Interface
{
    internal interface IAuditedEntityBase
    {
        public DateTime? Createdat { get; set; }

        public DateTime? Updatedat { get; set; }

        public int? IsDeleted { get; set; }
    }
}
