// ##############################################################################
//
// ice_utilities_math.cs | ICE.World.Utilities.MathTools
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

namespace ICE.World.Utilities
{
	public static class MathTools 
	{
		public static float FixedPercent( float _value )
		{
			if( _value < 0 ) _value = 0;
			if( _value > 100 ) _value = 100;

			return (float)System.Math.Round( _value, 2 );
		}

		/// <summary>
		/// Normalize the specified _value by using _min and _max.
		/// </summary>
		/// <param name="_value">Value.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		public static float Normalize( float _value, float _min,float _max) {
			return ( _value - _min) / (_max - _min);
		}

		/// <summary>
		/// Denormalize the specified _normalized by using _min and _max.
		/// </summary>
		/// <param name="_normalized">Normalized.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		public static float Denormalize( float _normalized, float _min, float _max) {
			return ( _normalized * ( _max - _min ) + _min );
		}

		/// <summary>
		/// Degrees to radian.
		/// </summary>
		/// <returns>The to radian.</returns>
		/// <param name="_angle">Angle.</param>
		public static float DegreeToRadian( float _angle ){
			return Mathf.PI * _angle / 180f;
		}

		/// <summary>
		/// Radians to degree.
		/// </summary>
		/// <returns>The to degree.</returns>
		/// <param name="_angle">Angle.</param>
		public static float RadianToDegree( float _angle ){
			return _angle * ( 180f / Mathf.PI );
		}

		/// <summary>
		/// Normalizes the angle.
		/// </summary>
		/// <returns>The angle.</returns>
		/// <param name="_angle">Angle.</param>
		public static float NormalizeAngle( float _angle ) 
		{
			while( _angle > 360 )
				_angle -= 360;
			while( _angle < 0 )
				_angle += 360;
			return _angle;
		}

		public static float NormalizeAngleInDegree( float _angle )
		{
			_angle = _angle % 360;
			if(_angle < 0) 
				_angle += 360;
			return _angle;
		}

		/// <summary>
		/// Clamps the angle.
		/// </summary>
		/// <returns>The angle.</returns>
		/// <param name="_angle">Angle.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		public static float ClampAngle( float _angle, float _min, float _max )
		{
			if( _angle < 90 || _angle > 270 ) 
			{       
				if( _angle > 180 ) _angle -= 360;  // convert all angles to -180..+180
				if( _max > 180 ) _max -= 360;
				if( _min > 180 ) _min -= 360;
			}    
			_angle = Mathf.Clamp( _angle, _min, _max );

			if( _angle < 0 ) _angle += 360;  // if angle negative, convert to 0..360
			return _angle;
		}

		public static float SignedVectorAngle( Vector3 _v1, Vector3 _v2  )
		{
			float _angle = Vector3.Angle( _v1, _v2 );
			Vector3 _cross = Vector3.Cross( _v1, _v2 );
			return ( _cross.y < 0 ? -_angle : _angle );
		}
	}
}
