using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisualIntelligentScissors
{
    interface ITypeGetterSetter<T>
    {
        T get();
       void set(T d);
    }
}
