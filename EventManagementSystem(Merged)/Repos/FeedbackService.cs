using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem_Merged_.Repos
{
    public class FeedbackService
    {
        //DB dalObject = new DB();
        private readonly AppDbContext _adc;
        public FeedbackService(AppDbContext ad)
        {
            _adc = ad;
        }
        #region AddFeedback ADO.NET
        public void AddFeedback(Feedback feed)
        {

            Ticket ticket = _adc.Tickets.FirstOrDefault(t => t.TicketID == feed.TicketID);

            if (ticket != null)
            {

                Event e = _adc.Events.FirstOrDefault(ev => ev.EventID == ticket.EventID);

                if (e != null && e.IsActive)
                {

                    _adc.Feedbacks.Add(feed);
                    _adc.SaveChanges();
                }
            }
        }


        #endregion

        #region ViewFeedbacks
        public List<Feedback> ViewFeedbacks(int eventid)
        {
            var data = _adc.Feedbacks.Where(e => e.EventID == eventid).OrderByDescending(r => r.SubmittedTimestamp).ToList();
            return data;
        }
        #endregion 
        public double GetAverageRating(int eventId)
        {
            var feedbacksForEvent = _adc.Feedbacks
                                        .Where(f => f.EventID == eventId);

            if (feedbacksForEvent.Any())
            {
                return feedbacksForEvent.Average(f => f.Rating);
            }
            return 0;
        }
        public Dictionary<int, int> GetRatingCounts(int eventId)
        {
            var ratingCounts = new Dictionary<int, int>();

            for (int i = 1; i <= 5; i++)
            {
                ratingCounts[i] = _adc.Feedbacks.Count(f => f.EventID == eventId && f.Rating == i);
            }

            return ratingCounts;
        }
        public bool DeleteFeedback(int id)
        {
            var feedback = _adc.Feedbacks.FirstOrDefault(f => f.FeedbackID == id);
            if (feedback == null)
            {
                return false;
            }

            _adc.Feedbacks.Remove(feedback);
            _adc.SaveChanges();
            return true;
        }
    }
}