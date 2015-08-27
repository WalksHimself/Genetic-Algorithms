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
	public class Rand : IRandom
	{

		private Random rand;
		public Rand()
		{
			this.rand = new Random(Guid.NewGuid().GetHashCode());
		}

		public int Next(int maxValue)
		{
			return this.rand.Next(maxValue);
		}

		public int Next(int minValue, int maxValue)
		{
			return this.rand.Next(minValue, maxValue);
		}

		public double NextDouble()
		{
			return this.rand.NextDouble();
		}
	}
}
