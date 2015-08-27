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
	public interface IRandom
	{
		double NextDouble();
		int Next(int maxValue);
		int Next(int minValue, int maxValue);
	}
}
