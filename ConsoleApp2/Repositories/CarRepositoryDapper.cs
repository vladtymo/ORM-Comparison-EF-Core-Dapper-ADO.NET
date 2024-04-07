using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Test_Dapper
{
    // Dapper
    public class CarRepositoryDapper : ICarRepository
    {
        string connectionString = null;
        public CarRepositoryDapper(string conn)
        {
            connectionString = conn;
        }
        public List<Car> GetCars()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Car>("SELECT * FROM Cars").ToList();
            }
        }

        public Car Get(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Car>("SELECT * FROM Cars WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }

        public Car Create(Car car)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                //var sqlQuery = "INSERT INTO Cars (Make, Model, ModelYear) VALUES(@Make, @Model, @ModelYear)";
                //db.Execute(sqlQuery, car);

                // якщо нам потрібно отримати Id доданого авто
                var sqlQuery = "INSERT INTO Cars (Make, Model, ModelYear) VALUES(@Make, @Model, @ModelYear); SELECT CAST(SCOPE_IDENTITY() as int)";
                int? carId = db.Query<int>(sqlQuery, car).FirstOrDefault();
                car.Id = carId.Value;
                return car;
            }
        }

        public void Update(Car car)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Cars SET Make = @Make, Model = @Model, ModelYear = @ModelYear WHERE Id = @Id";
                db.Execute(sqlQuery, car);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Cars WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}
