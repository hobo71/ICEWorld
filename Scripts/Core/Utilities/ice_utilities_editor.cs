// ##############################################################################
//
// ice_utilities_editor.cs | ICE.World.Utilities.EditorTools
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
using UnityEditor;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

namespace ICE.World.EditorUtilities
{
	public static class EditorTools 
	{
		public static AxisInputData[] ReadAxes()
		{
			var _input_manager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
			SerializedObject _input_object = new SerializedObject(_input_manager);
			SerializedProperty _axes_array = _input_object.FindProperty("m_Axes");

			if( _axes_array.arraySize == 0 )
				return new AxisInputData[0];

			AxisInputData[] _axes = new AxisInputData[_axes_array.arraySize];

			for( int i = 0; i < _axes_array.arraySize; ++i )
			{
				_axes[i] = new AxisInputData();

				SerializedProperty axis = _axes_array.GetArrayElementAtIndex(i);

				_axes[i].Name = axis.FindPropertyRelative("m_Name").stringValue;
				_axes[i].Value = axis.FindPropertyRelative("axis").intValue;
				_axes[i].Type = (AxisInputType)axis.FindPropertyRelative("type").intValue;

				//Debug.Log(_axes[i].Name);
				//Debug.Log(_axes[i].Value);
				//Debug.Log(_axes[i].Type);
			}

			return _axes;
		}

		public static int StringToIndex( string _text, string[] _data )
		{
			int _i = 0;
			if( _data.Length > 0 )
			{
				foreach( string _msg in _data )
				{
					if( _text == _msg )
						return _i;

					_i++;
				}
			}

			return 0;
		}


		public static void AddTag( string _tag )
		{
			#if UNITY_EDITOR
			UnityEngine.Object[] _asset = UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
			if( ( _asset != null ) && ( _asset.Length > 0 ) )
			{
				UnityEditor.SerializedObject _object = new UnityEditor.SerializedObject(_asset[0]);
				UnityEditor.SerializedProperty _tags = _object.FindProperty("tags");

				for( int i = 0; i < _tags.arraySize; ++i )
				{
					if( _tags.GetArrayElementAtIndex(i).stringValue == _tag )
						return;    
				}

				_tags.InsertArrayElementAtIndex(0);
				_tags.GetArrayElementAtIndex(0).stringValue = _tag;
				_object.ApplyModifiedProperties();
				_object.Update();
			}
			#endif
		}

		public static bool AddLayer( string _name )
		{
			#if UNITY_EDITOR
			if( LayerMask.NameToLayer( _name ) != -1 )
				return true;

			UnityEditor.SerializedObject _tag_manager = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

			UnityEditor.SerializedProperty _layers = _tag_manager.FindProperty("layers");
			if( _layers == null || ! _layers.isArray )
			{
				Debug.LogWarning( "Sorry, can't set up the layers! It's possible the format of the layers and tags data has changed in this version of Unity. Please add the required layer '" + _name + "' by hand!" );
				return false;
			}

			int _layer_index = -1;
			for ( int _i = 8 ; _i < 32 ; _i++ )
			{
				_layer_index = _i;
				UnityEditor.SerializedProperty _layer = _layers.GetArrayElementAtIndex(_i);

				//Debug.Log( _layer_index + " - " + _layer.stringValue );

				if( _layer.stringValue == "" )
				{
					Debug.Log( "Setting up layers.  Layer " + _layer_index + " is now called " + _name );
					_layer.stringValue = _name;
					break;
				}
			}

			_tag_manager.ApplyModifiedProperties();

			if( LayerMask.NameToLayer( _name ) != -1 )
				return true;
			else
				return false;
			#else
			return true;
			#endif
		}





		public static bool IsPrefab( GameObject _object )
		{
			#if UNITY_EDITOR
			if( _object != null && UnityEditor.PrefabUtility.GetPrefabParent( _object ) == null && UnityEditor.PrefabUtility.GetPrefabObject( _object ) != null ) // Is a prefab
				return true;
			else
				return false;
			#else
				return true;
			#endif
		}
	}

}