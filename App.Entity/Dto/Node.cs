using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entity.Dto
{
    public class Node
    {
        public string Name { get; set; }
        public double Rate { get; set; }
        public List<Node> Children { get; set; }
    }
}
