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
	public static class AnimationEditor
	{	
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

			if( DataObjectEditor.IsEnabledFoldoutType( _type ) )
			{
				EditorGUI.BeginDisabledGroup( _anim.Enabled == false );
			}		

			DataObjectEditor.DrawObjectHeaderLine( _anim, DataObjectEditor.GetSimpleFoldout( _type ), _title , _hint );		
			GUILayout.FlexibleSpace();
			if( _anim.Enabled )
				_anim.AllowInterfaceSelector = ICEEditorLayout.ButtonCheck( "SELECTOR", "", _anim.AllowInterfaceSelector, ICEEditorStyle.ButtonMiddle );

			if( DataObjectEditor.IsEnabledFoldoutType( _type ) )
			{
				EditorGUI.EndDisabledGroup();
				_anim.Enabled = ICEEditorLayout.ButtonEnabled( _anim.Enabled );
			}
			ICEEditorLayout.EndHorizontal( _help );

			if( _enabled != _anim.Enabled && _anim.Enabled == true )
				_anim.Foldout = true;



			// CONTENT BEGIN
			if( DataObjectEditor.BeginObjectContentOrReturn( _type, _anim ) )
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
				{
					Info.Help ( _help );
				}
			}
			else
				Info.Help ( Info.ANIMATION_NONE );

			DataObjectEditor.EndObjectContent();
			// CONTENT END
		}


		private static AnimationClipDataContainer DrawBehaviourAnimationAnimationClipData( ICEWorldBehaviour _control, AnimationClipDataContainer _clip )
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


		/*
			private static int AnimPopup( ICECreatureControl _control, string _title, int _selected )
			{
				Animator _animator = _control.GetComponentInChildren<Animator>();
				Animation _animation = _control.GetComponentInChildren<Animation>();
				
				if( _animator != null && _animator.enabled == true && _animator.runtimeAnimatorController != null && _animator.avatar != null )
					_selected = AnimatorPopup( _animator, _selected, _title);
				else if( _animation != null && _animation.enabled == true )
					_selected = ICEEditorLayout.AnimationPopup( _animation, _selected, _title);

				return _selected;
			}*/

		private static AnimationDataContainer DrawBehaviourAnimationAnimationData( ICEWorldBehaviour _control, AnimationDataContainer _animation_data )
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
					string _animation_name = ICEEditorLayout.AnimationPopup( _animation, _animation_data.Name, "Animation (" + _animation_data.Length.ToString() + " secs.)", Info.ANIMATION_NAME );
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

		private static int AnimatorPopup( Animator _animator, int _selected, string _title = "" )
		{
			ICEEditorLayout.BeginHorizontal();
			_selected = AnimatorPopupBase( _animator, _selected, _title );
			ICEEditorLayout.EndHorizontal();

			return _selected;
		}
		/*
			public static string AnimatorPopupBaseExt( Animator _animator, string _selected_state_name, string _title = "" )
			{
				if( _animator == null || _animator.runtimeAnimatorController == null ) 
					return -1;

				UnityEditor.Animations.ChildAnimatorState[] _states = SystemTools.GetChildAnimatorStates( _animator, 0 );

				string[] _state_names = new string[ _states.Length ];
				int[] _state_index = new int[  _states.Length ];

				int i = 0;	
				int _selected_state_index = 0;
				foreach( UnityEditor.Animations.ChildAnimatorState _state in _states )
				{
					_state_index[i] = i;
					_state_names[i] = _state.state.name;

					if( _state.state.name == _selected_state_name )
						_selected_state_index = i;

					i++;
				}

				if( _title == "" )
					_title = "Animation";

				_selected_state_index = (int)EditorGUILayout.IntPopup( _title , _selected_state_index, _state_names, _state_index);

				if (GUILayout.Button("<", ICEEditorStyle.CMDButtonDouble ))
				{
					_selected_state_index--;
					if( _selected_state_index < 0 ){ 
						_selected_state_index = _states.Length-1;
					}
				}
				if (GUILayout.Button(">", ICEEditorStyle.CMDButtonDouble))
				{
					_selected_state_index++;
					if( _selected_state_index >= _states.Length ){ 
						_selected_state_index = 0;
					}
				}

				return _state_names[_selected_state_index];

			}

	*/

		public static int AnimatorPopupBase( Animator _animator, int _selected, string _title = "" )
		{
			if( _animator == null || _animator.runtimeAnimatorController == null ) 
				return -1;

			AnimationClip[] _clips = _animator.runtimeAnimatorController.animationClips; //AnimationUtility.GetAnimationClips( _animator.gameObject );

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

			if (GUILayout.Button("<", ICEEditorStyle.CMDButtonDouble ))
			{
				_selected--;
				if( _selected < 0 ){ 
					_selected = _clips.Length-1;
				}
			}
			if (GUILayout.Button(">", ICEEditorStyle.CMDButtonDouble))
			{
				_selected++;
				if( _selected >= _clips.Length ){ 
					_selected = 0;
				}
			}

			return _selected;
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

		private static AnimatorDataContainer DrawBehaviourAnimationAnimatorData( ICEWorldBehaviour _control, AnimatorDataContainer _animator_data )
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
						_animator_data.Index = AnimatorPopup( m_animator, _animator_data.Index );

						if( m_animator.runtimeAnimatorController.animationClips.Length == 0 )
						{
							Info.Warning( Info.ANIMATION_ANIMATOR_ERROR_NO_CLIPS );
						}
						else
						{
							AnimationClip _animation_clip = m_animator.runtimeAnimatorController.animationClips[_animator_data.Index];

							if( _animation_clip != null )
							{				
								if( _animator_data.Name != _animation_clip.name )
									_animator_data.Init();

								_animation_clip.wrapMode = (WrapMode)ICEEditorLayout.EnumPopup( "WarpMode", "Determines how time is treated outside of the keyframed range of an AnimationClip or AnimationCurve.", _animation_clip.wrapMode );
								_animation_clip.legacy = false;

								_animator_data.StateName = AnimationTools.GetAnimationStateName( m_animator, _animation_clip.name );
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