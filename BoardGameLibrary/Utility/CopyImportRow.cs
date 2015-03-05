using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Utility
{
    public class CopyImportRow
    {
        public string GameTitle { get; set; }
        public int LibraryID { get; set; }
        public string OwnerName { get; set; }
        public int FileRowNumber { get; set; }
    }
}