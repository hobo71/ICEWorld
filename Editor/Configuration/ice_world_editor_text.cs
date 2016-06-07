// ##############################################################################
//
// ice_world_editor_text.cs | Info
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

namespace ICE.World.EditorInfos
{
	public class Info : ICEEditorInfo
	{
		public static string Version = "1.2.10";

		public static readonly string BASE_OFFSET = "Base Offset defines the relative vertical displacement of the owning GameObject.";

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
		public static readonly string ANIMATION_CUSTOM = "While using CUSTOM animation you could handle the desired animation for this behaviour rule by your own " +
			"script. OnCustomAnimation and OnCustomAnimationUpdate ";
		public static readonly string ANIMATION_EVENTS = "AnimationEvent lets you call a script function similar to SendMessage " +
			"as part of playing back an animation. Animation events support functions that take zero or one parameter. The parameter can " +
			"be a float, an int or a string. In cases you would like to use also events with an object reference or an AnimationEvent you " +
			"have to define such events directly in the Animation Window.\n\n" +
			"Please note that Animation Events calls their methods on every MonoBehaviour in this GameObject only, if you want " +
			"to call a method within one of its children you could use the Methods feature instead.";
		public static readonly string ANIMATION_EVENTS_METHOD = "The name of the function that will be called.";
		public static readonly string ANIMATION_EVENTS_TIME = "The time at which the event will be fired off.";

		public static readonly string METHODS = "TODO";
		public static readonly string METHOD = "TODO";
		public static readonly string METHOD_POPUP = "The Method Popup displays all available PublicMethods ";
		public static readonly string METHOD_PARAMETER_BOOLEAN = "Boolean parameter that is stored in the event and will be sent to the function.";
		public static readonly string METHOD_PARAMETER_INTEGER = "Int parameter that is stored in the event and will be sent to the function.";
		public static readonly string METHOD_PARAMETER_FLOAT = "Float parameter that is stored in the event and will be sent to the function.";
		public static readonly string METHOD_PARAMETER_STRING = "String parameter that is stored in the event and will be sent to the function.";
		public static readonly string MESSAGE_NAME = "TODO";

		public static readonly string IMPULSE_TIMER = "TODO";
		public static readonly string IMPULSE_TIMER_INTERVAL = "TODO";
		public static readonly string IMPULSE_TIMER_LIMITS = "TODO";
		public static readonly string IMPULSE_TIMER_BREAK_LENGTH = "TODO";

		public static readonly string STATUS_INITIAL_DURABILITY = "Initial Durability defines the fundamental capability of resistance of a creature in terms of " +
			"physical integrity and its vital fitness. The durability will be affected by several influences (e.g. damage, age etc. ) during the runtime and the " +
			"creature will die as soon as its durability is exhausted. By default the Initial Durability is adjusted to 100 but you are free to define a suitable " +
			"value according to your needs and requirements; the lower the value, the greater the impact of several influences and vice versa (e.g. increase this " +
			"value to 1000 for your level boss or decrease it to 10 for homely antagonists). Please note that changing the durability value is ineffective while " +
			"using influence values in percent, in such a case a damage of 50% for example will also reduce the durability value to 50%, independent of the defined " +
			"initial durability value. \n\nThe Initial Durability based on a minimum and maximum value, which allows you to define a random range. If you prefer to " +
			"define a fixed value, simply set minimum and maximum to the same value, also you can adapt the third field to modify the range of the slider. ";

	}
}

