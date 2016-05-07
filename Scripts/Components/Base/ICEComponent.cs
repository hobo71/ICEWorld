// ##############################################################################
//
// ICE.World.ICEComponent.cs
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

namespace ICE.World
{
	/// <summary>
	/// ICE component.
	/// </summary>
	public abstract class ICEComponent : MonoBehaviour {

		/// <summary>
		/// Enables debug logs.
		/// </summary>
		public bool EnableDebugLogs = false;

		public void PrintDebugLog( string _log )
		{
			if( EnableDebugLogs )
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

		public delegate void OnUpdateBeginEvent();
		public event OnUpdateBeginEvent OnUpdateBegin;

		public delegate void OnUpdateEvent();
		public event OnUpdateEvent OnUpdate;

		public delegate void OnUpdateCompleteEvent();
		public event OnUpdateCompleteEvent OnUpdateComplete;

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

		public virtual void Update () {
			DoUpdateBegin();
			DoUpdate();
			DoUpdateComplete();
		}

		protected virtual void DoUpdateBegin () {
			if( OnUpdateBegin != null )
				OnUpdateBegin();
		}

		protected virtual void DoUpdate () {
			if( OnUpdate != null )
				OnUpdate();
		}

		protected virtual void DoUpdateComplete () {
			if( OnUpdateComplete != null )
				OnUpdateComplete();
		}
	}
}
