using SetParsing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SetCalculations.Adapters
{
    public class Parser
    {
        public virtual object Parse(string line)
        {
            return SetParsing.Parser.Parse<object>(line);
        }

        public bool TryParse(string line, out IReadOnlySet<object> res, out IEnumerable<Exception> errors)
        {
            res = new HashSet<object>();
            errors = Enumerable.Empty<Exception>();

            try
            {
                var obj = Parse(line);
                res = obj as IReadOnlySet<object> ?? new HashSet<object>(new[] { obj }, new StructuralEqualityComparer());
                return true;
            }
            catch (AggregateException ex)
            {
                errors = ex.InnerExceptions;
                return false;
            }
        }
    }
}
