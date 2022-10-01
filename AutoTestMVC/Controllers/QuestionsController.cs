using AutoTestMVC.Models;
using AutoTestMVC.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace AutoTestMVC.Controllers
{
    public class QuestionsController : Controller
    {
        // private TicketsRepository _ticketsRepo = new TicketsRepository();
        private Ticket _ticket = new Ticket();
        private QuestionsRepository _questionsRepo = new QuestionsRepository();
        public IActionResult Index(int? ticketIndex = null, int questionIndex = 0, int? choiceIndex = null, bool isGettingNewTicket = false)
        {
            Ticket ticket;

            if (isGettingNewTicket)
            {
                if (ticketIndex is null)
                {
                    ticketIndex = new Random().Next(_questionsRepo.TicketsCount());
                    ticket = new Ticket(ticketIndex.Value, _questionsRepo.GetQuestionsRange(ticketIndex.Value));
                }
                else
                {
                    ticket = new Ticket(ticketIndex.Value, _questionsRepo.GetQuestionsRange(ticketIndex.Value));
                }

                if(TicketsRepository.TicketsList.Any(t=>t.Index == ticketIndex))
                {
                    TicketsRepository.TicketsList.Remove(
                        TicketsRepository.TicketsList.First(t => t.Index == ticketIndex));
                }

                TicketsRepository.TicketsList.Add(ticket);
            }
            else
            {
                ticket = TicketsRepository.TicketsList.First(t => t.Index == ticketIndex.Value);
            }

            var question = ticket.Questions[questionIndex];

            if (choiceIndex != null)
            {
                ViewBag.choiceIndex = choiceIndex.Value;
                ViewBag.choiceResult = question!.Choices[choiceIndex.Value].Answer;

                if(!ticket.SolvedQuestionsDictionary.ContainsKey(questionIndex))
                    ticket.SolvedQuestionsDictionary.Add(questionIndex, choiceIndex.Value);
            }

            ViewBag.ticket = ticket;
            ViewBag.questionIndex = questionIndex;

            if (ticket.QuestionsCount == ticket.SolvedQuestionsDictionary.Count)
                return RedirectToAction("ShowResult", new {questionsCount = ticket.QuestionsCount, correctAnswersCount = ticket.SolvedQuestionsDictionary.Count(q => ticket.Questions[q.Key].Choices[q.Value].Answer == true) });

            return View(question);
        }

        public IActionResult ShowTickets()
        {
            ViewBag.tickets = TicketsRepository.TicketsList;
            ViewBag.ticketsCount = _questionsRepo.TicketsCount();

            return View();
        }

        public IActionResult ShowResult(int questionsCount, int correctAnswersCount)
        {
            ViewBag.questionsCount = questionsCount;
            ViewBag.correctAnswersCount = correctAnswersCount;
            return View();
        }
    }
}
