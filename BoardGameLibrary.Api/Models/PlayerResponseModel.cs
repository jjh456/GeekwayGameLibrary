﻿namespace BoardGameLibrary.Api.Models
{
    public class PlayerResponseModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public bool WantsToWin { get; set; }
        public int? Rating { get; set; }
        internal int PlayID { get; set; }
    }
}