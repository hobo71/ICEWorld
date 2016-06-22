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
	/// ICEWorldEntityPart represents an object or body part of an ICEWorldEntity. 
	/// This class will be used to manipulate and forward received impacts to the 
	/// owner entity. 
	/// </summary>
	public class ICEWorldEntityPart : ICEWorldBehaviour {

		public float DamageMultiplier = 10;
		public float DamageMultiplierMaximum = 100;

		void Start()
		{
			if( transform.GetComponent<Collider>() == null )
				PrintError( "Start - Collider required!" );

			m_RootEntity = GetRootEntity();
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
			if( ! enabled )
				return;
			
			// if we have a valid root entity we apply the damage to it, otherwise we try to forward the damage to an higher damage handler.
			// In any case we are using RootEntity instead of m_RootEntity to make sure that the entity will be up-to-date
			if( RootEntity != null ) 
				m_RootEntity.ApplyDamage( _damage * DamageMultiplier, _damage_direction, _attacker_position, _attacker, _force );
			else
				gameObject.SendMessageUpwards("ApplyDamage", _damage * DamageMultiplier, SendMessageOptions.DontRequireReceiver);				
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
			// if root is identic with the own transform something goes wrong because an entity part should 
			// never be the root
			if( transform.root == transform )
			{
				PrintError( "GetRootEntity - an Entity Part should not be the root." );
				return null;
			}
				
			// as long as the given parent entity is root we don't need to check it again
			else if( m_RootEntity != null && m_RootEntity.transform.root == m_RootEntity.transform )
				return m_RootEntity;

			// in all other cases we have to check all parents because the root could be changed during the 
			// runtime.
			else
				return SystemTools.GetRoot<ICEWorldEntity>( ObjectTransform );

			// if there are no higher entities within the hierarchy we assume that this entity is the root entity
			// so we return null to repeat the check if required.
			return null;
		}
	}
}

