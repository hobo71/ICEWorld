// *** ##############################################################################
//
// *** ice_utilities_types.cs
// *** Version 1.2.10
//
// *** Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// *** http://www.icecreaturecontrol.com
// *** mailto:support@icecreaturecontrol.com
//
// *** Permission is hereby granted, free of charge, to any person obtaining a copy 
// *** of this software and associated documentation files (the "Software"), to deal 
// *** in the Software without restriction, including without limitation the rights 
// *** to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// *** copies of the Software, and to permit persons to whom the Software is furnished 
// *** to do so, subject to the following conditions:
//
// *** The above copyright notice and this permission notice shall be included in all 
// *** copies or substantial portions of the Software.
//
// *** THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// *** INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// *** PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// *** HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// *** OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// *** SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// *** ##############################################################################

using UnityEngine;
using System.Collections;

namespace ICE.World.EnumTypes
{
	// *** A ***

	/// <summary>
	/// Animator control type.
	/// </summary>
	public enum AnimatorControlType
	{
		DIRECT,
		ADVANCED
	}
		
	/// <summary>
	/// Animation interface type.
	/// </summary>
	public enum AnimationInterfaceType
	{
		NONE=0,
		MECANIM,
		LEGACY,
		CLIP,
		CUSTOM
	}

	/// <summary>
	/// Axis input type.
	/// </summary>
	public enum AxisInputType
	{
		KeyOrMouseButton,
		MouseMovement,
		JoystickAxis
	}

	// *** B *** 

	/// <summary>
	/// Boolean value type.
	/// </summary>
	public enum BooleanValueType
	{
		TRUE=0,
		FALSE
	}

	// *** C *** 

	/// <summary>
	/// Conditional operator type.
	/// </summary>
	public enum ConditionalOperatorType
	{
		AND = 0,
		OR = 1
	}

	/// <summary>
	/// Supported Collider types.
	/// </summary>
	public enum ColliderType
	{
		Sphere,
		Box,
		Capsule
	}

	// *** D *** 

	/// <summary>
	/// Dynamic boolean value type.
	/// </summary>
	public enum DynamicBooleanValueType
	{
		IsGrounded,
		IsJumping,
		Deadlocked,
		MovePositionReached,
		MovePositionUpdateRequired,
		TargetMovePositionReached
	}

	/// <summary>
	/// Dynamic integer value type.
	/// </summary>
	public enum DynamicIntegerValueType
	{
		undefined
	}

	/// <summary>
	/// Dynamic float value type.
	/// </summary>
	public enum DynamicFloatValueType
	{
		ForwardSpeed,
		AngularSpeed,
		FallSpeed,
		Direction,
		Altitude,
		AbsoluteAltitude,
		MovePositionDistance
	}

	// *** E *** 
	// *** F *** 
	// *** G *** 

	/// <summary>
	/// Ground check type.
	/// </summary>
	public enum GroundCheckType
	{
		NONE,
		RAYCAST,
		SAMPLEHEIGHT
	}

	// *** H *** 
	// *** I *** 

	/// <summary>
	/// Influence type. TODO: BETA dynamic influences coming soon
	/// </summary>
	public enum InfluenceType
	{
		Unknown

	}

	// *** J *** 
	// *** K *** 
	// *** L *** 

	/// <summary>
	/// Logical operator type.
	/// </summary>
	public enum LogicalOperatorType
	{
		EQUAL = 0,
		NOT = 1,
		LESS = 2,
		LESS_OR_EQUAL = 3,
		GREATER = 4,
		GREATER_OR_EQUAL = 5
	}
	// *** M *** 

	/// <summary>
	/// Method parameter type.
	/// </summary>
	public enum BehaviourEventParameterType
	{
		None=0,
		Float,
		Integer,
		String,
		Boolean
	}
	// *** N *** 
	// *** O *** 

	/// <summary>
	/// Obstacle check type.
	/// </summary>
	public enum ObstacleCheckType
	{
		NONE,
		BASIC
	}

	// *** P *** 
	// *** Q *** 
	// *** R *** 

	/// <summary>
	/// Random offset type.
	/// </summary>
	public enum RandomOffsetType
	{
		EXACT,
		CIRCLE,
		HEMISPHERE,
		SPHERE
	}

	/// <summary>
	/// Random seed type.
	/// </summary>
	public enum RandomSeedType
	{
		DEFAULT = 0,
		TIME,
		CUSTOM
	}

	// *** S *** 

	/// <summary>
	/// String operator type.
	/// </summary>
	public enum StringOperatorType
	{
		EQUAL,
		NOT
	}

	/// <summary>
	/// Sequence order type.
	/// </summary>
	public enum SequenceOrderType
	{
		CYCLE,
		RANDOM,
		PINGPONG
	}

	// *** T *** 
	// *** U *** 
	// *** V *** 
	// *** W *** 
	// *** XYZ *** 
}