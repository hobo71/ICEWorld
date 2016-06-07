// ##############################################################################
//
// ice_objects_events.cs | ICE.World.Objects.BehaviourEventsObject | BehaviourEventObject | BehaviourEvent | BehaviourEventInfo
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
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

using ICE.World;
using ICE.World.Utilities;
using ICE.World.Objects;

namespace ICE.World.Objects
{

	/// <summary>
	/// The BehaviourEventsObject contains and handles the list of defined BehaviourEventObjects
	/// </summary>
	[System.Serializable]
	public class BehaviourEventsObject : ICEOwnerObject
	{
		public BehaviourEventsObject(){}
		public BehaviourEventsObject( BehaviourEventsObject _events ) : base( _events as ICEOwnerObject )
		{
			m_Events.Clear();
			foreach( BehaviourEventObject _msg in _events.Events )
				m_Events.Add( _msg );
		}

		[SerializeField]
		private List<BehaviourEventObject> m_Events = new List<BehaviourEventObject>();
		public List<BehaviourEventObject> Events{
			get{ return m_Events; }
			set{ m_Events = value; }
		}

		public void Start( GameObject _owner )
		{
			m_Owner = _owner;			
			foreach( BehaviourEventObject _msg in Events )
				_msg.Start( _owner );
		}

		public void Stop()
		{
			foreach( BehaviourEventObject _msg in Events )
				_msg.Stop();
		}

		public void Update()
		{
			foreach( BehaviourEventObject _msg in Events )
				_msg.Update();
		}
	}

	/// <summary>
	/// The BehaviourEventObject contains and handles the defined start, stop and impluse events during the runtime
	/// </summary>
	[System.Serializable]
	public class BehaviourEventObject : ICEImpulsTimerObject
	{
		public BehaviourEventObject(){}
		public BehaviourEventObject( BehaviourEventObject _event ) : base( _event as ICEImpulsTimerObject )
		{
			Enabled = _event.Enabled;
			Event.Copy( _event.Event );
		}

		[SerializeField]
		private BehaviourEvent m_Event = null;
		public BehaviourEvent Event{
			get{ return m_Event = ( m_Event == null ? new BehaviourEvent() : m_Event ); }
			set{ m_Event = value; }
		}

		private Transform m_Transform = null;

		public void Start( GameObject _owner )
		{
			m_Owner = _owner;		
			Start();
		}

		public override void Stop()
		{
			base.Stop();
		}

		protected override void Action()
		{
			SendMessage( Event );
		}

		/// <summary>
		/// Sends the message.
		/// </summary>
		/// <param name="_method">Method.</param>
		protected virtual void SendMessage( BehaviourEvent _event )
		{
			if( m_Owner == null || _event == null || string.IsNullOrEmpty( _event.FunctionName ) )
				return;

			if( m_Transform == null )
				m_Transform = SystemTools.FindChildByName( _event.ComponentName, m_Owner.transform );

			if( m_Transform != null )
			{
				switch( _event.ParameterType ) 
				{
					case BehaviourEventParameterType.Boolean:
						m_Transform.SendMessage( _event.FunctionName, _event.ParameterBoolean, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.Integer:
						m_Transform.SendMessage( _event.FunctionName, _event.ParameterInteger, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.Float:
						m_Transform.SendMessage( _event.FunctionName, _event.ParameterFloat, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.String:
						m_Transform.SendMessage( _event.FunctionName, _event.ParameterString, SendMessageOptions.DontRequireReceiver );
						break;
					default:
						m_Transform.SendMessage( _event.FunctionName, null, SendMessageOptions.DontRequireReceiver );
						break;
				}
			}
			else
			{
				switch( _event.ParameterType ) 
				{
					case BehaviourEventParameterType.Boolean:
						m_Owner.BroadcastMessage( _event.FunctionName, _event.ParameterBoolean, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.Integer:
						m_Owner.BroadcastMessage( _event.FunctionName, _event.ParameterInteger, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.Float:
						m_Owner.BroadcastMessage( _event.FunctionName, _event.ParameterFloat, SendMessageOptions.DontRequireReceiver );
						break;
					case BehaviourEventParameterType.String:
						m_Owner.BroadcastMessage( _event.FunctionName, _event.ParameterString, SendMessageOptions.DontRequireReceiver );
						break;
					default:
						m_Owner.BroadcastMessage( _event.FunctionName, null, SendMessageOptions.DontRequireReceiver );
						break;
				}
			}
		}
	}

	[System.Serializable]
	public class BehaviourEventInfo : ICEObject
	{
		/// <summary>
		/// The name of the owner component.
		/// </summary>
		public string ComponentName = "";
		public string FunctionName = "";
		public BehaviourEventParameterType ParameterType = BehaviourEventParameterType.None;

		public BehaviourEventInfo(){}
		public BehaviourEventInfo( string _component, string _name, BehaviourEventParameterType _type )
		{
			this.ComponentName = _component;
			this.FunctionName = _name;
			this.ParameterType = _type;
		}

		public BehaviourEventInfo( BehaviourEventInfo _method )
		{
			this.ComponentName = _method.ComponentName;
			this.FunctionName = _method.FunctionName;
			this.ParameterType = _method.ParameterType;
		}

		public void Copy( BehaviourEventInfo _method )
		{
			this.ComponentName = _method.ComponentName;
			this.FunctionName = _method.FunctionName;
			this.ParameterType = _method.ParameterType;
		}

		public string Key{
			get{ return ComponentName + "." + FunctionName; }
		}

		public void Reset()
		{
			this.ComponentName = "";
			this.FunctionName = "";
			this.ParameterType = BehaviourEventParameterType.None;
		}
	}

	[System.Serializable]
	public class BehaviourEvent : BehaviourEventInfo
	{
		public bool UseCustomFunction = false;

		public string ParameterString; 
		public int ParameterInteger;
		public float ParameterFloat; 
		public bool ParameterBoolean;

		public BehaviourEvent() : base(){}
		public void Copy( BehaviourEvent _method )
		{
			this.ComponentName = _method.ComponentName;
			this.FunctionName = _method.FunctionName;

			this.ParameterType = _method.ParameterType;
			this.ParameterString = _method.ParameterString;
			this.ParameterInteger = _method.ParameterInteger;
			this.ParameterFloat = _method.ParameterFloat;
			this.ParameterBoolean = _method.ParameterBoolean;
		}

		public BehaviourEventInfo Info{
			get{ return new BehaviourEventInfo( ComponentName, FunctionName, ParameterType ); }
			set{ 
				ComponentName = value.ComponentName;
				FunctionName = value.FunctionName; 
				ParameterType = value.ParameterType; 
			}
		}


	}
}
	
