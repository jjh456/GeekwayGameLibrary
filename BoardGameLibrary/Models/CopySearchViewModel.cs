using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Models
{
    public class CopySearchViewModel
    {
        [Display(Name = "Library ID #")]
        public string CopyLibraryID { get; set; }
        [Display(Name = "Attendee Badge #")]
        public string AttendeeBadgeID { get; set; }
        [Display(Name = "Game Title")]
        public string GameTitle { get; set; }
    }
}