// ##############################################################################
//
// ice_editor_templates.cs | ICEWorldTemplateDesigner
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
using UnityEditor;

using System;
using System.IO;
using System.Text;
using System.Collections;

namespace ICE.World.EditorUtilities
{
	public class ICEWorldTemplateData : System.Object
	{
		public string ProjectName = "MyProject";
		public string Namespace = "MyNamespace";
	}

	public class ICEWorldTemplateDesigner
	{
		public static void CreateWorldTemplate( ICEWorldTemplateData _data )
		{
			if( string.IsNullOrEmpty( _data.ProjectName ) )
				_data.ProjectName = "MYWORLD_PROJECT";
	
			if( string.IsNullOrEmpty( _data.Namespace ) )
				_data.Namespace = "MYWORLD";

			string guid = AssetDatabase.CreateFolder("Assets", _data.ProjectName );
			string _template_path = AssetDatabase.GUIDToAssetPath(guid);

				guid = AssetDatabase.CreateFolder( _template_path, "Editor");
				string _editor_path = AssetDatabase.GUIDToAssetPath(guid);

					guid = AssetDatabase.CreateFolder( _editor_path, "Components");
					string _editor_components_path = AssetDatabase.GUIDToAssetPath(guid);

						guid = AssetDatabase.CreateFolder( _editor_components_path, "Debug");
						string _editor_components_debug_path = AssetDatabase.GUIDToAssetPath(guid);

					guid = AssetDatabase.CreateFolder( _editor_path, "Configuration");
					string _editor_configuration_path = AssetDatabase.GUIDToAssetPath(guid);

						PrintInfoFile( _editor_configuration_path, _data.Namespace );
						PrintInitFile( _editor_configuration_path, _data.Namespace );

					guid = AssetDatabase.CreateFolder( _editor_path, "Core");
					string _editor_core_path = AssetDatabase.GUIDToAssetPath(guid);

					guid = AssetDatabase.CreateFolder( _editor_path, "Recources");
					string _editor_recources_path = AssetDatabase.GUIDToAssetPath(guid);

			guid = AssetDatabase.CreateFolder( _template_path, "Scripts");
			string _scripts_path = AssetDatabase.GUIDToAssetPath(guid);

				guid = AssetDatabase.CreateFolder( _scripts_path, "Components");
				string _scripts_components_path = AssetDatabase.GUIDToAssetPath(guid);

					guid = AssetDatabase.CreateFolder( _scripts_components_path, "Debug");
					string _scripts_components_debug_path = AssetDatabase.GUIDToAssetPath(guid);

					//CreateScript( _scripts_components_path, "MyNewComponent" );

				guid = AssetDatabase.CreateFolder( _scripts_path, "Core");
				string _scripts_core_path = AssetDatabase.GUIDToAssetPath(guid);

				guid = AssetDatabase.CreateFolder( _scripts_core_path, "Base");
				string _scripts_core_base_path = AssetDatabase.GUIDToAssetPath(guid);

				guid = AssetDatabase.CreateFolder( _scripts_core_path, "Objects");
				string _scripts_core_objects_path = AssetDatabase.GUIDToAssetPath(guid);

				guid = AssetDatabase.CreateFolder( _scripts_core_path, "Utilities");
				string _scripts_core_utilities_path = AssetDatabase.GUIDToAssetPath(guid);
	
		}

		public static void PrintHead( StreamWriter _outfile, string _filename, string _classname )
		{
			_outfile.WriteLine("// ##############################################################################");
			_outfile.WriteLine("//");
			_outfile.WriteLine("// " + _filename + ".cs | " + _classname );
			_outfile.WriteLine("// Version 0.1");
			_outfile.WriteLine("//");
			_outfile.WriteLine("// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.");
			_outfile.WriteLine("// http://www.icecreaturecontrol.com");
			_outfile.WriteLine("// mailto:support@icecreaturecontrol.com");
			_outfile.WriteLine("//");
			_outfile.WriteLine("// Unity Asset Store End User License Agreement (EULA)");
			_outfile.WriteLine("// http://unity3d.com/legal/as_terms");
			_outfile.WriteLine("//");
			_outfile.WriteLine("// ##############################################################################");
		}

		public static void PrintHeader( StreamWriter _outfile, string _classname, string _namespace )
		{
			_outfile.WriteLine(" ");
			_outfile.WriteLine("using UnityEngine;");
			_outfile.WriteLine("using System.Collections;");
			_outfile.WriteLine("");
			_outfile.WriteLine("using ICE;");
			_outfile.WriteLine("using ICE.World;");
			_outfile.WriteLine("using ICE.World.Objects;");
			_outfile.WriteLine("using ICE.World.Utilities;");
			_outfile.WriteLine("");
			_outfile.WriteLine("namespace ICE." + _namespace + " // TODO: ADAPT THE NAMESPACE AS DESIRED" );
			_outfile.WriteLine("{");
			_outfile.WriteLine("");
			_outfile.WriteLine("public class "+ _classname + " : ICEWorldBehaviour { // TODO: ADAPT THE CLASS NAME AS DESIRED" );
			_outfile.WriteLine(" ");


			PrintBehaviourContent( _outfile, _classname );

		}

		public static void PrintFooter( StreamWriter _outfile, string _name )
		{
			_outfile.WriteLine(" ");
			_outfile.WriteLine(" }"); // close class
			_outfile.WriteLine("}"); // close namespace
		}

