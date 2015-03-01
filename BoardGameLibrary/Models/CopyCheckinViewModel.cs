using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Models
{
    [Validator(typeof(CopyValidator))]
    public class CopyCheckInViewModel
    {
        [Display(Name = "Library ID #")]
        public string CopyLibraryID { get; set; }
    }

    public class CopyCheckinValidator : AbstractValidator<CopyCheckInViewModel>
    {
        ApplicationDbContext _db;
        public CopyCheckinValidator()
        {
            _db = new ApplicationDbContext();
            RuleFor(x => Convert.ToInt32(x.CopyLibraryID.Replace("*", ""))).Must(Exist).WithMessage("Could not find a game with that ID.  Make sure the ID is correct and the game is in the system.");
            RuleFor(x => Convert.ToInt32(x.CopyLibraryID.Replace("*", ""))).Must(BeCheckedOut).WithMessage("This copy is checked out already.  Please check it in first.");
        }

        private bool Exist(int copyLibraryID)
        {
            if (_db.Copies.SingleOrDefault(c => c.LibraryID == copyLibraryID) != null)
                return false;

            return true;
        }

        private bool BeCheckedOut(int copyLibraryID)
        {
            if (_db.Copies.SingleOrDefault(c => c.LibraryID == copyLibraryID && c.CurrentCheckout == null) == null)
                return false;

            return true;
        }
    }
}