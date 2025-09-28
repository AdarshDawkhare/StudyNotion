using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Response
    {
        public bool IsSuccess { get; set; } = false;

        public string Message { get; set; } = string.Empty;
    }
}