		public static void PrintBehaviourContent( StreamWriter _outfile, string _name )
		{
			_outfile.WriteLine("/// <summary>");
			_outfile.WriteLine("/// OnRegisterPublicMethods is called within the GetPublicMethods() method to update the");
			_outfile.WriteLine("/// m_PublicMethods list. Override this event to register your own methods by using the");
			_outfile.WriteLine("/// RegisterPublicMethod(); while doing so you can use base.OnRegisterPublicMethods();");
			_outfile.WriteLine("/// to call the event in the base classes too.");
			_outfile.WriteLine("/// </summary>");
			_outfile.WriteLine("protected override void OnRegisterBehaviourEvents()");
			_outfile.WriteLine("{");
			_outfile.WriteLine("// base.OnRegisterPublicMethods();" );
			_outfile.WriteLine("// RegisterBehaviourEvent( \"ApplyDamage\", BehaviourEventParameterType.Float );");
			_outfile.WriteLine("// RegisterBehaviourEvent( \"TriggerBehaviourRuleAudio\" );");
			_outfile.WriteLine("}");
			_outfile.WriteLine("");

			_outfile.WriteLine("");
			_outfile.WriteLine("public override void Awake () {");
			_outfile.WriteLine("base.Awake();");
			_outfile.WriteLine("}");

			_outfile.WriteLine("");
			_outfile.WriteLine("public override void Start () {");
			_outfile.WriteLine("base.Start();");
			_outfile.WriteLine("}");

			_outfile.WriteLine("");
			_outfile.WriteLine("public override void OnEnable () {");
			_outfile.WriteLine("base.OnEnable();");
			_outfile.WriteLine("}");

			_outfile.WriteLine("");
			_outfile.WriteLine("public override void OnDisable () {");
			_outfile.WriteLine("base.OnDisable();");
			_outfile.WriteLine("}");

			_outfile.WriteLine("");
			_outfile.WriteLine("public override void OnDestroy() {");
			_outfile.WriteLine("base.OnDestroy();");
			_outfile.WriteLine("}");

			_outfile.WriteLine("");
			_outfile.WriteLine("public override void Update()");
			_outfile.WriteLine("{");
			_outfile.WriteLine("");
			_outfile.WriteLine("DoUpdateBegin();");
			_outfile.WriteLine("DoUpdate();");
			_outfile.WriteLine("DoUpdateComplete();");
			_outfile.WriteLine("");
			_outfile.WriteLine("}");
		}

		public static void PrintInitFile( string _path, string _namespace )
		{
			string _classname = "Init";
			string _filename = "editor_configuration_init";

			string _copy_path = GetValidPath( _path,_filename );
			if( File.Exists( _copy_path ) == true )
				return;

			using( StreamWriter _outfile = new StreamWriter( _copy_path ) )
			{
				PrintHead( _outfile, _filename, _classname );
					_outfile.WriteLine(" ");
					_outfile.WriteLine("using UnityEngine;");
					_outfile.WriteLine("");
					_outfile.WriteLine("namespace ICE." + _namespace + ".EditorInfos // TODO: ADAPT THE NAMESPACE AS DESIRED" );
					_outfile.WriteLine("{");
					_outfile.WriteLine("");
					_outfile.WriteLine("public class " + _classname + " : ICE.World.EditorInfos.Init {" );
					_outfile.WriteLine("");
					_outfile.WriteLine("// example of use : Init.YOUR_DEFAULT_VALUE");
					_outfile.WriteLine("public static readonly float YOUR_DEFAULT_VALUE = 0; // TODO: ADAPT NAME AND VALUE");
					_outfile.WriteLine("");
				PrintFooter( _outfile, _classname );
			}
		}

		public static void PrintInfoFile( string _path, string _namespace )
		{
			string _classname = "Info";
			string _filename = "editor_configuration_text";

			string _copy_path = GetValidPath( _path, _filename );
			if( File.Exists( _copy_path ) == true )
				return;

			using( StreamWriter _outfile = new StreamWriter( _copy_path ) )
			{
				PrintHead( _outfile, _filename, _classname );
					_outfile.WriteLine(" ");
					_outfile.WriteLine("using UnityEngine;");
					_outfile.WriteLine("");
					_outfile.WriteLine("namespace ICE." + _namespace + ".EditorInfos // TODO: ADAPT THE NAMESPACE AS DESIRED" );
					_outfile.WriteLine("{");
					_outfile.WriteLine("");
					_outfile.WriteLine("public class " + _classname + " : ICE.World.EditorInfos.Info {" );
					_outfile.WriteLine("");
					_outfile.WriteLine("// example of use : Info.YOUR_TEXT_KEY");
					_outfile.WriteLine("public static readonly string YOUR_TEXT_KEY = \"YOUR TEXT\"; // TODO: DUPLICATE LINE AND ADAPT KEY AND TEXT\"");
					_outfile.WriteLine("");
				PrintFooter( _outfile, _classname );
			}
		}

		private static string GetValidPath(  string _path, string _name  )
		{
			_name = _name.Replace("-","_");
			string _copy_path = _path + "/" + _name + ".cs";
			Debug.Log( "Creating Classfile: " + _copy_path );

			return _copy_path;
		}
	}
}
