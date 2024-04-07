using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Test_Dapper.Repositories;

namespace Test_Dapper
{
    struct Stat
    {
        public long ReadByIdTime { get; set; }
        public long ReadAllTime { get; set; }
        public long CreateTime { get; set; }
        public long UpdateTime { get; set; }
        public long DeleteTime { get; set; }
    }
    class Program
    {
        static int nextId = 100;
        static Stat TestProvider(ICarRepository repos)
        {
            Stat stat = new Stat();
            Stopwatch sw;

            //repos.Get(1);

            // READ
            sw = Stopwatch.StartNew();
            repos.Get(nextId++);
            sw.Stop();
            stat.ReadByIdTime = sw.ElapsedMilliseconds;

            sw = Stopwatch.StartNew();
            repos.GetCars();
            sw.Stop();
            stat.ReadAllTime = sw.ElapsedMilliseconds;

            // CREATE
            sw = Stopwatch.StartNew();
            Car car = repos.Create(new Car()
            {
                Make = "Lada",
                Model = "Semirka",
                ModelYear = 2007
            });
            sw.Stop();
            stat.CreateTime = sw.ElapsedMilliseconds;

            // UPDATE
            car.Model = "NewModel";
            sw = Stopwatch.StartNew();
            repos.Update(car);
            sw.Stop();
            stat.UpdateTime = sw.ElapsedMilliseconds;

            // DELETE
            sw = Stopwatch.StartNew();
            repos.Delete(car.Id);
            sw.Stop();
            stat.DeleteTime = sw.ElapsedMilliseconds;

            foreach (var prop in stat.GetType().GetProperties())
            {
                Console.WriteLine($"{prop.Name}: {prop.GetValue(stat)}ms");
            }

            return stat;
        }
        static void Main(string[] args)
        {
            string connStr = ConfigurationManager.ConnectionStrings["CarDbModel"].ConnectionString;

            Console.WriteLine("---------- ADO.NET ----------");
            TestProvider(new CarRepositoryADO_NET(connStr));

            Console.WriteLine("---------- DAPPER ----------");
            TestProvider(new CarRepositoryDapper(connStr));

            Console.WriteLine("---------- ENTITY FRAMEWORK ----------");
            TestProvider(new CarRepositoryEF(connStr));

            Console.WriteLine("---------- ENTITY FRAMEWORK SQL ----------");
            //TestProvider(new CarRepositoryEF_SQL(connStr));
        }
    }
}
