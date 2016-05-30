﻿// ##############################################################################
//
// ICE.World.ICEWorldBehaviour.cs
// Version 1.2.10
//
// © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
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

namespace ICE.World
{
	/// <summary>
	/// ICEComponent is the abstract base class of all ICEWorld based components.
	/// </summary>
	public abstract class ICEWorldBehaviour : MonoBehaviour {

		public static bool IsMultiplayer{
			get{ return ICEWorldInfo.IsMultiplayer; }
		}

		public static bool IsMasterClient{
			get{ return ICEWorldInfo.IsMasterClient; }
		}

		/// <summary>
		/// Enables/disables available debug features
		/// </summary>
		public bool UseDebug = false;
		/// <summary>
		/// Enables debug logs.
		/// </summary>
		public bool UseDebugLogs = false;
		public void PrintDebugLog( string _log )
		{
			if( UseDebugLogs )
				Debug.Log( name + " (" + InstanceID + ") - " + _log );
		}

		/// <summary>
		/// Activates or deactivates 'Dont Destroy On Load'.
		/// </summary>
		public bool UseDontDestroyOnLoad = false;

		/// <summary>
		/// The cached InstanceID.
		/// </summary>
		private int m_InstanceID = 0;
		/// <summary>
		/// Gets the cached InstanceID.
		/// </summary>
		/// <value>The ID.</value>
		public int InstanceID{
			get{  return m_InstanceID = ( m_InstanceID == 0 ? transform.gameObject.GetInstanceID():m_InstanceID ); }
		}

		public delegate void OnLateUpdateEvent();
		public event OnLateUpdateEvent OnLateUpdate;

		public delegate void OnUpdateBeginEvent();
		public event OnUpdateBeginEvent OnUpdateBegin;

		public delegate void OnUpdateEvent();
		public event OnUpdateEvent OnUpdate;

		public delegate void OnUpdateCompleteEvent();
		public event OnUpdateCompleteEvent OnUpdateComplete;

		public delegate void OnFixedUpdateBeginEvent();
		public event OnFixedUpdateBeginEvent OnFixedUpdateBegin;

		public delegate void OnFixedUpdateEvent();
		public event OnFixedUpdateEvent OnFixedUpdate;

		public delegate void OnFixedUpdateCompleteEvent();
		public event OnFixedUpdateCompleteEvent OnFixedUpdateComplete;

		// PUBLIC METHODS
		/// <summary>
		/// m_PublicMethods. PublicMethods represents a list of method names.
		/// </summary>
		protected List<string> m_PublicMethods = new List<string>();
		/// <summary>
		/// Gets the public methods.
		/// </summary>
		/// <value>The public methods.</value>
		public string[] PublicMethods{
			get{
				m_PublicMethods.Clear();
				RegisterPublicMethods();
				return m_PublicMethods.ToArray(); }
		}


		/// <summary>
		/// Gets all public methods.
		/// </summary>
		/// <value>All public methods.</value>
		public string[] AllPublicMethods{
			get{ 
				List<string> _methods = new List<string>();

				ICEWorldBehaviour[] _components = GetComponentsInChildren<ICEWorldBehaviour>();

				foreach( ICEWorldBehaviour _component in _components )
					foreach( string _method in _component.PublicMethods )						
						_methods.Add( _method );
		
				return _methods.ToArray();
			}
		}

		/// <summary>
		/// Register public methods. Override this method to register your own methods by using the RegisterPublicMethod();
		/// </summary>
		protected virtual void RegisterPublicMethods(){}

		public void RegisterPublicMethod( string _method )
		{
			if( string.IsNullOrEmpty( _method ) )
				return;

			m_PublicMethods.Add( _method );
		}

		public void ClearPublicMethods(){
			m_PublicMethods.Clear();
		}

		public virtual void Awake () {

			if( UseDontDestroyOnLoad )
				DontDestroyOnLoad(this);
		}

		public virtual void Start () {

		}

		public virtual void OnEnable () {

		}

		public virtual void OnDisable () {

		}

		public virtual void OnDestroy() {

		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		public virtual void Update () {
			DoUpdateBegin();
			DoUpdate();
			DoUpdateComplete();
		}
			
		/// <summary>
		/// Dos the update begin.
		/// </summary>
		protected virtual void DoUpdateBegin () {
			if( OnUpdateBegin != null )
				OnUpdateBegin();
		}
			
		/// <summary>
		/// Dos the update.
		/// </summary>
		protected virtual void DoUpdate () {
			if( OnUpdate != null )
				OnUpdate();
		}

		/// <summary>
		/// Dos the update complete.
		/// </summary>
		protected virtual void DoUpdateComplete () {
			if( OnUpdateComplete != null )
				OnUpdateComplete();
		}

		public virtual void LateUpdate () {
			DoLateUpdate();
		}

		protected virtual void DoLateUpdate () {
			if( OnLateUpdate != null )
				OnLateUpdate();
		}

		/// <summary>
		/// Fixeds the update.
		/// </summary>
		public virtual void FixedUpdate () {
			DoFixedUpdateBegin();
			DoFixedUpdate();
			DoFixedUpdateComplete();
		}

		/// <summary>
		/// Dos the fixed update begin.
		/// </summary>
		protected virtual void DoFixedUpdateBegin () {
			if( OnFixedUpdateBegin != null )
				OnFixedUpdateBegin();
		}

		/// <summary>
		/// Dos the fixed update.
		/// </summary>
		protected virtual void DoFixedUpdate () {
			if( OnFixedUpdate != null )
				OnFixedUpdate();
		}

		/// <summary>
		/// Dos the fixed update complete.
		/// </summary>
		protected virtual void DoFixedUpdateComplete () {
			if( OnFixedUpdateComplete != null )
				OnFixedUpdateComplete();
		}
	}
}