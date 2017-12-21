using System.Collections.Generic;

namespace BoardGameLibrary.Api.Models
{
    public class GetGamesResponseModel
    {
        public IEnumerable<GameResponseModel> Games { get; set; }
    }
}