// ##############################################################################
//
// ice_editor_world_objects.cs | WorldObjectEditor
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
	public class WorldObjectEditor : ObjectEditor
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

			_timer.InitialImpulsTime = ICEEditorLayout.MaxDefaultSlider( "", "", _timer.InitialImpulsTime , Init.DECIMAL_PRECISION_TIMER, 0, ref _timer.InitialImpulsTimeMaximum, 0, "" );

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
		public static void DrawEventsObject( ICEWorldBehaviour _component, BehaviourEventsObject _events, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _events == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Events";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.EVENTS;

			ICEEditorLayout.BeginHorizontal();

				if( IsEnabledFoldoutType( _type ) )
				{
					EditorGUI.BeginDisabledGroup( _events.Enabled == false );
				}			

				DrawObjectHeaderLine( _events, GetSimpleFoldout( _type ), _title, _hint );

				EditorGUI.BeginDisabledGroup( _events.Enabled == false );
				if( ICEEditorLayout.Button( "ADD", "", ICEEditorStyle.CMDButtonDouble ) )
					_events.Events.Add( new BehaviourEventObject() );

				EditorGUI.BeginDisabledGroup( _events.Events.Count == 0 );
				if( ICEEditorLayout.Button( "RES", "", ICEEditorStyle.CMDButtonDouble ) )
					_events.Events.Clear();
				EditorGUI.EndDisabledGroup();
				EditorGUI.EndDisabledGroup();				

				if( IsEnabledFoldoutType( _type ) )
				{
					EditorGUI.EndDisabledGroup();
					_events.Enabled = ICEEditorLayout.ButtonEnabled( _events.Enabled );
				}
			ICEEditorLayout.EndHorizontal( _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _events ) )
				return;

			for( int i = 0 ; i < _events.Events.Count ; i++ )
			{
				BehaviourEventObject _event = _events.Events[i];

				ICEEditorLayout.BeginHorizontal();
				ICEEditorLayout.Label( ( string.IsNullOrEmpty( _event.ImpulseEvent.FunctionName ) ? "Event #" + i:_event.ImpulseEvent.FunctionName ), true );
				if( ICEEditorLayout.Button( "COPY", "", ICEEditorStyle.CMDButtonDouble ) )
					_events.Events.Add( new BehaviourEventObject( _event ) );

				if( ICEEditorLayout.Button( "X", "", ICEEditorStyle.CMDButton ) )
				{
					_events.Events.Remove( _event );
					return;
				}
				ICEEditorLayout.EndHorizontal();

				EditorGUI.indentLevel++;						
				DrawBehaviourEvent( _component, _event.StartEvent, "", "Start Event" );
				DrawBehaviourEvent( _component, _event.ImpulseEvent, "", "Impulse Event" );
				if( ! string.IsNullOrEmpty( _event.ImpulseEvent.FunctionName ) )
					DrawImpulsTimerObject( _event );	
				else
				{
					//TODO:Deactivate Timer
			
				}
				DrawBehaviourEvent( _component, _event.StopEvent, "", "Stop Event" );
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
		public static void DrawBehaviourEvent( ICEWorldBehaviour _component, BehaviourEvent _event, string _help = "", string _title = "", string _hint = "" )
		{
			if( _event == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Event";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.EVENT;

			_event.Info = WorldPopups.EventPopup( _component, _event.Info, _component.BehaviourEventsInChildren, ref _event.UseCustomFunction, Info.EVENT_POPUP,  _title, _hint );

			EditorGUI.indentLevel++;
			if( _event.ParameterType == BehaviourEventParameterType.Boolean )
				_event.ParameterBoolean = ICEEditorLayout.Toggle( "Parameter Boolean", "", _event.ParameterBoolean, Info.EVENT_PARAMETER_BOOLEAN );
			else if( _event.ParameterType == BehaviourEventParameterType.Integer )
				_event.ParameterInteger = ICEEditorLayout.Integer( "Parameter Integer", "", _event.ParameterInteger, Info.EVENT_PARAMETER_INTEGER );
			else if( _event.ParameterType == BehaviourEventParameterType.Float )
				_event.ParameterFloat = ICEEditorLayout.Float( "Parameter Float", "", _event.ParameterFloat, Info.EVENT_PARAMETER_FLOAT );
			else if( _event.ParameterType == BehaviourEventParameterType.String )
				_event.ParameterString = ICEEditorLayout.Text( "Parameter String", "", _event.ParameterString, Info.EVENT_PARAMETER_STRING );
			EditorGUI.indentLevel--;
		}
	}
}
