// ##############################################################################
//
// ICE.World.Objects.LifespanObject.cs
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
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace ICE.World.Objects
{
	[System.Serializable]
	public class BasicStatusDataObject : ICEObject
	{
		public float Integrity = 100;
		public float MaxIntegrity = 100;
		public float MaxIntegrityMaximum = 100;

		public bool UseAging = false;
		public float MaxAge = 60f;	
		public float MaxAgeMaximum = 60f;

		protected float m_Age = 0.0f;
		public float Age{ 
			get{ return m_Age; }
		}

		public void SetAge( float _age )
		{ 
			if( _age >= 0 && _age <= MaxAge )
				m_Age = _age;
		}
	}

	[System.Serializable]
	public class BasicStatusObject : BasicStatusDataObject
	{
		public virtual void Reset()
		{
		}

		public virtual void Update()
		{
			if( UseAging )
			{
				m_Age +=  Time.deltaTime;
			}
		}
	}
}
