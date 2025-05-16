﻿namespace GameStore.Models.DTOs.User
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string? RoleName { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
