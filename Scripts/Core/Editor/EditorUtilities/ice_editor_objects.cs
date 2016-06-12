﻿// ##############################################################################
//
// ice_editor_objects.cs | ObjectEditor
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

namespace ICE.World.EditorUtilities
{
	public class ObjectEditor {

		public static bool BeginObjectContentOrReturn( EditorHeaderType _type, ICEDataObject _object )
		{
			if( _object == null )
				return true;

			if( ( IsFoldoutType( _type ) && _object.Foldout == false ) || ( ! IsFoldoutType( _type ) && _object.Enabled == false ) ) 
				return true;

			EditorGUI.BeginDisabledGroup( _object.Enabled == false );
			EditorGUI.indentLevel++;
			return false;
		}

		public static void EndObjectContent()
		{
			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.Separator();
		}

		public static bool IsHeaderRequired( EditorHeaderType _type )
		{
			switch( _type )
			{
			case EditorHeaderType.NONE:
			case EditorHeaderType.FOLDOUT_CUSTOM:
			case EditorHeaderType.TOGGLE_CUSTOM:
				return false;
			default:
				return true;
			}
		}

		/// <summary>
		/// Determines if is foldout type the specified _type.
		/// </summary>
		/// <returns><c>true</c> if is foldout type the specified _type; otherwise, <c>false</c>.</returns>
		/// <param name="_type">Type.</param>
		public static bool IsFoldoutType( EditorHeaderType _type )
		{
			switch( _type )
			{
			case EditorHeaderType.FOLDOUT:
			case EditorHeaderType.FOLDOUT_BOLD:
			case EditorHeaderType.FOLDOUT_ENABLED:
			case EditorHeaderType.FOLDOUT_ENABLED_BOLD:
			case EditorHeaderType.FOLDOUT_CUSTOM:
				return true;
			default:
				return false;
			}
		}

		public static bool IsEnabledFoldoutType( EditorHeaderType _type )
		{
			switch( _type )
			{
			case EditorHeaderType.FOLDOUT_ENABLED:
			case EditorHeaderType.FOLDOUT_ENABLED_BOLD:
				return true;
			default:
				return false;
			}
		}

		public static EditorHeaderType GetSimpleFoldout( EditorHeaderType _type )
		{
			switch( _type )
			{
			case EditorHeaderType.FOLDOUT_ENABLED_BOLD:
				return EditorHeaderType.FOLDOUT_BOLD;
			case EditorHeaderType.FOLDOUT_ENABLED:
				return EditorHeaderType.FOLDOUT;
			default:
				return _type;
			}
		}


		/// <summary>
		/// Draws the object header.
		/// </summary>
		/// <param name="_object">Object.</param>
		/// <param name="_type">Type.</param>
		/// <param name="_bold">If set to <c>true</c> bold.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_help">Help.</param>
		public static void DrawObjectHeader( ICEDataObject _object, EditorHeaderType _type, string _title, string _hint = "", string _help = "" )
		{
			if( _object == null || IsHeaderRequired( _type ) == false )
				return;

			ICEEditorLayout.BeginHorizontal();
			DrawObjectHeaderLine( _object, _type, _title, _hint );
			ICEEditorLayout.EndHorizontal( _help );
		}

		/// <summary>
		/// Draws the object header line.
		/// </summary>
		/// <param name="_object">Object.</param>
		/// <param name="_type">Type.</param>
		/// <param name="_bold">If set to <c>true</c> bold.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_help">Help.</param>
		public static void DrawObjectHeaderLine( ICEDataObject _object, EditorHeaderType _type, string _title, string _hint, string _help = "" )
		{
			if( _object == null || IsHeaderRequired( _type ) == false )
				return;

			if( _type == EditorHeaderType.TOGGLE )
			{
				_object.Enabled = ICEEditorLayout.Toggle( _title, _hint, _object.Enabled, _help );
				_object.Foldout = _object.Enabled;
			}
			else if( _type == EditorHeaderType.TOGGLE_LEFT )
			{
				_object.Enabled = ICEEditorLayout.ToggleLeft( _title, _hint, _object.Enabled, false, _help );	
				_object.Foldout = _object.Enabled;
			}
			else if( _type == EditorHeaderType.TOGGLE_LEFT_BOLD )
			{
				_object.Enabled = ICEEditorLayout.ToggleLeft( _title, _hint, _object.Enabled, true, _help );
				_object.Foldout = _object.Enabled;
			}
			else if( _type == EditorHeaderType.FOLDOUT )
			{
				//EditorGUI.BeginDisabledGroup( _object.Enabled == false );
				_object.Foldout = ICEEditorLayout.Foldout( _object.Foldout, _title, _help, false );
				//EditorGUI.EndDisabledGroup();

			}
			else if( _type == EditorHeaderType.FOLDOUT_BOLD )
			{
				//EditorGUI.BeginDisabledGroup( _object.Enabled == false );
				_object.Foldout = ICEEditorLayout.Foldout( _object.Foldout, _title, _help, true );
				//EditorGUI.EndDisabledGroup();
			}
			else 
			{
				bool _enabled = _object.Enabled;

				EditorGUI.BeginDisabledGroup( _object.Enabled == false );
				if( _type == EditorHeaderType.FOLDOUT_ENABLED_BOLD )
					_object.Foldout = ICEEditorLayout.Foldout( _object.Foldout, _title, _help, true );
				else
					_object.Foldout = ICEEditorLayout.Foldout( _object.Foldout, _title, _help, false );
				EditorGUI.EndDisabledGroup();

				_object.Enabled = ICEEditorLayout.ButtonCheck( "ENABLED", "Enables/disables this feature", _object.Enabled, ICEEditorStyle.ButtonMiddle );
			
				// Auto foldout if the feature was enabled by the user
				if( _enabled != _object.Enabled && _object.Enabled == true )
					_object.Foldout = true;
			}
		}
	}
}