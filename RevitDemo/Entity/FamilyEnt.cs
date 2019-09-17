using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitDemo.Entity
{
    public class FamilyEnt
    {
        public Guid Id { get; set; }

        public bool IsChecked { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }
    }
}
