// ##############################################################################
//
// ice_animation.cs | ICE.World.Utilities.AnimationTools
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

namespace ICE.World.Utilities
{
	public static class AnimationTools 
	{
		/// <summary>
		/// Compares two animation curves.
		/// </summary>
		/// <returns><c>true</c>, if animation curve was compared, <c>false</c> otherwise.</returns>
		/// <param name="_curve_1">Curve 1.</param>
		/// <param name="_curve_2">Curve 2.</param>
		public static bool CompareAnimationCurve( AnimationCurve _curve_1, AnimationCurve _curve_2 )
		{
			if( _curve_1.length != _curve_2.length )
				return false;
			else
			{
				for( int i = 0; i < _curve_1.length ; i++ )
				{
					if( _curve_1[i].time != _curve_2[i].time ||
						_curve_1[i].value != _curve_2[i].value ||
						_curve_1[i].inTangent != _curve_2[i].inTangent  ||
						_curve_1[i].outTangent != _curve_2[i].outTangent   ||
						_curve_1[i].tangentMode != _curve_2[i].tangentMode )
						return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Gets the name of the animation state.
		/// </summary>
		/// <returns>The animation state name.</returns>
		/// <param name="_animator">Animator.</param>
		/// <param name="_name">Name.</param>
		public static string GetAnimationStateName( Animator _animator, string _name )
		{
			if( string.IsNullOrEmpty( _name ) || _animator == null || _animator.runtimeAnimatorController == null )
				return "";
			#if UNITY_EDITOR
			UnityEditor.Animations.AnimatorController _controller = _animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;

			foreach( UnityEditor.Animations.AnimatorControllerLayer _layer in _controller.layers )
			{
				UnityEditor.Animations.AnimatorStateMachine _state_machine = _layer.stateMachine;

				foreach( UnityEditor.Animations.ChildAnimatorState _state in _state_machine.states )
				{
					if( _state.state != null && _state.state.motion != null && _state.state.motion.name == _name )
						return _state.state.name;
				}
			}
			#endif
			return "";
		}

		/// <summary>
		/// Determines if the specified _object has valid animations components.
		/// </summary>
		/// <returns><c>true</c> if the specified _object has animations; otherwise, <c>false</c>.</returns>
		/// <param name="_object">Object.</param>
		public static bool HasAnimations( GameObject _object )
		{
			if( _object != null && (
				( _object.GetComponentInChildren<Animation>() != null && _object.GetComponentInChildren<Animation>().GetClipCount() > 0 ) ||
				( _object.GetComponentInChildren<Animator>() != null && _object.GetComponentInChildren<Animator>().runtimeAnimatorController != null ) ) )
				return true;
			else
				return false;
		}

		/// <summary>
		/// Get AnimationState by Index
		/// </summary>
		/// <returns>The state by index.</returns>
		/// <param name="_control">_control.</param>
		/// <param name="index">Index.</param>
		public static AnimationState GetAnimationStateByName( GameObject _object, string _name )
		{
			Animation _anim = _object.GetComponentInChildren<Animation>();

			if( _anim == null )
				return null;

			foreach (AnimationState state in _anim )
			{
				if ( state.name == _name)
					return state;
			}
			return null;
		}

		/// <summary>
		/// Get AnimationName by Index 
		/// </summary>
		/// <returns>The index to name.</returns>
		/// <param name="_control">_control.</param>
		/// <param name="index">Index.</param>
		private static string GetAnimationNameByIndex( GameObject _object, int _index)
		{
			AnimationState state = GetAnimationStateByIndex( _object, _index );
			if (state == null)
				return "";

			return state.name;
		}

		/// <summary>
		/// Get AnimationState by Index
		/// </summary>
		/// <returns>The state by index.</returns>
		/// <param name="_control">_control.</param>
		/// <param name="index">Index.</param>
		public static AnimationState GetAnimationStateByIndex( GameObject _object, int index)
		{
			Animation _anim = _object.GetComponentInChildren<Animation>();

			if( _anim == null )
				return null;

			int i = 0;
			foreach (AnimationState state in _anim )
			{
				if (i == index)
					return state;
				i++;
			}
			return null;
		}

		/*
		public static UnityEditor.Animations.ChildAnimatorState GetChildAnimatorStates( Animator _animator, int _layer_index )
		{
			UnityEditor.Animations.AnimatorController _controller = _animator.runtimeAnimatorController as  UnityEditor.Animations.AnimatorController;

			UnityEditor.Animations.AnimatorControllerLayer _layer = _controller.layers[ _layer_index ];

			UnityEditor.Animations.AnimatorStateMachine _state_machine = _layer.stateMachine;

			return  _state_machine.states;
		}*/

	}




}