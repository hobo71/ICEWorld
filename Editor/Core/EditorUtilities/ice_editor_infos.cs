// ##############################################################################
//
// ice_editor_info.cs | ICEEditorInfo
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

namespace ICE.World.EditorUtilities
{
	/// <summary>
	/// ICE editor info.
	/// </summary>
	public class ICEEditorInfo
	{
		public static bool HelpEnabled = false;
		public static bool DescriptionEnabled = true;

		public static void Desc( string _text )
		{
			if( _text != "" )
				EditorGUILayout.HelpBox( _text , MessageType.None); 
		}

		public static void Help( string _text )
		{
			if( HelpEnabled && _text != "" )
				EditorGUILayout.HelpBox( _text , MessageType.None); 
		}

		public static void Note( string _text )
		{
			EditorGUILayout.HelpBox( _text , MessageType.None); 
		}

		public static void Warning( string _text )
		{
			EditorGUILayout.HelpBox( _text , MessageType.Warning); 
		}

		public static int HelpButtonIndex = 0;
		public static bool[] HelpFlag = new bool[1000];
		public static void HelpButton()
		{
			if( HelpEnabled == true )
				return;

			HelpButtonIndex++;

			if( HelpFlag[HelpButtonIndex] == true )
				GUI.backgroundColor = Color.yellow;
			else
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

			if (GUILayout.Button( "?", ICEEditorStyle.InfoButton ))
				HelpFlag[HelpButtonIndex] = ! HelpFlag[HelpButtonIndex];

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		}

		public static void HelpButton( Rect _rect )
		{
			if( HelpEnabled == true )
				return;

			HelpButtonIndex++;

			if( HelpFlag[HelpButtonIndex] == true )
				GUI.backgroundColor = Color.yellow;
			else
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

			if( GUI.Button( _rect, new GUIContent( "?", ""), ICEEditorStyle.InfoButton ) )
				HelpFlag[HelpButtonIndex] = ! HelpFlag[HelpButtonIndex];

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		}

		public static void ShowHelp( string _text )
		{
			if( ( HelpEnabled || HelpFlag[HelpButtonIndex] ) && _text != "" )
				EditorGUILayout.HelpBox( _text , MessageType.None); 
		}
	}
}