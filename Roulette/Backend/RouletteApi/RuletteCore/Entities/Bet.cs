using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuletteCore.Entities
{
    public class Bet
    {
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public string Color { get; set; }
        public int? Number { get; set; }
        public bool? IsEven { get; set; }
    }
}
