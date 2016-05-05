// ##############################################################################
//
// ICE.World.Utilities.Converter.cs
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

namespace ICE.World.Utilities
{
	/// <summary>
	/// Converter contains several converter tools 
	/// </summary>
	public static class Converter 
	{
		/// <summary>
		/// Fahrenheits to celsius.
		/// </summary>
		/// <returns>The to celsius.</returns>
		/// <param name="_fahrenheit">Fahrenheit.</param>
		public static float FahrenheitToCelsius( float _fahrenheit ){
			return (5f / 9f) * (_fahrenheit - 32f);
		}

		/// <summary>
		/// Celsiuses to fahrenheit.
		/// </summary>
		/// <returns>The to fahrenheit.</returns>
		/// <param name="_celsius">Celsius.</param>
		public static float CelsiusToFahrenheit( float _celsius ){
			return _celsius * (9f / 5f) + 32f;
		}
	}
}