// ##############################################################################
//
// ice_DataObjectEditor.cs
// Version 1.2.10
//
// © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EditorUtilities;
using ICE.World.EditorInfos;

namespace ICE.World.EditorUtilities
{
	/// <summary>
	/// Popup editor.
	/// </summary>
	public class WorldPopups
	{
		/// <summary>
		/// Draws the message popup.
		/// </summary>
		/// <returns>The message popup.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_component">Component.</param>
		/// <param name="_msg">Message.</param>
		/// <param name="_messages">Messages.</param>
		/// <param name="_custom">Custom.</param>
		/// <param name="_help">Help.</param>
		public static MethodDataContainer MethodPopup( ICEWorldBehaviour _component, MethodDataContainer _method, MethodDataContainer[] _methods, ref bool _custom, string _help = "", string _title = "", string _hint = ""  )
		{
			if( string.IsNullOrEmpty( _title ) )
				_title = "Method";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.METHOD;

			ICEEditorLayout.BeginHorizontal();

			if( _custom || _methods.Length == 0 )
			{
				_method.ComponentName = "";
				_method.MethodName = ICEEditorLayout.Text( _title, _hint, _method.MethodName, "" );
				int indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				_method.MethodType = (MethodParameterType)EditorGUILayout.EnumPopup( _method.MethodType, GUILayout.Width( 60 ) );
				EditorGUI.indentLevel = indent;
			}
			else
			{
				string[] _array = new string[_methods.Length+1];
				_array[0] = " ";

				for( int i = 0 ; i < _methods.Length ; i++ )
					_array[i+1] = _methods[i].MethodKey;

				int _selected = ICEEditorLayout.Popup( _title, _hint, EditorTools.StringToIndex( _method.MethodKey, _array ), _array, "" );

				if( _selected == 0 )
				{
					_method.Reset();
				}
				else
				{
					for( int i = 0 ; i < _methods.Length ; i++ )
					{
						if( _methods[i].MethodKey == _array[_selected] )
						{
							_method.Copy( _methods[i] );
						}
					}
				}
			}

			_custom = ICEEditorLayout.ButtonCheck( "CUSTOM", "", _custom, ICEEditorStyle.ButtonMiddle );
			ICEEditorLayout.EndHorizontal( _help );
			return _method;
		}

	}
}
