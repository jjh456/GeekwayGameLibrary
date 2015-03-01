using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FluentValidation;
using FluentValidation.Attributes;

namespace BoardGameLibrary.Models
{
    [Validator(typeof(CopySearchValidator))]
    public class CopySearchViewModel
    {
        [Display(Name = "Library ID #")]
        public string CopyLibraryID { get; set; }
        [Display(Name = "Attendee Badge #")]
        public string AttendeeBadgeID { get; set; }
        [Display(Name = "Game Title")]
        public string GameTitle { get; set; }
    }

    public class CopySearchValidator : AbstractValidator<CopySearchViewModel>
    {
        ApplicationDbContext _db;

        public CopySearchValidator()
        {
            _db = new ApplicationDbContext();
            RuleFor(x => x.GameTitle).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("You must provide search criteria.");
        }
    }
}