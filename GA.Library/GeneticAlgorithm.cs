using System;
using System.Collections.Generic;
using System.Linq;
namespace GA.Library
{
    public class GeneticAlgorithm
	{
		private Population candidates;
		private Func<IOrganism, double> fitnessFunction;
		public Func<IOrganism, IOrganism> encodingCorrector;
		private ISelector selector;
		private IMutator mutator;
		private ICrossover crossLinker;
		private double elitismProportion;
		private Dictionary<int, double> generationInformation;
		private int initialCount;

		private int maximumGenerations;
		private int _generationCount;
		private IRandom rand;
		public int GenerationCount {
			get { return this._generationCount; }
		}

		public GeneticAlgorithm(IRandom rand, Population candidates, Func<IOrganism, double> fitnessFunction) : this(rand, candidates, fitnessFunction, new LinearRankingSelector(rand), new Mutator(rand, 0.05), new OnePointCrossover(rand, 0.7), 0.1, 15000)
		{
			this.mutator.AddMutator(new BitFlipMutation(rand));
		}

		public GeneticAlgorithm(IRandom rand, Population candidates, Func<IOrganism, double> fitnessFunction, ISelector selector, IMutator mutator, ICrossover crossLinker, double elitismProportion, int maximumGenerations)
		{
			this.rand = rand;
			this.candidates = candidates;
			this.fitnessFunction = fitnessFunction;
			this.selector = selector;
			this.mutator = mutator;
			this.crossLinker = crossLinker;
			this.elitismProportion = elitismProportion;
			this._generationCount = 0;
			this.generationInformation = new Dictionary<int, double>();
			this.initialCount = candidates.Count;
			this.maximumGenerations = maximumGenerations;

			this.candidates.CalculateFitnesses(this.fitnessFunction);
		}

		public IOrganism FindSolution()
		{
			while (this.candidates.PopulationIsDiverse() & (maximumGenerations == 0 | (maximumGenerations > 0 & GenerationCount <= maximumGenerations))) {
				this.CreateNextGeneration();
			}

			return this.candidates.GetSolution();
		}

		private void CreateNextGeneration()
		{
			Population nextGeneration = new Population();
			this.selector.SetCandidates(this.candidates);

			for (int i = 0; i <= Convert.ToInt32((this.candidates.Count / 2) * (1 - this.elitismProportion)) - 1; i++) {
				this.CreateOffspring(nextGeneration);
			}

			// Elitism involves bringing the top X% of the current into the next generation
			if (this.elitismProportion > 0) {
				int numberToKeep = Convert.ToInt32(this.elitismProportion * this.initialCount);
				this.candidates.GetBest(this.elitismProportion).ToList().ForEach(c => nextGeneration.Add(c));
			}

			this.candidates = nextGeneration;
			this.candidates.CalculateFitnesses(this.fitnessFunction);
			this.generationInformation.Add(this.GenerationCount, this.candidates.TotalFitness / this.candidates.Count);
			this._generationCount += 1;
		}

		private void CreateOffspring(Population nextGeneration)
		{
			var parents = this.selector.PickCandidates(2).Take(2).ToList();
			var offspring = this.crossLinker.CrossLink(parents[0], parents[1]);
			this.mutator.Mutate(offspring.Item1);
			this.mutator.Mutate(offspring.Item2);

			if (this.encodingCorrector != null) {
				offspring = new Tuple<IOrganism, IOrganism>(this.encodingCorrector(offspring.Item1), this.encodingCorrector(offspring.Item2));
			}

			nextGeneration.Add(offspring.Item1);
			nextGeneration.Add(offspring.Item2);
		}
	}
}
