// ##############################################################################
//
// ICE.World.ICEWorldEntityDebug.cs
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

using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World
{
	[RequireComponent (typeof (ICEWorldEntity)), ExecuteInEditMode]
	public class ICEWorldEntityDebug : ICEWorldBehaviour {

		public float BaseOffset = 0;
		public float BaseOffsetMaximum = 1;
		public bool GroundedInEditorMode = false;

		public bool UseCustomGizmoColor = false;
		public Color CustomGizmoColor = Color.red;
		public float GizmoSize = 0.25f;


		public bool DrawSelectedOnly = false;

		public override void Update () {

			#if UNITY_EDITOR
			if( GroundedInEditorMode == true && ! Application.isPlaying && UnityEditor.Selection.activeGameObject == transform.gameObject )
				WorldRegister.SetGroundLevel( transform, BaseOffset );	
			#endif
		}

		public virtual void OnDrawGizmos(){
			DrawGizmos( ! DrawSelectedOnly );
		}

		public virtual void OnDrawGizmosSelected(){
			DrawGizmos( DrawSelectedOnly );
		}

		public virtual void DrawGizmos( bool _draw )
		{
			if( ! _draw || ! this.enabled )
				return;

			//if( CreatureRegister.UseDebug )

			Color _color = WorldRegister.GetDebugDefaultColor( this.gameObject );
			if( UseCustomGizmoColor )
				_color = CustomGizmoColor;

			Gizmos.color = _color;
			Gizmos.DrawSphere( this.transform.position, GizmoSize );
			//Gizmos.DrawWireCube( this.transform.position, Vector3.one );
		}
	}
}
