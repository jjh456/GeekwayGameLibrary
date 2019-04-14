using FluentValidation;
using FluentValidation.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BoardGameLibrary.Data.Models
{
    [Validator(typeof(CopyValidator))]
    public class Copy
    {
        public int ID { get; set; }
        [Display(Name = "Library ID")]
        public string LibraryID { get; set; }
        [NotMapped]
        public string Title {
            get { return Game.Title; }
        }
        public int? CopyCollectionID { get; set; }
        [ForeignKey("CopyCollectionID")]
        public virtual CopyCollection CopyCollection { get; set; }
        [Display(Name = "Owner")]
        public string OwnerName { get; set; }
        [Display(Name = "Checkout Info")]
        public virtual Checkout CurrentCheckout { get; set; }
        [Display(Name = "Checkout History")]
        public virtual IList<Checkout> CheckoutHistory { get; set; }
        public bool Winnable { get; set; }
        public string Notes { get; set; }
        
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }
        [Display(Name = "Game Identifer")]
        public int GameID { get; set; }

        public Copy()
        {
            Winnable = false;
            CheckoutHistory = new List<Checkout>();
        }
    }

    public class CopyValidator : AbstractValidator<Copy>
    {
        public CopyValidator()
        {
            RuleFor(x => x.LibraryID).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Library ID is required.");

            RuleFor(x => x.GameID).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Must provide a game ID.");

            RuleFor(x => x.OwnerName).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Must provide the owner's name.");
        }

        private bool BeUnique(string libID)
        {
            var _db = new ApplicationDbContext();
            if (_db.Copies.SingleOrDefault(c => c.LibraryID == libID) != null) 
                return false;

            return true;
        }
    }
}