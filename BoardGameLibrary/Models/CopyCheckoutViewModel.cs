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
            RuleFor(x => x.AttendeeBadgeID).Must(BeAnExistingAttendee).WithMessage("Attendee not found in the system.");
            var gameAlreadyCheckedOut = "";

            RuleFor(x => x.AttendeeBadgeID).Must(badgeId => NotAlreadyHaveACopyCheckedOut(badgeId, ref gameAlreadyCheckedOut))
                                           .WithMessage(string.Format("Attendee already has {0} checked out."), gameAlreadyCheckedOut);

            RuleFor(x => Convert.ToInt32(x.CopyLibraryID.Replace("*", ""))).Must(BeAnExistingGameCopy).WithMessage("Could not find a game with that ID.  Make sure the ID is correct and the game is in the system.");
            RuleFor(x => Convert.ToInt32(x.CopyLibraryID.Replace("*", ""))).Must(NotBeCheckedOut).WithMessage("This copy is checked out already.  Please check it in first.");
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

        private bool BeAnExistingGameCopy(int copyLibraryID)
        {
            if (_db.Copies.SingleOrDefault(c => c.LibraryID == copyLibraryID) != null)
                return false;

            return true;
        }

        private bool NotBeCheckedOut(int copyLibraryID)
        {
            if (_db.Copies.SingleOrDefault(c => c.LibraryID == copyLibraryID && c.CurrentCheckout == null) != null)
                return false;

            return true;
        }
    }
}