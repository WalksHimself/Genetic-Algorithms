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
	public class RouletteWheelSelector : ISelector
	{

		private Population candidates;
		private IRandom rand;
		public RouletteWheelSelector(IRandom rand)
		{
			this.rand = rand;
		}

		public void SetCandidates(Population candidates)
		{
			this.candidates = candidates;
		}

		public List<IOrganism> PickCandidates(int count)
		{
			return this.PickItems().Take(count).ToList();
		}

		private IEnumerable<IOrganism> PickItems()
		{
			if (this.candidates.Count == 0) {
				throw new ArgumentException("candidates");
			}

			double threshold = rand.NextDouble();
			double totalFitnessSoFar = 0;

			for (int i = 0; i <= this.candidates.Count - 1; i++) {
				totalFitnessSoFar += this.candidates[i].FitnessProportion;
				if (totalFitnessSoFar >= threshold) {
					totalFitnessSoFar = 0;
					threshold = rand.NextDouble();
					yield return this.candidates[i];
				}

				if (i == this.candidates.Count - 1) {
					i = 0;
				}
			}
		}
	}
}
