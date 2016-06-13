// ##############################################################################
//
// ICE.World.ICEEntity.cs
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

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

namespace ICE.World
{
	/// <summary>
	/// Within the ICE World an entity is something that exists as itself, as a subject or as an object, actually 
	/// or potentially, concretely or abstractly, physically or not. On this note an ICE World Entity represents 
	/// an interactive GameObject within your scenes which could affect the gameplay in someways. ICEWorldEntity 
	/// is the lowermost base class for all interactive components within the ICE World (e.g. ICECreatureControl 
	/// or ICEPlayer, but also items, vehicles, construction elements etc.). 
	/// 
	/// ICEWorldEntity is derived from ICEWorldBehaviour and declared as abstract.
	/// 
	/// Use ICEWorldEntity or one of its child classes as base class for your own interaction components, such as your 
	/// own FPSController or your custom AI script, so your code will be automatically downwards compatible with the 
	/// rest of the ICE world.			
	/// </summary>
	public abstract class ICEWorldEntity : ICEWorldBehaviour {

		[SerializeField]
		protected EntityStatusObject m_Status = null;
		public virtual EntityStatusObject Status{
			set{ m_Status = value; }
			get{ return m_Status = ( m_Status == null ? new EntityStatusObject(this):m_Status ); }
		}
			
		/// <summary>
		/// OnRegisterPublicMethods is called within the GetPublicMethods() method to update the 
		/// m_PublicMethods list. Override this event to register your own methods by using the 
		/// RegisterPublicMethod(); while doing so you can use base.OnRegisterPublicMethods(); 
		/// to call the event in the base classes too.
		/// </summary>
		protected override void OnRegisterBehaviourEvents()
		{
			//base.OnRegisterBehaviourEvents(); 
			RegisterBehaviourEvent( "ApplyDamage", BehaviourEventParameterType.Float );
			RegisterBehaviourEvent( "Reset" );
		}
			
		private ICEWorldSingleton m_World = null;
		/// <summary>
		/// Gets the ICEWorld.
		/// </summary>
		/// <value>The current ICEWorld</value>
		public ICEWorldSingleton World{
			get{ return m_World = ( m_World == null?ICEWorldSingleton.Instance:m_World ); }
		}
			
		private ICEWorldEnvironment m_Environment = null;
		/// <summary>
		/// Gets the ICEWorldEnvironment.
		/// </summary>
		/// <value>The current ICEWorldEnvironment or null</value>
		public ICEWorldEnvironment Environment{
			get{ return m_Environment = ( m_Environment == null?ICEWorldEnvironment.Instance:m_Environment ); }
		}

		private ICEWorldRegister m_Registry = null;
		/// <summary>
		/// Gets the ICEWorldRegistry.
		/// </summary>
		/// <value>The current ICEWorldRegistry or null</value>
		public ICEWorldRegister Registry{
			get{ return m_Registry = ( m_Registry == null?ICEWorldRegister.Instance:m_Registry ); }
		}

		protected Transform m_Transform = null;
		/// <summary>
		/// Gets the cached object transform.
		/// </summary>
		/// <value>The object transform.</value>
		public Transform ObjectTransform {
			get{ return m_Transform  = ( m_Transform == null?GetComponent<Transform>():m_Transform ); }
		}

		/// <summary>
		/// m root entity contains the stored root entity or null in cases that this entity is the root entity
		/// you should use RootEntity or GetRootEntity() instead of the stored value because the parent could 
		/// be changed during the runtime, so this value would be obsolete.
		/// </summary>
		protected ICEWorldEntity m_RootEntity = null;

		/// <summary>
		/// Gets the parent entity.
		/// </summary>
		/// <value>The parent entity.</value>
		public ICEWorldEntity RootEntity { 
			get{ return m_RootEntity = ( m_RootEntity == null ? GetRootEntity() : m_RootEntity ); }
		}

		/// <summary>
		/// Gets the root entity or null in cases that this entity is the root entity
		/// </summary>
		/// <returns>The parent entity.</returns>
		public ICEWorldEntity GetRootEntity()
		{
			// if root is identic with the own transform we will return null, m_ParentEntity stays null 
			// and the check will be repeated all the time because the parent could be changed during the 
			// runtime 
			if( transform.root == transform )
				return null;

			// as long as the given parent entity is root we don't need to check it again
			else if( m_RootEntity != null && m_RootEntity.transform.root == m_RootEntity.transform )
				return m_RootEntity;

			// in all other cases we have to check all parents because the root could be changed during the 
			// runtime.
			else
			{
				ICEWorldEntity[] _entities = ObjectTransform.GetComponentsInParent<ICEWorldEntity>();
				foreach( ICEWorldEntity _entity in _entities )
					if( _entity.transform.root == _entity.transform )
						return _entity;
			}

			// if there are no higher entities within the hierarchy we assume that this entity is the root entity
			// so we return null to repeat the check if required.
			return null;
		}

		public override void Awake () {
			base.Awake();
		}

		public override void Start () {
			base.Start();
		}

		public override void OnEnable () {
			base.OnEnable();
			Status.Init( this );
		}

		public override void OnDisable () {
			base.OnDisable();
		}

		public override void OnDestroy() {
			base.OnDestroy();
		}

		public override void Update(){
			DoUpdate();
		}

		public override void LateUpdate () {
			DoLateUpdate();
		}

		protected virtual void Register(){
			WorldRegister.Register( transform.gameObject );
		}

		protected virtual void Deregister(){
			WorldRegister.Deregister( transform.gameObject );
		}

		/// <summary>
		/// Removes this instance according to the defined reference group settings of the 
		/// WorldRegister. In cases UseSoftRespawn is active the target will be dactivate, 
		/// stored and prepared for its next action, otherwise the object will be destroyed.
		/// </summary>
		protected virtual void Remove(){
			WorldRegister.Remove( transform.gameObject );
		}

		public virtual void Reset()
		{
			Status.Reset();
		}

		/// <summary>
		/// Damage the specified _damage.
		/// </summary>
		/// <param name="_damage">Damage.</param>
		public virtual void Damage( float _damage ){
			ApplyDamage( _damage, Vector3.zero, Vector3.zero, null, 0 );
		}

		/// <summary>
		/// Applies the damage.
		/// </summary>
		/// <param name="_damage">Damage.</param>
		public virtual void ApplyDamage( float _damage ){
			ApplyDamage( _damage, Vector3.zero, Vector3.zero, null, 0 );
		}

		/// <summary>
		/// Applies the damage.
		/// </summary>
		/// <param name="_damage">Damage.</param>
		/// <param name="_damage_direction">Damage direction.</param>
		/// <param name="_attacker_position">Attacker position.</param>
		/// <param name="_attacker">Attacker.</param>
		/// <param name="_force">Force.</param>
		protected virtual void ApplyDamage( float _damage, Vector3 _damage_direction, Vector3 _attacker_position, Transform _attacker, float _force = 0  )
		{
			// use RootEntity instead of m_RootEntity to make sure that the values will be up-to-date
			if( RootEntity != null ) 
				m_RootEntity.Status.ApplyDamage( _damage );
			else
				Status.ApplyDamage( _damage );				
		}

		public virtual void OnCollisionEnter(Collision _collision) {}
		public virtual void OnCollisionStay(Collision _collision) {}
		public virtual void OnCollisionExit( Collision _collision )  {}
		public virtual void OnTriggerEnter( Collider _collider ) {}
		public virtual void OnTriggerStay( Collider _collider ) {}
		public virtual void OnTriggerExit( Collider _collider ) {}
		public virtual void OnControllerColliderHit( ControllerColliderHit hit ) {}


	}
}
