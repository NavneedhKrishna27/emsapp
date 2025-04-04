using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem_Merged_.Repos
{
    public class FeedbackService
    {
        private readonly AppDbContext _adc;
        public FeedbackService(AppDbContext ad)
        {
            _adc = ad;
        }
        #region AddFeedback
        public void addFeedback(Feedback Feed)
        {
            _adc.Feedbacks.Add(Feed);
            _adc.SaveChanges();
        }
        #endregion
        #region ViewFeedbacks
        public List<Feedback> ViewFeedbacks()
        {
            var data = _adc.Feedbacks.ToList();
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
    }
}
