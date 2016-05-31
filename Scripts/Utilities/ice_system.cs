// ##############################################################################
//
// ice_system.cs | ICE.World.Utilities.SystemTools
// Version 1.2.10
//
// © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
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
using System.Collections;
using System.Collections.Generic;

namespace ICE.World.Utilities
{
	public static class SystemTools 
	{
		/// <summary>
		/// Determines if is in layer mask the specified _object _layerMask.
		/// </summary>
		/// <returns><c>true</c> if is in layer mask the specified _object _layerMask; otherwise, <c>false</c>.</returns>
		/// <param name="_object">Object.</param>
		/// <param name="_layerMask">Layer mask.</param>
		public static bool IsInLayerMask(GameObject _object, LayerMask _layerMask )
		{
			// Convert the object's layer to a bitfield for comparison
			int _mask = (1 << _object.layer);
			if ((_layerMask.value & _mask) > 0)  // Extra round brackets required!
				return true;
			else
				return false;
		}

		/// <summary>
		/// Determines if is in layer mask the specified layer layerMask.
		/// </summary>
		/// <returns><c>true</c> if is in layer mask the specified layer layerMask; otherwise, <c>false</c>.</returns>
		/// <param name="layer">Layer.</param>
		/// <param name="layerMask">Layer mask.</param>
		public static bool IsInLayerMask(int layer, int layerMask)
		{
			return (layerMask & 1<<layer) == 0;
		}

		/// <summary>
		/// Layer names to mask.
		/// </summary>
		/// <returns>The to mask.</returns>
		/// <param name="layerNames">Layer names.</param>
		public static LayerMask NamesToMask(params string[] layerNames)
		{
			LayerMask ret = (LayerMask)0;
			foreach(var name in layerNames)
			{
				ret |= (1 << LayerMask.NameToLayer(name));
			}
			return ret;
		}

		/// <summary>
		/// Gets the layer mask.
		/// </summary>
		/// <returns>The layer mask.</returns>
		/// <param name="_layers">Layers.</param>
		/// <param name="_mask">Mask.</param>
		/// <param name="_water">If set to <c>true</c> water.</param>
		public static LayerMask GetLayerMask( List<string> _layers, LayerMask _mask, bool _water = false )
		{
			if( _mask == -1 )
			{
				if( _layers.Count > 0 )
				{
					_mask = 0;
					foreach( string _layer in _layers )
					{
						int _index = LayerMask.NameToLayer( _layer );
						if( _index != -1 )
							_mask |= (1 << _index );
					}
				}
				else
					_mask = Physics.DefaultRaycastLayers;

				if( _water )
					_mask |= (1 << 4 );
				else if( IsInLayerMask( 4, _mask ) )
					_mask |= (1 >> 4 );
			}

			return _mask;
		}

		/// <summary>
		/// Gets the size of the object.
		/// </summary>
		/// <returns>The object size.</returns>
		/// <param name="_object">Object.</param>
		public static Vector3 GetObjectSize( GameObject _object )
		{
			Vector3 _total_size = Vector3.zero;

			Transform[] _transforms = _object.GetComponentsInChildren<Transform>();

			foreach( Transform _transform in _transforms )
			{
				Renderer _renderer = _transform.GetComponent<Renderer>();
				if( _renderer != null )
				{
					Vector3 _size = _renderer.bounds.size;

					if( _size.x > _total_size.x )
						_total_size.x = _size.x;
					if( _size.y > _total_size.y )
						_total_size.y = _size.y;
					if( _size.z > _total_size.z )
						_total_size.z = _size.z;
				}
			}

			//_total_size = Vector3.zero;

			if( _total_size == Vector3.zero )
				_total_size = GetObjectSizeByTransforms( _object );

			return _total_size;
		}

