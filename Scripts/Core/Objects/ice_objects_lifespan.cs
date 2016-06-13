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
	public enum LifespanType
	{
		NONE,
		LIFESPAN,
		AGING
	}

	[System.Serializable]
	public class LifespanObject : ICEOwnerObject
	{
		public LifespanObject(){}
		public LifespanObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		/// <summary>
		/// Gets the total lifetime since the start
		/// </summary>
		/// <value>The total lifetime.</value>
		public float TotalLifetime{ 
			get{ return Time.time - m_InitTime; } 
		}

		/// <summary>
		/// Gets the lifetime since the last reset.
		/// </summary>
		/// <value>The lifetime.</value>
		public float Lifetime{ 			
			get{ return Time.time - m_ResetTime; } 
		}
		/// <summary>
		/// The init time.
		/// </summary>
		private float m_InitTime = 0;

		/// <summary>
		/// The reset time.
		/// </summary>
		private float m_ResetTime = 0;
	
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


		public float LifespanInPercent{		
			get{ return FixedPercent( 100 - ( 100/MaxAge*m_Age) ); }
		}

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

		public bool UseAging = false;
		public float MaxAge = 60f;	
		public float MaxAgeMaximum = 60f;

		protected float m_Age = 0.0f;
		public float Age{ 
			get{ return m_Age; }
		}

		public void SetAge( float _age )
		{ 
			if( _age >= 0 && _age <= MaxAge )
				m_Age = _age;
		}

		/// <summary>
		/// Overrides the parent Init method to initiate the lifespan procedure
		/// </summary>
		/// <param name="_parent">Parent.</param>
		public override void Init( ICEWorldBehaviour _owner )
		{
			base.Init( _owner );

			if( m_OwnerComponent == null )
				return;

			m_InitTime = Time.time;
			m_ResetTime = Time.time;

			m_CurrentLifespan = UpdateRandomLifespan();
				
			m_OwnerComponent.OnUpdate += DoUpdate;

			if( UseLifespan )
				m_OwnerComponent.Invoke( "Remove", m_CurrentLifespan );
		}

		public virtual void Reset()
		{
			m_ResetTime = Time.time;

			m_InitTime = Time.time;
			m_CurrentLifespan = UpdateRandomLifespan();
		}


		/// <summary>
		/// Gets a random lifespan.
		/// </summary>
		/// <value>The random lifespan.</value>
		public float UpdateRandomLifespan(){
			return Random.Range( LifespanMin, LifespanMax ); 
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.Creatures.Objects.LifespanObject"/> use lifespan.
		/// </summary>
		/// <value><c>true</c> if use lifespan; otherwise, <c>false</c>.</value>
		public bool UseLifespan = false;

		/// <summary>
		/// Dos the update begin.
		/// </summary>
		private void DoUpdate()
		{

		}

		public virtual void Update()
		{
			if( UseAging )
			{
				m_Age +=  Time.deltaTime;
			}

			m_AgeInSeconds = Time.time - m_InitTime;

			if( UseLifespan || UseAging )
				PrintDebugLog( this, "Lifespan : " + ( ( UseLifespan && m_CurrentLifespan > 0 ) ? m_CurrentLifespan.ToString() : "disabled" ) + " - Age :" + ( UseAging ? AgeInMinutes.ToString() + " min." : "disabled" ) );
		}

		/// <summary>
		/// Returns valid rounded percent value
		/// </summary>
		/// <returns>The percent.</returns>
		/// <param name="_value">Value.</param>
		protected float FixedPercent( float _value )
		{
			if( _value < 0 ) _value = 0;
			if( _value > 100 ) _value = 100;

			return (float)System.Math.Round( _value, 2 );
		}

		/// <summary>
		/// Returns valid multiplier between 0 and 1
		/// </summary>
		/// <returns>The multiplier.</returns>
		/// <param name="_value">Value.</param>
		protected float FixedMultiplier( float _value )
		{
			if( _value < 0 ) _value = 0;
			if( _value > 1 ) _value = 1;

			return _value;
		}
	}


}
