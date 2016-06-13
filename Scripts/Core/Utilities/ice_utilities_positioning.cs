// ##############################################################################
//
// ice_utilities_positioning.cs | ICE.World.Utilities.PositionTools
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
using System.Collections;
using System.Collections.Generic;

using ICE;
using ICE.World;
using ICE.World.EnumTypes;

namespace ICE.World.Utilities
{
	public static class PositionTools 
	{
		/// <summary>
		/// Gets a clamped position by using the specified start and end vector.
		/// </summary>
		/// <returns>The clamped position.</returns>
		/// <param name="_start">Start.</param>
		/// <param name="_end">End.</param>
		/// <param name="_level">Level.</param>
		/// <param name="_precision">Step.</param>
		public static Vector3 GetClampedPosition( Vector3 _start, Vector3 _end, float _level, float _step_angle = 45 )
		{
			float _distance = Vector3.Distance( _start, _end );
			Quaternion _rot = Quaternion.FromToRotation( Vector3.right, _end - _start );
			Vector3 _euler = _rot.eulerAngles;
			float _angle = _step_angle * (int)Mathf.Round( _euler.y / _step_angle );
			float _end_x = _start.x + _distance * Mathf.Cos(_angle * (Mathf.PI / 180f));
			float _end_z = _start.z + _distance * Mathf.Sin(-_angle * (Mathf.PI / 180f));
			return new Vector3( _end_x, _level, _end_z );
		}

		/// <summary>
		/// Gets the random position within the defined circle.
		/// </summary>
		/// <returns>The random circle position.</returns>
		/// <param name="_center">Center.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		public static Vector3 GetRandomCirclePosition( Vector3 _center, float _min, float _max ) 
		{ 
			float _distance = Random.Range( _min, _max );
			float _angle = Random.Range( 0, 360 );

			return GetOffsetPositionByAngleAndRadius( _center, _angle, _distance );
		}

		/// <summary>
		/// Gets the random position within the defined rect.
		/// </summary>
		/// <returns>The random rect position.</returns>
		/// <param name="origin">Origin.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="z">The z coordinate.</param>
		/// <param name="centered">If set to <c>true</c> centered.</param>
		public static Vector3 GetRandomRectPosition( Vector3 origin, float x = 10.0f, float z = 10.0f, bool centered = false ) 
		{ 
			Vector3 new_position = origin;
			
			if( centered )
			{
				new_position.x += UnityEngine.Random.Range( -(x/2), (x/2) );
				new_position.z += UnityEngine.Random.Range( -(z/2), (z/2) );
			}
			else
			{
				new_position.x += UnityEngine.Random.Range( 0, x );
				new_position.z += UnityEngine.Random.Range( 0, z );
			}
			
			//new_position.y = GetGroundLevel( new_position );
			
			return new_position;
		}

		/// <summary>
		/// Gets the random position.
		/// </summary>
		/// <returns>The random position.</returns>
		/// <param name="_position">Position.</param>
		/// <param name="_radius">Radius.</param>
		public static Vector3 GetRandomPosition( Vector3 _position, float _radius ) { 
			
			if( _radius == 0 )
				return _position;
			
			Vector2 _new_circle_point = UnityEngine.Random.insideUnitCircle * _radius;
			
			Vector3 _new_position = Vector3.zero;
			_new_position.x = _position.x + _new_circle_point.x;
			_new_position.z = _position.z + _new_circle_point.y;
			_new_position.y = GetGroundLevel( _position );

			return _new_position;
			
		}

		public static Vector3 GetDirectionPosition( Transform _transform, float _angle, float _distance )
		{
			if( _transform == null )
				return Vector3.zero;
			
			_angle += _transform.eulerAngles.y;
			
			if( _angle > 360 )
				_angle = _angle - 360;
			
			Vector3 _world_offset = GetOffsetPositionByAngleAndRadius( _transform.position, _angle, _distance );
			
			return _world_offset;
		}

		public static float GetGroundLevel( Vector3 _position, GroundCheckType _type, LayerMask _layerMask, float _min_offset = 0.5f, float _max_offset = 1000, float _base_offset = 0 )
		{
			if( _type == GroundCheckType.NONE )
				return _position.y;
			
			if( _type == GroundCheckType.RAYCAST )
			{
				RaycastHit hit;
				if( Physics.Raycast( new Vector3( _position.x, _position.y + _min_offset + ( _base_offset * -1 ), _position.z ), Vector3.down, out hit, Mathf.Infinity, _layerMask ) )
					_position.y = hit.point.y;
				else if( Physics.Raycast( new Vector3( _position.x, _position.y + _max_offset + ( _base_offset * -1 ) , _position.z ), Vector3.down, out hit, Mathf.Infinity, _layerMask ) )
					_position.y = hit.point.y;
				else if( Terrain.activeTerrain != null )
					_position.y = Terrain.activeTerrain.SampleHeight( _position );
				
			}
			else if( Terrain.activeTerrain != null )
				_position.y = Terrain.activeTerrain.SampleHeight( _position );
			
			return _position.y;
		}

