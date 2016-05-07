// ##############################################################################
//
// ICE.World.ICEObject.cs
// Version 1.1.21
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
			if( EnableDebugLog || ParentEnabledDebugLog )
				Debug.Log( ParentName + " (" + ParentInstanceID + ") - " + ( _object != null?_object.GetType().ToString() + " ":"" ) + _log );
		}

		/// <summary>
		/// The m parent represents the owner component
		/// </summary>
		protected ICEComponent m_Parent = null;
		/// <summary>
		/// Gets the owner component.
		/// </summary>
		/// <value>The parent or null</value>
		public ICEComponent Parent{
			get{ return m_Parent; }
		}

		/// <summary>
		/// Gets the name of the parent.
		/// </summary>
		/// <value>The name of the parent.</value>
		public string ParentName{
			get{ return ( m_Parent != null ? m_Parent.name:"" ); }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.World.Objects.ICEObject"/> parent allows to print the debug log.
		/// </summary>
		/// <value><c>true</c> if parent print debug log; otherwise, <c>false</c>.</value>
		public bool ParentEnabledDebugLog{
			get{ return ( m_Parent != null ? m_Parent.EnableDebugLogs:false ); }
		}

		/// <summary>
		/// Gets the parent InstanceID.
		/// </summary>
		/// <value>The parent InstanceID or 0</value>
		public int ParentInstanceID{
			get{ return ( m_Parent != null ? m_Parent.InstanceID :0 ); }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ICE.World.Objects.ICEObject"/> class.
		/// </summary>
		public ICEObject(){}

		/// <summary>
		/// Initializes a new instance of the <see cref="ICE.World.Objects.ICEObject"/> class.
		/// </summary>
		/// <param name="_parent">Parent.</param>
		public ICEObject( ICEComponent _parent ){
			m_Parent = _parent;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ICE.World.Objects.ICEObject"/> class.
		/// </summary>
		/// <param name="_object">Object.</param>
		public ICEObject( ICEObject _object ){
			m_Parent = ( _object != null?_object.Parent:null );
		}

		/// <summary>
		/// Default Init method to initiate the object.
		/// </summary>
		/// <param name="_parent">Parent.</param>
		public virtual void Init( ICEComponent _parent ){
			m_Parent = _parent;
		}
	}
}
