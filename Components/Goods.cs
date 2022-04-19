using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.components
{
    // Generic good definition. JSON load at innit to create fixed sized arrays.
    public class Goods
    {
        public String name { get; set; }
        public int id { get; set; }
        public int size { get; set; }
        public int buildEffort { get; set; }
        public List<int[]> cost { get; set; }
    }
}