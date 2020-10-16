using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMFS.Formatters
{
    public class Formatter
    {
        private IFormatterStrategy strategy;

        public Formatter() { }
        public Formatter(IFormatterStrategy strategy)
        {
            this.strategy = strategy;
        }
        public void SetStrategy(IFormatterStrategy strategy)
        {
            this.strategy = strategy;
        }

        public string Print(Folder folder)
        {
            return this.strategy.Format(folder);
        }
    }
}
