// ##############################################################################
//
// ICE.World.ICEWorldAttribute.cs
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

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World
{
	/// <summary>
	/// ICEWorldAttribute is the base class for all ICEWorld based attributes. Attributes represents specific data classes 
	/// to enhance ICEWorldEntity based objects.
	/// </summary>
	public class ICEWorldAttribute : ICEWorldBehaviour {

		/// <summary>
		/// Gets the attributes.
		/// </summary>
		/// <returns>The attributes.</returns>
		public ICEWorldAttribute[] GetAttributes()
		{
			return transform.GetComponents<ICEWorldAttribute>();
		}

		/// <summary>
		/// Gets the attributes in children.
		/// </summary>
		/// <returns>The attributes in children.</returns>
		public ICEWorldAttribute[] GetAttributesInChildren()
		{
			return transform.GetComponentsInChildren<ICEWorldAttribute>();
		}

		/// <summary>
		/// Gets the attributes in parent.
		/// </summary>
		/// <returns>The attributes in parent.</returns>
		public ICEWorldAttribute[] GetAttributesInParent()
		{
			return transform.GetComponentsInParent<ICEWorldAttribute>();
		}
	}
}
