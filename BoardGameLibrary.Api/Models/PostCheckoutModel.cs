using BoardGameLibrary.Api.Validators;
using FluentValidation.Attributes;

namespace BoardGameLibrary.Api.Models
{
    [Validator(typeof(PostCheckoutModelValidator))]
    public class PostCheckoutModel
    {
        public string LibraryId { get; set; }
        public string AttendeeBadgeNumber { get; set; }
        public bool OverrideLimit { get; set; }
    }
}