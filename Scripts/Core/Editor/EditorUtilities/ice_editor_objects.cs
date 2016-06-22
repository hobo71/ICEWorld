// ##############################################################################
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

		/// <summary>
		/// Begins the object content or return.
		/// </summary>
		/// <returns><c>true</c>, if object content or return was begun, <c>false</c> otherwise.</returns>
		/// <param name="_type">Type.</param>
		/// <param name="_object">Object.</param>
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

		/// <summary>
		/// Ends the content of the object.
		/// </summary>
		public static void EndObjectContent()
		{
			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.Separator();
		}

		/// <summary>
		/// Determines if is header required the specified _type.
		/// </summary>
		/// <returns><c>true</c> if is header required the specified _type; otherwise, <c>false</c>.</returns>
		/// <param name="_type">Type.</param>
		public static bool IsHeaderRequired( EditorHeaderType _type )
		{
			switch( _type )
			{
			case EditorHeaderType.NONE:
			case EditorHeaderType.FOLDOUT_CUSTOM:
			case EditorHeaderType.TOGGLE_CUSTOM:
			case EditorHeaderType.LABEL_CUSTOM:
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

		/// <summary>
		/// Determines if is enabled foldout type the specified _type.
		/// </summary>
		/// <returns><c>true</c> if is enabled foldout type the specified _type; otherwise, <c>false</c>.</returns>
		/// <param name="_type">Type.</param>
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

		/// <summary>
		/// Gets the simple foldout.
		/// </summary>
		/// <returns>The simple foldout.</returns>
		/// <param name="_type">Type.</param>
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
			if( _object == null )
				return;

			DrawObjectHeaderLine( ref _object.Enabled, ref _object.Foldout, _type, _title, _hint, _help );
			/*
			// TOOGLE
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

			// LABEL
			else if( _type == EditorHeaderType.LABEL )
			{
				ICEEditorLayout.Label( _title, false, _help );
				_object.Foldout = true;
			}
			else if( _type == EditorHeaderType.LABEL_BOLD )
			{
				ICEEditorLayout.Label( _title, true, _help );
				_object.Foldout = true;
			}
			else if( _type == EditorHeaderType.LABEL_ENABLED || _type == EditorHeaderType.LABEL_ENABLED_BOLD )
			{
				EditorGUI.BeginDisabledGroup( _object.Enabled == false );
					if( _type == EditorHeaderType.LABEL_ENABLED_BOLD )
						ICEEditorLayout.Label( _title, true, _help );
					else
						ICEEditorLayout.Label( _title, false, _help );
					_object.Foldout = true;
				EditorGUI.EndDisabledGroup();

				_object.Enabled = ICEEditorLayout.ButtonCheck( "ENABLED", "Enables/disables this feature", _object.Enabled, ICEEditorStyle.ButtonMiddle );
			}

			// FOLDOUT
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
			}*/
		}

		public static void DrawObjectHeaderLine( ref bool _enabled, ref bool _foldout, EditorHeaderType _type, string _title, string _hint, string _help = "" )
		{
			if( IsHeaderRequired( _type ) == false )
				return;

			// TOOGLE
			if( _type == EditorHeaderType.TOGGLE )
			{
				_enabled = ICEEditorLayout.Toggle( _title, _hint, _enabled, _help );
				_foldout = _enabled;
			}
			else if( _type == EditorHeaderType.TOGGLE_LEFT )
			{
				_enabled = ICEEditorLayout.ToggleLeft( _title, _hint, _enabled, false, _help );	
				_foldout = _enabled;
			}
			else if( _type == EditorHeaderType.TOGGLE_LEFT_BOLD )
			{
				_enabled = ICEEditorLayout.ToggleLeft( _title, _hint, _enabled, true, _help );
				_foldout = _enabled;
			}

			// LABEL
			else if( _type == EditorHeaderType.LABEL )
			{
				ICEEditorLayout.Label( _title, false, _help );
				_foldout = true;
			}
			else if( _type == EditorHeaderType.LABEL_BOLD )
			{
				ICEEditorLayout.Label( _title, true, _help );
				_foldout = true;
			}
			else if( _type == EditorHeaderType.LABEL_ENABLED || _type == EditorHeaderType.LABEL_ENABLED_BOLD )
			{
				EditorGUI.BeginDisabledGroup( _enabled == false );
				if( _type == EditorHeaderType.LABEL_ENABLED_BOLD )
					ICEEditorLayout.Label( _title, true, _help );
				else
					ICEEditorLayout.Label( _title, false, _help );
				_foldout = true;
				EditorGUI.EndDisabledGroup();

				_enabled = ICEEditorLayout.ButtonCheck( "ENABLED", "Enables/disables this feature", _enabled, ICEEditorStyle.ButtonMiddle );
			}

			// FOLDOUT
			else if( _type == EditorHeaderType.FOLDOUT )
			{
				//EditorGUI.BeginDisabledGroup( _object.Enabled == false );
				_foldout = ICEEditorLayout.Foldout( _foldout, _title, _help, false );
				//EditorGUI.EndDisabledGroup();

			}
			else if( _type == EditorHeaderType.FOLDOUT_BOLD )
			{
				//EditorGUI.BeginDisabledGroup( _object.Enabled == false );
				_foldout = ICEEditorLayout.Foldout( _foldout, _title, _help, true );
				//EditorGUI.EndDisabledGroup();
			}
			else 
			{
				bool _enabled_in = _enabled;

				EditorGUI.BeginDisabledGroup( _enabled == false );
				if( _type == EditorHeaderType.FOLDOUT_ENABLED_BOLD )
					_foldout = ICEEditorLayout.Foldout( _foldout, _title, _help, true );
				else
					_foldout = ICEEditorLayout.Foldout( _foldout, _title, _help, false );
				EditorGUI.EndDisabledGroup();

				_enabled = ICEEditorLayout.ButtonCheck( "ENABLED", "Enables/disables this feature", _enabled, ICEEditorStyle.ButtonMiddle );

				// Auto foldout if the feature was enabled by the user
				if( _enabled_in != _enabled && _enabled == true )
					_foldout = true;
			}
		}
	}
}
