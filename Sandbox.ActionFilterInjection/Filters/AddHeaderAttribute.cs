using System;
using Sandbox.ActionFilterInjection.Services;

namespace Sandbox.ActionFilterInjection.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AddHeaderAttribute : Attribute, IServiceAttribute
    {
        readonly string _name;
        readonly string _value;

        public AddHeaderAttribute(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Value
        {
            get { return _value; }
        }

        public Type ServiceType
        {
            get { return typeof (AddHeaderFilter); }
        }
    }
}