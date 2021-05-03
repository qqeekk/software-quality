namespace SetCalculations
{
    public interface IState<T>
    {
        T State { get; }
    }
}
