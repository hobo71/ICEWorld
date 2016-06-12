// ##############################################################################
//
// ICE.World.ICEWorldEntityDebug.cs
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

using UnityEditor;
using UnityEngine;
using System.Collections;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EditorUtilities;

namespace ICE.World
{
	[RequireComponent (typeof (ICEWorldEntityDebug))]
	public class ICEWorldEntityDebugEditor : ICEWorldBehaviourDebugEditor
	{
		public override void OnInspectorGUI()
		{
			ICEWorldEntityDebug _target = DrawDefaultHeader<ICEWorldEntityDebug>();
			DrawEntityDebugContent( _target );
			DrawDefaultFooter( _target );
		}

		public virtual void DrawEntityDebugContent( ICEWorldEntityDebug _target )
		{
			if( _target == null )
				return;


			_target.BaseOffset = ICEEditorLayout.DrawBaseOffsetGround( _target.transform, "Base Offset", "", _target.BaseOffset, ref _target.BaseOffsetMaximum, ref _target.GroundedInEditorMode, "" );

			_target.DrawSelectedOnly = ICEEditorLayout.Toggle( "Draw Selected Only", "" , _target.DrawSelectedOnly , "" );

			EditorGUILayout.Separator();
			_target.GizmoSize = ICEEditorLayout.DefaultSlider( "Gizmo Size", "", _target.GizmoSize, 0.01f, 0, 10, 0.25f, "");
		
			ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _target.UseCustomGizmoColor == false );
					_target.CustomGizmoColor = ICEEditorLayout.DefaultColor( "Custom Gizmo Color", "", _target.CustomGizmoColor, Color.red, "");
				EditorGUI.EndDisabledGroup();
				_target.UseCustomGizmoColor = ICEEditorLayout.ButtonCheck( "ENABLED", "", _target.UseCustomGizmoColor , ICEEditorStyle.ButtonMiddle );
			ICEEditorLayout.EndHorizontal( "TODO" );
		}
	}
}
