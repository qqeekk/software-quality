namespace SetCalculations.Adapters
{
    public class Parser
    {
        public virtual object Parse(string line)
        {
            return SetParsing.Parser.Parse<object>(line);
        }
    }
}
