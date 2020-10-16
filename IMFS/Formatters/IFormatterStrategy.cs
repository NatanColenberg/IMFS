using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMFS
{
    public interface IFormatterStrategy
    {
        string Format(Folder folder);
    }
}
