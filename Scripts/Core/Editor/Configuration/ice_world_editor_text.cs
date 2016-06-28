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
		public static string Version = "1.0.1025";

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
		public static readonly string ANIMATION_EVENTS = "AnimationEvent lets you call a script function similar to SendMessage as part of playing back an " +
			"animation. Animation events support functions that take zero or one parameter. The parameter can be a float, an integer or a string. In cases " +
			"you would like to use also events with an object reference or an AnimationEvent you have to define such events directly in the Animation Window. \n\n" +
			"Please note that Animation Events calls their methods on MonoBehaviours of the ‘animated’ GameObject only, if you want to call a method within " +
			"one of its children you could use the Methods feature instead. Also please consider that AnimationEvents will be assigned directly to the " +
			"associated animation and will try to call their defined methods on each GameObject which used these animation, so please be careful by " +
			"defining such events.";
		public static readonly string ANIMATION_EVENTS_METHOD = "Use the Event section to define as much events as you want. For this simply enable the " +
			"section and ADD a new event. Disabling the section will deactivate all listed events, which means that all defined events will be activated in " +
			"the list and removed from the animation. To reset the complete Event section press the RES button, this removes all listed events completely.";
		public static readonly string ANIMATION_EVENTS_METHOD_POPUP = "The Event Popup provides you a preselected list with available methods. These methods " +
			"represents registered PublicMethods which are directly related to the game play and suitable to steering movements and/or behaviour (see also: ICE World, PublicMethods).\n\n" +
			"In addition to the listed methods, you can activate CUSTOM to enter arbitrary function names, in doing so you can define each available function you want. \n\n" +
			"By default a new created event will be inactive and not assigned to the animation, so you have to press ACTIVE first to assign an event to the selected " +
			"animation, also deactivate the ACTION flag to remove an event from the animation. To remove obsolete events completely from list press " +
			"the X button, this will removes both the listed event template and the assigned AnimationEvent.";
		public static readonly string ANIMATION_EVENTS_METHOD_TIME = "The time at which the event will be fired off.";


		public static readonly string CORPSE = "If Use Corpse is flagged you can assign a GameObject which will be used if your creature dies " +
			"(e.g. a Ragdoll Object of your creature which will be used instead of the original model). The corpse object have to be a prefab which will be " +
			"instantiate automatically if your creature dies.";
		public static readonly string CORPSE_REFERENCE = "The Corpse Prefab defines the prefab which will be used to replace the original object. Activate SCALE to allow" +
			"scaling the corpse according to the original object, but please note that this could results funny effects while scaling ragdolls.";
		public static readonly string CORPSE_REMOVING_DELAY = "Corpse Removing Delay defines the delay time in seconds until the spawned corpse object will " +
			"be removed from scene. You can adjust this value to zero to handle the removing process by using external scripts or to keep the spawned corpse durable " +
			"into your scene.";
		public static readonly string CORPSE_REMOVING_DELAY_VARIANCE = "The Variance Multiplier defines the threshold variance value, which will be used to " +
			"randomize the associated delay time during the runtime.";
		
		public static readonly string INPUT_POPUP = "TODO";

		public static readonly string EVENTS = "TODO";
		public static readonly string EVENT = "TODO";
		public static readonly string EVENT_POPUP = "The Event Popup displays all available Behaviour Events ";
		public static readonly string EVENT_PARAMETER_BOOLEAN = "Boolean parameter that is stored in the event and will be sent to the function.";
		public static readonly string EVENT_PARAMETER_INTEGER = "Int parameter that is stored in the event and will be sent to the function.";
		public static readonly string EVENT_PARAMETER_FLOAT = "Float parameter that is stored in the event and will be sent to the function.";
		public static readonly string EVENT_PARAMETER_STRING = "String parameter that is stored in the event and will be sent to the function.";
		public static readonly string EVENT_NAME = "TODO";

		public static readonly string EFFECT = "TODO";
		public static readonly string EFFECT_REFERENCE = "TODO";
		public static readonly string EFFECT_MOUNTPOINT = "TODO";
		public static readonly string EFFECT_OFFSET_TYPE = "TODO";
		public static readonly string EFFECT_OFFSET_POSITION = "TODO";
		public static readonly string EFFECT_OFFSET_RADIUS = "TODO";

		public static readonly string IMPULSE_TIMER = "TODO";
		public static readonly string IMPULSE_TIMER_TIME = "TODO";
		public static readonly string IMPULSE_TIMER_INTERVAL = "TODO";
		public static readonly string IMPULSE_TIMER_LIMITS = "TODO";
		public static readonly string IMPULSE_TIMER_SEQUENCE_LIMITS = "TODO";
		public static readonly string IMPULSE_TIMER_SEQUENCE_BREAK_LENGTH = "TODO";

		public static readonly string DURABILITY_INITIAL = "Initial Durability defines the fundamental capability of resistance of a creature in terms of " +
			"physical integrity and its vital fitness. The durability will be affected by several influences (e.g. damage, age etc. ) during the runtime and the " +
			"creature will die as soon as its durability is exhausted. By default the Initial Durability is adjusted to 100 but you are free to define a suitable " +
			"value according to your needs and requirements; the lower the value, the greater the impact of several influences and vice versa (e.g. increase this " +
			"value to 1000 for your level boss or decrease it to 10 for homely antagonists). Please note that changing the durability value is ineffective while " +
			"using influence values in percent, in such a case a damage of 50% for example will also reduce the durability value to 50%, independent of the defined " +
			"initial durability value. \n\nThe Initial Durability based on a minimum and maximum value, which allows you to define a random range. If you prefer to " +
			"define a fixed value, simply set minimum and maximum to the same value, also you can adapt the third field to modify the range of the slider. ";

		public static readonly string BODYPART = "TODO";

		public static readonly string BODYPART_DAMAGE_TRANSFER = "TODO";

		public static readonly string DURABILITY_PERCENT = "TODO";

		public static readonly string COLLIDER = "TODO";
		public static readonly string COLLIDER_INFO = "TODO";



		public static readonly string LIFESPAN = "Lifespan defines a default runtime limit (in seconds). If this limit is reached the object will remove itself.";
		public static readonly string LIFESPAN_INTERVAL = "TODO";
		public static readonly string LIFESPAN_DETACH = "TODO";



	}
}

