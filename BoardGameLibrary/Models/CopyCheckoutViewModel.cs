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
    }

    public class CopyCheckoutValidator : AbstractValidator<CopyCheckOutViewModel>
    {
        ApplicationDbContext _db;
        public CopyCheckoutValidator()
        {
            _db = new ApplicationDbContext();
            var gameAlreadyCheckedOut = "";
            RuleFor(x => x.AttendeeBadgeID).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("You must provide a badge ID.")
                .Must(BeAnExistingAttendee).WithMessage("Attendee not found in the system.")
                .Must(badgeId => NotAlreadyHaveACopyCheckedOut(badgeId, ref gameAlreadyCheckedOut))
                                           .WithMessage(string.Format("Attendee already has {0} checked out.", gameAlreadyCheckedOut));

            RuleFor(x => x.CopyLibraryID).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("You must provide a library ID.")
                .Must(BeAnExistingGameCopy).WithMessage("Could not find a copy with that ID.  Make sure the ID is correct and the copy is in the system.")
                .Must(NotBeCheckedOut).WithMessage("This copy is checked out already.  Please check it in first.");
        }

        private bool BeAnExistingAttendee(string attendeeBadgeID)
        {
            if (_db.Attendees.SingleOrDefault(a => a.BadgeID == attendeeBadgeID) == null)
                return false;

            return true;
        }

        private bool NotAlreadyHaveACopyCheckedOut(string attendeeBadgeID, ref string gameCheckedOutAlready)
        {
            var currentlyCheckedOutCopy = _db.Copies.SingleOrDefault(c => c.CurrentCheckout.Attendee.BadgeID == attendeeBadgeID);
            if (currentlyCheckedOutCopy != null)
            {
                gameCheckedOutAlready = currentlyCheckedOutCopy.Game.Title + " Copy #: " + currentlyCheckedOutCopy.LibraryID;
                return false;
            }

            return true;
        }

        private bool BeAnExistingGameCopy(string copyLibraryID)
        {
            var copyLibraryIDInt = Convert.ToInt32(copyLibraryID.Replace("*", ""));
            if (_db.Copies.SingleOrDefault(c => c.LibraryID == copyLibraryIDInt) != null)
                return false;

            return true;
        }

        private bool NotBeCheckedOut(string copyLibraryID)
        {
            var copyLibraryIDInt = Convert.ToInt32(copyLibraryID.Replace("*", ""));
            if (_db.Copies.SingleOrDefault(c => c.LibraryID == copyLibraryIDInt && c.CurrentCheckout == null) != null)
                return false;

            return true;
        }
    }
}