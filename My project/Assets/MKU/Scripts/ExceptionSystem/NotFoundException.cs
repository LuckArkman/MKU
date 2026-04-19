using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKU.Scripts.ExceptionSystem
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string? message) : base(message) { }

        public static void ThrowIfNull(object? o, string msg)
        {
            if (o == null) throw new NotFoundException(msg);
        }
    }
}
