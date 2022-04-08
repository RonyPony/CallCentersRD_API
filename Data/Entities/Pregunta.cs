using System.ComponentModel.DataAnnotations;

namespace CallCentersRD_API.Data.Entities
{
    public class Pregunta
    {
        [Key]
        public int Id { get; set; }
        public string pregunta { get; set; }
        public DateTime creationDate { get; set; }
        public bool enable { get; set; } = true;    
    }
}
