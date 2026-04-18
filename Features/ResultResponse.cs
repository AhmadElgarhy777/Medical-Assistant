using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features
{
    public class ResultResponse<T> 
    {
        public bool ISucsses { get; set; } = true;
        public string? Message { get; set; }
        public T? Obj { get; set; }

        public string? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

    }
}
