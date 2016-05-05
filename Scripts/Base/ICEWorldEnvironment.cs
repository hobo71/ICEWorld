// ##############################################################################
//
// ICE.World.ICEWorldEnvironment.cs
// Version 1.1.21
//
// The MIT License (MIT)
//
// Copyright © Pit Vetterick, ICE Technologies Consulting LTD. http://www.icecreaturecontrol.com (mailto:support@icecreaturecontrol.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files 
// (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, 
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do 
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// ##############################################################################

using UnityEngine;
using System.Collections;

namespace ICE.World
{
	/// <summary>
	/// Temperature scale type.
	/// </summary>
	public enum TemperatureScaleType
	{
		CELSIUS,
		FAHRENHEIT
	}

	/// <summary>
	/// Weather type.
	/// </summary>
	public enum WeatherType
	{
		UNDEFINED = 0,
		FOGGY,
		RAIN,
		HEAVY_RAIN,
		WINDY,
		STORMY,
		CLEAR,
		PARTLY_CLOUDY,
		MOSTLY_CLOUDY,
		CLOUDY
	}

	/// <summary>
	/// ICE world environment.
	/// </summary>
	/// <description>
	/// ICEWorldEnvironment contains several environment parameter, such as Date and Time, Temperature and Weather Conditions etc. 
	/// You can use ICEWorldEnvironment as base class for your own Day&Night Cycle and/or Weather System, so it will be automatically 
	/// compatible with the rest of the ICE world. 
	/// <description>
	public class ICEWorldEnvironment : ICEWorld {

		protected static ICEWorldEnvironment m_Instance = null;
		public static ICEWorldEnvironment Instance
		{
			get
			{
				if( m_Instance == null )
					m_Instance = GameObject.FindObjectOfType<ICEWorldEnvironment>();

				return m_Instance;
			}
		}

		public TemperatureScaleType TemperatureScale;
		public float Temperature;
		public float MinTemperature;
		public float MaxTemperature;

		public int DateDay;
		public int DateMonth;
		public int DateYear;

		public int TimeHour;
		public int TimeMinutes;
		public int TimeSeconds;

		public WeatherType Weather;

		public float WindSpeed;
		public float WindDirection;

		public void UpdateTemperatureScale( TemperatureScaleType _scale )
		{
			if( _scale == TemperatureScaleType.CELSIUS && TemperatureScale == TemperatureScaleType.FAHRENHEIT )
			{
				TemperatureScale = _scale;
				Temperature = ICE.World.Utilities.Converter.FahrenheitToCelsius( Temperature );
				MinTemperature = ICE.World.Utilities.Converter.FahrenheitToCelsius( MinTemperature );
				MaxTemperature = ICE.World.Utilities.Converter.FahrenheitToCelsius( MaxTemperature );

			}
			else if( _scale == TemperatureScaleType.FAHRENHEIT && TemperatureScale == TemperatureScaleType.CELSIUS )
			{
				TemperatureScale = _scale;
				Temperature = ICE.World.Utilities.Converter.CelsiusToFahrenheit( Temperature );
				MinTemperature = ICE.World.Utilities.Converter.CelsiusToFahrenheit( MinTemperature );
				MaxTemperature = ICE.World.Utilities.Converter.CelsiusToFahrenheit( MaxTemperature );
			}
		}
	}
}
