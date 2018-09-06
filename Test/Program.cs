using IMS.IService;
using IMS.Service;
using IMS.Service.Entity;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IMS.Service.Service;
using IMS.DTO;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                dbc.Database.Log = (sql) =>
                {
                    Console.WriteLine(sql);
                };

            }
            Console.ReadKey();
        }
    }
}
