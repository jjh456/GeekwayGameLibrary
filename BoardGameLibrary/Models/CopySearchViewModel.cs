using BoardGameLibrary.Data.Models;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;

namespace BoardGameLibrary.Models
{
    [Validator(typeof(CopySearchValidator))]
    public class CopySearchViewModel
    {
        [Display(Name = "Title/ID")]
        public string Info { get; set; }

        public bool NavSearch { get; set; }
    }

    public class CopySearchValidator : AbstractValidator<CopySearchViewModel>
    {
        ApplicationDbContext _db;

        public CopySearchValidator()
        {
            _db = new ApplicationDbContext();
            RuleFor(x => x.Info).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("You must provide search criteria.");
        }
    }
}