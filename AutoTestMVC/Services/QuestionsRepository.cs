using AutoTestMVC.Models;
using Newtonsoft.Json;

namespace AutoTestMVC.Services
{
    public class QuestionsRepository
    {
        public int TicketQuestionsCount = 5;
        public List<QuestionEntity> Questions { get; set; }

        public QuestionsRepository()
        {
            LoadQuestionsFromJsonFile();
        }

        private void LoadQuestionsFromJsonFile()
        {
            try
            {
                var jsonStringData = File.ReadAllText("JsonData/uzlotin.json");
                Questions = JsonConvert.DeserializeObject<List<QuestionEntity>>(jsonStringData);
            }
            catch (Exception exception)
            {
                Questions = new List<QuestionEntity>();
            }
        }

        public int TicketsCount()
        {
            return Questions.Count / TicketQuestionsCount;
        }

        public List<QuestionEntity> GetQuestionsRange(int ticketIndex)
        {
            return Questions.Skip(ticketIndex * TicketQuestionsCount).Take(TicketQuestionsCount).ToList();
        }
    }
}