using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data
{
    public class MongoConfiguration
    {
        public string _conexion;
        public MongoConfiguration(string conexion) => _conexion = conexion;
        //Lo que se ocupa para hacer la conexion epicamente
    }  
}
