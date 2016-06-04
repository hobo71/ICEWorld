// ##############################################################################
//
// ice_objects_animation.cs | ICE.World.Objects.AnimationObject | AnimationDataObject | AnimatorParameterObject
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
using System.Xml;
using System.Xml.Serialization;

using ICE;
using ICE.World;

namespace ICE.World.Objects
{
	[System.Serializable]
	public class AnimatorParameterObject : ICEDataObject
	{
		public AnimatorParameterObject(){}
		public AnimatorParameterObject( AnimatorParameterObject _parameter ) : base( _parameter ) {
			Copy( _parameter );
		}

		public void Copy( AnimatorParameterObject _parameter )
		{
			base.Copy( _parameter );

			UseDynamicValue = _parameter.UseDynamicValue;
			Name = _parameter.Name;
			Type = _parameter.Type;
			IntegerValueType = _parameter.IntegerValueType;
			IntegerValue = _parameter.IntegerValue;
			FloatValueType = _parameter.FloatValueType;
			FloatValue = _parameter.FloatValue;
			BooleanValueType = _parameter.BooleanValueType;
			BooleanValue = _parameter.BooleanValue;
		}

		public bool UseDynamicValue = false;
		public string Name = "";
		public AnimatorControllerParameterType Type = AnimatorControllerParameterType.Float;

		public DynamicIntegerValueType IntegerValueType = DynamicIntegerValueType.undefined;
		public int IntegerValue = 0;

		public DynamicFloatValueType FloatValueType = DynamicFloatValueType.AngularSpeed;
		public float FloatValue = 0;

		public DynamicBooleanValueType BooleanValueType = DynamicBooleanValueType.IsGrounded;
		public bool BooleanValue = false;
	}

	[System.Serializable]
	public struct AnimatorDataContainer
	{
		public void Copy( AnimatorDataContainer _data )
		{
			StateName = _data.StateName;
			Name = _data.Name;
			Index = _data.Index;
			Length = _data.Length;
			DefaultWrapMode = _data.DefaultWrapMode;
			AutoSpeed = _data.AutoSpeed;
			Speed = _data.Speed;
			TransitionDuration = _data.TransitionDuration;
			Type = _data.Type;
			ApplyRootMotion = _data.ApplyRootMotion;

			m_Parameters = new List<AnimatorParameterObject>();
			foreach( AnimatorParameterObject _parameter in _data.Parameters ){
				m_Parameters.Add( new AnimatorParameterObject( _parameter ) );
			}
		}

		[SerializeField]
		private List<AnimatorParameterObject> m_Parameters;
		public List<AnimatorParameterObject> Parameters{
			set{ m_Parameters = value;}
			get{

				if( m_Parameters == null )
					m_Parameters = new List<AnimatorParameterObject>();

				return m_Parameters;
			}
		}

		public string StateName;
		public string Name;
		public int Index;
		public float Length;
		public WrapMode DefaultWrapMode;
		public bool AutoSpeed;
		public float Speed;
		public float TransitionDuration;

		public AnimatorControlType Type;
		public bool ApplyRootMotion;

		public void Init()
		{
			this.AutoSpeed = false;
			this.Speed = 1;
			this.TransitionDuration = 0.5f;
		}
	}

	[System.Serializable]
	public struct AnimationDataContainer
	{
		public void Copy( AnimationDataContainer _data )
		{
			Name = _data.Name;
			Index = _data.Index;
			Length = _data.Length;
			wrapMode = _data.wrapMode;
			DefaultSpeed = _data.DefaultSpeed;
			DefaultWrapMode = _data.DefaultWrapMode;
			Speed = _data.Speed;
			AutoSpeed = _data.AutoSpeed;
			AutoTransitionDuration = _data.AutoTransitionDuration;
			TransitionDuration = _data.TransitionDuration;
		}

		public string Name;
		public int Index;
		public float Length;
		public WrapMode wrapMode;
		public float DefaultSpeed;
		public WrapMode DefaultWrapMode;
		public float Speed;
		public bool AutoSpeed;
		public bool AutoTransitionDuration;
		public float TransitionDuration;

		public void Init()
		{
			this.AutoSpeed = false;
			this.AutoTransitionDuration = false;
			this.Speed = 1;
			this.TransitionDuration = 0.5f;
		}
	}

	[System.Serializable]
	public struct AnimationClipDataContainer
	{
		public void Copy( AnimationClipDataContainer _data )
		{
			Clip = _data.Clip;
			TransitionDuration = _data.TransitionDuration;
		}

		[XmlIgnore]
		public AnimationClip Clip;

