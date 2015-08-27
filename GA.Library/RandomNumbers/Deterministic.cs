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
	public class Deterministic : IRandom
	{

		private Stack<double> numbers;
		public Deterministic(params double[] numbers)
		{
			this.numbers = new Stack<double>(numbers.Reverse());
		}

		public int Next(int maxValue)
		{
			return Convert.ToInt32(this.numbers.Pop());
		}

		public int Next(int minValue, int maxValue)
		{
			return Convert.ToInt32(this.numbers.Pop());
		}

		public double NextDouble()
		{
			return this.numbers.Pop();
		}
	}
}
