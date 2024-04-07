using System.Collections.Generic;

namespace Test_Dapper
{
    public interface ICarRepository
    {
        Car Create(Car car);
        void Delete(int id);
        Car Get(int id);
        List<Car> GetCars();
        void Update(Car car);
    }
}
