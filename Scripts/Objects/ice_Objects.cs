using UnityEngine;
using System.Collections;

using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace ICE.World.Objects
{
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
		public bool Enabled = false;

		/// <summary>
		/// The foldout parameter is a display option and should be used in the editor only 
		/// </summary>
		public bool Foldout = false;


	}

	/// <summary>
	/// ICEObject represents the abstract base class for all ICE related System Objects.
	/// </summary>
	[System.Serializable]
	public abstract class ICEOwnerObject : ICEDataObject {

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
		/// Gets a value indicating whether this <see cref="ICE.World.Objects.ICEObject"/> parent allows to print the debug log.
		/// </summary>
		/// <value><c>true</c> if parent print debug log; otherwise, <c>false</c>.</value>
		public bool OwnerEnabledDebugLog{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.UseDebugLogs:false ); }
		}

		/// <summary>
		/// Gets the parent InstanceID.
		/// </summary>
		/// <value>The parent InstanceID or 0</value>
		public int OwnerInstanceID{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.InstanceID :0 ); }
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
	}

}
