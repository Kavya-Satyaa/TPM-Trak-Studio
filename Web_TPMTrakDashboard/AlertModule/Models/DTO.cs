using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.AlertModule.Models
{
    public class DTO
    {
    }
    public class DefineRuleDetails
    {
        public string RuleId { get; set; }
        public string Description { get; set; }
        public string Parameter { get; set; }
        public string Compare { get; set; }
        public string Threshold { get; set; }
        public string ThresholdUnit { get; set; }
        public string ThresholdValue { get; set; }
        public string EvaluateEvery { get; set; }
        public string EvaluateEveryUnit { get; set; }
        public string EvaluateEveryValue { get; set; }
        public bool Enable { get; set; }
        public string AppliesTo { get; set; }
        public bool SMSEnable { get; set; }
        public bool EmailEnable { get; set; }
        public string Message { get; set; }
        public bool TelegramEnabled { get; set; }
        public bool MobileEnabled { get; set; }
    }
}