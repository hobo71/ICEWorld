﻿// ##############################################################################
//
// ice_objects_status.cs | EntityStatusObject
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

namespace ICE.World.Objects
{
	[System.Serializable]
	public class EntityStatusObject : LifespanObject {

		public EntityStatusObject(){}
		public EntityStatusObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		public bool IsDestructible = true;

		protected float m_Durability = 0;
		public virtual float Durability{
			get{ return m_Durability; }
		}
		public virtual float DurabilityInPercent{
			get{ return ( m_DefaultDurability > 0 ? 100 / m_DefaultDurability * Durability:100 ); }
		}
		public virtual float DurabilityMultiplier{
			get{ return (m_Durability > 0?100/m_Durability:1); }
		}

		protected float m_DefaultDurability = 100;
		public virtual float DefaultDurability{
			get{ return m_DefaultDurability; }
		}
		public float DefaultDurabilityMin = 100;
		public float DefaultDurabilityMax = 100;
		public float DefaultDurabilityMaximum = 100;

		public virtual void SetDefaultDurability( float _value ){

			if( _value < DefaultDurabilityMin ) 
				m_DefaultDurability = DefaultDurabilityMin;
			else if( _value > DefaultDurabilityMax ) 
				m_DefaultDurability = DefaultDurabilityMax;
			else
				m_DefaultDurability = _value;				
		}

		public virtual void SetDurability( float _value ){

			if( _value < 0 ) 
				m_Durability = 0;
			else if( _value > DefaultDurabilityMax ) 
				m_Durability = DefaultDurabilityMax;
			else
				m_Durability = _value;				
		}

		/// <summary>
		/// Gets a value indicating whether this instance is destroyed.
		/// </summary>
		/// <value><c>true</c> if this instance is destroyed; otherwise, <c>false</c>.</value>
		public virtual bool IsDestroyed{
			get{ return ( IsDestructible && m_Durability <= 0 ? true:false ); }
		}
			
		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );

			m_DefaultDurability = Random.Range( DefaultDurabilityMin, DefaultDurabilityMax );
			m_Durability = m_DefaultDurability;

			PrintDebugLog( this, "Init" );
		}

		public virtual void Reset()
		{
			base.Reset();

			m_DefaultDurability = Random.Range( DefaultDurabilityMin, DefaultDurabilityMax );
			m_Durability = m_DefaultDurability;

			PrintDebugLog( this, "Reset" );
		}

		public override void Update(){
			base.Update();
		}

		/// <summary>
		/// Applies the damage.
		/// </summary>
		/// <param name="_damage">Damage.</param>
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

			PrintDebugLog( this, "ProcessDamage - " + ( IsDestructible && m_DefaultDurability > 0 ? "DefaultDurability :" + m_DefaultDurability +" - Durability :" + m_Durability  + " - Damage :" + _damage : "disabled" ) );

			return m_Durability;
		}
	}
}
