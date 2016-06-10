// ##############################################################################
//
// ICECreatureControlMenu.cs
// Version 1.2.10
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEditor;
using UnityEngine;

using System;
using System.IO;
using System.Text;
using System.Collections;

using ICE;
using ICE.World.EditorUtilities;
using ICE.World.Windows;


namespace ICE.World.Menus
{
	public class ICEWorldMenu : MonoBehaviour {

		[MenuItem ("ICE/ICE World/Repository", false, 9999 )]
		static void Repository (){
			Application.OpenURL("https://github.com/icetec/ICEWorld");
		}

		[MenuItem ("ICE/ICE World/Wiki", false, 9999 )]
		static void Wiki (){
			Application.OpenURL("https://github.com/icetec/ICEWorld/wiki");
		}
					
		[MenuItem ("ICE/ICE World/Template Designer (BETA)", false, 9999 )]
		static void ShowTemplateDesigner(){
			TemplateDesigner.Create();
		} 

		[MenuItem ("ICE/ICE World/About", false, 9999 )]
		static void ShowAbout(){
			ICEWorldAbout.Create();
		} 


		/*
		[MenuItem ("ICE/ICE Creature Control/Manual (online)", false, 2001 )]
		static void ManualOnline ()
		{
			Application.OpenURL("http://www.ice-technologies.de/unity/ICECreatureControl/ICECreatureControlManual.pdf");
		}

		[MenuItem ("ICE/ICE Creature Control/Homepage", false, 2001 )]
		static void Homepage ()
		{
			Application.OpenURL("http://www.icecreaturecontrol.com");
		}

		[MenuItem ("ICE/ICE Creature Control/FAQ", false, 2001 )]
		static void FAQ ()
		{
			Application.OpenURL("http://www.icecreaturecontrol.com/FAQ/" );
		}

		[MenuItem ("ICE/ICE Creature Control/Tutorials", false, 2001 )]
		static void Tutorials ()
		{
			Application.OpenURL("http://www.icecreaturecontrol.com/TUTORIALS/" );
		}

		[MenuItem ("ICE/ICE Creature Control/Bug Report", false, 2001 )]
		static void BugReport ()
		{
			Application.OpenURL("http://www.ice-technologies.de/mantis/");
		}

		[MenuItem ("ICE/ICE Creature Control/Unity Forum", false, 2001 )]
		static void UnityForum ()
		{
			Application.OpenURL("http://forum.unity3d.com/threads/347147/");
		}

		// WIZARD
		[MenuItem ("ICE/ICE Creature Control/Wizard", false, 8000 )]
		static void Wizard ()
		{
			ice_CreatureWizard.Create();
		}

		// ABOUT
		[MenuItem ("ICE/ICE Creature Control/About", false, 9000 )]
		static void AboutICE ()
		{
			ice_CreatureAbout.Create();
		} */


	}
}