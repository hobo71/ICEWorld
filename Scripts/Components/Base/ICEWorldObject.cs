// ##############################################################################
//
// ICE.World.ICEObject.cs
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
	public class ICEWorldObject : ICEWorldEntity {

		public float Mass = 1;
		public Vector3 Size = Vector3.one;

		public float Integrity = 100;
		public float IntegrityMaximum = 100;

		protected Rigidbody m_Rigidbody = null;
		public Rigidbody ObjectRigidbody{
			get{ return m_Rigidbody = ( m_Rigidbody == null?GetComponent<Rigidbody>():m_Rigidbody ); }
		}

		protected Renderer[] m_Renderer = null;
		public Renderer[] ObjectRenderer{
			get{ return m_Renderer = ( m_Renderer == null?GetComponentsInChildren<Renderer>():m_Renderer ); }
		}

		protected Mesh[] m_Meshes = null;
		public Mesh[] ObjectMeshes{
			get{ return m_Meshes = ( m_Meshes == null?GetComponentsInChildren<Mesh>():m_Meshes ); }
		}

		protected Transform[] m_Transforms = null;
		public Transform[] ObjectTransforms {
			get{ return m_Transforms  = ( m_Transforms == null?GetComponentsInChildren<Transform>():m_Transforms ); }
		}

		public override void Start () {
			base.Start();
		}

		public override void Update () {

		}
	}
}