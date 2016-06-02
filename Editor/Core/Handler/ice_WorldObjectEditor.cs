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
	public class WorldObjectEditor : DataObjectEditor
	{	

		/// <summary>
		/// Draws the initial durability.
		/// </summary>
		/// <param name="_status">Status.</param>
		public static void DrawInitialDurability( ICE.World.Objects.ICEStatusObject _status )
		{
			ICEEditorLayout.BeginHorizontal();
			ICEEditorLayout.MinMaxGroupSimple( "Initial Durability (" + ( Mathf.Round( _status.DefaultDurability / 0.01f ) * 0.01f ) + ")", "Defines the default physical integrity of the creature.", 
				ref _status.DefaultDurabilityMin, 
				ref _status.DefaultDurabilityMax,
				1, ref _status.DefaultDurabilityMaximum, 1, 40, "" );

			ICEEditorLayout.ButtonMinMaxDefault( ref _status.DefaultDurabilityMin, ref _status.DefaultDurabilityMax, 100, 100 );
			ICEEditorLayout.EndHorizontal( Info.STATUS_INITIAL_DURABILITY  );
			EditorGUILayout.Separator();
		}

		/// <summary>
		/// Draws the impuls timer object.
		/// </summary>
		/// <param name="_timer">Timer.</param>
		public static void DrawImpulsTimerObject( ICEImpulsTimerObject _timer )
		{
			if( _timer.ImpulseIntervalMaximum == 0 )
				_timer.ImpulseIntervalMaximum = 10;
			if( _timer.ImpulseSequenceLimitMaximum == 0 )
				_timer.ImpulseSequenceLimitMaximum = 10;
			if( _timer.ImpulseBreakLengthMaximum == 0 )
				_timer.ImpulseBreakLengthMaximum = 10;

			ICEEditorLayout.RandomMinMaxGroupExt( "Impulse Interval (secs.)", "", ref _timer.ImpulseIntervalMin, ref _timer.ImpulseIntervalMax, 0, ref _timer.ImpulseIntervalMaximum,0,0,30, 0.01f, Info.IMPULSE_TIMER_INTERVAL );
			if( Mathf.Max( _timer.ImpulseIntervalMin, _timer.ImpulseIntervalMax ) > 0 )
			{
				EditorGUI.indentLevel++;
				float _send_limits_min = _timer.ImpulseSequenceLimitMin;
				float _send_limits_max = _timer.ImpulseSequenceLimitMax;
				float _send_limits_maximum = _timer.ImpulseSequenceLimitMaximum;
				ICEEditorLayout.RandomMinMaxGroupExt( "Sequence Limit", "", ref _send_limits_min, ref _send_limits_max, 0, ref _send_limits_maximum,0,0,30, 1, Info.IMPULSE_TIMER_LIMITS );
				_timer.ImpulseSequenceLimitMin = (int)_send_limits_min;
				_timer.ImpulseSequenceLimitMax = (int)_send_limits_max;
				_timer.ImpulseSequenceLimitMaximum = (int)_send_limits_maximum;

				EditorGUI.BeginDisabledGroup( _timer.ImpulseSequenceLimitMax == 0 );
				ICEEditorLayout.RandomMinMaxGroupExt( "Break Length (secs.)", "", ref _timer.ImpulseBreakLengthMin, ref _timer.ImpulseBreakLengthMax, 0, ref _timer.ImpulseBreakLengthMaximum,2,5,30, 0.01f, Info.IMPULSE_TIMER_BREAK_LENGTH );
				EditorGUI.EndDisabledGroup();
				EditorGUI.indentLevel--;
			}
			else
			{
				ICEEditorLayout.BeginHorizontal();
				ICEEditorLayout.EndHorizontal();
			}
		}

		/// <summary>
		/// Draws the message object.
		/// </summary>
		/// <param name="_component">Component.</param>
		/// <param name="_message">Message.</param>
		/// <param name="_left">If set to <c>true</c> left.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_help">Help.</param>
		public static void DrawMethodsObject( ICEWorldBehaviour _component, MethodsObject _methods, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _methods == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Methods";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.METHODS;

			ICEEditorLayout.BeginHorizontal();

			if( IsEnabledFoldoutType( _type ) )
			{
				EditorGUI.BeginDisabledGroup( _methods.Enabled == false );
			}			

			DrawObjectHeaderLine( _methods, GetSimpleFoldout( _type ), _title, _hint );

			EditorGUI.BeginDisabledGroup( _methods.Enabled == false );
			if( ICEEditorLayout.Button( "ADD", "", ICEEditorStyle.CMDButtonDouble ) )
				_methods.Methods.Add( new MethodObject() );

			EditorGUI.BeginDisabledGroup( _methods.Methods.Count == 0 );
			if( ICEEditorLayout.Button( "RES", "", ICEEditorStyle.CMDButtonDouble ) )
				_methods.Methods.Clear();
			EditorGUI.EndDisabledGroup();
			EditorGUI.EndDisabledGroup();				

			if( IsEnabledFoldoutType( _type ) )
			{
				EditorGUI.EndDisabledGroup();
				_methods.Enabled = ICEEditorLayout.ButtonEnabled( _methods.Enabled );
			}
			ICEEditorLayout.EndHorizontal( _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _methods ) )
				return;

			for( int i = 0 ; i < _methods.Methods.Count ; i++ )
			{
				MethodObject _method = _methods.Methods[i];

				ICEEditorLayout.BeginHorizontal();
				ICEEditorLayout.Label( ( string.IsNullOrEmpty( _method.ImpulseMethod.MethodName ) ? "Undefined":_method.ImpulseMethod.MethodName ), true );
				if( ICEEditorLayout.Button( "COPY", "", ICEEditorStyle.CMDButtonDouble ) )
					_methods.Methods.Add( new MethodObject( _method ) );

				if( ICEEditorLayout.Button( "X", "", ICEEditorStyle.CMDButton ) )
				{
					_methods.Methods.Remove( _method );
					return;
				}
				ICEEditorLayout.EndHorizontal();

				EditorGUI.indentLevel++;						
				DrawMethodDataObject( _component, _method.StartMethod, "", "Start Method" );
				DrawMethodDataObject( _component, _method.ImpulseMethod, "", "Impulse Method" );
				if( ! string.IsNullOrEmpty( _method.ImpulseMethod.MethodName ) )
					DrawImpulsTimerObject( _method );	
				else
				{
					//TODO:Deactivate Timer
				}
				DrawMethodDataObject( _component, _method.StopMethod, "", "Stop Method" );
				EditorGUI.indentLevel--;
			}

			EndObjectContent();
			// CONTENT END
		}

		/// <summary>
		/// Draws the method data object.
		/// </summary>
		/// <param name="_component">Component.</param>
		/// <param name="_method">Method.</param>
		/// <param name="_help">Help.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		public static void DrawMethodDataObject( ICEWorldBehaviour _component, MethodDataObject _method, string _help = "", string _title = "", string _hint = "" )
		{
			if( _method == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Method";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.METHOD;

			_method.MethodData = WorldPopups.MethodPopup( _component, _method.MethodData, _component.AllPublicMethods, ref _method.UseCustomMethod, Info.METHOD_POPUP,  _title, _hint );

			EditorGUI.indentLevel++;
			if( _method.MethodType == MethodParameterType.Boolean )
				_method.ParameterBoolean = ICEEditorLayout.Toggle( "Parameter Boolean", "", _method.ParameterBoolean, Info.METHOD_PARAMETER_BOOLEAN );
			else if( _method.MethodType == MethodParameterType.Integer )
				_method.ParameterInteger = ICEEditorLayout.Integer( "Parameter Integer", "", _method.ParameterInteger, Info.METHOD_PARAMETER_INTEGER );
			else if( _method.MethodType == MethodParameterType.Float )
				_method.ParameterFloat = ICEEditorLayout.Float( "Parameter Float", "", _method.ParameterFloat, Info.METHOD_PARAMETER_FLOAT );
			else if( _method.MethodType == MethodParameterType.String )
				_method.ParameterString = ICEEditorLayout.Text( "Parameter String", "", _method.ParameterString, Info.METHOD_PARAMETER_STRING );
			EditorGUI.indentLevel--;
		}
	}
}
