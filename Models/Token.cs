﻿namespace AuthService.Models
{
    public class Token
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
