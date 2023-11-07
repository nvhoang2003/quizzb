using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzBankBE.DataAccessLayer.DataObject
{
    public partial class Ranking
    {
        public string? FullName { get; set; }
        public float? TotalPoint { get; set; }
        public int? TotalTime { get; set; }
    }
}
