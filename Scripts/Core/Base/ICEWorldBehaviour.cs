// ##############################################################################
//
// ICE.World.ICEWorldBehaviour.cs
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

using ICE;
using ICE.World.Objects;

namespace ICE.World
{


	/// <summary>
	/// ICEComponent is the abstract base class of all ICEWorld based components.
	/// </summary>
	public abstract class ICEWorldBehaviour : MonoBehaviour {

		public static bool IsMultiplayer{
			get{ return ICEWorldInfo.IsMultiplayer; }
		}

		public static bool IsMasterClient{
			get{ return ICEWorldInfo.IsMasterClient; }
		}

		/// <summary>
		/// Enables/disables available debug features
		/// </summary>
		public bool UseDebug = false;
		/// <summary>
		/// Enables debug logs.
		/// </summary>
		public bool UseDebugLogs = false;
		public void PrintDebugLog( string _log )
		{
			if( UseDebugLogs )
				Debug.Log( name + " (" + ObjectInstanceID + ") - " + _log );
		}

		/// <summary>
		/// Activates or deactivates 'Dont Destroy On Load'.
		/// </summary>
		public bool UseDontDestroyOnLoad = false;

		/// <summary>
		/// The cached InstanceID.
		/// </summary>
		private int m_ObjectInstanceID = 0;
		/// <summary>
		/// Gets the cached InstanceID.
		/// </summary>
		/// <value>The ID.</value>
		public int ObjectInstanceID{
			get{  return m_ObjectInstanceID = ( m_ObjectInstanceID == 0 ? transform.gameObject.GetInstanceID():m_ObjectInstanceID ); }
		}
			
		/// <summary>
		/// OnUpdateEvent. This delegate can be used in ICEOwnerObject classes to handle updates
		/// </summary>
		public delegate void OnUpdateEvent();

		/// <summary>
		/// OnUpdate. This event can be used in ICEOwnerObject classes to handle updates
		/// </summary>
		public event OnUpdateEvent OnUpdate;

		/// <summary>
		/// OnLateUpdateEvent. This delegate can be used in ICEOwnerObject classes to handle late updates
		/// </summary>
		public delegate void OnLateUpdateEvent();

		/// <summary>
		/// OnLateUpdate. This event can be used in ICEOwnerObject classes to handle late updates
		/// </summary>
		public event OnLateUpdateEvent OnLateUpdate;

		/// <summary>
		/// OnFixedUpdateEvent. This delegate can be used in ICEOwnerObject classes to handle fixed updates
		/// </summary>
		public delegate void OnFixedUpdateEvent();

		/// <summary>
		/// OnFixedUpdate. This event can be used in ICEOwnerObject classes to handle fixed updates
		/// </summary>
		public event OnFixedUpdateEvent OnFixedUpdate;

		// PUBLIC METHODS
		/// <summary>
		/// m_PublicMethods. PublicMethods represents a list of method names.
		/// </summary>
		protected List<BehaviourEventInfo> m_BehaviourEvents = new List<BehaviourEventInfo>();
		/// <summary>
		/// Gets the public methods.
		/// </summary>
		/// <value>The public methods.</value>
		public BehaviourEventInfo[] BehaviourEvents{
			get{ return GetBehaviourEvents(); }
		}

		protected BehaviourEventInfo[] GetBehaviourEvents()
		{
			m_BehaviourEvents.Clear();
			OnRegisterBehaviourEvents();

			List<BehaviourEventInfo> _events = new List<BehaviourEventInfo>();
			foreach( BehaviourEventInfo _event in m_BehaviourEvents )						
				_events.Add( new BehaviourEventInfo( _event ) );

			return _events.ToArray();
		}


		/// <summary>
		/// Gets all available behaviour events.
		/// </summary>
		/// <value>All available behaviour events.</value>
		public BehaviourEventInfo[] BehaviourEventsInChildren{
			get{
				List<BehaviourEventInfo> _events = new List<BehaviourEventInfo>();
				ICEWorldBehaviour[] _components = GetComponentsInChildren<ICEWorldBehaviour>();
				if( _components != null )
				{
					foreach( ICEWorldBehaviour _component in _components )
						foreach( BehaviourEventInfo _event in _component.BehaviourEvents )						
							_events.Add( new BehaviourEventInfo( _event ) );
				}

				return _events.ToArray();
			}
		}

