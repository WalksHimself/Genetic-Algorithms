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
    public class LinearRankingSelector : ISelector
    {

        public double ReproductionRateOfWorstIndividual { get; set; }
        private IRandom rand;
        public LinearRankingSelector(IRandom rand)
        {
            this.rand = rand;
            this.ReproductionRateOfWorstIndividual = 0.0001;
        }

        private Population candidates;
        public void SetCandidates(Population candidates)
        {
            this.candidates = candidates;
        }

        public List<IOrganism> PickCandidates(int count)
        {
            var totalMembers = this.candidates.Members.Count;
            double currentTotalLinearRankValue = 0;

            var members = this.candidates.Members.OrderBy(c => c.Fitness).Select((m, i) =>
                {
                    var linearRankValue = (1.0 / totalMembers) * (this.ReproductionRateOfWorstIndividual + (2 - 2 * this.ReproductionRateOfWorstIndividual) * i / (totalMembers - 1.0));
                    currentTotalLinearRankValue += linearRankValue;
                    return new
                    {
                        linearRankValue = currentTotalLinearRankValue,
                        member = m
                    };
                }).ToList();

            List<IOrganism> output = new List<IOrganism>();
            for (int i = 0; i <= count - 1; i++)
            {
                double threshold = rand.NextDouble() * currentTotalLinearRankValue;
                output.Add(members.OrderBy(m => m.linearRankValue).Where(m => m.linearRankValue >= threshold).Select(m => m.member).First());
            }

            return output;
        }
    }
}
