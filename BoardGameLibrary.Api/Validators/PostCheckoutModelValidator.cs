using BoardGameLibrary.Api.Models;
using BoardGameLibrary.Data.Models;
using FluentValidation;
using System;
using System.Linq;

namespace BoardGameLibrary.Api.Validators
{
    public class PostCheckoutModelValidator : AbstractValidator<PostCheckoutModel>
    {
        ApplicationDbContext _db;
        public PostCheckoutModelValidator()
        {
            _db = new ApplicationDbContext();
            var gameAlreadyCheckedOut = "";
            RuleFor(x => x.AttendeeBadgeNumber).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Badge ID required.")
                .Must(BeAnExistingAttendee).WithMessage("Attendee not found.")
                .Must(badgeId => NotAlreadyHaveACopyCheckedOut(badgeId, out gameAlreadyCheckedOut)).Unless(model => model.OverrideLimit)
                .WithMessage(x => 
                    string.Format("Attendee has {0} checked out already. Check the override option if it's an expansion.", gameAlreadyCheckedOut)
                );

            RuleFor(x => x.LibraryId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("You must provide a library ID.")
                .Must(BeAnExistingGameCopy).WithMessage("Copy not found.")
                .Must(NotBeCheckedOut).WithMessage("That copy is checked out already. Check it in first.");
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
            var copyLibraryIDInt = copyLibraryID.Replace("*", "");
            if (_db.Copies.AsNoTracking().FirstOrDefault(c => c.LibraryID == copyLibraryIDInt) == null)
                return false;

            return true;
        }

        private bool NotBeCheckedOut(string copyLibraryID)
        {
            var copyLibraryIDInt = copyLibraryID.Replace("*", "");
            var copy = _db.Copies.AsNoTracking().FirstOrDefault(c => c.LibraryID == copyLibraryIDInt);
            if (copy.CurrentCheckout != null)
                return false;

            return true;
        }
    }
}