		public  float TransitionDuration;

		public void Init()
		{
			//this.AutoSpeed = false;
			//this.Speed = 1;
			this.TransitionDuration = 0.5f;
		}
	}
		
	[System.Serializable]
	public class AnimationDataObject : ICEDataObject
	{
		public AnimationDataObject(){}
		public AnimationDataObject( AnimationDataObject _data ) : base( _data ){ Copy( _data ); }

		public void Copy( AnimationDataObject _data )
		{
			base.Copy( _data );
			AllowInterfaceSelector = _data.AllowInterfaceSelector;
			InterfaceType = _data.InterfaceType;

			Animator.Copy( _data.Animator );
			Animation.Copy( _data.Animation );
			Clip.Copy( _data.Clip );
		}

		public bool AllowInterfaceSelector;
		public AnimationInterfaceType InterfaceType;

		public AnimatorDataContainer Animator;
		public AnimationDataContainer Animation;
		public AnimationClipDataContainer Clip;

		public float GetAnimationLength()
		{
			if( InterfaceType == AnimationInterfaceType.LEGACY )
				return Animation.Length;
			else if( InterfaceType == AnimationInterfaceType.MECANIM )
				return Animator.Length;
			else if( InterfaceType == AnimationInterfaceType.CLIP && Clip.Clip != null )
				return Clip.Clip.length;
			else 
				return 0;
		}

		public string GetAnimationName()
		{
			if( InterfaceType == AnimationInterfaceType.LEGACY )
				return Animation.Name;
			else if( InterfaceType == AnimationInterfaceType.MECANIM )
				return Animator.Name;
			else if( InterfaceType == AnimationInterfaceType.CLIP && Clip.Clip != null )
				return Clip.Clip.name;
			else 
				return "";
		}

		public bool UseRootMotion{
			get{ 
				if( InterfaceType == AnimationInterfaceType.MECANIM )
					return Animator.ApplyRootMotion;
				else
					return false;
			}
		}

		public void Init()
		{
			Animator.Init();
			Animation.Init();
			Clip.Init();
		}

		public bool IsValid{
			get{
				if( Enabled == true &&
					InterfaceType != AnimationInterfaceType.NONE )
					return true;
				else
					return false;
			}
		}
	}

	[System.Serializable]
	public class AnimationPlayerObject : ICEOwnerObject
	{
		public AnimationPlayerObject(){}
		public AnimationPlayerObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		protected Animation m_Animation = null;
		protected Animator m_Animator = null;


		public delegate void OnCustomAnimationEvent();
		public event OnCustomAnimationEvent OnCustomAnimation;

		public delegate void OnCustomAnimationUpdateEvent();
		public event OnCustomAnimationUpdateEvent OnCustomAnimationUpdate;

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );

