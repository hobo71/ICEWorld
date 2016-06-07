// ##############################################################################
//
// ice_objects_methods.cs | ICE.World.Objects.MethodsObject | MethodObject | MethodDataObject
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

	[System.Serializable]
	public class MethodsObject : ICEOwnerObject
	{
		public MethodsObject(){}
		public MethodsObject( MethodsObject _method ) : base( _method as ICEOwnerObject )
		{
			m_Methods.Clear();
			foreach( MethodObject _msg in _method.Methods )
				m_Methods.Add( _msg );
		}

		[SerializeField]
		private List<MethodObject> m_Methods = new List<MethodObject>();
		public List<MethodObject> Methods{
			get{ return m_Methods; }
			set{ m_Methods = value; }
		}

		public void Start( GameObject _owner )
		{
			m_Owner = _owner;			
			foreach( MethodObject _msg in Methods )
				_msg.Start( _owner );
		}

		public void Stop()
		{
			foreach( MethodObject _msg in Methods )
				_msg.Stop();
		}

		public void Update()
		{
			foreach( MethodObject _msg in Methods )
				_msg.Update();
		}
	}

	[System.Serializable]
	public class MethodObject : ICEImpulsTimerObject
	{
		public MethodObject(){}
		public MethodObject( MethodObject _method ) : base( _method as ICEImpulsTimerObject )
		{
			Enabled = _method.Enabled;
			StartMethod.Copy( _method.StartMethod );
			ImpulseMethod.Copy( _method.ImpulseMethod );
			StopMethod.Copy( _method.StopMethod );
		}
			
		public MethodDataObject StartMethod = new MethodDataObject(); 
		public MethodDataObject ImpulseMethod = new MethodDataObject(); 
		public MethodDataObject StopMethod = new MethodDataObject(); 

		private Transform m_Transform = null;

		public void Start( GameObject _owner )
		{
			m_Owner = _owner;	
			SendMessage( StartMethod );
			Start();
		}

		public override void Stop()
		{
			base.Stop();
			SendMessage( StopMethod );
		}

		protected override void Action()
		{
			SendMessage( ImpulseMethod );
		}

		/// <summary>
		/// Sends the message.
		/// </summary>
		/// <param name="_method">Method.</param>
		protected virtual void SendMessage( MethodDataObject _method )
		{
			if( m_Owner == null || _method == null || string.IsNullOrEmpty( _method.MethodName ) )
				return;

			if( m_Transform == null )
				m_Transform = SystemTools.FindChildByName( _method.ComponentName, m_Owner.transform );

			if( m_Transform != null )
			{
				switch( _method.ParameterType ) 
				{
					case MethodParameterType.Boolean:
						m_Transform.SendMessage( _method.MethodName, _method.ParameterBoolean, SendMessageOptions.DontRequireReceiver );
						break;
					case MethodParameterType.Integer:
						m_Transform.SendMessage( _method.MethodName, _method.ParameterInteger, SendMessageOptions.DontRequireReceiver );
						break;
					case MethodParameterType.Float:
						m_Transform.SendMessage( _method.MethodName, _method.ParameterFloat, SendMessageOptions.DontRequireReceiver );
						break;
					case MethodParameterType.String:
						m_Transform.SendMessage( _method.MethodName, _method.ParameterString, SendMessageOptions.DontRequireReceiver );
						break;
					default:
						m_Transform.SendMessage( _method.MethodName, null, SendMessageOptions.DontRequireReceiver );
						break;
				}
			}
			else
			{
				switch( _method.ParameterType ) 
				{
					case MethodParameterType.Boolean:
						m_Owner.BroadcastMessage( _method.MethodName, _method.ParameterBoolean, SendMessageOptions.DontRequireReceiver );
						break;
					case MethodParameterType.Integer:
						m_Owner.BroadcastMessage( _method.MethodName, _method.ParameterInteger, SendMessageOptions.DontRequireReceiver );
						break;
					case MethodParameterType.Float:
						m_Owner.BroadcastMessage( _method.MethodName, _method.ParameterFloat, SendMessageOptions.DontRequireReceiver );
						break;
					case MethodParameterType.String:
						m_Owner.BroadcastMessage( _method.MethodName, _method.ParameterString, SendMessageOptions.DontRequireReceiver );
						break;
					default:
						m_Owner.BroadcastMessage( _method.MethodName, null, SendMessageOptions.DontRequireReceiver );
						break;
				}
			}
		}
	}

	[System.Serializable]
	public class MethodDataObject : ICEObject
	{
		public bool UseCustomFunction = false;
		public string ComponentName = "";
		public string MethodName = "";

		public MethodParameterType ParameterType = MethodParameterType.None;
		public string ParameterString; 
		public int ParameterInteger;
		public float ParameterFloat; 
		public bool ParameterBoolean;

		public MethodDataObject(){}
		public void Copy( MethodDataObject _method )
		{
			this.ComponentName = _method.ComponentName;
			this.MethodName = _method.MethodName;

			this.ParameterType = _method.ParameterType;
			this.ParameterString = _method.ParameterString;
			this.ParameterInteger = _method.ParameterInteger;
			this.ParameterFloat = _method.ParameterFloat;
			this.ParameterBoolean = _method.ParameterBoolean;
		}

		public MethodDataContainer MethodData{
			get{ return new MethodDataContainer( ComponentName, MethodName, ParameterType ); }
			set{ 
				ComponentName = value.ComponentName;
				MethodName = value.MethodName; 
				ParameterType = value.ParameterType; 
			}
		}


	}
}
	
