using GA.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace GA.Testing
{
	[TestClass()]
	public class TravellingSalesmanTests
	{
		private List<CityInformation> cityInfo;
		private int geneLength;
		[TestMethod()]
		public void TravellingSalesmanTest()
		{
			int populationSize = 100;
			IRandom rand = new Rand();

			this.geneLength = 6;
			this.cityInfo = this.BuildCityInfo();
			Population candidates = RandomPopulationGenerator.GeneratePopulation(rand, populationSize, 1, geneLength, this.cityInfo.Count, this.cityInfo.Count, new List<Chromosome>(), new List<Chromosome>(), CorrectEncoding);

			GeneticAlgorithm ga = new GeneticAlgorithm(rand, candidates, CalculateFitness);
			ga.encodingCorrector = CorrectEncoding;
            

			IOrganism solution = ga.FindSolution();
			string path = WritePath(solution);
			double totalDistance = 1 / CalculateFitness(solution);
            Assert.IsTrue(totalDistance <= 25000);
		}

		private IOrganism CorrectEncoding(IOrganism o)
		{
			var genes = o.Chromosomes[0].GetGenes();
            var geneCount = genes.Count();
            for (int geneId = 0; geneId < geneCount; geneId++) {
				o.Chromosomes[0].Gene(geneId, new Chromosome(geneLength, Convert.ToString(Convert.ToInt32(o.Chromosomes[0].Gene(geneId).ToString(), 2) % (genes.Count() - geneId), 2).PadLeft(geneLength, '0')));
			}

			return o;
		}

		private double CalculateFitness(IOrganism o)
		{
			List<City> cities = this.cityInfo.Select(c => c.Name).ToList();
			var numbers = o.Chromosomes[0].GetGenes().Select((g, i) => Convert.ToInt32(g.ToString(), 2)).ToList();

			double totalDistance = 0;
			City lastCity = City.Unknown;
			numbers.ForEach(i =>
			{
				City currentCity = cities[i];
				cities.RemoveAt(i);

				if (lastCity == City.Unknown) {
					lastCity = currentCity;
				} else {
					totalDistance += this.GetDistance(lastCity, currentCity);
					lastCity = currentCity;
				}
			});

			return 1 / totalDistance;
		}

		private double GetDistance(City cityA, City cityB)
		{
			CityInformation infoA = this.cityInfo.Where(l => l.Name == cityA).Single();
			CityInformation infoB = this.cityInfo.Where(l => l.Name == cityB).Single();

			var earthsRadius = 6373;
			var deltaLatitude = infoA.Latitude - infoB.Latitude;
			var deltaLongitude = infoA.Longitude - infoB.Longitude;
			double a = Math.Pow(Math.Sin(deltaLatitude / 2), 2) + Math.Cos(infoA.Latitude) * Math.Cos(infoB.Latitude) * Math.Pow(Math.Sin(deltaLongitude / 2), 2);
			var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			return earthsRadius * c;
		}

		private string WritePath(IOrganism o)
		{
			List<City> cities = this.cityInfo.Select(c => c.Name).ToList();
			var numbers = o.Chromosomes[0].GetGenes().Select((g, i) => Convert.ToInt32(g.ToString(), 2) % (cities.Count - i)).ToList();
			City[] output = new City[cities.Count + 1];
			int counter = 0;
			numbers.ForEach(i =>
			{
				output[counter] = cities[i];
				cities.RemoveAt(i);
				counter += 1;
			});

			return string.Join(",", output);
		}

		private List<CityInformation> BuildCityInfo()
		{
			List<CityInformation> info = new List<CityInformation>();
			info.Add(new CityInformation(City.AnchorageAlaska, 61.217, 149.9));
			info.Add(new CityInformation(City.AustinTexas, 30.267, 97.733));
			info.Add(new CityInformation(City.BostonMassachusetts, 42.35, 71.083));
			info.Add(new CityInformation(City.ChicagoIllinois, 41.833, 87.617));
			info.Add(new CityInformation(City.DallasTexas, 32.767, 96.767));
			info.Add(new CityInformation(City.DenverColorado, 39.75, 105));
			info.Add(new CityInformation(City.DetroitMichigan, 42.333, 83.05));
			info.Add(new CityInformation(City.FargoNorthDakota, 46.867, 96.8));
			info.Add(new CityInformation(City.HonoluluHawaii, 21.3, 157.833));
			info.Add(new CityInformation(City.HoustonTexas, 29.75, 95.35));
			info.Add(new CityInformation(City.JuneauAlaska, 58.3, 134.4));
			info.Add(new CityInformation(City.LasVegasNevada, 36.167, 115.2));
			info.Add(new CityInformation(City.LosAngelesCalifornia, 34.05, 118.25));
			info.Add(new CityInformation(City.NewarkNewJersey, 40.733, 74.167));
			info.Add(new CityInformation(City.PhiladelphiaPennsylvania, 39.95, 75.167));
			info.Add(new CityInformation(City.SanDiegoCalifornia, 32.7, 117.167));
			info.Add(new CityInformation(City.SanFranciscoCalifornia, 37.783, 122.433));
			info.Add(new CityInformation(City.SanJuanPuertoRico, 18.5, 66.167));
			info.Add(new CityInformation(City.WashingtonDC, 38.883, 77.033));

			return info;
		}

		private class CityInformation
		{
			public City Name;
			public double Longitude;

			public double Latitude;
			public CityInformation(City name, double longitude, double latitude)
			{
				this.Name = name;
				this.Longitude = this.ConvertDegreesToRadians(longitude);
				this.Latitude = this.ConvertDegreesToRadians(latitude);
			}

			public override string ToString()
			{
				return string.Format("{0} ({1}, {2})", this.Name.ToString(), this.Longitude, this.Latitude);
			}

			private double ConvertDegreesToRadians(double degrees)
			{
				return degrees * Math.PI / 180;
			}
		}

		private enum City
		{
			Unknown,
			AnchorageAlaska,
			AustinTexas,
			BostonMassachusetts,
			ChicagoIllinois,
			DallasTexas,
			DenverColorado,
			DetroitMichigan,
			FargoNorthDakota,
			HonoluluHawaii,
			HoustonTexas,
			JuneauAlaska,
			LasVegasNevada,
			LosAngelesCalifornia,
			NewarkNewJersey,
			PhiladelphiaPennsylvania,
			SanDiegoCalifornia,
			SanFranciscoCalifornia,
			SanJuanPuertoRico,
			WashingtonDC
		}
	}
}
