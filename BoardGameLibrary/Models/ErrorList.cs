using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Models
{
    public class ErrorList
    {
        public IList<string> Errors { get; set; }

        public ErrorList()
        {
            Errors = new List<string>();
        }
    }
}