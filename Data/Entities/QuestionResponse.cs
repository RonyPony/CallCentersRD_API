using System.ComponentModel.DataAnnotations;

namespace CallCentersRD_API.Data.Entities
{
    public class QuestionResponse
    {
        [Key]
        public int Id { get; set; }
        public string questionContent { get; set; }
        public int questionId { get; set; }
        public int userId { get; set; }
        public string responseContent { get; set; }
        public string responserName { get; set; }
        public DateTime answerDate { get; set; }
    }
}