		/// <summary>
		/// Gets the object size by transform.
		/// </summary>
		/// <returns>The object size by transform.</returns>
		/// <param name="_object">Object.</param>
		public static Vector3 GetObjectSizeByTransform( GameObject _object )
		{
			Vector3 _size = Vector3.zero;

			Transform[] _transforms = _object.GetComponentsInChildren<Transform>();

			Vector3 _min = Vector3.zero;
			Vector3 _max = Vector3.zero;

			foreach( Transform _transform in _transforms )
			{
				if( _transform == _object.transform )
					continue;

				Vector3 _position = _object.transform.TransformPoint( _transform.localPosition );

				_position.x = _position.x/_object.transform.lossyScale.x;
				_position.y = _position.y/_object.transform.lossyScale.y;
				_position.z = _position.z/_object.transform.lossyScale.z;

				_position = _object.transform.InverseTransformPoint( _position );

				if( _position.x <= _min.x && _position.y <= _min.y )
				{
					_min.x = _position.x;
					_min.y = _position.y;
				}

				if( _position.x >= _max.x && _position.y <= _max.y )
				{
					_max.x = _position.x;
					_max.y = _position.y;
				}
				if( _position.z <= _min.z && _position.y <= _min.y )
				{
					_min.z = _position.z;
					_min.y = _position.y;
				}

				if( _position.z >= _max.z && _position.y <= _max.y )
				{
					_max.z = _position.z;
					_max.y = _position.y;
				}
			}

			_size.x = (_min.x * -1 ) + _max.x;
			_size.z = (_min.z * -1 ) + _max.z;
			_size.y = (_min.y * -1 ) + _max.y;

			return _size;
		}

		/// <summary>
		/// Gets the object size by transforms.
		/// </summary>
		/// <returns>The object size by transforms.</returns>
		/// <param name="_object">Object.</param>
		public static Vector3 GetObjectSizeByTransforms( GameObject _object )
		{
			Vector3 _size = Vector3.zero;

			Transform[] _transforms = _object.GetComponentsInChildren<Transform>();

			Vector3 _width_l = Vector3.zero;
			Vector3 _width_r = Vector3.zero;
			Vector3 _depth_f = Vector3.zero;
			Vector3 _depth_b = Vector3.zero;
			Vector3 _height_t = Vector3.zero;
			Vector3 _height_b = Vector3.zero;

			_width_l.y = Mathf.Infinity;
			_width_r.y = Mathf.Infinity;
			_depth_f.y = Mathf.Infinity;
			_depth_b.y = Mathf.Infinity;
			_height_t.y = Mathf.Infinity;
			_height_b.y = Mathf.Infinity;

			foreach( Transform _transform in _transforms )
			{
				if( _transform == _object.transform )
					continue;

				Vector3 _position = _object.transform.TransformPoint( _transform.localPosition );

				_position.x = _position.x/_object.transform.lossyScale.x;
				_position.y = _position.y/_object.transform.lossyScale.y;
				_position.z = _position.z/_object.transform.lossyScale.z;

				_position = _object.transform.InverseTransformPoint( _position );

				if( _position == Vector3.zero )
					continue;

				if( _position.x <= _width_l.x && _position.y <= _width_l.y )
				{
					_width_l.x = _position.x;
					_width_l.y = _position.y;
				}

				if( _position.x >= _width_r.x && _position.y <= _width_r.y )
				{
					_width_r.x = _position.x;
					_width_r.y = _position.y;
				}
				if( _position.z <= _depth_b.z && _position.y <= _depth_b.y )
				{
					_depth_b.z = _position.z;
					_depth_b.y = _position.y;
				}

				if( _position.z >= _depth_f.z && _position.y <= _depth_f.y )
				{
					_depth_f.z = _position.z;
					_depth_f.y = _position.y;
				}

				if( _position.y >= _height_t.y )
				{
					_height_t.z = _position.z;
					_height_t.y = _position.y;
				}

				if( _position.y <= _height_b.y )
				{
					_height_b.z = _position.z;
					_height_b.y = _position.y;
				}
			}

			_size.x = (_width_l.x * -1 ) + _width_r.x;
			_size.z = (_depth_b.z * -1 ) + _depth_f.z;
			_size.y = (_height_b.y * -1 ) + _height_t.y;

			return _size;
		}

