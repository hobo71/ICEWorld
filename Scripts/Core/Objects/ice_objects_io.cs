﻿// ##############################################################################
//
// ice_objects_io.cs | ICE.World.ICEWorldIO.cs
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

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World.Objects
{
	#if UNITY_EDITOR
	public class ICEIO : System.Object
	{
		protected static string m_Path = "";

		protected static string Save( string _name, string _suffix ){
			return UnityEditor.EditorUtility.SaveFilePanelInProject( "Save File As", _name.ToLower() + "." + _suffix , _suffix, "" );
		}

		protected static string Load( string _suffix ){
			return UnityEditor.EditorUtility.OpenFilePanel( "Open File", Application.dataPath, _suffix );
		}

		protected static void SaveObjectToFile<T>( T _object ) where T : ICEObject
		{
			XmlSerializer serializer = new XmlSerializer( typeof( T ) );
			FileStream _stream = new FileStream( m_Path, FileMode.Create);
			serializer.Serialize( _stream, _object );
			_stream.Close();
		}

		protected static T LoadObjectFromFile<T>( T _object ) where T : ICEObject
		{
			XmlSerializer serializer = new XmlSerializer(typeof( T ));
			FileStream _stream = new FileStream( m_Path, FileMode.Open);
			_object = serializer.Deserialize(_stream) as T;
			_stream.Close();

			return _object;
		}
	}
	#endif
}