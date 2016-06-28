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
using ICE.World.EnumTypes;

using ICE.World.EditorUtilities;
using ICE.World.EditorInfos;

namespace ICE.World.EditorUtilities
{
	public class WorldObjectEditor : ObjectEditor
	{	
		public static DurabilityInfluenceObject InfluencePopup( string _title, string _hint, DurabilityInfluenceObject _influence, List<DurabilityInfluenceObject> _list, string _help = "", params GUILayoutOption[] _gui )
		{
			ICEEditorLayout.BeginHorizontal();
	
				GUIContent[] _options = new GUIContent[_list.Count];
				int _selected = 0;
				for( int _i = 0 ; _i < _list.Count ; _i++ )
				{
					_options[ _i ] = new GUIContent( _list[_i].Key );

					if( _list[_i].Key == _influence.Key )
						_selected = _i;
				}

				_selected = EditorGUILayout.Popup( new GUIContent( _title, _hint ) , _selected, _options, _gui );

				_influence = _list[_selected];

			ICEEditorLayout.EndHorizontal( _help );
			return _influence;
		}

		public static bool DrawDurabilityInfluenceObject( ICEWorldBehaviour _component, DurabilityCompositionObject _composition, int _index )
		{
			if( _composition == null || _index < 0 || _index >= _composition.Influences.Count )
				return false;

			DurabilityInfluenceObject _influence = _composition.Influences[_index];


			ICEEditorLayout.BeginHorizontal();

			_influence.Key = ICEEditorLayout.Text( "Key", "", _influence.Key , "" );


			if( ICEEditorLayout.ListUpDownButtons<DurabilityInfluenceObject>( _composition.Influences, _index ) )
				return true;

			if( ICEEditorLayout.ListDeleteButton<DurabilityInfluenceObject>( _composition.Influences, _influence ) )
				return true;

			ICEEditorLayout.EndHorizontal( "TODO" );

			return false;
		}

		public static bool DrawDurabilityAttributeObject( ICEWorldBehaviour _component, DurabilityCompositionObject _composition, int _index )
		{
			if( _composition == null || _index < 0 || _index >= _composition.Attributes.Count )
				return false;

			DurabilityAttributeObject _attribute = _composition.Attributes[_index];


			ICEEditorLayout.BeginHorizontal();

					_attribute.Key = ICEEditorLayout.Text( "Key", "", _attribute.Key , "" );

				if( ICEEditorLayout.ListUpDownButtons<DurabilityAttributeObject>( _composition.Attributes, _index ) )
					return true;

				if( ICEEditorLayout.ListDeleteButton<DurabilityAttributeObject>( _composition.Attributes, _attribute ) )
					return true;

				if( ICEEditorLayout.Button( "ADD", "", ICEEditorStyle.CMDButtonDouble ) )
					_attribute.Multiplier.Add( new DurabilityInfluenceMultiplierObject() );

			ICEEditorLayout.EndHorizontal( "TODO" );

			for( int i = 0 ; i < _attribute.Multiplier.Count ; i++ )
			{
				DurabilityInfluenceMultiplierObject _multiplier = _attribute.Multiplier[i];

				if( _multiplier == null )
					continue;
				
				ICEEditorLayout.BeginHorizontal();

				//_multiplier.Influence = InfluencePopup( "", "", _multiplier.Influence, _composition.Influences, "", GUILayout.MinWidth( 120 ), GUILayout.MaxWidth( 250 ) );
				_multiplier.Multiplier = ICEEditorLayout.DefaultSlider( _multiplier.Influence.Key + " Multiplier", "", _multiplier.Multiplier, 0.001f, -1, 1, 0 );

				if( ICEEditorLayout.ListDeleteButton<DurabilityInfluenceMultiplierObject>( _attribute.Multiplier, _multiplier ) )
					return true;

				ICEEditorLayout.EndHorizontal( "TODO" );
			}

			return false;
		}


		public static void DrawDurabilityCompositionObject( ICEWorldBehaviour _component, DurabilityCompositionObject _composition, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _composition == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Durability Composition";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = "TODO";

			ICEEditorLayout.BeginHorizontal();

			if( IsEnabledFoldoutType( _type ) )
				EditorGUI.BeginDisabledGroup( _composition.Enabled == false );

				DrawObjectHeaderLine( _composition, GetSimpleFoldout( _type ), _title, _hint );	

				if( ICEEditorLayout.Button( "SAVE", "", ICEEditorStyle.CMDButtonDouble ) )
					ICEWorldIO.SaveDurabilityCompositionToFile( _composition, _component.name );
				if( ICEEditorLayout.Button( "LOAD", "", ICEEditorStyle.CMDButtonDouble ) )
					_composition.Copy( ICEWorldIO.LoadDurabilityCompositionFromFile( new DurabilityCompositionObject() ) );
				if( ICEEditorLayout.Button( "RESET", "", ICEEditorStyle.CMDButtonDouble ) )
					_composition.Reset();

				if( IsEnabledFoldoutType( _type ) )
				{
					EditorGUI.EndDisabledGroup();
					_composition.Enabled = ICEEditorLayout.ButtonEnabled( _composition.Enabled );
				}

			ICEEditorLayout.EndHorizontal( _help );

	
			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _composition ) )
				return;


