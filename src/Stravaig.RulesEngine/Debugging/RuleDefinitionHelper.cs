using System.Text;

namespace Stravaig.RulesEngine.Debugging
{
    public static class RuleDefinitionHelper
    {
        private const int IndentSize = 2;

        public static StringBuilder Indent(this StringBuilder sb, int level)
        {
            sb.Append(' ', IndentSize * level);
            return sb;
        }
    }
}