using System.Collections.Generic;

namespace Confab.Shared.Infrastructure.Logging
{
    internal class LoggerOptions
    {
        public string Level { get; set; }
        public IDictionary<string, string> Overrides { get; set; }
        public IEnumerable<string> ExcludePaths { get; set; }
        public IEnumerable<string> ExcludeProperties { get; set; }
        public IDictionary<string, object> Tags { get; set; }
    }
}