﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Usuarios
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Contrasena { get; set; }

        //public int IDUsuario { get; set; }

    }
}
