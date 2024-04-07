using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace Test_Dapper
{
    public class CarDbModel : DbContext
    {
        private readonly string connectionString;

        public CarDbModel(string connectionString) : base()
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }
        public virtual DbSet<Car> Cars { get; set; }
    }
    // EF
    public class CarRepositoryEF : ICarRepository
    {
        CarDbModel context = null;
        public CarRepositoryEF(string conn)
        {
            context = new CarDbModel(conn);
            //context.Database = Console.Write;
        }

        public Car Create(Car car)
        {
            var added = context.Cars.Add(car);
            Save();
            return added.Entity;
        }

        public void Delete(int id)
        {
            var car = context.Cars.Find(id);
            if (car != null)
            {
                context.Cars.Remove(car);
                Save();
            }
        }

        public Car Get(int id)
        {
            return context.Cars.Find(id); 
        }

        public List<Car> GetCars()
        {
            return context.Cars.ToList();
        }

        public void Update(Car car)
        {
            context.Entry(car).State = EntityState.Modified;
            Save();
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}