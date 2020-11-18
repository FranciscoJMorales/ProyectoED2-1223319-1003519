﻿using System;
using Processors;

namespace Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Key { get; set; }

        public User(string name, string password)
        {
            Id = Guid.NewGuid();
            Name = name;
            var cesar = new CesarEncryptor("");
            Password = cesar.CipherPassword(password);
            Random rng = new Random();
            Key = rng.Next(0, 1021);
        }
    }
}
