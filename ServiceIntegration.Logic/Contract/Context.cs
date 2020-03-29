using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceIntegration.Logic.Contract
{

    internal class Context<T> where T : class
    {
        internal List<T> Entities { get; set; }

        internal T GetEntity()
        {
            return Entities.Where(x => x.GetType() == typeof(T)).FirstOrDefault() as T;
        }

        internal Context()
        {
            Entities = new List<T>();
        }
    }
}
