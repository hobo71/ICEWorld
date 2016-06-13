// ##############################################################################
//
// ice_objects_effect.cs | ICE.World.Objects.EffectDataObject | EffectObject
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
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

namespace ICE.World.Objects
{
	[System.Serializable]
	public class EffectDataObject : ICEImpulsTimerObject
	{
		public EffectDataObject(){}
		public EffectDataObject( ICEWorldBehaviour _component ) : base( _component ){}
		public EffectDataObject( EffectDataObject _effect ) : base( _effect as ICEImpulsTimerObject )
		{
			Init( _effect.OwnerComponent );

			ReferenceObject = _effect.ReferenceObject;
			Offset = _effect.Offset;
			Rotation = _effect.Rotation;
			OffsetType = _effect.OffsetType;
			OffsetRadius = _effect.OffsetRadius;
			Detach = _effect.Detach;
			MountPointName = _effect.MountPointName;
		}

		public override void Init (ICEWorldBehaviour _component)
		{
			base.Init (_component);
		}
			
		[XmlIgnore]
		public GameObject ReferenceObject = null;
		public Vector3 Offset = Vector3.zero;
		public Quaternion Rotation = Quaternion.identity;
		public RandomOffsetType OffsetType = RandomOffsetType.EXACT;
		public float OffsetRadius = 0;
		public float OffsetRadiusMaximum = 15;

		public bool Detach = false;

		public string MountPointName = "";
		protected Transform m_MountPointTransform = null;
	}

	[System.Serializable]
	public class EffectObject : EffectDataObject
	{
		public EffectObject(){}
		public EffectObject( ICEWorldBehaviour _component ) : base( _component ) {}
		public EffectObject( EffectObject _effect ) : base( _effect as EffectDataObject ){}

		[XmlIgnore]
		private GameObject m_CurrentEffect = null;
		public void Start( GameObject _owner ) 
		{
			m_Owner = _owner;		
			Start();

			/*
	


			if( Enabled == true && ReferenceObject != null )
			{

			}
			else if( Enabled == false && CurrentEffect != null )
			{
				GameObject.Destroy( CurrentEffect );
				CurrentEffect = null;
			}*/
		}

		public override void Stop() 
		{
			base.Stop();

			if( Detach == false && m_CurrentEffect != null )
				m_CurrentEffect.SetActive( false );
		}

		protected override void Action()
		{
			InitializeEffect();
		}

		public void InitializeEffect()
		{
			if( m_Owner == null || ReferenceObject == null )
				return;

			if( m_CurrentEffect == null )
			{
				GameObject _effect = InstantiateNewEffect();

				if( Detach == false )
					m_CurrentEffect = _effect;
			}
			else
				m_CurrentEffect.SetActive( true );
		}

		private GameObject InstantiateNewEffect()
		{
			if( m_Owner == null )
				return null;

			if( ! string.IsNullOrEmpty( MountPointName ) && ( m_MountPointTransform == null || m_MountPointTransform.name != MountPointName ) )
				m_MountPointTransform = SystemTools.FindChildByName( MountPointName, m_Owner.transform ); 

			if( m_MountPointTransform == null )
				m_MountPointTransform = m_Owner.transform;

			Vector3 _position = m_MountPointTransform.position;
			Vector3 _offset = Vector3.zero;

			if( OffsetType == RandomOffsetType.EXACT )
			{
				_offset = Offset;
			}
			else if( OffsetRadius > 0 )
			{
				Vector2 _pos = Random.insideUnitCircle * OffsetRadius;

				_offset.x = _pos.x;
				_offset.z = _pos.y;

				if( OffsetType == RandomOffsetType.HEMISPHERE )
					_offset.y = Random.Range(0,OffsetRadius ); 
				else if( OffsetType == RandomOffsetType.SPHERE )
					_offset.y = Random.Range( - OffsetRadius , OffsetRadius ); 
			}

			_position = PositionTools.GetWorldPosition( m_MountPointTransform, _offset );

			GameObject _effect = (GameObject)Object.Instantiate( ReferenceObject, _position, Quaternion.identity );

			if( _effect != null )
			{
				_effect.name = ReferenceObject.name;
				_effect.transform.rotation = Rotation;

				if( Detach == false )
				{
					_effect.transform.SetParent( m_MountPointTransform, true );

				}

				_effect.SetActive( true );
			}

			return _effect;
		}
	}
}


