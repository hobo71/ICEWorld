// ##############################################################################
//
// ICECreatureTargetEditor.cs
// Version 1.2.10
//
// © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE;
using ICE.World;
using ICE.World.EditorUtilities;
using ICE.World.Utilities;

namespace ICE.World
{
	[CustomEditor(typeof(ICEWorldBehaviour))]
	public class ICEWorldBehaviourEditor : Editor 
	{
		private ICEWorldBehaviour m_target;
		protected EditorHeaderType m_HeaderType = EditorHeaderType.TOGGLE;

		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI()
		{
			m_target = DrawDefaultHeader<ICEWorldBehaviour>();

			DrawDefaultFooter( m_target );

		}

		/// <summary>
		/// Draws the default header.
		/// </summary>
		/// <returns>The default header.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public virtual T DrawDefaultHeader<T>() where T : ICEWorldBehaviour
		{
			T _target = (T)target;

			GUI.changed = false;
			//Info.HelpButtonIndex = 0;

			EditorGUILayout.Separator();
			return _target;
		}

		/// <summary>
		/// Draws the default footer.
		/// </summary>
		/// <param name="_target">Target.</param>
		public virtual void DrawDefaultFooter( ICEWorldBehaviour _target )
		{
			if( _target == null )
				return;

			EditorGUILayout.Separator();
			if( GUI.changed )
				EditorUtility.SetDirty( _target );
		}
	}
}