		/// <summary>
		/// Enables the colliders.
		/// </summary>
		/// <param name="_transform">Transform.</param>
		/// <param name="_enabled">If set to <c>true</c> enabled.</param>
		public static void EnableColliders( Transform _transform, bool _enabled )
		{
			if( _transform == null )
				return;
			
			Collider[] _colliders = _transform.GetComponentsInChildren<Collider>();

			foreach( Collider _collider in _colliders )
				_collider.enabled = _enabled;
		}

		/// <summary>
		/// Enables the renderer.
		/// </summary>
		/// <param name="_transform">Transform.</param>
		/// <param name="_enabled">If set to <c>true</c> enabled.</param>
		public static void EnableRenderer( Transform _transform, bool _enabled )
		{
			if( _transform == null )
				return;

			Renderer[] _renderers = _transform.GetComponentsInChildren<Renderer>();

			foreach( Renderer _renderer in _renderers )
				_renderer.enabled = _enabled;
		}

		/// <summary>
		/// Destroy the specified _object.
		/// </summary>
		/// <param name="_object">Object.</param>
		public static bool Destroy( GameObject _object )
		{
			if( _object == null )
				return false;

			if( Application.isEditor )
				GameObject.DestroyImmediate( _object );
			else
				GameObject.Destroy( _object );

			return true;
		}

		/// <summary>
		/// Attachs to transform.
		/// </summary>
		/// <returns><c>true</c>, if to transform was attached, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		/// <param name="_parent">Parent.</param>
		public static bool AttachToTransform( GameObject _object, Transform _parent )
		{
			if( _object == null ||  _parent == null )
				return false;

			_object.transform.parent = _parent;
			_object.transform.position = _parent.position;
			_object.transform.rotation = _parent.rotation;
			_object.SetActive( true );

			if( _object.GetComponent<Rigidbody>() != null )
			{
				_object.GetComponent<Rigidbody>().useGravity = false;
				_object.GetComponent<Rigidbody>().isKinematic = true;
				_object.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			}

			return true;
		}

		/// <summary>
		/// Detachs from transform.
		/// </summary>
		/// <returns><c>true</c>, if from transform was detached, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		public static bool DetachFromTransform( GameObject _object )
		{
			if( _object == null )
				return false;

			_object.transform.SetParent( null, true );
			_object.SetActive( true );

			if( _object.GetComponent<Rigidbody>() != null )
			{
				_object.GetComponent<Rigidbody>().useGravity = true;
				_object.GetComponent<Rigidbody>().isKinematic = false;
				_object.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			}

			return true;
		}

		/// <summary>
		/// Processes the child.
		/// </summary>
		/// <param name="aObj">A object.</param>
		/// <param name="aList">A list.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private static void ProcessChild<T>(Transform aObj, ref List<T> aList) where T : Component
		{
			T c = aObj.GetComponent<T>();
			if (c != null)
				aList.Add(c);
			foreach(Transform child in aObj)
				ProcessChild<T>(child,ref aList);
		}

		/// <summary>
		/// Gets all children.
		/// </summary>
		/// <returns>The all children.</returns>
		/// <param name="aObj">A object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] GetAllChildren<T>(this Transform aObj) where T : Component
		{
			List<T> result = new List<T>();
			ProcessChild<T>(aObj, ref result);
			return result.ToArray();
		}

		/// <summary>
		/// Gets all children.
		/// </summary>
		/// <returns>The all children.</returns>
		/// <param name="aObj">A object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] GetAllChildren<T>(this GameObject aObj) where T : Component
		{
			List<T> result = new List<T>();
			ProcessChild<T>(aObj.transform, ref result);
			return result.ToArray();
		}

