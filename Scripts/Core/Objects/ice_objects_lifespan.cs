// ##############################################################################
//
// ice_objects_lifespan.cs
// Version 1.2.10
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using System.Collections;

using ICE.World;
using ICE.World.Objects;

namespace ICE.World.Objects
{
	[System.Serializable]
	public class LifespanObject : ICEOwnerObject
	{
		/// <summary>
		/// The start time.
		/// </summary>
		private float m_StartTime = 0;
	
		/// <summary>
		/// The current lifespan.
		/// </summary>
		private float m_CurrentLifespan = 0;

		/// <summary>
		/// The age in seconds.
		/// </summary>
		protected float m_AgeInSeconds = 0;

		/// <summary>
		/// The lifespan minimum.
		/// </summary>
		public float LifespanMin = 0;
		/// <summary>
		/// The lifespan max.
		/// </summary>
		public float LifespanMax = 0;
		/// <summary>
		/// The lifespan default max.
		/// </summary>
		public float LifespanDefaultMax = 360;

		/// <summary>
		/// The age in seconds.
		/// </summary>
		public float AgeInSeconds{
			get{ return m_AgeInSeconds; }
		}

		/// <summary>
		/// Gets the age in minutes.
		/// </summary>
		/// <value>The age in minutes.</value>
		public float AgeInMinutes{
			get{ return m_AgeInSeconds/60; }
		}

		/// <summary>
		/// Gets the age in hours.
		/// </summary>
		/// <value>The age in hours.</value>
		public float AgeInHours{
			get{ return m_AgeInSeconds/3600; }
		}

		/// <summary>
		/// Overrides the parent Init method to initiate the lifespan procedure
		/// </summary>
		/// <param name="_parent">Parent.</param>
		public override void Init( ICEWorldBehaviour _parent )
		{
			m_OwnerComponent = _parent;
			if( m_OwnerComponent == null )
				return;

			m_StartTime = Time.time;
			m_CurrentLifespan = RandomLifespan;
				
			m_OwnerComponent.OnUpdate += DoUpdate;

			if( UseLifespan )
				m_OwnerComponent.Invoke( "Remove", m_CurrentLifespan );
		}

		/// <summary>
		/// Gets a random lifespan.
		/// </summary>
		/// <value>The random lifespan.</value>
		public float RandomLifespan{
			get{ return Random.Range( LifespanMin, LifespanMax ); }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.Creatures.Objects.LifespanObject"/> use lifespan.
		/// </summary>
		/// <value><c>true</c> if use lifespan; otherwise, <c>false</c>.</value>
		public bool UseLifespan{
			get{ return ( ( m_CurrentLifespan > 0 && Enabled )?true:false); }
		}

		/// <summary>
		/// Dos the update begin.
		/// </summary>
		private void DoUpdate()
		{
			m_AgeInSeconds = Time.time - m_StartTime;

			PrintDebugLog( this, "Lifespan : " + m_CurrentLifespan + " - Age :" + m_AgeInSeconds );
		}
	}


}
