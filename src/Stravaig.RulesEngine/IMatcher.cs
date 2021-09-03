namespace Stravaig.RulesEngine
{
    public interface IMatcher
    {
        bool IsMatch<TContext>(TContext context);
    }
}