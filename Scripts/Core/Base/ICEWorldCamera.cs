﻿// ##############################################################################
//
// ICE.World.ICEWorldCamera.cs
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

using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World
{
	using UnityEngine;
	using System.Collections;

	/// <summary>
	/// ICE world camera.
	/// </summary>
	public class ICEWorldCamera : ICEWorldBehaviour {

		[SerializeField]
		private UnderwaterCameraEffect m_UnderwaterEffect = null;
		public UnderwaterCameraEffect Underwater{
			get{ return m_UnderwaterEffect = ( m_UnderwaterEffect == null ? new UnderwaterCameraEffect( this ) : m_UnderwaterEffect ); }
			set{ m_UnderwaterEffect = value; }
		}

		/// <summary>
		/// Start this instance.
		/// </summary>
		public override void Start () {
			Underwater.Init( this );
		}
			
		/// <summary>
		/// Raises the trigger enter event.
		/// </summary>
		/// <param name="_other">Other.</param>
		public virtual void OnTriggerEnter( Collider _other ){
			Underwater.CheckColliderEnterOrStay( _other );
		}

		/// <summary>
		/// Raises the trigger stay event.
		/// </summary>
		/// <param name="_other">Other.</param>
		public virtual void OnTriggerStay( Collider _other ){
			Underwater.CheckColliderEnterOrStay( _other );
		}

		/// <summary>
		/// Raises the trigger exit event.
		/// </summary>
		/// <param name="_other">Other.</param>
		public virtual void OnTriggerExit( Collider _other ){
			Underwater.CheckColliderExit( _other );
		}
	}
}