		/// <summary>
		/// Gets the children.
		/// </summary>
		/// <param name="_list">List.</param>
		/// <param name="_transform">Transform.</param>
		public static void GetChildren( List<Transform> _list, Transform _transform )
		{
			if( _list == null || _transform == null )
				return;

			_list.Add( _transform );
			
			foreach (Transform _child in _transform )
				GetChildren( _list, _child );
		}

		/// <summary>
		/// Finds the name of the child by.
		/// </summary>
		/// <returns>The child by name.</returns>
		/// <param name="_name">Name.</param>
		/// <param name="_parent">Parent.</param>
		public static Transform FindChildByName( string _name, Transform _parent )
		{
			if( string.IsNullOrEmpty( _name ) || _parent == null )
				return null;

			if( _parent.name == _name )
				return _parent;

			foreach (Transform _child in _parent )
			{
				Transform _result = FindChildByName( _name, _child );

				if( _result != null && _result.name == _name )
					return _result;
			}

			return null;
		}

		/// <summary>
		/// Finds the game objects by layer.
		/// </summary>
		/// <returns>The game objects by layer.</returns>
		/// <param name="_layer">Layer.</param>
		public static GameObject[] FindGameObjectsByLayer( int _layer ) 
		{
			GameObject[] _objects = GameObject.FindObjectsOfType<GameObject>();

			List<GameObject> _result_objects = new List<GameObject>();

			for( int i = 0; i < _objects.Length; i++) 
			{
			 	if( _objects[i].layer == _layer )
			    	_result_objects.Add(_objects[i]);
		    }
		     
			if( _result_objects.Count == 0 )
				return null;
		     
		     return _result_objects.ToArray();
		 }

		/// <summary>
		/// Copies the transforms.
		/// </summary>
		/// <param name="_source_transform">Source transform.</param>
		/// <param name="_target_transform">Target transform.</param>
		public static void CopyTransforms( Transform _source_transform , Transform _target_transform )
		{
			if( _source_transform == null || _target_transform == null )
				return;

			_target_transform.position = _source_transform.position;
			_target_transform.rotation = _source_transform.rotation;
			
			foreach( Transform _child in _target_transform) 
			{
				Transform _source = _source_transform.Find( _child.name );
				if( _source )
					CopyTransforms( _source, _child );
			}
		}

		/// <summary>
		/// Copies the component.
		/// </summary>
		/// <returns>The component.</returns>
		/// <param name="_original">_original.</param>
		/// <param name="_destination">_destination.</param>
		public static Component CopyComponent( Component _original, GameObject _destination )
		{
			System.Type _type = _original.GetType();
			Component _copy = _destination.AddComponent(_type);
			// Copied fields can be restricted with BindingFlags
			System.Reflection.FieldInfo[] _fields = _type.GetFields(); 
			foreach (System.Reflection.FieldInfo _field in _fields)
				_field.SetValue( _copy, _field.GetValue(_original));
			return _copy;
		}

		/// <summary>
		/// Copies the component (generic).
		/// </summary>
		/// <returns>The component.</returns>
		/// <param name="_original">_original.</param>
		/// <param name="_destination">_destination.</param>
		public static T CopyComponent<T>(T _original, GameObject _destination) where T : Component
		{
			System.Type _type = _original.GetType();
			Component _copy = _destination.AddComponent(_type);
			System.Reflection.FieldInfo[] _fields = _type.GetFields();
			foreach( System.Reflection.FieldInfo _field in _fields )
				_field.SetValue( _copy, _field.GetValue(_original) );
			return _copy as T;
		}

		/// <summary>
		/// Lists the assemblies.
		/// </summary>
		/// <param name="_types">If set to <c>true</c> types.</param>
		public static void ListAssemblies( bool _types = false )
		{
			System.Reflection.Assembly[] _assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
			foreach( System.Reflection.Assembly _assembly in _assemblies )
			{
				Debug.Log( "Name : " + _assembly.FullName );

				if( _types == true )
					ListAssemblyTypes( _assembly );
			}
		}

