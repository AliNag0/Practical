using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Game
{
    public class Card
    {
        public Button Button { get; set; }
        public string Value { get; set; }
        public bool IsMatched { get; set; } = false;
    }
}
