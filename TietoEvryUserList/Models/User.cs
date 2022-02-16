﻿namespace TietoEvryUserList.Models
{
    public class User
    {
        public int? Id { get; set; }


        public string? Name { get; set; }


        public string? Username { get; set; }


        public string? Email { get; set; }


        public string? Phone { get; set; }


        public string? Website { get; set; }


        public Address? Address { get; set; }


        public Company? Company { get; set; }


        public bool IsFavorite { get; set; }
    }
}
