﻿using System.Collections.Generic;

namespace DevelopingYou.API.Models.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<Goal> Goals { get; set; }

        public static implicit operator UserDTO(User user)
        {
            if (user == null)
                return null;

            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Goals = user.Goals,
            };
        }
    }

    public class UserWithToken : UserDTO
    {
        public string Token { get; set; }
    }
}
