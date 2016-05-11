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

		public bool IsDestructible = true;

		protected float m_Durability = 0;
		public virtual float Durability{
			get{ return m_Durability; }
		}
		public virtual float DurabilityInPercent{
			get{ return DefaultDurability / 100 * Durability; }
		}
		public virtual float DurabilityMultiplier{
			get{ return (m_Durability > 0?100/m_Durability:1); }
		}

		public float DefaultDurability = 0;
		public float DefaultDurabilityMin = 0;
		public float DefaultDurabilityMax = 0;
		public float DefaultDurabilityMaximum = 100;

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
			get{ return ( IsDestructible && m_Durability <= 0 ? true:false ); }
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
			DefaultDurability = Random.Range( DefaultDurabilityMin, DefaultDurabilityMax );
			m_Durability = DefaultDurability;
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
			m_Durability -= _damage;

			if( m_Durability < 0 )
				m_Durability = 0;

			return m_Durability;
		}
	}
}
