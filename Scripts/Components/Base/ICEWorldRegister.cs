﻿// ##############################################################################
//
// ICE.World.ICEWorldRegistry.cs
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

namespace ICE.World
{
	public class ICEWorldRegister : ICEWorld {

		protected static new ICEWorldRegister m_Instance = null;
		public static new ICEWorldRegister Instance{
			get{ return m_Instance = ( m_Instance == null?GameObject.FindObjectOfType<ICEWorldRegister>():m_Instance ); }
		}

		public virtual void Register( GameObject _object )
		{
		}

		public virtual bool Deregister( GameObject _object )
		{
			return true;
		}

		public virtual void Remove( GameObject _object )
		{
			DestroyObject( _object );
		}
	}

	public static class WorldRegister{

		public static void Register( GameObject _object )
		{
			if( ICEWorldRegister.Instance != null )
				ICEWorldRegister.Instance.Register( _object );
		}

		public static void Deregister( GameObject _object )
		{
			if( ICEWorldRegister.Instance != null )
				ICEWorldRegister.Instance.Deregister( _object );
		}

		public static void Remove( GameObject _object )
		{
			if( ICEWorldRegister.Instance != null )
				ICEWorldRegister.Instance.Remove( _object );
		}
	}
}
