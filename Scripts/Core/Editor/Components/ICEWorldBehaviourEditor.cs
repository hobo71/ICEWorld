// ##############################################################################
//
// ICEWorldBehaviourEditor.cs | ICEWorldBehaviourEditor : Editor
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
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE;
using ICE.World;
using ICE.World.EditorUtilities;
using ICE.World.EditorInfos;
using ICE.World.Utilities;

namespace ICE.World
{
	[CustomEditor(typeof(ICEWorldBehaviour))]
	public class ICEWorldBehaviourEditor : Editor 
	{
		protected EditorHeaderType m_HeaderType = EditorHeaderType.FOLDOUT_ENABLED_BOLD;

		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI()
		{
			ICEWorldBehaviour _target = DrawDefaultHeader<ICEWorldBehaviour>();

			DrawDefaultFooter( _target );

		}

		/// <summary>
		/// Draws the default header.
		/// </summary>
		/// <returns>The default header.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public virtual T DrawDefaultHeader<T>() where T : ICEWorldBehaviour
		{
			ICEEditorLayout.SetDefaults();
			T _target = (T)target;

			GUI.changed = false;
			Info.HelpButtonIndex = 0;

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

		public virtual T DrawMonoHeader<T>() where T : MonoBehaviour
		{
			ICEEditorLayout.SetDefaults();
			T _target = (T)target;

			GUI.changed = false;
			Info.HelpButtonIndex = 0;

			EditorGUILayout.Separator();
			return _target;
		}

		public virtual void DrawMonoFooter( MonoBehaviour _target )
		{
			if( _target == null )
				return;

			EditorGUILayout.Separator();
			if( GUI.changed )
				EditorUtility.SetDirty( _target );
		}
	}
}

