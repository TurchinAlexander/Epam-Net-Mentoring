using System;

namespace IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ImportAttribute : Attribute
    {
    }
}