// ##############################################################################
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
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World.Objects
{
	[System.Serializable]
	public class EntityBodyPartObject : ICEOwnerObject {

		public EntityBodyPartObject(){}
		public EntityBodyPartObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );
		}

		public float DamageMultiplier = 1;
		public float DamageMultiplierMaximum = 100;
		public bool UseDamageTransfer = true;
		public bool DamageTransferRequired{
			get{
				if( m_OwnerComponent != null && m_OwnerComponent.transform.root != m_OwnerComponent.transform && UseDamageTransfer )
					return true;
				else
					return false;
			}
		}
	}

	[System.Serializable]
	public class EntityStatusObject : LifespanObject {

		public EntityStatusObject(){}
		public EntityStatusObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		/* TODO: UNDER CONSTRUCTION 
		[SerializeField]
		private DurabilityCompositionObject m_DurabilityComposition = null;
		public DurabilityCompositionObject DurabilityComposition{
			get{ return m_DurabilityComposition = ( m_DurabilityComposition == null ? new DurabilityCompositionObject() : m_DurabilityComposition ); }
			set{ m_DurabilityComposition = value; }
		}*/

		[SerializeField]
		private CorpseObject m_Corpse = null;
		public CorpseObject Corpse{
			get{ return m_Corpse = ( m_Corpse == null ? new CorpseObject( m_OwnerComponent ) : m_Corpse ); }
			set{ m_Corpse = value; }
		}



		public bool IsDestructible = true;

		protected float m_Durability = 0;
		public virtual float Durability{
			get{ 

				//m_Durability = DurabilityComposition.UpdateDurability( m_DefaultDurability );

				if( m_Durability < 0 ) 
					m_Durability = 0;
				else if( m_Durability > m_InitialDurability ) 
					m_Durability = m_InitialDurability;	

				return m_Durability; 
			}
		}

		public virtual float DurabilityInPercent{
			get{ return FixedPercent( m_InitialDurability > 0 ? 100 / m_InitialDurability * Durability:100 ); }
		}

		public virtual float InitialDurabilityMultiplier{
			get{ return ( m_InitialDurability > 0?100/m_InitialDurability:1 ); }
		}

		protected float m_InitialDurability = 100;
		public virtual float InitialDurability{
			get{ return m_InitialDurability; }
		}
		public float InitialDurabilityMin = 100;
		public float InitialDurabilityMax = 100;
		public float InitialDurabilityMaximum = 100;

		public virtual void SetInitialDurability( float _value ){

			if( _value < InitialDurabilityMin ) 
				m_InitialDurability = InitialDurabilityMin;
			else if( _value > InitialDurabilityMax ) 
				m_InitialDurability = InitialDurabilityMax;
			else
				m_InitialDurability = _value;				
		}

		public virtual void SetDurability( float _value ){

			if( _value < 0 ) 
				m_Durability = 0;
			else if( _value > InitialDurabilityMax ) 
				m_Durability = InitialDurabilityMax;
			else
				m_Durability = _value;				
		}

		public virtual void UpdateDurabilityByPercent( float _percent ){

			_percent = FixedPercent( _percent );

			m_Durability = m_InitialDurability / 100 * _percent;			
		}

		/// <summary>
		/// Gets a value indicating whether this instance is destroyed.
		/// </summary>
		/// <value><c>true</c> if this instance is destroyed; otherwise, <c>false</c>.</value>
		public virtual bool IsDestroyed{
			get{ return ( IsDestructible && Durability <= 0 ? true:false ); }
		}
			
		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );

			Corpse.Init( m_OwnerComponent );

			m_InitialDurability = Random.Range( InitialDurabilityMin, InitialDurabilityMax );
			m_Durability = m_InitialDurability;

			PrintDebugLog( this, "Init" );
		}

		public override void Reset()
		{
			base.Reset();

			m_InitialDurability = Random.Range( InitialDurabilityMin, InitialDurabilityMax );
			m_Durability = m_InitialDurability;

			PrintDebugLog( this, "Reset" );
		}

		public override void Update(){
			base.Update();
		}

		public virtual void Remove()
		{
			PrintDebugLog( this, "Remove - Durability :" + m_Durability );

			// Removes the entity from the world
			WorldManager.Remove( m_Owner );
		}

		/// <summary>
		/// Processes the received damage.
		/// </summary>
		/// <returns>The damage.</returns>
		/// <param name="_damage">Damage.</param>
		public virtual void AddDamage( float _damage )
		{
			m_Durability -= _damage;

			if( m_Durability < 0 )
				m_Durability = 0;

			PrintDebugLog( this, "AddDamage - " + ( IsDestructible && m_InitialDurability > 0 ? "InitialDurability :" + m_InitialDurability +" - Durability :" + m_Durability  + " - Damage :" + _damage : "disabled" ) );
		}
	}
}
