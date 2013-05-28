using System.Collections.Generic;

namespace Sandbox.ActionFilterInjection.Services
{
    public interface IService<in T>
        where T : IServiceAttribute
    {
        IEnumerable<T> Attributes { set; }
    }
}