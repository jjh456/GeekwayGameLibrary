using BoardGameLibrary.Data.Models;
using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BoardGameLibrary.Models
{
    [Validator(typeof(CopyCheckinValidator))]
    public class CopyCheckInViewModel
    {
        [Display(Name = "Library ID #")]
        public string CopyLibraryID { get; set; }

        public IList<string> Messages { get; set; }

        public CopyCheckInViewModel()
        {
            Messages = new List<string>();
        }
    }

    public class CopyCheckinValidator : AbstractValidator<CopyCheckInViewModel>
    {
        ApplicationDbContext _db;
        public CopyCheckinValidator()
        {
            _db = new ApplicationDbContext();
            
            RuleFor(x => x.CopyLibraryID).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Library ID required.")
                .Must(Exist).WithMessage("Not found.  Make sure the ID is correct and in the system.")
                .Must(BeCheckedOut).WithMessage("This copy is not checked out.");
        }

        private bool Exist(string copyLibraryID)
        {
            var copyLibraryIDInt = Convert.ToInt32(copyLibraryID.Replace("*", ""));
            if (_db.Copies.AsNoTracking().FirstOrDefault(c => c.LibraryID == copyLibraryIDInt) == null)
                return false;

            return true;
        }

        private bool BeCheckedOut(string copyLibraryID)
        {
            var copyLibraryIDInt = Convert.ToInt32(copyLibraryID.Replace("*", ""));
            var copy = _db.Copies.AsNoTracking().FirstOrDefault(c => c.LibraryID == copyLibraryIDInt);
            var currentCheckout = copy.CurrentCheckout;
            if (currentCheckout == null)
                return false;

            return true;
        }
    }
}