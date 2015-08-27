using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
namespace GA.Library
{
    public class ExponentialRankingSelector : ISelector
    {

        public double ExponentialFactor { get; set; }
        private IRandom rand;
        public ExponentialRankingSelector(IRandom rand)
        {
            this.rand = rand;
            this.ExponentialFactor = 0.95;
        }

        private Population candidates;
        public void SetCandidates(Population candidates)
        {
            this.candidates = candidates;
        }

        public List<IOrganism> PickCandidates(int count)
        {
            var totalMembers = this.candidates.Members.Count;
            double currentTotalExponentialRankValue = 0;
            var members = this.candidates.Members.OrderBy(c => c.Fitness).Select((m, i) =>
            {
                var exponentialRankValue = (this.ExponentialFactor - 1) / (Math.Pow(this.ExponentialFactor, totalMembers) - 1) * Math.Pow(this.ExponentialFactor, totalMembers - i - 1);
                currentTotalExponentialRankValue += exponentialRankValue;
                return new
                {
                    exponentialRankValue = currentTotalExponentialRankValue,
                    member = m
                };
            }).ToList();

            List<IOrganism> output = new List<IOrganism>();
            for (int i = 0; i <= count - 1; i++)
            {
                double threshold = rand.NextDouble();
                output.Add(members.OrderBy(m => m.exponentialRankValue).Where(m => m.exponentialRankValue >= threshold).Select(m => m.member).First());
            }

            return output;
        }
    }
}
