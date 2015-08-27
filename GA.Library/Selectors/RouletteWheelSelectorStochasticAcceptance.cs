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
	public sealed class RouletteWheelSelectorStochasticAcceptance : ISelector
	{

		private Population candidates;
		private IRandom rand;
		public RouletteWheelSelectorStochasticAcceptance(IRandom rand)
		{
			this.rand = rand;
		}

		public void SetCandidates(Population candidates)
		{
			this.candidates = candidates;
		}

		public List<IOrganism> PickItems(int count)
		{
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

			while (true) {
				int index = rand.Next(this.candidates.Count);
				if (this.candidates[index].FitnessProportion > rand.NextDouble()) {
					yield return this.candidates[index];
				}
			}
		}
	}
}
