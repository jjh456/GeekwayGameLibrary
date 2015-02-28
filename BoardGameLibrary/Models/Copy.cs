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
        [Required]
        public int LibraryID { get; set; }
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }
        [Required]
        public int GameID { get; set; }
        [Required]
        public string OwnerName { get; set; }
        public virtual Checkout CurrentCheckout { get; set; }
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
            RuleFor(x => x.LibraryID).NotEmpty().WithMessage("Library ID is required.");
            RuleFor(x => x.LibraryID).Must(BeUnique).WithMessage("A copy with that library ID exists already.");
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