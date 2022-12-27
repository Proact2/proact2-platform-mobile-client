using System;
using Proact.Mobile.Core.Models;

namespace Proact.Mobile.Core {
    public class SurveyMoodAnswerModel : ISurveyAnswersModel {
        public MessageMood MoodAnswer { get; set; } = MessageMood.None;
    }
}
