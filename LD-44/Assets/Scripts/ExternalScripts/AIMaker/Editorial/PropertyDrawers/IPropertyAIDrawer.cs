namespace aim
{
    public interface IPropertyDrawer
    {
        object DrawProperty(object instance, System.Type instanceType, string label, AIAgent aiAgent);
    }
}

