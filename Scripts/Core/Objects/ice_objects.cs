// ##############################################################################
//
// ice_objects.cs | ICE.World.Objects.ICEObject | ICEDataObject | ICEInfoDataObject | ICEOwnerObject
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

namespace ICE.World.Objects
{
	[System.Serializable]
	public struct AxisInputData
	{
		public AxisInputType Type;
		public string Name;
		public int Value;

		public void Copy( AxisInputData _data )
		{
			Type = _data.Type;
			Name = _data.Name;
			Value = _data.Value;
		}
	}

	[System.Serializable]
	public abstract class ICEObject : System.Object 
	{

	}

	[System.Serializable]
	public abstract class ICEDataObject : ICEObject 
	{
		public ICEDataObject(){}
		public ICEDataObject( ICEDataObject _object )
		{
			Copy( _object );
		}

		public void Copy( ICEDataObject _object )
		{
			if( _object == null )
				return;
			
			Enabled = _object.Enabled;
			Foldout = _object.Foldout;
		}

		/// <summary>
		/// Enables or disables the use of the object.
		/// </summary>
		public bool Enabled = true;

		/// <summary>
		/// The foldout parameter is a display option and should be used in the editor only 
		/// </summary>
		public bool Foldout = true;


	}

	[System.Serializable]
	public abstract class ICEInfoDataObject : ICEDataObject 
	{
		public ICEInfoDataObject(){}
		public ICEInfoDataObject( ICEInfoDataObject _object ) : base( _object )
		{
			Copy( _object );
		}

		public void Copy( ICEInfoDataObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			InfoText = _object.InfoText;
			ShowInfoText = _object.ShowInfoText;
		}

		/// <summary>
		/// The info text.
		/// </summary>
		public string InfoText = "";

		/// <summary>
		/// The info text enabled.
		/// </summary>
		public bool ShowInfoText = false;

	}

	[System.Serializable]
	public abstract class ICEEntityObject : ICEInfoDataObject 
	{
		public ICEEntityObject(){}
		public ICEEntityObject( ICEEntityObject _object ) : base( _object )
		{
			Copy( _object );
		}

		public void Copy( ICEInfoDataObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );
		}

		[SerializeField, XmlIgnore]
		protected GameObject m_EntityGameObject = null;
		[XmlIgnore]
		public GameObject EntityGameObject{
			get{ return m_EntityGameObject = ( m_EntityGameObject == null ?( m_EntityComponent != null ? m_EntityComponent.gameObject:null ):m_EntityGameObject ); }
		}

		public void SetEntityGameObject( GameObject _object )
		{
			m_EntityGameObject = _object;
			m_EntityComponent = ( m_EntityGameObject != null ? m_EntityGameObject.GetComponent<ICEWorldEntity>():null );
		}

		[SerializeField, XmlIgnore]
		protected ICEWorldEntity m_EntityComponent = null;
		public virtual ICEWorldEntity EntityComponent{
			get{ return m_EntityComponent = ( m_EntityGameObject != null && m_EntityComponent == null ? m_EntityGameObject.GetComponent<ICEWorldEntity>():m_EntityComponent ); }
		}

		public virtual void Init( ICEWorldEntity _component ){
			m_EntityComponent = _component;
			m_EntityGameObject = ( m_EntityComponent != null ? m_EntityComponent.gameObject:null );
		}

		public string Name{
			get{ return ( EntityGameObject != null ? EntityGameObject.name:"" ); }
		}

		public string Tag{
			get{ return ( EntityGameObject != null ? EntityGameObject.tag:"" ); }
		}

		public int ID{
			get{ return ( EntityGameObject != null ? EntityGameObject.GetInstanceID():0 ); }
		}

	}


	/// <summary>
	/// ICEObject represents the abstract base class for all ICE related System Objects.
	/// </summary>
	[System.Serializable]
	public abstract class ICEOwnerObject : ICEInfoDataObject {

		/// <summary>
		/// Prints debug log.
		/// </summary>
		public bool EnableDebugLog = false;

		/// <summary>
		/// Prints the debug log.
		/// </summary>
		/// <param name="_log">Log.</param>
		public void PrintDebugLog( ICEOwnerObject _object, string _log )
		{
			if( EnableDebugLog || OwnerEnabledDebugLog )
				Debug.Log( OwnerName + " (" + OwnerInstanceID + ") - " + ( _object != null?_object.GetType().ToString() + " ":"" ) + _log );
		}

		/// <summary>
		/// m_Owner represents the owning GameObject
		/// </summary>
		[XmlIgnore]
		protected GameObject m_Owner = null;
		/// <summary>
		/// Gets the owning GameObject.
		/// </summary>
		/// <value>The parent or null</value>
		[XmlIgnore]
		public GameObject Owner{
			get{ return m_Owner = ( m_Owner == null ?( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null ):m_Owner ); }
		}

		/// <summary>
		/// The m parent represents the owner component
		/// </summary>
		[XmlIgnore]
		protected ICEWorldBehaviour m_OwnerComponent = null;
		/// <summary>
		/// Gets the owner component.
		/// </summary>
		/// <value>The parent or null</value>
		[XmlIgnore]
		public ICEWorldBehaviour OwnerComponent{
			get{ return m_OwnerComponent; }
		}

		/// <summary>
		/// Gets the name of the parent.
		/// </summary>
		/// <value>The name of the parent.</value>
		public string OwnerName{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.name:"" ); }
		}

		/// <summary>
		/// Gets the tag of the parent.
		/// </summary>
		/// <value>The name of the parent.</value>
		public string OwnerTag{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.tag:"" ); }
		}

		/// <summary>
		/// Gets the parent InstanceID.
		/// </summary>
		/// <value>The parent InstanceID or 0</value>
		public int OwnerInstanceID{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.ObjectInstanceID :0 ); }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.World.Objects.ICEObject"/> parent allows to print the debug log.
		/// </summary>
		/// <value><c>true</c> if parent print debug log; otherwise, <c>false</c>.</value>
		public bool OwnerEnabledDebugLog{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.UseDebugLogs:false ); }
		}
			
		public ICEOwnerObject(){}
		public ICEOwnerObject( ICEWorldBehaviour _component ){
			m_OwnerComponent = _component;
			m_Owner = ( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null );
		}
		public ICEOwnerObject( ICEOwnerObject _object ) : base( _object ){
			m_OwnerComponent = ( _object != null?_object.OwnerComponent:null );
			m_Owner = ( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null );

			EnableDebugLog = _object.EnableDebugLog;
		}

		/// <summary>
		/// Default Init method to initiate the object.
		/// </summary>
		/// <param name="_parent">Parent.</param>
		public virtual void Init( ICEWorldBehaviour _component ){
			m_OwnerComponent = _component;
			m_Owner = ( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null );
		}

		public void SetOwner( GameObject _object )
		{
			m_Owner = _object;
			m_OwnerComponent = ( m_Owner != null ? m_Owner.GetComponent<ICEWorldBehaviour>():null );
		}
	}

}
