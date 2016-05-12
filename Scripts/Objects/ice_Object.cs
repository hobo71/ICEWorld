// ##############################################################################
//
// ICE.World.ICEObject.cs
// Version 1.2.10
//
// The MIT License (MIT)
//
// Copyright © Pit Vetterick, ICE Technologies Consulting LTD. 
// http://www.icecreaturecontrol.com (mailto:support@icecreaturecontrol.com)
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
	/// <summary>
	/// ICEObject represents the abstract base class for all ICE related System Objects.
	/// </summary>
	[System.Serializable]
	public abstract class ICEObject : System.Object {

		/// <summary>
		/// Enables or disables the use of the object.
		/// </summary>
		public bool Enabled = false;
		/// <summary>
		/// The foldout parameter is a display option and should be used in the editor only 
		/// </summary>
		public bool Foldout = false;

		/// <summary>
		/// Prints debug log.
		/// </summary>
		public bool EnableDebugLog = false;

		/// <summary>
		/// Prints the debug log.
		/// </summary>
		/// <param name="_log">Log.</param>
		public void PrintDebugLog( ICEObject _object, string _log )
		{
			if( EnableDebugLog || OwnerEnabledDebugLog )
				Debug.Log( OwnerName + " (" + OwnerInstanceID + ") - " + ( _object != null?_object.GetType().ToString() + " ":"" ) + _log );
		}

		/// <summary>
		/// m_Owner represents the owning GameObject
		/// </summary>
		protected GameObject m_Owner = null;
		/// <summary>
		/// Gets the owning GameObject.
		/// </summary>
		/// <value>The parent or null</value>

		public GameObject Owner{
			get{ return m_Owner = ( m_Owner == null ?( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null ):m_Owner ); }
		}

		/// <summary>
		/// The m parent represents the owner component
		/// </summary>
		protected ICEComponent m_OwnerComponent = null;
		/// <summary>
		/// Gets the owner component.
		/// </summary>
		/// <value>The parent or null</value>
		public ICEComponent OwnerComponent{
			get{ return m_OwnerComponent; }
		}

		/// <summary>
		/// Gets the name of the parent.
		/// </summary>
		/// <value>The name of the parent.</value>
		public string OwnerName{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.name:"" ); }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.World.Objects.ICEObject"/> parent allows to print the debug log.
		/// </summary>
		/// <value><c>true</c> if parent print debug log; otherwise, <c>false</c>.</value>
		public bool OwnerEnabledDebugLog{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.EnableDebugLogs:false ); }
		}

		/// <summary>
		/// Gets the parent InstanceID.
		/// </summary>
		/// <value>The parent InstanceID or 0</value>
		public int OwnerInstanceID{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.InstanceID :0 ); }
		}

		public ICEObject(){}
		public ICEObject( ICEComponent _component ){
			m_OwnerComponent = _component;
			m_Owner = ( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null );
		}

		public ICEObject( ICEObject _object ){
			m_OwnerComponent = ( _object != null?_object.OwnerComponent:null );
			m_Owner = ( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null );
		}

		/// <summary>
		/// Default Init method to initiate the object.
		/// </summary>
		/// <param name="_parent">Parent.</param>
		public virtual void Init( ICEComponent _component ){
			m_OwnerComponent = _component;
			m_Owner = ( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null );
		}
	}
}