			ICEEditorLayout.BeginHorizontal();
			ICEEditorLayout.Label( "Entity Influences", true );

			if( ICEEditorLayout.Button( "ADD", "", ICEEditorStyle.CMDButtonDouble ) )
			{
				_composition.AddInfluenceByKey( "NEW" );
			}


			ICEEditorLayout.EndHorizontal();

			for( int i = 0 ; i < _composition.Influences.Count ; i++ )
				if( DrawDurabilityInfluenceObject( _component, _composition, i ) )
					return;


			EditorGUILayout.Separator();

			ICEEditorLayout.BeginHorizontal();
			ICEEditorLayout.Label( "Entity Attributes", true );

			if( ICEEditorLayout.Button( "ADD", "", ICEEditorStyle.CMDButtonDouble ) )
			{
				_composition.AddAttributeByKey( "NEW" );
			}

			ICEEditorLayout.EndHorizontal();

			for( int i = 0 ; i < _composition.Attributes.Count ; i++ )
				if( DrawDurabilityAttributeObject( _component, _composition, i ) )
					return;





			
			EndObjectContent();
			// CONTENT END
		}

		public static void DrawCorpseObject( ICEWorldBehaviour _component, CorpseObject _corpse, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _corpse == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Corpse";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.CORPSE;

			DrawObjectHeader( _corpse, _type,_title, _hint, _help );			

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _corpse ) )
				return;

			EditorGUI.indentLevel++;
				ICEEditorLayout.BeginHorizontal();
					_corpse.CorpseReferencePrefab = (GameObject)EditorGUILayout.ObjectField( "Corpse Prefab", _corpse.CorpseReferencePrefab, typeof(GameObject), false );
					_corpse.UseCorpseScaling = ICEEditorLayout.ButtonCheck( "SCALE", "", _corpse.UseCorpseScaling, ICEEditorStyle.ButtonMiddle );
				ICEEditorLayout.EndHorizontal( Info.CORPSE_REFERENCE );
				_corpse.CorpseRemovingDelay = ICEEditorLayout.MaxDefaultSlider("Corpse Removing Delay (secs.)","Defines how long the corpse will be visible after dying.", _corpse.CorpseRemovingDelay, 0.5f, 0, ref _corpse.CorpseRemovingDelayMaximum, 0, Info.CORPSE_REMOVING_DELAY );
				EditorGUI.indentLevel++;
					_corpse.CorpseRemovingDelayVariance = ICEEditorLayout.DefaultSlider("Variance Multiplier","", _corpse.CorpseRemovingDelayVariance,0.025f, 0,1, 0.25f, Info.CORPSE_REMOVING_DELAY_VARIANCE );
				EditorGUI.indentLevel--;
			EditorGUI.indentLevel--;


			EndObjectContent();
			// CONTENT END
		}

		public static void DrawEntityStatusObject( ICEWorldBehaviour _component, EntityStatusObject _status, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _status == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Status";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.LIFESPAN;

			//LIFESPAN BEGIN
			DrawObjectHeader( _status, _type,_title, _hint, _help );			

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _status ) )
				return;

				DrawStatusLifespan( _status );
				//DrawDamageTransfer( _status );
				DrawInitialDurability( _status );

				EditorGUI.indentLevel++;
					_status.SetDurability( ICEEditorLayout.DefaultSlider( "Durability", "", _status.Durability, 0.0001f, 0, _status.InitialDurability, _status.InitialDurability, "" ) );
				EditorGUI.indentLevel--;

				

			EndObjectContent();
			// CONTENT END
		}

		public static void DrawStatusLifespan( EntityStatusObject _status )
		{
			ICEEditorLayout.BeginHorizontal();
				ICEEditorLayout.MinMaxGroupSimple( "Lifespan", "", ref _status.LifespanMin, ref _status.LifespanMax, 0, ref _status.LifespanDefaultMax, 0.25f, 40, "" );

				if( ICEEditorLayout.Button( "RND", "", ICEEditorStyle.CMDButtonDouble ) )
				{
					_status.LifespanMax = Random.Range( _status.LifespanMin, _status.LifespanDefaultMax );
					_status.LifespanMin = Random.Range( 0, _status.LifespanMax );
				}

				ICEEditorLayout.ButtonMinMaxDefault( ref _status.LifespanMin, ref _status.LifespanMax, 0, 0 );

				_status.UseLifespan = ICEEditorLayout.ButtonEnabled( _status.UseLifespan );
			ICEEditorLayout.EndHorizontal( Info.LIFESPAN );
		}

		/// <summary>
		/// Draws the initial durability.
		/// </summary>
		/// <param name="_status">Status.</param>
		public static void DrawInitialDurability( EntityStatusObject _status )
		{
			ICEEditorLayout.BeginHorizontal();

				if( ! Application.isPlaying )
					_status.SetInitialDurability( _status.InitialDurabilityMax );

				EditorGUI.BeginDisabledGroup( _status.IsDestructible == false );
					ICEEditorLayout.MinMaxGroupSimple( "Initial Durability (" + ( Mathf.Round( _status.InitialDurability / 0.01f ) * 0.01f ) + ")", "Defines the default physical integrity of the creature.", 
						ref _status.InitialDurabilityMin, 
						ref _status.InitialDurabilityMax,
						1, ref _status.InitialDurabilityMaximum, 1, 40, "" );

					ICEEditorLayout.ButtonMinMaxDefault( ref _status.InitialDurabilityMin, ref _status.InitialDurabilityMax, 100, 100 );
				EditorGUI.EndDisabledGroup();

				_status.IsDestructible = ICEEditorLayout.ButtonEnabled( _status.IsDestructible );
			ICEEditorLayout.EndHorizontal( Info.DURABILITY_INITIAL );
			//EditorGUILayout.Separator();

			EditorGUI.BeginDisabledGroup( _status.IsDestructible == false );

 
				//ICEEditorLayout.DrawProgressBar( "Durability (%)", _status.DurabilityInPercent, Info.DURABILITY_PERCENT );
			EditorGUI.EndDisabledGroup();
		}

		public static void DrawBodyPartObject( EntityBodyPartObject _part, EditorHeaderType _type, string _help = "", string _title = "", string _hint = ""  )
		{
			if( _part == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Body Part";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BODYPART;

			DrawObjectHeader( _part, _type,_title, _hint, _help );			

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _part ) )
				return;
			
			ICEEditorLayout.BeginHorizontal();

				_part.DamageMultiplier = ICEEditorLayout.MaxDefaultSlider( "Damage Transfer Multiplier", "", _part.DamageMultiplier, Init.DECIMAL_PRECISION, - _part.DamageMultiplierMaximum, ref _part.DamageMultiplierMaximum, 1 );

				_part.UseDamageTransfer = ICEEditorLayout.ButtonCheck( "TRANSFER", "Allows damage transfer for body parts", _part.UseDamageTransfer, ICEEditorStyle.ButtonMiddle );
			ICEEditorLayout.EndHorizontal( Info.BODYPART_DAMAGE_TRANSFER );

			EndObjectContent();
			// CONTENT END
		}

		/// <summary>
		/// Draws the lifespan object.
		/// </summary>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_lifespan">Lifespan.</param>
		/// <param name="_help">Help.</param>
		public static void DrawEntityLifespanObject( LifespanObject _lifespan, EditorHeaderType _type, string _help = "", string _title = "", string _hint = ""  )
		{
			if( _lifespan == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Lifespan";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.LIFESPAN;

			//LIFESPAN BEGIN
			DrawObjectHeader( _lifespan, _type,_title, _hint, _help );			

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _lifespan ) )
				return;

			ICEEditorLayout.BeginHorizontal();
			ICEEditorLayout.MinMaxGroupSimple( "Lifespan", "", ref _lifespan.LifespanMin, ref _lifespan.LifespanMax, 0, ref _lifespan.LifespanDefaultMax, 0.25f, 40, "" );

			if( ICEEditorLayout.Button( "RND", "", ICEEditorStyle.CMDButtonDouble ) )
			{
				_lifespan.LifespanMax = Random.Range( _lifespan.LifespanMin, _lifespan.LifespanDefaultMax );
				_lifespan.LifespanMin = Random.Range( 0, _lifespan.LifespanMax );
			}

			ICEEditorLayout.ButtonMinMaxDefault( ref _lifespan.LifespanMin, ref _lifespan.LifespanMax, 0, 0 );

			ICEEditorLayout.EndHorizontal();
			EndObjectContent();
			// CONTENT END
		}

		/// <summary>
		/// Draws the effect object.
		/// </summary>
		/// <returns>The effect object.</returns>
		/// <param name="_control">Control.</param>
		/// <param name="_effect">Effect.</param>
		/// <param name="_help">Help.</param>
		public static void DrawEffectObject( ICEWorldBehaviour _control, EffectObject _effect, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _effect == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Effect";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.EFFECT;

			DrawObjectHeader( _effect, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _effect ) )
				return;

			ICEEditorLayout.BeginHorizontal();
				_effect.ReferenceObject = (GameObject)EditorGUILayout.ObjectField( "Reference", _effect.ReferenceObject, typeof(GameObject), false);
				EditorGUI.BeginDisabledGroup( _effect.ReferenceObject == null );
					_effect.Detach = ICEEditorLayout.ButtonCheck( "DETACH", "Detaches the effect instance and will create further ones acording to the given interval" , _effect.Detach, ICEEditorStyle.ButtonMiddle ); 
				EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal( Info.EFFECT_REFERENCE );

			EditorGUI.BeginDisabledGroup( _effect.ReferenceObject == null );

				DrawImpulsTimerObject( _effect );

				_effect.MountPointName = ICEEditorLayout.TransformPopup( "Mount Point", "", _effect.MountPointName, _control.transform, true, Info.EFFECT_MOUNTPOINT );
				_effect.OffsetType = (RandomOffsetType)ICEEditorLayout.EnumPopup( "Offset Type","", _effect.OffsetType, Info.EFFECT_OFFSET_TYPE );
				EditorGUI.indentLevel++;
					if( _effect.OffsetType == RandomOffsetType.EXACT )
						_effect.Offset = ICEEditorLayout.OffsetGroup( "Offset", _effect.Offset, Info.EFFECT_OFFSET_POSITION );
					else 
						_effect.OffsetRadius = ICEEditorLayout.MaxDefaultSlider( "Offset Radius", "", _effect.OffsetRadius, 0.25f, 0, ref _effect.OffsetRadiusMaximum, 0, Info.EFFECT_OFFSET_RADIUS );
				
					_effect.Rotation.eulerAngles = ICEEditorLayout.EulerGroup( "Rotation", _effect.Rotation.eulerAngles, Info.EFFECT_OFFSET_RADIUS );
				EditorGUI.indentLevel--;

			EditorGUI.EndDisabledGroup();

			EndObjectContent();
			// CONTENT END
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
			if( _timer.ImpulseSequenceBreakLengthMaximum == 0 )
				_timer.ImpulseSequenceBreakLengthMaximum = 10;

			ICEEditorLayout.BeginHorizontal();
			EditorGUI.BeginDisabledGroup( _timer.UseEnd == true );
				_timer.InitialImpulsTime = ICEEditorLayout.MaxDefaultSlider( "Start Time (secs.)", "Time in seconds of the first impulse.", _timer.InitialImpulsTime , Init.DECIMAL_PRECISION_TIMER, 0, ref _timer.InitialImpulsTimeMaximum, 0 );

				_timer.UseInterval = ICEEditorLayout.ButtonCheck( "INT", "", _timer.UseInterval, ICEEditorStyle.CMDButtonDouble );

			EditorGUI.EndDisabledGroup();

				EditorGUI.BeginDisabledGroup( _timer.UseInterval == true );
					_timer.UseEnd = ICEEditorLayout.ButtonCheck( "END", "", _timer.UseEnd, ICEEditorStyle.CMDButtonDouble );
				EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal( Info.IMPULSE_TIMER_TIME );

			if( _timer.UseInterval )
			{

				ICEEditorLayout.RandomMinMaxGroupExt( "Impulse Interval (secs.)", "", ref _timer.ImpulseIntervalMin, ref _timer.ImpulseIntervalMax, 0, ref _timer.ImpulseIntervalMaximum,0,0,30, 0.01f, Info.IMPULSE_TIMER_INTERVAL );

				EditorGUI.indentLevel++;

				float _limits_min = _timer.ImpulseLimitMin;
				float _limits_max = _timer.ImpulseLimitMax;
				float _limits_maximum = _timer.ImpulseLimitMaximum;
				ICEEditorLayout.RandomMinMaxGroupExt( "Impulse Limit", "", ref _limits_min, ref _limits_max, 0, ref _limits_maximum,0,0,30, 1, Info.IMPULSE_TIMER_LIMITS );
				_timer.ImpulseLimitMin = (int)_limits_min;
				_timer.ImpulseLimitMax = (int)_limits_max;
				_timer.ImpulseLimitMaximum = (int)_limits_maximum;

				if( Mathf.Max( _timer.ImpulseIntervalMin, _timer.ImpulseIntervalMax ) > 0 )
				{
					float _send_limits_min = _timer.ImpulseSequenceLimitMin;
					float _send_limits_max = _timer.ImpulseSequenceLimitMax;
					float _send_limits_maximum = _timer.ImpulseSequenceLimitMaximum;
					ICEEditorLayout.RandomMinMaxGroupExt( "Sequence Limit", "", ref _send_limits_min, ref _send_limits_max, 0, ref _send_limits_maximum,0,0,30, 1, Info.IMPULSE_TIMER_SEQUENCE_LIMITS );
					_timer.ImpulseSequenceLimitMin = (int)_send_limits_min;
					_timer.ImpulseSequenceLimitMax = (int)_send_limits_max;
					_timer.ImpulseSequenceLimitMaximum = (int)_send_limits_maximum;

					EditorGUI.BeginDisabledGroup( _timer.ImpulseSequenceLimitMax == 0 );
					ICEEditorLayout.RandomMinMaxGroupExt( "Break Length (secs.)", "", ref _timer.ImpulseSequenceBreakLengthMin, ref _timer.ImpulseSequenceBreakLengthMax, 0, ref _timer.ImpulseSequenceBreakLengthMaximum,2,5,30, 0.01f, Info.IMPULSE_TIMER_SEQUENCE_BREAK_LENGTH );
					EditorGUI.EndDisabledGroup();

				}

				EditorGUI.indentLevel--;
			}
		}

		public static void DrawUnderwaterCameraEffect( ICEWorldBehaviour _component, UnderwaterCameraEffect _underwater, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _underwater == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Underwater Effect";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.EVENTS;

			DrawObjectHeader( _underwater, _type, _title, _hint );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _underwater ) )
				return;

			_underwater.UseWaterZone = ICEEditorLayout.Toggle( "Use Water Zone", "", _underwater.UseWaterZone ); 

			_underwater.WaterLevel = ICEEditorLayout.MaxDefaultSlider( "Water Level", "", _underwater.WaterLevel, 0.25f, - _underwater.WaterLevelMaximum, ref _underwater.WaterLevelMaximum, 0, "" );

			_underwater.FogEnabled = ICEEditorLayout.Toggle( "Fog Enabled", "", _underwater.FogEnabled ); 

			_underwater.FogColor = ICEEditorLayout.ColorField( "Fog Color", "", _underwater.FogColor, "" ); 

			_underwater.FogDensity = ICEEditorLayout.DefaultSlider( "Fog Density", "", _underwater.FogDensity, 0.001f, 0, 1, 0.04f, "" );

			_underwater.UnderwaterBackgroundColor = ICEEditorLayout.ColorField( "Background Color", "", _underwater.UnderwaterBackgroundColor, "" ); 

			EndObjectContent();
			// CONTENT END
		}

		public static void DrawBehaviourEventObject( ICEWorldBehaviour _component, BehaviourEventsObject _events, BehaviourEventObject _event, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _event == null || _events == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = ( string.IsNullOrEmpty( _event.Event.FunctionName ) ? "Event":_event.Event.FunctionName );
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.EVENTS;

			ICEEditorLayout.BeginHorizontal();

				if( IsEnabledFoldoutType( _type ) )
					EditorGUI.BeginDisabledGroup( _event.Enabled == false );
		
				DrawObjectHeaderLine( _event, GetSimpleFoldout( _type ), _title, _hint );

				if( ICEEditorLayout.Button( "COPY", "", ICEEditorStyle.CMDButtonDouble ) )
					_events.Events.Add( new BehaviourEventObject( _event ) );

				if( ICEEditorLayout.Button( "DEL", "", ICEEditorStyle.CMDButtonDouble ) )
				{
					_events.Events.Remove( _event );
					return;
				}

				if( IsEnabledFoldoutType( _type ) )
				{
					EditorGUI.EndDisabledGroup();
					_event.Enabled = ICEEditorLayout.ButtonEnabled( _event.Enabled );
				}

	

			ICEEditorLayout.EndHorizontal( _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _event ) )
				return;

				DrawBehaviourEvent( _component, _event.Event, "", "" );
				//EditorGUILayout.Separator();
				DrawImpulsTimerObject( _event );

			EndObjectContent();
			// CONTENT END
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
					DrawBehaviourEventObject( _component, _events, _events.Events[i], EditorHeaderType.FOLDOUT_ENABLED_BOLD );


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
				_title = "Function Name";
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
