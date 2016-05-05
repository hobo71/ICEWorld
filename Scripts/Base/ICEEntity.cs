// ##############################################################################
//
// ICE.World.ICEEntity.cs
// Version 1.1.21
//
// The MIT License (MIT)
//
// Copyright © Pit Vetterick, ICE Technologies Consulting LTD. http://www.icecreaturecontrol.com (mailto:support@icecreaturecontrol.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files 
// (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, 
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do 
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// ##############################################################################

using UnityEngine;
using System.Collections;

namespace ICE.World
{
	/// <summary>
	/// ICE Entity. Abstract base class for all ICE related world objects.
	/// </summary>
	public abstract class ICEEntity : MonoBehaviour {

		private ICEWorld m_World = null;
		/// <summary>
		/// Gets the ICEWorld.
		/// </summary>
		/// <value>The current ICEWorld</value>
		public ICEWorld World{
			get{ return ( m_World == null?ICEWorld.Instance:null ); }
		}
			
		private ICEWorldEnvironment m_Environment = null;
		/// <summary>
		/// Gets the ICEWorldEnvironment.
		/// </summary>
		/// <value>The current ICEWorldEnvironment or null</value>
		public ICEWorldEnvironment Environment{
			get{ return ( m_Environment == null?ICEWorldEnvironment.Instance:null ); }
		}

		protected virtual void Start () {
		
		}
		
		protected virtual void Update () {
		
		}
	}
}