			m_Animation = m_Owner.GetComponentInChildren<Animation>();
			m_Animator = m_Owner.GetComponentInChildren<Animator>();
		}

		public void Play()
		{
			if( m_Animator != null  )
				m_Animator.StartPlayback();
			else if( m_Animation != null && ! m_Animation.isPlaying )
				m_Animation.Play();				
		}

		public void Play( AnimationDataObject _data )
		{
			if( _data == null || _data.Enabled == false || _data.InterfaceType == AnimationInterfaceType.NONE )
				return;

			if( _data.InterfaceType == AnimationInterfaceType.LEGACY )
			{
				if ( m_Animation == null ) 
				{	
					Debug.LogError( "CAUTION : Missing Animation Component on " + m_Owner.gameObject.name );
					return;
				}

				WrapMode _mode = _data.Animation.wrapMode;

				m_Animation[ _data.Animation.Name ].wrapMode = _mode;
				m_Animation[ _data.Animation.Name ].speed = _data.Animation.Speed;//Mathf.Clamp( m_BehaviourData.MoveVelocity. controller.velocity.magnitude, 0.0, runMaxAnimationSpeed);
				m_Animation.CrossFade( _data.Animation.Name, _data.Animation.TransitionDuration );	

			}
			else if( _data.InterfaceType == AnimationInterfaceType.CLIP )
			{
				if ( m_Animation == null ) 
					m_Animation = m_Owner.AddComponent<Animation>();

				if ( m_Animation != null && _data.Clip.Clip != null ) 
				{
					m_Animation.AddClip( _data.Clip.Clip, _data.Clip.Clip.name );
					m_Animation.CrossFade( _data.Clip.Clip.name, _data.Clip.TransitionDuration );	
				}

			}
			else if( _data.InterfaceType == AnimationInterfaceType.MECANIM )
			{
				if ( m_Animator == null ) 
				{	
					Debug.LogError( "Missing Animator Component!" );
					return;
				}

				if( _data.Animator.Type == AnimatorControlType.DIRECT )
				{			
					try{

						m_Animator.CrossFade( _data.Animator.StateName, _data.Animator.TransitionDuration, -1, 0); 
						m_Animator.speed = _data.Animator.Speed;
					}
					catch{

					}
					//m_AnimatorAutoSpeed = _rule.Animation.Animator.AutoSpeed;
					/*if( _rule.Animation.Animator.AutoSpeed )
						m_Animator.speed = Mathf.Clamp( m_BehaviourData.MoveVelocity. controller.velocity.magnitude, 0.0, runMaxAnimationSpeed);
					else*/

				}
				else if( _data.Animator.Type == AnimatorControlType.ADVANCED )
				{
					m_Animator.applyRootMotion = _data.Animator.ApplyRootMotion;

					UpdateAnimatorParameter( _data.Animator );
				}
			}
			else if( _data.InterfaceType == AnimationInterfaceType.CUSTOM )
			{
				if( OnCustomAnimation != null )
					OnCustomAnimation();
			}
		}

		public void Update( AnimationDataObject _data )
		{
			if( _data == null || _data.Enabled == false || _data.InterfaceType == AnimationInterfaceType.NONE )
				return;


			if( _data.InterfaceType == AnimationInterfaceType.LEGACY )
			{

			}
			else if( _data.InterfaceType == AnimationInterfaceType.CLIP )
			{

			}
			else if( _data.InterfaceType == AnimationInterfaceType.MECANIM )
			{
				if ( m_Animator == null ) 
					return;

				if( _data.Animator.Type == AnimatorControlType.DIRECT )
				{

				}
				else if( _data.Animator.Type == AnimatorControlType.ADVANCED )
				{
					UpdateAnimatorParameter( _data.Animator );
				}
			}
			else if( _data.InterfaceType == AnimationInterfaceType.CUSTOM )
			{
				if( OnCustomAnimationUpdate != null )
					OnCustomAnimationUpdate();
			}

			/*
			if( m_AnimatorAutoSpeed && m_Animator != null )
				m_Animator.speed = Mathf.Clamp( m_BehaviourData.MoveVelocity. controller.velocity.magnitude, 0.0, runMaxAnimationSpeed);*/

		}

		public virtual void UpdateAnimatorParameter( AnimatorDataContainer _animator_data )
		{
			foreach( AnimatorParameterObject _parameter in _animator_data.Parameters )
			{
				if( _parameter.Enabled )
				{
					if( _parameter.Type == AnimatorControllerParameterType.Bool )
					{
						if( _parameter.UseDynamicValue )
							m_Animator.SetBool( _parameter.Name, ( m_OwnerComponent != null ? m_OwnerComponent.GetDynamicBooleanValue( _parameter.BooleanValueType ):_parameter.BooleanValue ) );
						else
							m_Animator.SetBool( _parameter.Name, _parameter.BooleanValue );
					}
					else if( _parameter.Type == AnimatorControllerParameterType.Float )
					{
						if( _parameter.UseDynamicValue )
							m_Animator.SetFloat( _parameter.Name, ( m_OwnerComponent != null ? m_OwnerComponent.GetDynamicFloatValue( _parameter.FloatValueType ):_parameter.FloatValue ) );
						else
							m_Animator.SetFloat( _parameter.Name, _parameter.FloatValue );
					}
					else if( _parameter.Type == AnimatorControllerParameterType.Int )
					{
						if( _parameter.UseDynamicValue )
							m_Animator.SetInteger( _parameter.Name, ( m_OwnerComponent != null ? m_OwnerComponent.GetDynamicIntegerValue( _parameter.IntegerValueType ):_parameter.IntegerValue ) );
						else
							m_Animator.SetInteger( _parameter.Name, _parameter.IntegerValue );
					}
					else if( _parameter.Type == AnimatorControllerParameterType.Trigger )
					{
						m_Animator.SetTrigger( _parameter.Name );
					}
				}
			}

		}


		public void Break()
		{
			if( m_Animator != null  )
			{
				m_Animator.StopPlayback();
				/*
				AnimationInfo[] _animator_info = m_Animator.GetCurrentAnimationClipState(0);
				
				
				if( _animator_info.Length >> ){

				}else{
					for(int idx=0;idx<_animator_info.Length;idx++){

						_animator_info[idx].
					}
				}*/
			}
			else if( m_Animation != null && m_Animation.isPlaying )
				m_Animation.Stop();				
		}
	}

}
