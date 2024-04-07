using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Dapper.Repositories
{
    public class CarRepositoryEF_SQL : ICarRepository
    {
        CarDbModel context = null;
        public CarRepositoryEF_SQL(string conn)
        {
            context = new CarDbModel(conn);
            //context.Database.Log = Console.Write;
        }

        public Car Create(Car car)
        {
            //var added = context.Cars.Add(car);
            //Save();
            int rows = context.Database.ExecuteSqlRaw("insert into Cars(Make, Model, ModelYear) " +
                "values(@make, @model, @year)", new SqlParameter("@make", car.Make), new SqlParameter("@model", car.Model), new SqlParameter("@year", car.ModelYear));

            return car;
        }

        public void Delete(int id)
        {
            var car = this.Get(id);
            if (car != null)
            {
                int rows = context.Database.ExecuteSqlRaw("delete from Cars where Id = @id", new SqlParameter("@id", id));
                //Save();
            }
        }

        public Car Get(int id)
        {
            return context.Cars.FromSqlRaw("SELECT * from Cars where Id=@id", new SqlParameter("@id", id)).FirstOrDefault();
        }

        public List<Car> GetCars()
        {
            return context.Cars.FromSqlRaw("SELECT * from Cars").ToList();
        }

        public void Update(Car car)
        {
            int rows = context.Database.ExecuteSqlRaw("update Cars " +
                "set Make = @make, " +
                "Model = @model," +
                "ModelYear = @year", new SqlParameter("@make", car.Make), new SqlParameter("@model", car.Model), new SqlParameter("@year", car.ModelYear));

            //context.Entry(car).State = EntityState.Modified;
            //Save();
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
