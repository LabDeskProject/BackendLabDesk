using LabDesk.SeedWork.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.SeedWork.Domain
{
    public class BusinessRuleValidationException : Exception
    {
        public IBusinessRule BrokenRule;
        public string Details;
        public BusinessRuleValidationException(IBusinessRule brokenRule) 
           : base(brokenRule.Message)
        {
            BrokenRule = brokenRule;
            this.Details = brokenRule.Message;
        }

        public override string ToString()
        {
            return $"{BrokenRule.GetType().FullName} : {BrokenRule.Message}";
        }

    }
}