		public static float GetGroundLevel( Vector3 _position, float _min_offset = 0.5f, float _max_offset = 1000, float _base_offset = 0 )
		{
			RaycastHit hit;
			if( Physics.Raycast( new Vector3( _position.x, _position.y + _min_offset + ( _base_offset * -1 ), _position.z ), Vector3.down, out hit ) )
				_position.y = hit.point.y;
			else if( Physics.Raycast( new Vector3( _position.x, _position.y + _max_offset + ( _base_offset * -1 ) , _position.z ), Vector3.down, out hit ) )
				_position.y = hit.point.y;
			else if( Terrain.activeTerrain != null )
				_position.y = Terrain.activeTerrain.SampleHeight( _position );

			return _position.y;
		}







		public static float CourseDeviation( Transform _transform, Vector3 _position )
		{
			Vector3 _heading =  _position - _transform.position;
			Vector3 _forward = _transform.TransformDirection(Vector3.forward) - _heading.normalized;

			return AngleDirection( _transform.forward, _transform.up, _forward );
		}


		public static float AngleDirection( Vector3 _forward, Vector3 _up, Vector3 _heading )
		{
			Vector3 _perpendicular = Vector3.Cross( _forward, _heading );
			float _direction = Vector3.Dot( _perpendicular , _up );

			return _direction;
		}   

		//returns negative value when left, positive when right, and 0 for forward/backward
		public static float AngleDirectionExt( Vector3 _forward, Vector3 _up, Vector3 _heading )
		{
			Vector3 _perpendicular = Vector3.Cross( _forward, _heading );
			float _direction = Vector3.Dot( _perpendicular , _up  );

			if( _direction > 1f )
				_direction = 1;
			else if( _direction < -1f )
				_direction = -1;
			else
				_direction = 0;
			
			return _direction;
		}  

		public static Vector3 GetOffsetPositionByAngleAndRadius( Vector3 _origin, float _angle, float _radius )
		{ 
			float _rad = _angle * Mathf.Deg2Rad;			
			return _origin + new Vector3( Mathf.Sin( _rad ) * _radius, 0, Mathf.Cos( _rad ) * _radius );
		}



		/// <summary>
		/// Gets the relative position of the specified world position.
		/// </summary>
		/// <returns>The relative position.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_position">Position.</param>
		public static Vector3 GetRelativePosition( Transform _transform, Vector3 _position )
		{ 
			if( _transform == null )
				return Vector3.zero;
			
			Vector3 _offset = _transform.InverseTransformPoint( _position );

			_offset.x = _offset.x*_transform.lossyScale.x;
			_offset.y = _offset.y*_transform.lossyScale.y;
			_offset.z = _offset.z*_transform.lossyScale.z;	

			return _offset;
		}

		/// <summary>
		/// Gets the world position of the specified local offset position.
		/// </summary>
		/// <returns>The world position.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_offset">Offset.</param>
		public static Vector3 GetWorldPosition( Transform _transform, Vector3 _offset )
		{
			if( _transform == null )
				return Vector3.zero;

			_offset.x = _offset.x/_transform.lossyScale.x;
			_offset.y = _offset.y/_transform.lossyScale.y;
			_offset.z = _offset.z/_transform.lossyScale.z;

			return _transform.TransformPoint( _offset );
		}

		/// <summary>
		/// Gets the angle in degree.
		/// </summary>
		/// <returns>The angle in degree.</returns>
		/// <param name="_vector">Vector.</param>
		public static float GetAngleInDegree( Vector3 _vector ){ 
			return Mathf.Atan2( _vector.x, _vector.z ) * Mathf.Rad2Deg;
		}

		/// <summary>
		/// Gets the normalized angle in degree.
		/// </summary>
		/// <returns>The normalized angle.</returns>
		/// <param name="_vector">Vector.</param>
		public static float GetNormalizedAngle( Vector3 _vector ){ 
			return MathTools.NormalizeAngle( GetAngleInDegree( _vector ) );
		}

		/// <summary>
		/// Gets the direction angle.
		/// </summary>
		/// <returns>The direction angle.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_position">Position.</param>
		public static float GetDirectionAngle( Transform _transform, Vector3 _position )
		{
			if( _transform != null )
				return 0;

			float _angle = MathTools.NormalizeAngle( PositionTools.GetNormalizedAngle( _position - _transform.position ) - _transform.eulerAngles.y );

			if( _angle > 180 )
				_angle -= 360; 

			return _angle;
		}

		/// <summary>
		/// Gets the position angle.
		/// </summary>
		/// <returns>The position angle.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_position">Position.</param>
		public static float GetPositionAngle( Transform _transform, Vector3 _position )
		{ 
			return GetNormalizedAngle( GetRelativePosition( _transform, _position ) );
		}

		/// <summary>
		/// Gets the horizontal distance by ignoring the level difference
		/// </summary>
		/// <returns>The horizontal distance.</returns>
		/// <param name="pos1">Pos1.</param>
		/// <param name="pos2">Pos2.</param>
		public static float GetHorizontalDistance( Vector3 _pos1, Vector3 _pos2 )
		{
			_pos1.y = 0;
			_pos2.y = 0;
			
			return Vector3.Distance( _pos1, _pos2 );
		}
	}

}
