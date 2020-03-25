using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vMTS.Models
{
   public class Group<T1, T2>
    {
        public T1 Key;

        public IEnumerable<T2> Values;

    }
}
