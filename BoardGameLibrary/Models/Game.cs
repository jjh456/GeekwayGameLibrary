using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
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

        public Game()
        {
            Copies = new List<Copy>();
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