		/// <summary>
		/// OnRegisterBehaviourEvents is called whithin the GetBehaviourEvents() method to update the 
		/// m_BehaviourEvents list. Override this event to register your own events by using the 
		/// RegisterBehaviourEvent method, while doing so you can use base.OnRegisterBehaviourEvents(); 
		/// to call the event in the base classes too.
		/// </summary>
		protected virtual void OnRegisterBehaviourEvents(){}

		/// <summary>
		/// Registers the behaviour event with zero parameter.
		/// </summary>
		/// <param name="_event">Event.</param>
		public void RegisterBehaviourEvent( string _event ){
			RegisterBehaviourEvent( _event, BehaviourEventParameterType.None );
		}

		/// <summary>
		/// Registers the behaviour event with the defind parameter type.
		/// </summary>
		/// <param name="_event">Event.</param>
		/// <param name="_type">Type.</param>
		public void RegisterBehaviourEvent( string _event, BehaviourEventParameterType _type ){
			if( string.IsNullOrEmpty( _event ) )
				return;

			m_BehaviourEvents.Add( new BehaviourEventInfo( this.name, _event, _type ) );
		}

		/// <summary>
		/// Clears the behaviour events.
		/// </summary>
		public void ClearBehaviourEvents(){
			m_BehaviourEvents.Clear();
		}

		public virtual bool GetDynamicBooleanValue( DynamicBooleanValueType _type )
		{
			/*
			switch( _type )
			{
			case DynamicBooleanValueType.IsGrounded:
				return Creature.Move.IsGrounded;
			case DynamicBooleanValueType.Deadlocked:
				return Creature.Move.Deadlocked;
			case DynamicBooleanValueType.MovePositionReached:
				return Creature.Move.MovePositionReached;
			case DynamicBooleanValueType.TargetMovePositionReached:
				return Creature.Move.TargetMovePositionReached;
			case DynamicBooleanValueType.MovePositionUpdateRequired:
				return Creature.Move.MovePositionUpdateRequired;
			case DynamicBooleanValueType.IsJumping:
				return Creature.Move.IsJumping;


			}*/

			return false;
		}

		public virtual int GetDynamicIntegerValue( DynamicIntegerValueType _type )
		{/*
			switch( _type )
			{

			case DynamicIntegerValueType.CreatureForwardSpeed:
				return MoveForwardVelocity;
			case DynamicIntegerValueType.CreatureAngularSpeed:
				return MoveAngularVelocity;
			case DynamicIntegerValueType.CreatureDirection:
				return MoveDirection;
			}*/

			return 0;
		}

		public virtual float GetDynamicFloatValue( DynamicFloatValueType _type )
		{
			/*
			switch( _type )
			{
			case DynamicFloatValueType.ForwardSpeed:
				return MoveForwardVelocity;
			case DynamicFloatValueType.AngularSpeed:
				return MoveAngularVelocity;
			case DynamicFloatValueType.Direction:
				return MoveDirection;

			case DynamicFloatValueType.FallSpeed:
				return Creature.Move.FallSpeed;

			case DynamicFloatValueType.Altitude:
				return MoveAltitude;
			case DynamicFloatValueType.AbsoluteAltitude:
				return MoveAbsoluteAltitude;

			case DynamicFloatValueType.MovePositionDistance:
				return Creature.Move.MovePositionDistance;
			}*/

			return 0;
		}


		public virtual void Awake () {

			if( UseDontDestroyOnLoad )
				DontDestroyOnLoad(this);
		}

		public virtual void Start () {

		}

		public virtual void OnEnable () {

		}

		public virtual void OnDisable () {

		}

		public virtual void OnDestroy() {

		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		public virtual void Update () {
			DoUpdate();
		}

		public virtual void LateUpdate () {
			DoLateUpdate();
		}

		/// <summary>
		/// Fixeds the update.
		/// </summary>
		public virtual void FixedUpdate () {
			DoFixedUpdate();
		}

		/// <summary>
		/// Dos the update.
		/// </summary>
		protected virtual void DoUpdate () {
			if( OnUpdate != null )
				OnUpdate();
		}

		protected virtual void DoLateUpdate () {
			if( OnLateUpdate != null )
				OnLateUpdate();
		}

		/// <summary>
		/// Dos the fixed update.
		/// </summary>
		protected virtual void DoFixedUpdate () {
			if( OnFixedUpdate != null )
				OnFixedUpdate();
		}
	}
}
