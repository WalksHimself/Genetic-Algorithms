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
	public sealed class StochasticUniversalSampling : ISelector
	{

		private Population candidates;
		private IRandom rand;
		public StochasticUniversalSampling(IRandom rand)
		{
			this.rand = rand;
		}

		public void SetCandidates(Population candidates)
		{
			this.candidates = candidates;
		}

		private int count;
		public List<IOrganism> PickItems(int count)
		{
			this.count = count;
			return this.PickItems().Take(count).ToList();
		}
		List<IOrganism> ISelector.PickCandidates(int count)
		{
			return PickItems(count);
		}

		private IEnumerable<IOrganism> PickItems()
		{
			if (this.candidates.Count == 0) {
				throw new ArgumentException("candidates");
			}

			double threshold = rand.NextDouble();
			double stepSize = 1 / count;

			for (int i = 0; i <= count - 1; i++) {
				threshold = threshold + stepSize;
				if (threshold > 1) {
					threshold = threshold - 1;
				}

				yield return this.FirstBreachOfThreshold(threshold);
			}
		}

		private IOrganism FirstBreachOfThreshold(double threshold)
		{
			double totalFitnessSoFar = 0;
			for (int i = 0; i <= this.candidates.Count - 1; i++) {
				totalFitnessSoFar += this.candidates[i].FitnessProportion;
				if (totalFitnessSoFar >= threshold) {
					return this.candidates[i];
				}
			}

			throw new InvalidOperationException();
		}
	}
}
