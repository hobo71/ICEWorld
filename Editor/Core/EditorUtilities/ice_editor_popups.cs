﻿// ##############################################################################
//
// ice_editor_popups.cs | WorldPopups
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
		/// Logicals the operator popup.
		/// </summary>
		/// <returns>The operator popup.</returns>
		/// <param name="_selected">Selected.</param>
		/// <param name="_options">Options.</param>
		public static LogicalOperatorType LogicalOperatorPopup( LogicalOperatorType _selected, params GUILayoutOption[] _options )
		{
			string[] _values = new string[6];
			_values[(int)LogicalOperatorType.EQUAL ] = "==";
			_values[(int)LogicalOperatorType.NOT ] = "!=";
			_values[(int)LogicalOperatorType.LESS ] = "<";
			_values[(int)LogicalOperatorType.LESS_OR_EQUAL ] = "<=";
			_values[(int)LogicalOperatorType.GREATER ] = ">";
			_values[(int)LogicalOperatorType.GREATER_OR_EQUAL ] = ">=";

			return (LogicalOperatorType)EditorGUILayout.Popup( (int)_selected, _values, _options ); 
		}

		/// <summary>
		/// Operators the popup.
		/// </summary>
		/// <returns>The popup.</returns>
		/// <param name="_selected">Selected.</param>
		/// <param name="_options">Options.</param>
		public static LogicalOperatorType OperatorPopup( LogicalOperatorType _selected, params GUILayoutOption[] _options )
		{
			string[] _values = new string[2];
			_values[(int)LogicalOperatorType.EQUAL ] = "IS";
			_values[(int)LogicalOperatorType.NOT ] = "NOT";

			return (LogicalOperatorType)EditorGUILayout.Popup( (int)_selected, _values, _options ); 
		}

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