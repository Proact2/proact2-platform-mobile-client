using System;
using Proact.Mobile.Core.Models;

namespace Proact.Mobile.Core {
    public class ProjectPropertiesModel {
        public bool MedicsCanSeeOtherAnalisys { get; set; }
        public int MessageCanNotBeDeletedAfterMinutes { get; set; }
        public int MessageCanBeAnalizedAfterMinutes { get; set; }
        public int MessageCanBeRepliedAfterMinutes { get; set; }
        public bool IsAnalystConsoleActive { get; set; }
        public bool IsSurveysSystemActive { get; set; }
        public bool IsMessagingActive { get; set; }
        public LexiconModel Lexicon { get; set; }
    }
}
