using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Models
{
    [Validator(typeof(CopyValidator))]
    public class Copy
    {
        public int ID { get; set; }
        [Display(Name = "Library ID")]
        public int LibraryID { get; set; }
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }
        [Display(Name = "Game Identifer")]
        public int GameID { get; set; }
        [Display(Name = "Owner")]
        public string OwnerName { get; set; }
        [Display(Name = "Checkout Info")]
        public virtual Checkout CurrentCheckout { get; set; }
        [Display(Name = "Checkout History")]
        public virtual IList<Checkout> CheckoutHistory { get; set; }
        public string Notes { get; set; }

        public Copy()
        {
            CheckoutHistory = new List<Checkout>();
        }
    }

    public class CopyValidator : AbstractValidator<Copy>
    {
        public CopyValidator()
        {
            RuleFor(x => x.LibraryID).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Library ID is required.")
                .Must(BeUnique).WithMessage("A copy with that library ID exists already.");

            RuleFor(x => x.GameID).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Must provide a game ID.");

            RuleFor(x => x.OwnerName).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Must provide the owner's name.");
        }

        private bool BeUnique(int libID)
        {
            var _db = new ApplicationDbContext();
            if (_db.Copies.SingleOrDefault(c => c.LibraryID == libID) != null) 
                return false;

            return true;
        }
    }
}