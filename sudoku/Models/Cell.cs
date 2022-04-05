using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sudoku.Models
{
    public class Cell
    {
        public int Discriminator { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
