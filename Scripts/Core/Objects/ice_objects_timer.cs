// ##############################################################################
//
// ice_objects_timer.cs
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

namespace ICE.World.Objects
{
	[System.Serializable]
	public abstract class ICEImpulsTimerObject : ICEOwnerObject
	{
		public ICEImpulsTimerObject(){}
		public ICEImpulsTimerObject( ICEWorldBehaviour _component ) : base( _component ){} 
		public ICEImpulsTimerObject( ICEImpulsTimerObject _object ) : base( _object as ICEOwnerObject )
		{
			m_OwnerComponent = _object.OwnerComponent;

			if( m_OwnerComponent != null )
				m_OwnerComponent.OnLateUpdate += Update;

			ImpulseIntervalMin = _object.ImpulseIntervalMin;
			ImpulseIntervalMax = _object.ImpulseIntervalMax;
			ImpulseIntervalMaximum = _object.ImpulseIntervalMaximum;

			ImpulseSequenceLimitMin = _object.ImpulseSequenceLimitMin;
			ImpulseSequenceLimitMax = _object.ImpulseSequenceLimitMax;
			ImpulseSequenceLimitMaximum = _object.ImpulseSequenceLimitMaximum;

			ImpulseSequenceBreakLengthMin = _object.ImpulseSequenceBreakLengthMin;
			ImpulseSequenceBreakLengthMax = _object.ImpulseSequenceBreakLengthMax;
			ImpulseSequenceBreakLengthMaximum = _object.ImpulseSequenceBreakLengthMaximum;
		}

		private bool m_Active = false;
		private bool Active{
			get{ return m_Active; }
		}

		public void SetActive( bool _active ){

			if( _active && ! m_Active && Enabled )
			{
				m_Active = true;
				if( UseInterval )
				{
					m_ImpulseInterval = InitialImpulsTime;
					m_ImpulseLimit = Random.Range( ImpulseLimitMin, ImpulseLimitMax );
					m_ImpulseSequenceLimit = Random.Range( ImpulseSequenceLimitMin, ImpulseSequenceLimitMax );
					m_ImpulseSequenceBreakLength = Random.Range( ImpulseSequenceBreakLengthMin, ImpulseSequenceBreakLengthMax );
				}
				else if( UseEnd )
				{
					m_ImpulseInterval = 0;
					m_ImpulseLimit = 0;
					m_ImpulseSequenceLimit = 0;
					m_ImpulseSequenceBreakLength = 0;
				}
				else
				{
					m_ImpulseInterval = InitialImpulsTime;
					m_ImpulseLimit = 1;
					m_ImpulseSequenceLimit = 0;
					m_ImpulseSequenceBreakLength = 0;
				}

				m_ImpulseLimitCounter = 0;
				m_ImpulseSequenceLimitCounter = 0;	
				m_ImpulseIntervalTimer = 0;
				m_ImpulseSequenceBreakLengthTimer = 0;
			}
			else if( ! _active )
			{				
				if( UseEnd && m_Active )
					Action();
					
				m_Active = false;
				m_ImpulseLimit = 0;
				m_ImpulseSequenceLimit = 0;
				m_ImpulseInterval = 0;
				m_ImpulseSequenceBreakLength = 0;

				m_ImpulseLimitCounter = 0;
				m_ImpulseSequenceLimitCounter = 0;	
				m_ImpulseIntervalTimer = 0;
				m_ImpulseSequenceBreakLengthTimer = 0;
			}
		}

		public bool UseInterval = false;
		public bool UseEnd = false;

		public float InitialImpulsTime = 0;
		public float InitialImpulsTimeMaximum = 60;

		private float m_ImpulseLimitCounter = 0;
		private float m_ImpulseLimit = 0;
		public float ImpulseLimitMin = 0;
		public float ImpulseLimitMax = 0;
		public float ImpulseLimitMaximum = 5;

		private float m_ImpulseIntervalTimer = 0;
		private float m_ImpulseInterval = 0;
		public float ImpulseIntervalMin = 0;
		public float ImpulseIntervalMax = 0;
		public float ImpulseIntervalMaximum = 5;

		private int m_ImpulseSequenceLimitCounter = 0;
		private int m_ImpulseSequenceLimit = 0;
		public int ImpulseSequenceLimitMin = 0;
		public int ImpulseSequenceLimitMax = 0;
		public int ImpulseSequenceLimitMaximum = 10;

		private float m_ImpulseSequenceBreakLengthTimer = 0;
		private float m_ImpulseSequenceBreakLength = 0;
		public float ImpulseSequenceBreakLengthMin = 0;
		public float ImpulseSequenceBreakLengthMax = 0;
		public float ImpulseSequenceBreakLengthMaximum = 10;

		public virtual void Start(){
			SetActive( Enabled );
		}

		public virtual void Stop(){
			SetActive( false );
		}

		public virtual void Update()
		{
			if( ! m_Active || UseEnd )
				return;

			if( m_ImpulseLimit > 0 && m_ImpulseLimitCounter > m_ImpulseLimit )
				return;

			// if there is sequence limit or the counter is within the defined limit 
			if( m_ImpulseSequenceLimit == 0 || m_ImpulseSequenceLimitCounter < m_ImpulseSequenceLimit )
			{
				if( m_ImpulseInterval == 0 || m_ImpulseIntervalTimer >= m_ImpulseInterval )
				{
					Action();

					// prepare next interval and reset timer
					m_ImpulseInterval = Random.Range( ImpulseIntervalMin, ImpulseIntervalMax );
					m_ImpulseIntervalTimer = 0;

					// increase sequence limit counter after sending a message
					if( m_ImpulseSequenceLimit > 0 )
						m_ImpulseSequenceLimitCounter++;

					// increase limit counter after sending a message
					if( m_ImpulseLimit > 0 )
						m_ImpulseLimitCounter++;
				}
				else
					m_ImpulseIntervalTimer += Time.deltaTime;
			}

			// if the sequence limit is reached we have to do a break by using the m_ImpulseSequenceBreakLength
			else if( m_ImpulseSequenceBreakLength == 0 || m_ImpulseSequenceBreakLengthTimer >= m_ImpulseSequenceBreakLength )
			{
				// prepare next sequence and reset counter
				m_ImpulseSequenceLimit = Random.Range( ImpulseSequenceLimitMin, ImpulseSequenceLimitMax );
				m_ImpulseSequenceLimitCounter = 0;

				// prepare next break and reset timer
				m_ImpulseSequenceBreakLength = Random.Range( ImpulseSequenceBreakLengthMin, ImpulseSequenceBreakLengthMax );
				m_ImpulseSequenceBreakLengthTimer = 0;
			}
			else
				m_ImpulseSequenceBreakLengthTimer +=  Time.deltaTime;	
		}

		protected virtual void Action(){}
	}

}
