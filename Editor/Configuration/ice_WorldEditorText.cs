// ##############################################################################
//
// ice_WorldEditorText.cs
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

namespace ICE.World.EditorInfos
{
	public class Info : ICEEditorInfo
	{
		public static string Version = "1.2.10";

		public static readonly string ANIMATION = "Here you can define the desired animation you want use for the selected rule. " +
			"Simply choose the desired type and adapt the required settings.";

		public static readonly string ANIMATION_NAME = "TODO";
		public static readonly string ANIMATION_WRAP_MODE = "TODO";
		public static readonly string ANIMATION_SPEED = "TODO";
		public static readonly string ANIMATION_TRANSITION = "TODO";
		public static readonly string ANIMATION_NONE = "Animations are optional and not obligatory required, so you can " +
			"control also each kind of unanimated objects, such as dummies for testing and prototyping, simple bots and turrets or " +
			"movable waypoints.  ";
		public static readonly string ANIMATION_ANIMATOR = "By choosing the ANIMATOR ICECreatureControl will using the Animator " +
			"Interface to control Unity’s powerful Mecanim animation system. To facilitate setup and handling, ICECreatureControl provide " +
			"three different options to working with Mecanim: \n\n" +
			" - DIRECT – similar to the legacy animation handling \n" +
			" - CONDITIONS – triggering by specified values (float, integer, Boolean and trigger) \n" +
			" - ADVANCED – similar to CONDITIONS with additional settings for IK (ALPHA) \n";

		public static readonly string ANIMATION_ANIMATOR_CONTROL_TYPE_DIRECT = "DIRECT - similar to the legacy animation handling";
		public static readonly string ANIMATION_ANIMATOR_CONTROL_TYPE_CONDITIONS = "CONDITIONS – triggering by specified values (float, integer, Boolean and trigger)";
		public static readonly string ANIMATION_ANIMATOR_CONTROL_TYPE_ADVANCED = "ADVANCED – similar to CONDITIONS with additional settings for IK (ALPHA)";
		public static readonly string ANIMATION_ANIMATOR_CONTROL_TYPE_ADVANCED_INFO = "COMING SOON!";

		public static readonly string ANIMATION_ANIMATOR_ERROR_NO_CLIPS = "There are no clips available. Please check your Animator Controller!";
		public static readonly string ANIMATION_ANIMATION = "Working with legacy animations is the easiest and fastest way " +
			"to get nice-looking results. Simply select the desired animation, set the correct WrapMode and go.";
		public static readonly string ANIMATION_CLIP = "The direct use of animation clips is inadvisable and here only " +
			"implemented for the sake of completeness and for some single cases it could be helpful to have it. But apart from that it " +
			"works like the animation list. Simply assign the desired animation clip, set the correct WrapMode and go.";
		public static readonly string ANIMATION_CUSTOM = "";

		public static readonly string METHODS = "TODO";
		public static readonly string METHOD = "TODO";
		public static readonly string METHOD_POPUP = "TODO";
		public static readonly string METHOD_PARAMETER_BOOLEAN = "TODO";
		public static readonly string METHOD_PARAMETER_INTEGER = "TODO";
		public static readonly string METHOD_PARAMETER_FLOAT = "TODO";
		public static readonly string METHOD_PARAMETER_STRING = "TODO";
		public static readonly string MESSAGE_NAME = "TODO";

		public static readonly string IMPULSE_TIMER = "TODO";
		public static readonly string IMPULSE_TIMER_INTERVAL = "TODO";
		public static readonly string IMPULSE_TIMER_LIMITS = "TODO";
		public static readonly string IMPULSE_TIMER_BREAK_LENGTH = "TODO";

	}
}

