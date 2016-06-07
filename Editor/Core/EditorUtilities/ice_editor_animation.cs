﻿// ##############################################################################
//
// ice_editor_animation.cs | AnimationEditor
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
	public class AnimationEditor : WorldObjectEditor
	{	
		public static string AnimationPopup( Animation _animation, string _name, string _title = "", string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();
				_name = AnimationPopupBase( _animation, _name, _title );
			ICEEditorLayout.EndHorizontal( _help );

			return _name;
		}

		public static string AnimationPopupBase( Animation _animation, string _name, string _title = "" )
		{
			if( _animation == null )
				return "";
			
			int _count = AnimationTools.GetAnimationClipCount( _animation );
			string[] _animation_names = new string[ _count ];
			int[] _animation_index = new int[ _count];

			int _i = 0;
			int _selected = 0;
			if( _count > 0 )
			{
				foreach (AnimationState _state in _animation )
				{
					if( _state == null || _i >= _count )
						continue;

					_animation_index[_i] = _i;
					_animation_names[_i] = _state.name;

					if( _name == _animation_names[_i] )
						_selected = _i;

					_i++;
				}
			}

			if( _title == "" )
				_title = "Animation";

			_selected = (int)EditorGUILayout.IntPopup( _title , _selected, _animation_names,_animation_index);
			//new GUIContent( _title , "Animation name and length in seconds" )

			_selected = (int)ICEEditorLayout.PlusMinusGroup( _selected, 1, ICEEditorStyle.CMDButtonDouble );

			if( _selected < 0 )
				_selected = 0;
			else if( _selected >= _count - 1 )
				_selected = _count - 1;

			return _animation_names[_selected];
		}

		private static int AnimatorIntPopup( Animator _animator, int _selected, string _title = "" )
		{
			ICEEditorLayout.BeginHorizontal();
				_selected = AnimatorIntPopupBase( _animator, _selected, _title );
			ICEEditorLayout.EndHorizontal();

			return _selected;
		}

		public static int AnimatorIntPopupBase( Animator _animator, int _selected, string _title = "" )
		{
			if( _animator == null || _animator.runtimeAnimatorController == null ) 
				return -1;


			AnimationClip[] _clips = AnimationTools.GetAnimationClips( _animator );

			string[] _animation_names = new string[ _clips.Length ];//_animator.runtimeAnimatorController.animationClips.Length ];
			int[] _animation_index = new int[  _clips.Length ];//_animator.runtimeAnimatorController.animationClips.Length ];

			int i = 0;							
			foreach( AnimationClip _clip in _clips )// _animator.runtimeAnimatorController.animationClips )
			{
				_animation_index[i] = i;
				_animation_names[i] = _clip.name;

				i++;
			}

			if( _title == "" )
				_title = "Animation";

			_selected = (int)EditorGUILayout.IntPopup( _title , _selected, _animation_names,_animation_index);

			_selected = (int)ICEEditorLayout.PlusMinusGroup( _selected, 1, ICEEditorStyle.CMDButtonDouble );

			if( _selected < 0 )
				_selected = 0;
			else if( _selected >= _clips.Length - 1 )
				_selected = _clips.Length - 1;

			return _selected;
		}

		public static int AnimationIntPopup( Animation _animation, int _selected, string _title = "" )
		{
			ICEEditorLayout.BeginHorizontal();
			_selected = AnimationIntPopupBase( _animation, _selected, _title );
			ICEEditorLayout.EndHorizontal();

			return _selected;
		}

		public static int AnimationIntPopupBase( Animation _animation, int _selected, string _title = "" )
		{
			if( _animation == null )
				return 0;



			string[] _animation_names = new string[ AnimationTools.GetAnimationClipCount( _animation ) ];
			int[] _animation_index = new int[ AnimationTools.GetAnimationClipCount( _animation ) ];

			int i = 0;	
			foreach (AnimationState _animation_state in _animation )
			{
				_animation_index[i] = i;
				_animation_names[i] = _animation_state.name;

				i++;
			}
			if( _title == "" )
				_title = "Animation";

			_selected = (int)EditorGUILayout.IntPopup( _title , _selected, _animation_names,_animation_index);
			//new GUIContent( _title , "Animation name and length in seconds" )

			_selected = (int)ICEEditorLayout.PlusMinusGroup( _selected, 1, ICEEditorStyle.CMDButtonDouble );

			if( _selected < 0 )
				_selected = 0;
			else if( _selected >= AnimationTools.GetAnimationClipCount( _animation ) - 1 )
				_selected = AnimationTools.GetAnimationClipCount( _animation )  - 1;

			return _selected;
		}


		public static void DrawAnimationDataObject( ICEWorldBehaviour _component, AnimationDataObject _anim, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _anim == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Animation";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.ANIMATION;

			//--------------------------------------------------
			// ANIMATION
			//--------------------------------------------------

			bool _enabled = _anim.Enabled;
			//_anim.Enabled = (_anim.InterfaceType != AnimationInterfaceType.NONE?true:false);

			ICEEditorLayout.BeginHorizontal();

			if( WorldObjectEditor.IsEnabledFoldoutType( _type ) )
			{
				EditorGUI.BeginDisabledGroup( _anim.Enabled == false );
			}		

			WorldObjectEditor.DrawObjectHeaderLine( _anim, WorldObjectEditor.GetSimpleFoldout( _type ), _title , _hint );		
			GUILayout.FlexibleSpace();
			if( _anim.Enabled )
				_anim.AllowInterfaceSelector = ICEEditorLayout.ButtonCheck( "SELECTOR", "", _anim.AllowInterfaceSelector, ICEEditorStyle.ButtonMiddle );

			if( WorldObjectEditor.IsEnabledFoldoutType( _type ) )
			{
				EditorGUI.EndDisabledGroup();
				_anim.Enabled = ICEEditorLayout.ButtonEnabled( _anim.Enabled );
			}
			ICEEditorLayout.EndHorizontal( _help );

			if( _enabled != _anim.Enabled && _anim.Enabled == true )
				_anim.Foldout = true;



			// CONTENT BEGIN
			if( WorldObjectEditor.BeginObjectContentOrReturn( _type, _anim ) )
				return;

			if( ( _component.GetComponentInChildren<Animator>() != null && _component.GetComponentInChildren<Animation>() != null ) || _anim.AllowInterfaceSelector )
			{
				if( _anim.InterfaceType == AnimationInterfaceType.NONE && _component.GetComponentInChildren<Animator>() != null && _component.GetComponentInChildren<Animator>().runtimeAnimatorController != null )
					_anim.InterfaceType = AnimationInterfaceType.MECANIM;
				else if( _anim.InterfaceType == AnimationInterfaceType.NONE )
					_anim.InterfaceType = AnimationInterfaceType.LEGACY;

				_help = Info.ANIMATION_NONE;
				if( _anim.InterfaceType == AnimationInterfaceType.MECANIM )
					_help = Info.ANIMATION_ANIMATOR;
				else if( _anim.InterfaceType == AnimationInterfaceType.LEGACY )
					_help = Info.ANIMATION_ANIMATION;
				else if( _anim.InterfaceType == AnimationInterfaceType.CLIP )
					_help = Info.ANIMATION_CLIP;
				else if( _anim.InterfaceType == AnimationInterfaceType.CUSTOM )
					_help = Info.ANIMATION_CUSTOM;

				_anim.InterfaceType = (AnimationInterfaceType)ICEEditorLayout.EnumPopup( "Interface","", _anim.InterfaceType , _help );
			}
			else if( _component.GetComponentInChildren<Animator>() )
				_anim.InterfaceType = AnimationInterfaceType.MECANIM;
			else 
				_anim.InterfaceType = AnimationInterfaceType.LEGACY;

			if( _anim.InterfaceType != AnimationInterfaceType.NONE )
			{
				if( _anim.InterfaceType == AnimationInterfaceType.MECANIM )
					_anim.Animator = DrawBehaviourAnimationAnimatorData( _component, _anim.Animator );
				else if( _anim.InterfaceType == AnimationInterfaceType.LEGACY )
					_anim.Animation = DrawBehaviourAnimationAnimationData( _component,_anim.Animation );
				else if( _anim.InterfaceType == AnimationInterfaceType.CLIP )
					_anim.Clip = DrawBehaviourAnimationAnimationClipData( _component, _anim.Clip );
				else if( _anim.InterfaceType == AnimationInterfaceType.CUSTOM )
					Info.Help ( _help );

				if( _anim.InterfaceType == AnimationInterfaceType.MECANIM && _anim.Animator.Type == AnimatorControlType.DIRECT )
					DrawAnimationEventData( _component, _anim.Events, AnimationTools.GetAnimationClipByAnimatorAndName( AnimationTools.TryGetAnimatorComponent( _component.gameObject ), _anim.GetAnimationName() ), EditorHeaderType.FOLDOUT_ENABLED );
				else if( _anim.InterfaceType == AnimationInterfaceType.LEGACY )
					DrawAnimationEventData( _component, _anim.Events, AnimationTools.GetAnimationClipByName( AnimationTools.TryGetAnimationComponent( _component.gameObject ), _anim.GetAnimationName() ), EditorHeaderType.FOLDOUT_ENABLED );
				else if( _anim.InterfaceType == AnimationInterfaceType.CLIP )
					DrawAnimationEventData( _component, _anim.Events, _anim.Clip.Clip, EditorHeaderType.FOLDOUT_ENABLED );				
			}
			else
				Info.Help ( Info.ANIMATION_NONE );

			WorldObjectEditor.EndObjectContent();
			// CONTENT END
		}


		private static AnimationClipInterface DrawBehaviourAnimationAnimationClipData( ICEWorldBehaviour _control, AnimationClipInterface _clip )
		{
			Animation m_animation = _control.GetComponentInChildren<Animation>();

			if( m_animation != null && m_animation.enabled == true )
			{
				Info.Help ( Info.ANIMATION_CLIP );

				_clip.Clip = (AnimationClip)EditorGUILayout.ObjectField( "Animation Clip", _clip.Clip, typeof(AnimationClip), false );

				if( _clip.Clip != null )
				{
					ICEEditorLayout.Label( "Length", "Animation length in seconds. ", _clip.Clip.length.ToString() + " secs." );
					ICEEditorLayout.Label( "Frame Rate", "This is the frame rate that was used in the animation program you used to create the animation or model.", _clip.Clip.frameRate.ToString() + " secs." );

					_clip.Clip.legacy = ICEEditorLayout.Toggle( "Legacy", "Set to true to use it here with the Legacy Animation component",_clip.Clip.legacy );
					_clip.Clip.wrapMode = (WrapMode)ICEEditorLayout.EnumPopup( "WarpMode", "Determines how time is treated outside of the keyframed range of an AnimationClip or AnimationCurve." , _clip.Clip.wrapMode );

					bool _toggle = false;
					_clip.TransitionDuration = ICEEditorLayout.AutoSlider( "Transition Duration", "", _clip.TransitionDuration, 0.01f, 0, 10, ref _toggle, 0.5f  );

					if( _toggle )
						_clip.TransitionDuration = _clip.Clip.length / 3;

					_toggle = false;
				}
			}
			else
			{
				EditorGUILayout.HelpBox( "Check your Animation Component", MessageType.Warning ); 
			}

			return _clip;
		}

		public static void DrawAnimationEventData( ICEWorldBehaviour _component, AnimationEventsObject _container, AnimationClip _clip, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _container == null || _clip == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Events";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.ANIMATION_EVENTS;

			ICEEditorLayout.BeginHorizontal();

			if( IsEnabledFoldoutType( _type ) )
			{
				EditorGUI.BeginDisabledGroup( _container.Enabled == false );
			}			

			DrawObjectHeaderLine( _container, GetSimpleFoldout( _type ), _title, _hint );

			EditorGUI.BeginDisabledGroup( _container.Enabled == false );
				if( ICEEditorLayout.Button( "ADD", "", ICEEditorStyle.CMDButtonDouble ) )
					_container.Events.Add( new AnimationEventObject() );

				EditorGUI.BeginDisabledGroup( _container.Events.Count == 0 );

					if( ICEEditorLayout.Button( "RES", "", ICEEditorStyle.CMDButtonDouble ) )
					{
						_container.Events.Clear();
						AnimationUtility.SetAnimationEvents( _clip, _container.GetAnimationEvents() );
					}
			
				EditorGUI.EndDisabledGroup();
			EditorGUI.EndDisabledGroup();				

			if( IsEnabledFoldoutType( _type ) )
			{
				EditorGUI.EndDisabledGroup();
				_container.Enabled = ICEEditorLayout.ButtonEnabled( _container.Enabled );
			}
			ICEEditorLayout.EndHorizontal( _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _container ) )
				return;

			AnimationEvent[] _events = AnimationUtility.GetAnimationEvents( _clip );

			_container.UpdateAnimationEvents( _events );

			//foreach( AnimationEventObject _data in _container.Events )
			for( int i = 0 ; i < _container.Events.Count ; i++ )
			{
				AnimationEventObject _data = _container.Events[i];
				ICEEditorLayout.BeginHorizontal();

					MethodDataContainer _method = new MethodDataContainer();
					_method.ComponentName = _component.name;
					_method.MethodName = _data.MethodName;
					//_method.ParameterType = SystemTools.GetMethodParameterType( _control, _event.functionName );

					_method = WorldPopups.MethodPopupLine( _component, _method, _component.PublicMethods, ref _data.UseCustomFunction, "", "Event #" + i, "" );

					_data.MethodName = _method.MethodName;

					bool _active = ICEEditorLayout.ButtonCheck( "ACTIVE", "", _data.IsActive , ICEEditorStyle.ButtonMiddle );

					if( _active != _data.IsActive )
					{
						_data.IsActive = _active;
						AnimationUtility.SetAnimationEvents( _clip, _container.GetAnimationEvents() );						
					}

					if( ICEEditorLayout.Button( "X", "", ICEEditorStyle.CMDButton ) )
					{
						_container.Events.Remove( _data );
						AnimationUtility.SetAnimationEvents( _clip, _container.GetAnimationEvents() );
						return;
					}

				ICEEditorLayout.EndHorizontal( Info.ANIMATION_EVENTS_METHOD );

				EditorGUI.indentLevel++;

					if( _method.ParameterType == MethodParameterType.Integer )
						_data.ParameterInteger = ICEEditorLayout.Integer( "Parameter Integer", "", _data.ParameterInteger, Info.METHOD_PARAMETER_INTEGER );
					else if( _method.ParameterType == MethodParameterType.Float )
						_data.ParameterFloat = ICEEditorLayout.Float( "Parameter Float", "", _data.ParameterFloat, Info.METHOD_PARAMETER_FLOAT );
					else if( _method.ParameterType == MethodParameterType.String )
						_data.ParameterString = ICEEditorLayout.Text( "Parameter String", "", _data.ParameterString, Info.METHOD_PARAMETER_STRING );

					_data.Time = ICEEditorLayout.Slider( "Time", "The time at which the event will be fired off.", _data.Time, 0.0001f, 0, _clip.length, Info.ANIMATION_EVENTS_TIME );

				EditorGUI.indentLevel--;
			}

			if( _container.UpdateRequired( _events ) )
				AnimationUtility.SetAnimationEvents( _clip, _container.GetAnimationEvents() );

			EndObjectContent();
			// CONTENT END
		}

		private static AnimationInterface DrawBehaviourAnimationAnimationData( ICEWorldBehaviour _control, AnimationInterface _animation_data )
		{
			Animation _animation = _control.GetComponentInChildren<Animation>();

			if( _animation != null && _animation.enabled == true )
			{
				Info.Help ( Info.ANIMATION_ANIMATION );

				if( EditorApplication.isPlaying )
				{
					EditorGUILayout.LabelField("Name", _animation_data.Name );
				}
				else
				{
					string _animation_name = AnimationPopup( _animation, _animation_data.Name, "Animation (" + _animation_data.Length.ToString() + " secs.)", Info.ANIMATION_NAME );
					if( _animation_name != _animation_data.Name )
					{
						AnimationState _state = AnimationTools.GetAnimationStateByName( _control.gameObject, _animation_name );					
						if( _state != null )
						{				
							if( _state.clip != null )
								_state.clip.legacy = true;

							_animation_data.TransitionDuration = 0.5f;
							_animation_data.wrapMode = _state.wrapMode;
							_animation_data.DefaultWrapMode = _state.wrapMode;
							_animation_data.Speed =_state.speed;
							_animation_data.DefaultSpeed = _state.speed;
							_animation_data.Name = _state.name;
							_animation_data.Length = _state.length;
						}
					}
				}

				EditorGUI.indentLevel++;
					_animation_data.wrapMode = (WrapMode)ICEEditorLayout.EnumPopup( "WrapMode (" + _animation_data.DefaultWrapMode + ")", "Determines how time is treated outside of the keyframed range of an AnimationClip or AnimationCurve.", _animation_data.wrapMode, Info.ANIMATION_WRAP_MODE );
					_animation_data.Speed = ICEEditorLayout.AutoSlider( "Speed (" + _animation_data.DefaultSpeed + ")", "The playback speed of the animation. 1 is normal playback speed. A negative playback speed will play the animation backwards. Adapt this value to your movement settings.", _animation_data.Speed, 0.01f, -10, 10, ref _animation_data.AutoSpeed, 1, Info.ANIMATION_SPEED );
					_animation_data.TransitionDuration = ICEEditorLayout.AutoSlider( "Transition Duration", "", _animation_data.TransitionDuration, 0.01f, 0, 10, ref _animation_data.AutoTransitionDuration, 0.5f, Info.ANIMATION_TRANSITION );

					if( _animation_data.AutoTransitionDuration )
						_animation_data.TransitionDuration = _animation_data.Length / 3;

				EditorGUI.indentLevel--;

			}
			else
			{
				EditorGUILayout.HelpBox( "Check your Animation Component", MessageType.Warning ); 
			}

			return _animation_data;
		}


		private static List<AnimatorParameterObject> DrawBehaviourAnimationAnimatorParameterData( ICEWorldBehaviour _control, List<AnimatorParameterObject> _parameter_list )
		{
			Animator _animator = _control.GetComponentInChildren<Animator>();

			for( int _i = 0 ; _i < _parameter_list.Count; _i++ )
			{
				AnimatorParameterObject _parameter = _parameter_list[_i];
				var indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;


				// PARAMETER LINE BEGIN
				ICEEditorLayout.BeginHorizontal();

				EditorGUILayout.LabelField("",GUILayout.Width( 15 * indent ) );

				EditorGUI.BeginDisabledGroup( _parameter.Enabled == false );
				AnimatorControllerParameter _data = ICEEditorLayout.AnimatorParametersPopupData( _animator, _parameter.Name, GUILayout.MinWidth( 65 ), GUILayout.MaxWidth( 200 ) );

				if( _data != null )
				{
					_parameter.Name = _data.name;
					_parameter.Type = _data.type;

					if( _parameter.Type == AnimatorControllerParameterType.Bool )
					{
						if( _parameter.UseDynamicValue )
							_parameter.BooleanValueType = (DynamicBooleanValueType)EditorGUILayout.EnumPopup( _parameter.BooleanValueType, GUILayout.MinWidth( 65 ) );
						else
							_parameter.BooleanValue = ICEEditorLayout.ButtonCheck( (_parameter.BooleanValue?"TRUE":"FALSE" ),"", _parameter.BooleanValue, ICEEditorStyle.ButtonFlex, GUILayout.MinWidth( 65 ) );
					}
					else if( _parameter.Type == AnimatorControllerParameterType.Int )
					{
						if( _parameter.UseDynamicValue )
							_parameter.IntegerValueType = (DynamicIntegerValueType)EditorGUILayout.EnumPopup( _parameter.IntegerValueType );
						else
							_parameter.IntegerValue = EditorGUILayout.IntField( _parameter.IntegerValue, GUILayout.MinWidth( 65 ) );
					}
					else if( _parameter.Type == AnimatorControllerParameterType.Float )
					{
						if( _parameter.UseDynamicValue )
							_parameter.FloatValueType = (DynamicFloatValueType)EditorGUILayout.EnumPopup( _parameter.FloatValueType );
						else
							_parameter.FloatValue = EditorGUILayout.FloatField( _parameter.FloatValue, GUILayout.MinWidth( 65 ) );
					}
					else if( _parameter.Type == AnimatorControllerParameterType.Trigger )
					{
					}

					_parameter.UseDynamicValue = ICEEditorLayout.ButtonCheck( "DYN", "Use dynamic value", _parameter.UseDynamicValue, ICEEditorStyle.CMDButtonDouble );
				}
				EditorGUI.EndDisabledGroup();

				_parameter.Enabled = ICEEditorLayout.ButtonCheck( "ENABLED", "Use dynamic value", _parameter.Enabled, ICEEditorStyle.ButtonMiddle );

				if( ICEEditorLayout.Button( "x", "", ICEEditorStyle.CMDButton ) ) 
				{
					_parameter_list.RemoveAt(_i);
					return _parameter_list;
				}

				ICEEditorLayout.EndHorizontal(); // Info.GetTargetSelectionExpressionTypeHint( _condition.ExpressionType )
				// PARAMETER LINE END


				EditorGUI.indentLevel = indent;



			}

			// ADD CONDITION LINE BEGIN
			ICEEditorLayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			EditorGUILayout.LabelField( new GUIContent( "Add Parameter ", "" ), EditorStyles.wordWrappedMiniLabel );


			//GUILayout.FlexibleSpace();
			if (GUILayout.Button( new GUIContent( "ADD", "Add parameter" ) , ICEEditorStyle.CMDButtonDouble ) )
				_parameter_list.Add( new AnimatorParameterObject() );

			if (GUILayout.Button( new GUIContent( "DEL", "Removes all parameter" ) , ICEEditorStyle.CMDButtonDouble ) )
			{
				_parameter_list.Clear();
				return _parameter_list;
			}

			ICEEditorLayout.EndHorizontal();

			return _parameter_list;
		}

		private static AnimatorInterface DrawBehaviourAnimationAnimatorData( ICEWorldBehaviour _control, AnimatorInterface _animator_data )
		{
			Animator m_animator = _control.GetComponentInChildren<Animator>();

			if( m_animator != null && m_animator.enabled == true && m_animator.runtimeAnimatorController != null && m_animator.avatar != null )
			{
				if( ! EditorApplication.isPlaying )
				{
					string _help_control_type = Info.ANIMATION_ANIMATOR_CONTROL_TYPE_DIRECT;

					if( _animator_data.Type == AnimatorControlType.ADVANCED )
						_help_control_type = Info.ANIMATION_ANIMATOR_CONTROL_TYPE_ADVANCED;

					_animator_data.Type = (AnimatorControlType)ICEEditorLayout.EnumPopup( "Control Type", "", _animator_data.Type, _help_control_type );

					if( _animator_data.Type == AnimatorControlType.DIRECT )
					{
						_animator_data.Index = AnimatorIntPopup( m_animator, _animator_data.Index );

						if( AnimationTools.GetAnimationClipCount( m_animator ) == 0 )
						{
							Info.Warning( Info.ANIMATION_ANIMATOR_ERROR_NO_CLIPS );
						}
						else
						{
							AnimationClip _animation_clip = AnimationTools.GetAnimationClipByIndex( m_animator,_animator_data.Index );

							if( _animation_clip != null )
							{				
								if( _animator_data.Name != _animation_clip.name )
									_animator_data.Init();

								_animation_clip.wrapMode = (WrapMode)ICEEditorLayout.EnumPopup( "WarpMode", "Determines how time is treated outside of the keyframed range of an AnimationClip or AnimationCurve.", _animation_clip.wrapMode );
								_animation_clip.legacy = false;

								_animator_data.StateName = AnimationTools.GetAnimatorStateName( m_animator, _animation_clip.name );
								_animator_data.Name = _animation_clip.name;
								_animator_data.Length = _animation_clip.length;
								_animator_data.DefaultWrapMode = _animation_clip.wrapMode;

								_animator_data.Speed = ICEEditorLayout.AutoSlider("Speed", "", _animator_data.Speed, 0.01f, -5, 5, ref _animator_data.AutoSpeed, 1 );

								bool _toggle = false;
								_animator_data.TransitionDuration = ICEEditorLayout.AutoSlider( "Transition Duration", "", _animator_data.TransitionDuration, 0.01f, 0, 10, ref _toggle, 0.5f  );
								ICEEditorLayout.Label( _animator_data.StateName, false );
								if( _toggle )
									_animator_data.TransitionDuration = _animator_data.Length / 3;						
								_toggle = false;
							
							}
						}



					}
					else if( _animator_data.Type == AnimatorControlType.ADVANCED )
					{
						_animator_data.ApplyRootMotion = ICEEditorLayout.Toggle( "Apply Root Motion", "", _animator_data.ApplyRootMotion );

						_animator_data.Parameters = DrawBehaviourAnimationAnimatorParameterData( _control, _animator_data.Parameters );

					}

				}
				else
				{

					if( _animator_data.Type == AnimatorControlType.DIRECT )
					{
						ICEEditorLayout.Label( "Name", "Animation name.", _animator_data.Name );
						ICEEditorLayout.Label( "Length", "Animation length in seconds.", _animator_data.Length.ToString() + " secs." );
						ICEEditorLayout.Label( "WrapMode", "Determines how time is treated outside of the keyframed range of an AnimationClip.", _animator_data.DefaultWrapMode.ToString() );

						_animator_data.Speed = ICEEditorLayout.AutoSlider("Speed", "", _animator_data.Speed, 0.01f, -5, 5, ref _animator_data.AutoSpeed, 1 );


						bool _toggle = false;
						_animator_data.TransitionDuration = ICEEditorLayout.AutoSlider( "Transition Duration", "", _animator_data.TransitionDuration, 0.01f, 0, 10, ref _toggle, 0.5f  );

						if( _toggle )
							_animator_data.TransitionDuration = _animator_data.Length / 3;						
						_toggle = false;

				
						
					}
					else if( _animator_data.Type == AnimatorControlType.ADVANCED )
					{
						foreach( AnimatorParameterObject _parameter in _animator_data.Parameters )
						{
							switch( _parameter.Type )
							{
							case AnimatorControllerParameterType.Bool:
								EditorGUILayout.LabelField( _parameter.Name, "(BOOLEAN) " +  _parameter.BooleanValue.ToString() );
								break;
							case AnimatorControllerParameterType.Int:
								EditorGUILayout.LabelField( _parameter.Name, "(INTEGER) " +  _parameter.IntegerValue.ToString() );
								break;
							case AnimatorControllerParameterType.Float:
								EditorGUILayout.LabelField( _parameter.Name, "(FLOAT) " + ( _parameter.UseDynamicValue?_control.GetDynamicFloatValue( _parameter.FloatValueType ):_parameter.FloatValue ) );
								break;
							case AnimatorControllerParameterType.Trigger:
								EditorGUILayout.LabelField( _parameter.Name, "(TRIGGER)" );
								break;
							}
						}
					}
				}
			}
			else 
			{
				if( m_animator != null )
				{
					if( m_animator.enabled == false )
					{
						EditorGUILayout.HelpBox( "Sorry, your Animator Component is disabled!", MessageType.Warning ); 

						ICEEditorLayout.BeginHorizontal();
						EditorGUILayout.LabelField( "Enable Animator Component", EditorStyles.boldLabel);
						if (GUILayout.Button( new GUIContent("ENABLED", "Enable Animator Component"), ICEEditorStyle.ButtonMiddle ) )
							m_animator.enabled = true;
						ICEEditorLayout.EndHorizontal();
					}
					else if( m_animator.runtimeAnimatorController == null )
					{
						EditorGUILayout.HelpBox( "Sorry, there is no Runtime Animator Controller!", MessageType.Warning ); 

						ICEEditorLayout.BeginHorizontal();
						EditorGUILayout.LabelField( "Enable Animator Component", EditorStyles.boldLabel);
						if (GUILayout.Button( new GUIContent("ENABLED", "Enable Animator Component"), ICEEditorStyle.ButtonMiddle ) )
							m_animator.enabled = true;
						ICEEditorLayout.EndHorizontal();
					}
					else if( m_animator.avatar == null )
					{
						EditorGUILayout.HelpBox( "Sorry, there is no Avatar asigned to your Animator Component!", MessageType.Warning ); 

						ICEEditorLayout.BeginHorizontal();
						EditorGUILayout.LabelField( "Enable Animator Component", EditorStyles.boldLabel);
						if (GUILayout.Button( new GUIContent("ENABLED", "Enable Animator Component"), ICEEditorStyle.ButtonMiddle ) )
							m_animator.enabled = true;
						ICEEditorLayout.EndHorizontal();
					}

				}
				else
				{

					EditorGUILayout.HelpBox( "Sorry, there is no Animator Component!", MessageType.Warning ); 

					ICEEditorLayout.BeginHorizontal();
					EditorGUILayout.LabelField( "Add Animator Component", EditorStyles.boldLabel);
					if (GUILayout.Button( new GUIContent("ADD", "Add Animator Component"), ICEEditorStyle.ButtonMiddle ) )
						m_animator = _control.gameObject.AddComponent<Animator>();
					ICEEditorLayout.EndHorizontal();
				}

			}
			return _animator_data;
		}

	}
}