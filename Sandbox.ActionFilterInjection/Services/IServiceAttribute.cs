using System;

namespace Sandbox.ActionFilterInjection.Services
{
    public interface IServiceAttribute
    {
        Type ServiceType { get; }
    }
}