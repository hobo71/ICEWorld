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

using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World
{
	/// <summary>
	/// ICE World Entity represents the abstract base class for all ICE related world objects.
	/// </summary>
	public abstract class ICEWorldEntity : ICEWorldBehaviour {

		[SerializeField]
		protected ICEStatusObject m_Status = null;
		public virtual ICEStatusObject Status{
			set{ m_Status = value; }
			get{ return m_Status = ( m_Status == null ? new ICEStatusObject(this):m_Status ); }
		}

		protected override void RegisterPublicMethods()
		{
			RegisterPublicMethod( "ApplyDamage", MethodParameterType.Float );
		}


		private ICEWorld m_World = null;
		/// <summary>
		/// Gets the ICEWorld.
		/// </summary>
		/// <value>The current ICEWorld</value>
		public ICEWorld World{
			get{ return m_World = ( m_World == null?ICEWorld.Instance:m_World ); }
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

		public override void Awake () {
			base.Awake();
		}

		public override void Start () {
			base.Start();
		}

		public override void OnEnable () {
			base.OnEnable();
		}

		public override void OnDisable () {
			base.OnDisable();
		}

		public override void OnDestroy() {
			base.OnDestroy();
		}

		public override void Update()
		{
			DoUpdateBegin();
			DoUpdate();
			DoUpdateComplete();
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

		public virtual void Reset(){}

		public virtual void ApplyDamage( float _damage ){
			ApplyDamage( _damage, Vector3.zero, Vector3.zero, null, 0 );
		}

		public virtual void ApplyDamage( float _damage, Vector3 _damage_direction, Vector3 _attacker_position, Transform _attacker, float _force = 0  )
		{
			Status.ApplyDamage( _damage );
		}

		public virtual void OnCollisionEnter(Collision _collision) 
		{

		}

		public virtual void OnCollisionStay(Collision _collision) 
		{

		}

		public virtual void OnCollisionExit( Collision _collision ) 
		{

		}

		public virtual void OnTriggerEnter( Collider _collider ) 
		{

		}

		public virtual void OnTriggerStay( Collider _collider ) 
		{

		}

		public virtual void OnTriggerExit( Collider _collider ) 
		{

		}

		public virtual void OnControllerColliderHit( ControllerColliderHit hit ) 
		{

		}


	}
}
