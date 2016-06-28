// ##############################################################################
//
// ice_objects_corpse.cs | CorpseObject
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
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World.Objects
{

	[System.Serializable]
	public class CorpseObject : ICEOwnerObject {

		public CorpseObject(){}
		public CorpseObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );
			m_Corpse = null;
		}

		public void Reset()
		{
			m_Corpse = null;
		}

		[XmlIgnore]
		public GameObject CorpseReferencePrefab = null;
		public float CorpseRemovingDelay = 20.0f;
		public float CorpseRemovingDelayMaximum = 20.0f;
		public float CorpseRemovingDelayVariance = 0.25f;
		public bool UseCorpseScaling = false;

		private GameObject m_Corpse = null;
		public void SpawnCorpse()
		{
			if( Enabled == false || m_Corpse != null || m_Owner.activeInHierarchy == false || CorpseReferencePrefab == null )
				return;

			m_Corpse = WorldManager.Spawn( CorpseReferencePrefab, m_Owner.transform.position, m_Owner.transform.rotation );

			m_Corpse.name = CorpseReferencePrefab.name;
			SystemTools.CopyTransforms( m_Owner.transform, m_Corpse.transform );

			if( UseCorpseScaling )
				m_Corpse.transform.localScale = m_Owner.transform.localScale;

			if( CorpseRemovingDelay > 0 )
				GameObject.Destroy( m_Corpse, CorpseRemovingDelay + ( CorpseRemovingDelay * Random.Range( - CorpseRemovingDelayVariance, CorpseRemovingDelayVariance ) ) ); 
		}
	}
}
	
