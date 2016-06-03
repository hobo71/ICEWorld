// ##############################################################################
//
// ice_utilities_date.cs | ICE.World.Utilities.DateTools
// Version 1.2.10
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is furnished 
// to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// ##############################################################################

using UnityEngine;
using System;
using System.Globalization;

namespace ICE.World.Utilities
{
	public static class DateTools
	{
		/// <summary>
		/// Localizes the date time.
		/// </summary>
		/// <returns>The date time.</returns>
		/// <param name="_key">Key.</param>
		/// <param name="_datetime">Datetime.</param>
		public static string LocalizeDateTime( string _key, DateTime _datetime )
		{
			//       en-US: 6/19/2015 10:03:06 AM
			//       en-GB: 19/06/2015 10:03:06
			//       fr-FR: 19/06/2015 10:03:06
			//       de-DE: 19.06.2015 10:03:06

			return _datetime.ToString( new CultureInfo( _key ) );
		}
	}
}
