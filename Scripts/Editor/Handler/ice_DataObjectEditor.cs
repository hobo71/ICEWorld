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

namespace ICE.World.EditorUtilities
{
	public class DataObjectEditor {

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
