// ##############################################################################
//
// ICE.World.ICEWorldRegistry.cs
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

namespace ICE.World
{
	public class ICEWorldRegister : ICEWorld {

		[SerializeField]
		private List<string> m_GroundLayers = new List<string>();
		public List<string> GroundLayers{
			get{ return m_GroundLayers; }
		}

		public GroundCheckType GroundCheck = GroundCheckType.NONE;
		private LayerMask m_GroundLayerMask = -1;
		public LayerMask GroundLayerMask{
			get{
				m_GroundLayerMask = SystemTools.GetLayerMask( GroundLayers, m_GroundLayerMask );
				return m_GroundLayerMask;
			}
		}

		protected static new ICEWorldRegister m_Instance = null;
		public static new ICEWorldRegister Instance{
			get{ return m_Instance = ( m_Instance == null?GameObject.FindObjectOfType<ICEWorldRegister>():m_Instance ); }
		}

		public virtual void Register( GameObject _object )
		{
		}

		public virtual bool Deregister( GameObject _object )
		{
			return true;
		}

		public virtual void Remove( GameObject _object )
		{
			DestroyObject( _object );
		}

		public virtual Color GetDebugDefaultColor( GameObject _object )
		{
			return Color.red;
		}
	}

	public class WorldRegister{

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

		public static void Remove( GameObject _object )
		{
			if( ICEWorldRegister.Instance != null )
				ICEWorldRegister.Instance.Remove( _object );
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
