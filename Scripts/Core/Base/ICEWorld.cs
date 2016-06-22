// ##############################################################################
//
// ICEWorldRegister.cs | ICEWorldRegister
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
using ICE.World.Utilities;
using ICE.World.EnumTypes;

namespace ICE.World
{
	public class ICEWorldRegister : ICEWorldSingleton {

		public Vector3 GridSize = Vector3.one;

		public RandomSeedType RandomSeed = RandomSeedType.DEFAULT;
		public int CustomRandomSeed = 23;

		[SerializeField]
		private List<string> m_GroundLayers = new List<string>();
		public List<string> GroundLayers{
			get{ return m_GroundLayers; }
		}

		public GroundCheckType GroundCheck = GroundCheckType.NONE;
		private LayerMask m_GroundLayerMask = -1;
		public LayerMask GroundLayerMask{
			get{ return m_GroundLayerMask = ( m_GroundLayerMask == -1 ? SystemTools.GetLayerMask( GroundLayers, m_GroundLayerMask ) : m_GroundLayerMask ); }
		}

		[SerializeField]
		private List<string> m_ObstacleLayers = new List<string>();
		public List<string> ObstacleLayers{
			get{ return m_ObstacleLayers; }
		}

		public ObstacleCheckType ObstacleCheck = ObstacleCheckType.NONE;
		private LayerMask m_ObstacleLayerMask = -1;
		public LayerMask ObstacleLayerMask{
			get{ return m_ObstacleLayerMask = ( m_ObstacleLayerMask == -1 ? SystemTools.GetLayerMask( ObstacleLayers, m_ObstacleLayerMask ) : m_ObstacleLayerMask ); }
		}

		protected static new ICEWorldRegister m_Instance = null;
		public static new ICEWorldRegister Instance{
			get{ return m_Instance = ( m_Instance == null?GameObject.FindObjectOfType<ICEWorldRegister>():m_Instance ); }
		}

		public delegate void OnSpawnObjectEvent( out GameObject _object, GameObject _reference, Vector3 _position, Quaternion _rotation );
		public event OnSpawnObjectEvent OnSpawnObject;
		public delegate void OnRemoveObjectEvent( GameObject _object, out bool _removed );
		public event OnRemoveObjectEvent OnRemoveObject;
		public delegate void OnDestroyObjectEvent( GameObject _object, out bool _removed );
		public event OnDestroyObjectEvent OnDestroyObject;

		public virtual void Register( GameObject _object )
		{
		}

		public virtual bool Deregister( GameObject _object )
		{
			return true;
		}

		/// <summary>
		/// Spawn the specified _reference, _position and _rotation.
		/// </summary>
		/// <param name="_reference">Reference.</param>
		/// <param name="_position">Position.</param>
		/// <param name="_rotation">Rotation.</param>
		public virtual GameObject Spawn( GameObject _reference, Vector3 _position, Quaternion _rotation )
		{
			if( _reference == null )
				return null;

			GameObject _object = null;
			if( OnSpawnObject != null )
				OnSpawnObject( out _object, _reference, _position, _rotation );
			else
				_object = (GameObject)Object.Instantiate( _reference, _position, _rotation );

			return _object;
		}

		/// <summary>
		/// Tries to remove the specified _object from a potential list. Override this method 
		/// to remove the object from a list.
		/// </summary>
		/// <param name="_object">Object.</param>
		public virtual bool Remove( GameObject _object )
		{
			if( _object == null )
				return false;

			bool _removed = false;
			if( OnRemoveObject != null )
				OnRemoveObject( _object, out _removed );
			else
			{
				_removed = true;
				WorldManager.Destroy( _object );
			}

			return _removed;
		}
			
		public virtual Color GetDebugDefaultColor( GameObject _object )
		{
			return Color.red;
		}
	}

	public class WorldManager{

		public static void Register( GameObject _object )
		{
			if( ICEWorldRegister.Instance != null )
				ICEWorldRegister.Instance.Register( _object );
		}

		public static void Deregister( GameObject _object )
		{
			if( ICEWorldRegister.Instance != null )
				ICEWorldRegister.Instance.Deregister( _object );
		}

		public static GameObject Spawn( GameObject _object, Vector3 _position, Quaternion _rotation  )
		{
			if( ICEWorldRegister.Instance != null )
				return ICEWorldRegister.Instance.Spawn( _object, _position, _rotation );
			else
				return (GameObject)GameObject.Instantiate( _object, _position, _rotation );
		}

		public static void Remove( GameObject _object )
		{
			if( ICEWorldRegister.Instance != null )
				ICEWorldRegister.Instance.Remove( _object );
			else
				WorldManager.Destroy( _object );
		}

		public static void Destroy( GameObject _object )
		{
			GameObject.Destroy( _object );
		}


		/// <summary>
		/// Gets the default color of the gizmo.
		/// </summary>
		/// <returns>The debug default color.</returns>
		/// <param name="_object">Object.</param>
		public static Color GetDebugDefaultColor( GameObject _object ) 
		{
			if( ICEWorldRegister.Instance != null )
				return ICEWorldRegister.Instance.GetDebugDefaultColor( _object );
			else
				return Color.red;
		}

		/// <summary>
		/// Sets the ground level.
		/// </summary>
		/// <param name="_transform">Transform.</param>
		/// <param name="_offset">Offset.</param>
		public static void SetGroundLevel( Transform _transform, float _offset ) 
		{
			SystemTools.EnableColliders( _transform, false );
			float _ground_level = GetGroundLevel( _transform.position, _offset );

			_transform.position = new Vector3( 
				_transform.position.x,
				_ground_level + _offset, 
				_transform.position.z );
			SystemTools.EnableColliders( _transform, true );
		}

		/// <summary>
		/// Gets the ground level.
		/// </summary>
		/// <returns>The ground level.</returns>
		/// <param name="_position">Position.</param>
		/// <param name="_offset">Offset.</param>
		public static float GetGroundLevel( Vector3 _position, float _offset = 0 ) 
		{
			if( ICEWorldRegister.Instance != null && ICEWorldRegister.Instance.GroundCheck == GroundCheckType.RAYCAST )
				return PositionTools.GetGroundLevel( _position, ICEWorldRegister.Instance.GroundCheck , ICEWorldRegister.Instance.GroundLayerMask, 0.5f, 1000, _offset );
			else
				return PositionTools.GetGroundLevel( _position, 0.5f, 1000, _offset );
		}
	}
}
