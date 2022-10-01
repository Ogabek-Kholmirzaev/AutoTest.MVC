using AutoTestMVC.Models;
using Newtonsoft.Json;

namespace AutoTestMVC.Services
{
    public class TicketsRepository
    {
        private const string Folder = "UserData";
        private const string FileName = "tikestslist.json";
        public static List<Ticket> TicketsList = new List<Ticket>();

        public TicketsRepository()
        {
            //ReadJsonData();
        }

        public void WriteToJson()
        {
            List<Ticket> ticketDataList =
                TicketsList.Select(t => new Ticket(t.Index, t.CorrectAnswersCount, t.QuestionsCount)).ToList();

            var jsonData = JsonConvert.SerializeObject(ticketDataList);

            if (!Directory.Exists(Folder))
            {
                Directory.CreateDirectory(Folder);
            }

            File.WriteAllText(Path.Combine(Folder, FileName), jsonData);
        }

        public void ReadJsonData()
        {
            if (File.Exists(Path.Combine(Folder, FileName)))
            {
                var jsonData = File.ReadAllText(Path.Combine(Folder, FileName));
                
                if (jsonData == "") TicketsList = new List<Ticket>();
                else TicketsList = JsonConvert.DeserializeObject<List<Ticket>>(jsonData);
            }
        }
    }
}
