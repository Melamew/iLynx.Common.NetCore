namespace iLynx.Common
{
    public interface IBinding<TValue>
    {
        void SetValue(TValue value);
        TValue GetValue();
    }
}