using UnityEngine;
using System.Collections;

using ICE;
using ICE.World;

namespace ICE.World.Objects
{
	public class StatusObject : ICEObject {

		public StatusObject(){}
		public StatusObject( ICEComponent _component ) : base( _component )
		{
			Init( _component );
		}

	
		protected float m_Integrity = 0;
		public virtual float Integrity{
			get{ return m_Integrity; }
		}
		public virtual float IntegrityMultiplier{
			get{ return (m_Integrity > 0?100/m_Integrity:1); }
		}

		public float DefaultIntegrity = 0;
		public float DefaultIntegrityMin = 0;
		public float DefaultIntegrityMax = 0;
		public float DefaultIntegrityMaximum = 100;

		public bool UseAging = false;
		public float MaxAge = 60f;	
		public float MaxAgeMaximum = 60f;

		protected float m_Age = 0.0f;
		public float Age{ 
			get{ return m_Age; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is destroyed.
		/// </summary>
		/// <value><c>true</c> if this instance is destroyed; otherwise, <c>false</c>.</value>
		public virtual bool IsDestroyed{
			get{ return ( m_Integrity <= 0 ? true:false ); }
		}

		public void SetAge( float _age )
		{ 
			if( _age >= 0 && _age <= MaxAge )
				m_Age = _age;
		}

		public override void Init( ICEComponent _component )
		{
			base.Init( _component );

			Reset();
		}

		public virtual void Reset()
		{
			DefaultIntegrity = Random.Range( DefaultIntegrityMin, DefaultIntegrityMax );
			m_Integrity = DefaultIntegrity;
		}

		public virtual void Update()
		{
			if( UseAging )
			{
				m_Age +=  Time.deltaTime;
			}
		}

		public virtual void ApplyDamage( float _damage ){
			ProcessDamage( _damage );
		}


		/// <summary>
		/// Processes the received damage.
		/// </summary>
		/// <returns>The damage.</returns>
		/// <param name="_damage">Damage.</param>
		protected virtual float ProcessDamage( float _damage )
		{
			m_Integrity -= _damage;

			if( m_Integrity < 0 )
				m_Integrity = 0;

			return m_Integrity;
		}
	}
}