		/// <summary>
		/// Lists the assembly types.
		/// </summary>
		/// <param name="_assembly">Assembly.</param>
		public static void ListAssemblyTypes( System.Reflection.Assembly _assembly )
		{
			if( _assembly == null )
				return;

			foreach( System.Type _type in _assembly.GetTypes() )
			{
				Debug.Log( "    Type : " + _type.Name );
			}
		}

		/// <summary>
		/// Gets the name of the assembly by type.
		/// </summary>
		/// <returns>The assembly by type name.</returns>
		/// <param name="_name">Name.</param>
		public static System.Reflection.Assembly GetAssemblyByTypeName( string _name )
		{
			System.Reflection.Assembly[] _assemblies = GetAssemblies();
			
			foreach( System.Reflection.Assembly _assembly in _assemblies )
			{
				foreach( System.Type _type in _assembly.GetTypes() )
				{
					if ( _type.Name == _name )
						return _assembly;
				}
			}
			
			return null;
		}

		/// <summary>
		/// Gets the assemblies.
		/// </summary>
		/// <returns>The assemblies.</returns>
		public static System.Reflection.Assembly[] GetAssemblies()
		{
			return System.AppDomain.CurrentDomain.GetAssemblies();
		}
			
		/// <summary>
		/// Doeses the type exist.
		/// </summary>
		/// <returns><c>true</c>, if type exist was doesed, <c>false</c> otherwise.</returns>
		/// <param name="_name">Name.</param>
		public static bool DoesTypeExist( string _name )
		{
			/*
			SystemTools.ListAssemblies();

			Debug.Log( "test 1 " + SystemTools.DoesTypeExist("ICECreatureControl").ToString() );
			Debug.Log( "test 2 " + SystemTools.DoesTypeExist("PHOTON").ToString() );

			System.Reflection.Assembly _asm = SystemTools.GetAssemblyByTypeName( "ICECreatureControl" );

			if( _asm != null )
				Debug.Log( "test 3 " + _asm.FullName );
			 */
			System.Reflection.Assembly[] _assemblies = GetAssemblies();
			
			foreach( System.Reflection.Assembly _assembly in _assemblies )
			{
				foreach( System.Type _type in _assembly.GetTypes() )
				{
					if ( _type.Name == _name )
						return true;
				}
			}
			
			return false;
		}


		/// <summary>
		/// Gets the main texture of the terrain.
		/// </summary>
		/// <returns>The main terrain texture index</returns>
		/// <param name="_world_pos">_world_pos.</param>
		/// <param name="_terrain">_terrain.</param>
		/// <description>
		/// http://answers.unity3d.com/questions/34328/terrain-with-multiple-splat-textures-how-can-i-det.html
		/// </description>
		public static int GetMainTerrainTexture( Vector3 _world_pos, Terrain _terrain )
		{
			TerrainData _terrain_data = _terrain.terrainData;
			Vector3 _terrain_position = _terrain.transform.position;

			// evaluate the splat map cell 
			int _map_x = (int)((( _world_pos.x - _terrain_position.x ) / _terrain_data.size.x ) * _terrain_data.alphamapWidth );
			int _map_z = (int)((( _world_pos.z - _terrain_position.z ) / _terrain_data.size.z ) * _terrain_data.alphamapHeight );

			// get the splat map data 
			float[,,] _map_data = _terrain_data.GetAlphamaps(_map_x,_map_z,1,1);			

			// extracting the _map_data array data to the 1d mix array:
			float[] _mix = new float[_map_data.GetUpperBound(2)+1];
			for( int i=0; i<_mix.Length; ++i )
				_mix[i] = _map_data[0,0,i];    

			float _max_mix = 0;
			int _max_index = 0;

			// find the maximum by looping through the mix values 
			for( int _i = 0; _i < _mix.Length; ++_i)
			{
				if( _mix[_i] > _max_mix )
				{
					_max_index = _i;
					_max_mix = _mix[_i];
				}
			}

			return _max_index;

		}
	}
}
