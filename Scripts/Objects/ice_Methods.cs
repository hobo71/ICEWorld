// ##############################################################################
//
// Version 1.2.10
//
// © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
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
		private void SendMessage( MethodDataObject _method )
		{
			if( m_Owner == null || _method == null || string.IsNullOrEmpty( _method.MethodName ) )
				return;

			if( m_Transform == null )
				m_Transform = SystemTools.FindChildByName( _method.ComponentName, m_Owner.transform );

			if( m_Transform != null )
			{
				switch( _method.MethodType ) 
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
				switch( _method.MethodType ) 
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
		public bool UseCustomMethod = false;
		public string ComponentName = "";
		public string MethodName = "";
		public MethodParameterType MethodType = MethodParameterType.None;

		public string ParameterString; 
		public int ParameterInteger;
		public float ParameterFloat; 
		public bool ParameterBoolean;

		public MethodDataObject(){}
		public void Copy( MethodDataObject _method )
		{
			this.ComponentName = _method.ComponentName;
			this.MethodName = _method.MethodName;
			this.MethodType = _method.MethodType;

			this.ParameterString = _method.ParameterString;
			this.ParameterInteger = _method.ParameterInteger;
			this.ParameterFloat = _method.ParameterFloat;
			this.ParameterBoolean = _method.ParameterBoolean;
		}

		public MethodDataContainer MethodData{
			get{ return new MethodDataContainer( ComponentName, MethodName, MethodType ); }
			set{ 
				ComponentName = value.ComponentName;
				MethodName = value.MethodName; 
				MethodType = value.MethodType; 
			}
		}


	}
}
	
