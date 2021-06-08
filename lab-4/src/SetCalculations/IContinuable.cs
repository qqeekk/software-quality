namespace SetCalculations
{
    public interface IContinuable<T> : IState<T>
    {
        IState<T> Next(string param);
    }
}
