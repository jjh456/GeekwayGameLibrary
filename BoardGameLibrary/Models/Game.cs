using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;

namespace BoardGameLibrary.Models
{
    [Validator(typeof(GameValidator))]
    public class Game
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public virtual IList<Copy> Copies { get; set; }
        public virtual IList<Rating> Ratings { get; set; }

        public Game()
        {
            Copies = new List<Copy>();
            Ratings = new List<Rating>();
        }
    }

    public class GameValidator : AbstractValidator<Game>
    {
        ApplicationDbContext _db;
        public GameValidator()
        {
            RuleFor(x => x.Title).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Game title is required.");
        }
    }
}