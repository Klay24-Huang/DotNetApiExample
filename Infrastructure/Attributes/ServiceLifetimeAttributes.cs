namespace Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ScopedAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SingletonAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TransientAttribute : Attribute { }
}
