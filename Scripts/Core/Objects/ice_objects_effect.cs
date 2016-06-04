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
			IntervalMin = _effect.IntervalMin;
			IntervalMax = _effect.IntervalMax;
			MaxInterval = _effect.MaxInterval;
			ParentName = _effect.ParentName;
		}

		public override void Init (ICEWorldBehaviour _component)
		{
			base.Init (_component);
		}

		protected float m_IntervalTimer = 0;
		protected float m_Interval = 1;
		protected Transform m_Parent;

		[XmlIgnore]
		public GameObject ReferenceObject;
		public Vector3 Offset;
		public Quaternion Rotation;
		public RandomOffsetType OffsetType;
		public float OffsetRadius;
		public bool Detach;
		public float IntervalMin;
		public float IntervalMax;
		public float MaxInterval;
		public string ParentName;
	}

	[System.Serializable]
	public class EffectObject : EffectDataObject
	{
		public EffectObject(){}
		public EffectObject( ICEWorldBehaviour _component ) : base( _component ) {}
		public EffectObject( EffectObject _effect ) : base( _effect as EffectDataObject ){}

		private GameObject InstantiateNewEffect( GameObject _owner )
		{
			if( _owner == null )
				return null;

			Vector3 _position = _owner.transform.position;
			Vector3 _local_offset = Vector3.zero;

			if( OffsetType == RandomOffsetType.EXACT )
			{
				_local_offset = Offset;
			}
			else if( OffsetRadius > 0 )
			{
				Vector2 _pos = Random.insideUnitCircle * OffsetRadius;

				_local_offset.x = _pos.x;
				_local_offset.z = _pos.y;

				if( OffsetType == RandomOffsetType.HEMISPHERE )
					_local_offset.y = Random.Range(0,OffsetRadius ); 
				else if( OffsetType == RandomOffsetType.SPHERE )
					_local_offset.y = Random.Range( - OffsetRadius , OffsetRadius ); 
			}

			if( _local_offset != Vector3.zero )
			{
				_local_offset.x = _local_offset.x/_owner.transform.lossyScale.x;
				_local_offset.y = _local_offset.y/_owner.transform.lossyScale.y;
				_local_offset.z = _local_offset.z/_owner.transform.lossyScale.z;

				_position = _owner.transform.TransformPoint( _local_offset );
			}

			if( Rotation == Quaternion.identity )
				Rotation = _owner.transform.rotation;

			GameObject _effect = (GameObject)Object.Instantiate( ReferenceObject, _position, Rotation );

			if( _effect == null )
				return null;

			_effect.name = ReferenceObject.name;

			if( Detach == false )
			{
				if( m_Parent == null || ( ParentName != null && m_Parent.name != ParentName ) )
					m_Parent = SystemTools.FindChildByName( ParentName, _owner.transform ); 
				else
					m_Parent = _owner.transform;

				_effect.transform.SetParent( m_Parent, true );

				/*
				 *_effect.transform.parent = m_Parent;
				_effect.transform.position = m_Parent.position;
				_effect.transform.rotation = m_Parent.rotation;*/
			}

			_effect.SetActive( true );

			return _effect;
		}

		[XmlIgnore]
		public GameObject CurrentEffect;
		public void Start( GameObject _owner ) 
		{
			if( _owner == null )
				return;

			m_IntervalTimer = 0;
			m_Interval = Random.Range( IntervalMin, IntervalMax );

			if( Enabled == true && ReferenceObject != null )
			{
				if( CurrentEffect == null )
				{
					GameObject _effect = InstantiateNewEffect( _owner );

					if( Detach == false )
						CurrentEffect = _effect;
				}
				else
					CurrentEffect.SetActive( true );
			}
			else if( Enabled == false && CurrentEffect != null )
			{
				GameObject.Destroy( CurrentEffect );
				CurrentEffect = null;
			}
		}

		public override void Stop() 
		{
			base.Stop();
			if( CurrentEffect != null && Detach == false )
				CurrentEffect.SetActive( false );
		}

		public void Update( GameObject _owner )
		{
			base.Update();

			if( Mathf.Max( IntervalMin, IntervalMax ) > 0 && Detach == true )
			{
				m_IntervalTimer += Time.deltaTime;

				if( m_IntervalTimer > m_Interval )
					Start( _owner );
			}
		}

	}
}


