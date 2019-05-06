
namespace aim
{
    public interface IValueContainerAI<T>
    {
        T Value { get; set; }
    }

    public interface IValueContainerAI
    {
        System.Type GetValueType();
        object ValueAsObject { get; set; }
    }
}