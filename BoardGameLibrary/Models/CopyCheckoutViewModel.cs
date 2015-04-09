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
    [Validator(typeof(CopyCheckoutValidator))]
    public class CopyCheckOutViewModel
    {
        [Display(Name = "Library ID #")]
        public string CopyLibraryID { get; set; }
        [Display(Name = "Attendee Badge #")]
        public string AttendeeBadgeID { get; set; }
        public IList<string> Messages { get; set; }

        public CopyCheckOutViewModel()
        {
            Messages = new List<string>();
        }
    }

    public class CopyCheckoutValidator : AbstractValidator<CopyCheckOutViewModel>
    {
        ApplicationDbContext _db;
        public CopyCheckoutValidator()
        {
            _db = new ApplicationDbContext();
            var gameAlreadyCheckedOut = "";
            RuleFor(x => x.AttendeeBadgeID).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Badge ID required.")
                .Must(BeAnExistingAttendee).WithMessage("Attendee not found.")
                .Must(badgeId => NotAlreadyHaveACopyCheckedOut(badgeId, out gameAlreadyCheckedOut))
                                           .WithMessage("Attendee has {0} checked out.", x => gameAlreadyCheckedOut);

            RuleFor(x => x.CopyLibraryID).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("You must provide a library ID.")
                .Must(BeAnExistingGameCopy).WithMessage("Copy not found.")
                .Must(NotBeCheckedOut).WithMessage("That copy is checked out already.  Check it in first.");
        }

        private bool BeAnExistingAttendee(string attendeeBadgeID)
        {
            if (_db.Attendees.AsNoTracking().FirstOrDefault(a => a.BadgeID == attendeeBadgeID) == null)
                return false;

            return true;
        }

        private bool NotAlreadyHaveACopyCheckedOut(string attendeeBadgeID, out string gameCheckedOutAlready)
        {
            var currentlyCheckedOutCopy = _db.Copies.AsNoTracking().FirstOrDefault(c => c.CurrentCheckout.Attendee.BadgeID == attendeeBadgeID);
            if (currentlyCheckedOutCopy != null)
            {
                gameCheckedOutAlready = currentlyCheckedOutCopy.Game.Title + "(#" + currentlyCheckedOutCopy.LibraryID + ")";
                return false;
            }

            gameCheckedOutAlready = "";
            return true;
        }

        private bool BeAnExistingGameCopy(string copyLibraryID)
        {
            var copyLibraryIDInt = Convert.ToInt32(copyLibraryID.Replace("*", ""));
            if (_db.Copies.AsNoTracking().FirstOrDefault(c => c.LibraryID == copyLibraryIDInt) == null)
                return false;

            return true;
        }

        private bool NotBeCheckedOut(string copyLibraryID)
        {
            var copyLibraryIDInt = Convert.ToInt32(copyLibraryID.Replace("*", ""));
            var copy = _db.Copies.AsNoTracking().FirstOrDefault(c => c.LibraryID == copyLibraryIDInt);
            if (copy.CurrentCheckout != null)
                return false;

            return true;
        }
    }
}