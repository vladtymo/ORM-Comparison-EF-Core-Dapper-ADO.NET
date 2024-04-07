using System.ComponentModel.DataAnnotations;

namespace Test_Dapper
{
    // Model
    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        public int ModelYear { get; set; }
    }
}
