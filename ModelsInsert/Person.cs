using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsInsert
{
    public class Person
    {
        public string Address { set; get; }
        

        public string Name { set; get; }

        public string Sex { set; get; }

        public Guid ID { set; get; }

        public Guid? MarkerGuid { set; get; }
    }
}
