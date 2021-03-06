// ##############################################################################
//
// ice_editor_layouts.cs | ICEEditorLayout
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
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ICE;
using ICE.World;
using ICE.World.Utilities;
using ICE.World.Objects;
using ICE.World.EnumTypes;

using ICE.World.EditorUtilities;
using ICE.World.EditorInfos;

namespace ICE.World.EditorUtilities
{
	public enum LabelIcon {
		Gray = 0,
		Blue,
		Teal,
		Green,
		Yellow,
		Orange,
		Red,
		Purple
	}


	/// <summary>
	/// Header type.
	/// </summary>
	public enum EditorHeaderType{
		TOGGLE,
		TOGGLE_LEFT,
		TOGGLE_LEFT_BOLD,
		TOGGLE_CUSTOM,
		FOLDOUT,
		FOLDOUT_BOLD,
		FOLDOUT_ENABLED,
		FOLDOUT_ENABLED_BOLD,
		FOLDOUT_CUSTOM,
		LABEL,
		LABEL_BOLD,
		LABEL_ENABLED,
		LABEL_ENABLED_BOLD,
		LABEL_CUSTOM,
		NONE
	}

	/// <summary>
	/// ICEEditorLayout contains a collection of methods to standardize the layout of ICE components and to 
	/// simplify the GUI design. Here you will find methods for drawing standard controls 
	/// </summary>
	public class ICEEditorLayout
	{
		public static Color DefaultBackgroundColor;
		public static Color DefaultGUIColor;

		public static Color InfoColor = new HSBColor( 0.15f ,0.25f, 1 ).ToColor();

		static ICEEditorLayout() {	
			SetDefaults();
		}

		public static void SetDefaults(){
			DefaultGUIColor = GUI.color;
			DefaultBackgroundColor = GUI.backgroundColor;
		}

		/*
		public static Object ObjectField(Object obj, iOS.ADBannerView.Type objType, bool allowSceneObjects, params GUILayoutOption[] options);
		public static Object ObjectField(string label, Object obj, iOS.ADBannerView.Type objType, bool allowSceneObjects, params GUILayoutOption[] options);
		public static Object ObjectField(GUIContent label, Object obj, iOS.ADBannerView.Type objType, bool allowSceneObjects, params GUILayoutOption[] options); 



		public static System.Object ObjectField(GUIContent label, Object obj, iOS.ADBannerView.Type objType, bool allowSceneObjects, params GUILayoutOption[] options )
		{
		}
*/

		public static readonly char upArrow = '\u25B2';
		public static readonly char downArrow = '\u25BC';
		public static readonly char QuestionMark = '\uFE16';
		public static readonly char EXCLAMATION_MARK = '\uFE15';
		public static readonly char CROSS_MARK = '\u2764';

		public static void MiniLabel( string _text )
		{
			if( string.IsNullOrEmpty( _text ) )
				return;
			
			ICEEditorLayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
					EditorGUILayout.LabelField( _text , EditorStyles.wordWrappedMiniLabel );
				GUILayout.FlexibleSpace();
			ICEEditorLayout.EndHorizontal();
		}

		public static void MiniLabelLeft( string _text )
		{
			if( string.IsNullOrEmpty( _text ) )
				return;

			EditorGUILayout.LabelField( _text , EditorStyles.wordWrappedMiniLabel );
		}

		private static ColliderType _collider;
		public static void DrawAddCollider( GameObject _object ){

			if( _object == null || _object.GetComponent<Collider>() != null )
				return;

			ICEEditorLayout.BeginHorizontal();
			_collider = (ColliderType)ICEEditorLayout.EnumPopup( "Add Collider", "", _collider ); 
			if( ICEEditorLayout.Button( "ADD", "Adds the selected collider", ICEEditorStyle.CMDButtonDouble ) )
			{
				if( _collider == ColliderType.Sphere )
					_object.AddComponent<SphereCollider>();
				else if( _collider == ColliderType.Box )
					_object.AddComponent<BoxCollider>();
				else if( _collider == ColliderType.Capsule )
					_object.AddComponent<CapsuleCollider>();
			}
			ICEEditorLayout.EndHorizontal();

			EditorGUILayout.HelpBox( Info.COLLIDER_INFO, MessageType.Info );
		}

		public static Color ColorField( string _title, string _hint, Color _color, string _help = ""  )
		{
			ICEEditorLayout.BeginHorizontal();
				_color = EditorGUILayout.ColorField( new GUIContent( _title, _hint ), _color );
			ICEEditorLayout.EndHorizontal( _help );

			return _color;
		}

		public static void DrawPriorityInfo( int _priority, string _hint = "" )
		{
			if( _hint == "" )
				_hint = "Priority";
			
			GUI.backgroundColor = new HSBColor( _priority * 0.0025f ,0.5f,1f ).ToColor();
			ICEEditorLayout.Button( _priority.ToString(), _hint, ICEEditorStyle.CMDButtonDouble );
			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		}

		public static bool ListDeleteButton<T>( List<T> _list, T _item )
		{
			if( ICEEditorLayout.Button( "DEL", "", ICEEditorStyle.CMDButtonDouble ) )
			{
				_list.Remove( _item );
				return true;
			}

			return false;
		}

		public static bool ListUpDownButtons<T>( List<T> _list, int _i )
		{
			if( ICEEditorLayout.ButtonUp() )
			{
				T _tmp_obj = _list[_i]; 
				_list.RemoveAt( _i );

				if( _i - 1 < 0 )
					_list.Add( _tmp_obj );
				else
					_list.Insert( _i - 1, _tmp_obj );

				return true;
			}	
			if( ICEEditorLayout.ButtonDown() )
			{
				T _tmp_obj = _list[_i]; 
				_list.RemoveAt( _i );

				if( _i + 1 > _list.Count )
					_list.Insert( 0, _tmp_obj );
				else
					_list.Insert( _i +1, _tmp_obj );

				return true;
			}	

			return false;
		}

		public static float Round( float _value, float _precision )
		{
			if( Application.isPlaying )
				return _value;
			else
				return Mathf.Round( _value / _precision ) * _precision;
		}

		public static bool ButtonUp(){
			return GUILayout.Button( new GUIContent( upArrow.ToString(), "Moves the selected item up one position in the list"), ICEEditorStyle.CMDButtonDouble );
		}
		
		public static bool ButtonDown(){
			return GUILayout.Button( new GUIContent( downArrow.ToString(), "Moves the selected item down one position in the list"), ICEEditorStyle.CMDButtonDouble );
		}

		public static bool ButtonCloseDouble(){
			return GUILayout.Button( new GUIContent( "DEL", "Removes the selected item"), ICEEditorStyle.CMDButtonDouble );
		}

		public static bool ButtonClose(){
			return GUILayout.Button( new GUIContent( "X", "Removes the selected item"), ICEEditorStyle.CMDButton );
		}

		public static void ButtonSelectObject( GameObject _object, GUIStyle _style = null )
		{
			GUI.backgroundColor = InfoColor;

			if( _style == null )
				_style = ICEEditorStyle.ButtonMiddle;

			string _title = "SELECT";
			if( _style == ICEEditorStyle.CMDButtonDouble ) 
				_title = "SEL";
			
			EditorGUI.BeginDisabledGroup( _object == null );
			if( GUILayout.Button( new GUIContent( _title, "Select object" ), _style ) )
					Selection.activeGameObject = _object;
			EditorGUI.EndDisabledGroup();

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		}

		public static void ButtonDisplayObject( GameObject _object, string _title, Color _color, GUIStyle _style = null  )
		{
			GUI.backgroundColor = _color;

			if( _style == null )
				_style = ICEEditorStyle.ButtonMiddle;
			
			EditorGUI.BeginDisabledGroup( _object == null );
			if( GUILayout.Button( new GUIContent( _title, "Show object" ), _style ) )
			{
				SceneView view = SceneView.currentDrawingSceneView;
				
				if(view == null)
					view = SceneView.lastActiveSceneView;
				
				if(view != null && _object != null )
				{
					Vector3 _pos = _object.transform.position; 
					
					_pos.y += 20;
					view.rotation        = new Quaternion(1,0,0,1);
					view.LookAt( _pos );
					
				}
				else
				{
					Debug.Log ( "Sorry, currentDrawingSceneView and lastActiveSceneView are not available!");
				}
			}
			EditorGUI.EndDisabledGroup();

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		}

		public static void ButtonShowTransform( string _title, string _hint, Transform _transform, GUIStyle _style = null ){

			if( _style == null )
				_style = ICEEditorStyle.ButtonMiddle;
			
			EditorGUI.BeginDisabledGroup( _transform == null );
			if( GUILayout.Button( new GUIContent( _title, _hint ), _style ) )
			{
				SceneView view = SceneView.currentDrawingSceneView;

				if(view == null)
					view = SceneView.lastActiveSceneView;

				if(view != null && _transform != null )
				{
					Vector3 _pos = _transform.position; 
					//view.rotation = new Quaternion(0,0,0,1);
					view.LookAt( _pos );

				}
				else
				{
					Debug.Log ( "Sorry, currentDrawingSceneView and lastActiveSceneView are not available!");
				}
			}
			EditorGUI.EndDisabledGroup();
		}

		public static void ButtonDisplayObject( Vector3 _position, GUIStyle _style = null ){
	
			GUI.backgroundColor = InfoColor;

			if( _style == null )
				_style = ICEEditorStyle.CMDButtonDouble;
			
			EditorGUI.BeginDisabledGroup( _position == Vector3.zero );
			if( GUILayout.Button(new GUIContent( "DIS", "Display object" ), _style ) )
			{
				SceneView view = SceneView.currentDrawingSceneView;

				if(view == null)
					view = SceneView.lastActiveSceneView;

				if(view != null)
				{
					Vector3 _pos = _position; 
					
					_pos.y += 20;
					view.rotation        = new Quaternion(1,0,0,1);
					view.LookAt( _pos );

				}
				else
				{
					Debug.Log ( "Sorry, currentDrawingSceneView and lastActiveSceneView are not available!");
				}
			}
			EditorGUI.EndDisabledGroup();

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		}

		public static bool ButtonEnabled( bool _value ){
			return ButtonCheck( "ENABLED", "Enables/disables the related feature", _value, ICEEditorStyle.ButtonMiddle );
		}

		public static bool ButtonCheck( string _title, string _hint, bool _value, GUIStyle _style, params GUILayoutOption[] _options ){

			if( _value )
				GUI.backgroundColor = Color.cyan;

			if( GUILayout.Button( new GUIContent( _title, _hint ), _style, _options ) )
				_value = ! _value;

			GUI.backgroundColor = DefaultBackgroundColor;

			return _value;
		}

		public static bool ButtonCheck( Rect _rect, string _title, string _hint, bool _value, GUIStyle _style )
		{
			if( _value )
				GUI.backgroundColor = Color.yellow;
			else
				GUI.backgroundColor = DefaultBackgroundColor;
			
			if( GUI.Button( _rect, new GUIContent( _title, _hint ), _style ) )
				_value = ! _value;
			
			GUI.backgroundColor = DefaultBackgroundColor;
			
			return _value;
		}

		public static void ButtonMinMaxDefault( ref float _min, ref float _max, float _min_default, float _max_default )
		{
			if( _min != _min_default || _max != _max_default  )
				GUI.backgroundColor = Color.yellow;

			if ( GUILayout.Button( new GUIContent( "D", "Set default values (" + _min_default + "/" + _max_default + ")" ), ICEEditorStyle.CMDButtonDouble ))
			{
				_min = _min_default;
				_max = _max_default;
			}

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		}

		public static float ButtonDefault( float _value, float _default )
		{
			_value = Round( _value, 0.0001f );
			_default = Round( _default, 0.0001f );
			if( _value != _default )
				GUI.backgroundColor = Color.yellow;

			if ( GUILayout.Button( new GUIContent( "D", "Set default value (" + _default + ")" ), ICEEditorStyle.CMDButtonDouble ))
				_value = _default;

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

			return _value;
		}


		public static bool Button( string _title, string _hint, GUIStyle _style, params GUILayoutOption[] _options ){

			bool _value = false;

			if( GUILayout.Button( new GUIContent( _title, _hint ), _style, _options ) )
				_value = ! _value;
			
			return _value;
		}

		public static bool Button( Rect _rect, string _title, string _hint, GUIStyle _style  )
		{
			bool _value = false;

			if( GUI.Button( _rect, new GUIContent( _title, _hint ), _style ) )
				_value = ! _value;
			
			return _value;
		}

		public static void AssignLabel(GameObject g , int icon = 0 )
		{
			if( icon < 8 )
			{
				Texture2D tex = EditorGUIUtility.IconContent("sv_label_" + icon ).image as Texture2D;
				Type editorGUIUtilityType  = typeof(EditorGUIUtility);
				BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
				object[] args = new object[] {g, tex};
				editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);
			}
		}

		public static void DrawLabelIconBar( string _title, string[] _paths, int _width, int _height, int _left, int _top, int _space )
		{
			BeginHorizontal();

			EditorGUILayout.PrefixLabel( _title );
		//	Label( _title, true);
			
			int _offset = 0;
			foreach( string _path in _paths )
			{
				DrawIcon( _path, _width, _height, _offset, _top );
				_offset += _width + _space;
			}
			
			GUILayout.FlexibleSpace();
			EndHorizontal();
			
			GUILayout.Space(_height + _top);
		}

		public static void DrawIconBar( string[] _paths, int _width, int _height, int _left, int _top, int _space )
		{
			BeginHorizontal();

			int _offset = _left;
			foreach( string _path in _paths )
			{
				DrawIcon( _path, _width, _height, _offset, _top );
				_offset += _width + _space;
			}

			GUILayout.FlexibleSpace();
			EndHorizontal();
			
			GUILayout.Space(_height + _top);
		}

		public static Color DefaultColor( string _title, string _hint, Color _color, Color _default, string _help = "" )
		{
			BeginHorizontal();
			_color = EditorGUILayout.ColorField( new GUIContent( _title, _hint ), _color );
			
			if (GUILayout.Button( new GUIContent( "DEFAULT", "Sets default color." ), ICEEditorStyle.ButtonMiddle ))
				_color = _default;

			EndHorizontalHelp( _help );
			return _color;
		}


		public static void DrawIcon( string _path, int _width, int _height, int _left, int _top )
		{
			Texture _icon = (Texture)Resources.Load( _path );

			if( _icon == null )
				return;
			
			Rect _rect = EditorGUILayout.BeginVertical();
			EditorGUI.DrawPreviewTexture( new Rect( _rect.x + _left, _rect.y + _top, _width, _height ) , _icon );
			GUILayout.Space(_height + _top);
			EditorGUILayout.EndVertical();

		}

		public static string TransformPopup( string _title, string _hint, string _name, Transform _transform, bool _provide_empty = true, string _help = "" )
		{
			BeginHorizontal();
				List<Transform> _list = new List<Transform>(); 
				SystemTools.GetChildren( _list, _transform );

				_list = _list.OrderBy( Transform => Transform.name ).ToList();

				int _list_count = _list.Count;
				if( _provide_empty )
					_list_count += 1;

				GUIContent[] _options = new GUIContent[ _list_count ];
				int _selected = 0;
				int _index = 0;
				if( _provide_empty )
				{
					_options[0] = new GUIContent( " " );
					_index = 1;
				}

				for( int _i = 0 ; _i < _list.Count ; _i++ )
				{
					_options[ _index ] = new GUIContent( _list[_i].name );

					if( _list[_i].name == _name )
						_selected = _index;

					_index += 1;
				}

				_selected = EditorGUILayout.Popup( new GUIContent( _title, _hint ) , _selected, _options );

				_name = _options[_selected].text;

				Transform _child = SystemTools.FindChildByName( _name, _transform );

				//ButtonShowTransform( "DTL", "Show details", _child, ICEEditorStyle.CMDButtonDouble );
				ButtonSelectObject( (_child != null)?_child.gameObject:null, ICEEditorStyle.CMDButtonDouble  );
			EndHorizontal( _help );
			return _name;
		}

		/// <summary>
		/// Draws the water check.
		/// </summary>
		/// <param name="_type">Type.</param>
		/// <param name="_layers">Layers.</param>
		/// <param name="_help">Help.</param>
		public static void DrawWaterCheck( ref WaterCheckType _type, List<string> _layers, string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();
			_type = (WaterCheckType)ICEEditorLayout.EnumPopup("Water Check", "Method to handle water related checks and movements", _type );
			if( _type == WaterCheckType.CUSTOM )
			{
				if (GUILayout.Button("Add Layer", ICEEditorStyle.ButtonMiddle ))
					_layers.Add( (LayerMask.NameToLayer("Water") != -1?"Water":"Default") );
			}				
			ICEEditorLayout.EndHorizontal( _help );

			if( _type == WaterCheckType.CUSTOM )
			{
				EditorGUI.indentLevel++;
					DrawLayersList( _layers );
				EditorGUI.indentLevel--;
			}
		}

		/// <summary>
		/// Draws the ground check.
		/// </summary>
		/// <param name="_type">Type.</param>
		/// <param name="_layers">Layers.</param>
		/// <param name="_help">Help.</param>
		public static void DrawGroundCheck( ref GroundCheckType _type, List<string> _layers, string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();
			_type = (GroundCheckType)ICEEditorLayout.EnumPopup("Ground Check", "Method to handle ground related checks and movements", _type );
			if( _type == GroundCheckType.RAYCAST )
			{
				if (GUILayout.Button("Add Layer", ICEEditorStyle.ButtonMiddle ))
					_layers.Add( (LayerMask.NameToLayer("Terrain") != -1?"Terrain":"Default") );

			}				
			ICEEditorLayout.EndHorizontal( _help );

			if( _type == GroundCheckType.RAYCAST )
			{
				EditorGUI.indentLevel++;
					DrawLayersList( _layers );
				EditorGUI.indentLevel--;
			}
		}

		/// <summary>
		/// Draws the layers list.
		/// </summary>
		/// <param name="_layers">Layers.</param>
		public static void DrawLayersList( List<string> _layers )
		{
			if( _layers.Count == 0 )
			{
				ICEEditorLayout.BeginHorizontal();
				GUILayout.FlexibleSpace();					
				EditorGUILayout.LabelField( new GUIContent( " - No Layer defined -", "" ) );
				GUILayout.FlexibleSpace();
				ICEEditorLayout.EndHorizontal();
			}
			else
			{
				for( int i = 0 ; i < _layers.Count; i++ )
				{
					ICEEditorLayout.BeginHorizontal();
					GUI.backgroundColor = new Vector4( 0.7f, 0.9f, 0.9f, 0.5f);

					int _layer = LayerMask.NameToLayer(_layers[i]);

					string _title = "Layer #" + _layer;

					if( _layer == -1 )
					{
						GUI.backgroundColor = Color.red;
						EditorGUILayout.PrefixLabel( new GUIContent( _title + " (MISSING)", "CREATE MISSING '" + _layers[i] + "' LAYER" ) );
						if( GUILayout.Button("CREATE MISSING '" + _layers[i] + "' LAYER", ICEEditorStyle.ButtonFlex ) )
						{
							EditorTools.AddLayer( _layers[i] );
						}
					}
					else
					{
						_layers[i] = LayerMask.LayerToName( EditorGUILayout.LayerField( _title, _layer ) );
					}

					GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
					if (GUILayout.Button("X", ICEEditorStyle.CMDButton ))
					{
						_layers.RemoveAt(i);
						--i;
						return;
					}
					ICEEditorLayout.EndHorizontal();
				}

				EditorGUILayout.Separator();
			}
		}

		public static float DrawBaseOffset( Transform _transform, string _title, string _hint, float _offset, ref float _maximum, string _help = "" )
		{
			if( string.IsNullOrEmpty( _title ) )
				_title = "Base Offset";

			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";

			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BASE_OFFSET ;

			ICEEditorLayout.BeginHorizontal();
			_offset = ICEEditorLayout.MaxBasicSlider( _title, _hint, _offset, 0.01f, -_maximum, ref _maximum, "" );
			_offset = ICEEditorLayout.ButtonDefault( _offset, 0 );
			ICEEditorLayout.EndHorizontal( _help );

			return _offset;
		}

		/// <summary>
		/// Draws the base offset.
		/// </summary>
		/// <returns>The base offset.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_offset">Offset.</param>
		/// <param name="_maximum">Maximum.</param>
		/// <param name="_grounded">Grounded.</param>
		/// <param name="_help">Help.</param>
		public static float DrawBaseOffsetGround( Transform _transform, string _title, string _hint, float _offset, ref float _maximum, ref bool _grounded, string _help = "" )
		{
			if( string.IsNullOrEmpty( _title ) )
				_title = "Base Offset";

			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";

			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BASE_OFFSET ;

			if(  _transform.parent != null)
				_grounded = false;

			ICEEditorLayout.BeginHorizontal();
			_offset = ICEEditorLayout.MaxBasicSlider( _title, _hint, _offset, 0.01f, -_maximum, ref _maximum, "" );
			_offset = ICEEditorLayout.ButtonDefault( _offset, 0 );
			EditorGUI.BeginDisabledGroup( _transform.parent != null );
			_grounded = ICEEditorLayout.ButtonCheck( "GND", "Grounded In Editor Mode" , _grounded , ICEEditorStyle.CMDButtonDouble );
			EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal( _help );

			return _offset;
		}


		public static string CameraPopup( string _title, string _hint, string _camera )
		{
			GUIContent[] _options = new GUIContent[ Camera.allCameras.Length + 1 ];
			int _selected = 0;

			_options[0] = new GUIContent( " " );
			for( int i = 0 ; i < Camera.allCameras.Length ; i++ )
			{
				Camera _cam = Camera.allCameras[i];
				
				int _index = i + 1;
				
				_options[ _index ] = new GUIContent( _cam.name );
				
				if( _cam.name == _camera )
					_selected = _index;
			}

		
			_selected = EditorGUILayout.Popup( new GUIContent( _title, _hint ) , _selected, _options );

			return _options[_selected].text;
		}

		public static Vector3 OffsetGroup( string _title, string _hint, Vector3 _position, Vector3 _offset, GameObject _owner, float _scale, float _range )
		{
			GameObject _target = new GameObject();
			_target.transform.position = _position;

			
			BeginHorizontal();
			_offset = EditorGUILayout.Vector3Field( new GUIContent( _title, _hint ) , _offset );
			
			if (GUILayout.Button( new GUIContent( "GET", "" ), ICEEditorStyle.CMDButtonDouble ))
			{
				Vector3 _local_offset = _target.transform.InverseTransformPoint( _owner.transform.position );
				
				_local_offset.x = _local_offset.x*_target.transform.lossyScale.x;
				_local_offset.y = 0;//_local_offset.y*target.transform.lossyScale.y;
				_local_offset.z = _local_offset.z*_target.transform.lossyScale.z;					/**/
				
				_offset = _local_offset;
				//offset = new Vector3(Mathf.Round(offset.x/scale)*scale, Mathf.Round(offset.y/scale)*scale, Mathf.Round(offset.z/scale)*scale) ;
			}
			
			if (GUILayout.Button( new GUIContent( "SET", "Relocate your creature to the current offset position" ), ICEEditorStyle.CMDButtonDouble ))
			{
				/*
				owner.transform.position = target.transform.position 
					+ ( target.transform.forward * offset.z ) 
						+ ( target.transform.right * offset.x )
							+ ( target.transform.up * offset.y );*/
				
				Vector3 _local_offset = _offset;
				
				_local_offset.x = _local_offset.x/_target.transform.lossyScale.x;
				_local_offset.y = _local_offset.y/_target.transform.lossyScale.y;
				_local_offset.z = _local_offset.z/_target.transform.lossyScale.z;
				
				Vector3 _pos = _target.transform.TransformPoint( _local_offset );
				
				_pos.y = Terrain.activeTerrain.SampleHeight( _pos );
				
				_owner.transform.position = _pos;
			}
			
			if (GUILayout.Button( new GUIContent( "RESET", "Reset offset values" ), ICEEditorStyle.CMDButtonDouble ))
				_offset = Vector3.zero;
	
			EndHorizontal();

			_target = null;
			
			return _offset;
		}

		public static Vector3 OffsetGroup( string _title, Vector3 _offset, GameObject _owner, GameObject _target, float _scale = 0.5f, float _range = 25, string _help = ""  )
		{
	
			/*
			BeginHorizontal();
			offset.z = (int)ICEEditorLayout.SliderGroup("Offset z", offset.z , scale,-range,range );
			

			BeginHorizontal();
			offset.x = (int)ICEEditorLayout.SliderGroup("Offset x", offset.x ,scale,-range,range );
			*/

			BeginHorizontal();
			_offset = EditorGUILayout.Vector3Field( _title, _offset );

			if( _target != null )
			{
				if (GUILayout.Button( new GUIContent( "GET", "" ), ICEEditorStyle.CMDButtonDouble ))
				{
					Vector3 _local_offset = _target.transform.InverseTransformPoint( _owner.transform.position );

					_local_offset.x = _local_offset.x*_target.transform.lossyScale.x;
					_local_offset.y = 0;//_local_offset.y*target.transform.lossyScale.y;
					_local_offset.z = _local_offset.z*_target.transform.lossyScale.z;					/**/

					_offset = _local_offset;
				}

				if (GUILayout.Button( new GUIContent( "SET", "Relocate your creature to the current offset position" ), ICEEditorStyle.CMDButtonDouble ))
				{
					Vector3 _local_offset = _offset;					
					_local_offset.x = _local_offset.x/_target.transform.lossyScale.x;
					_local_offset.y = _local_offset.y/_target.transform.lossyScale.y;
					_local_offset.z = _local_offset.z/_target.transform.lossyScale.z;
					Vector3 _position = _target.transform.TransformPoint( _local_offset );
					//TODO:_position.y = CreatureRegister.GetGroundLevel( _position );

					_owner.transform.position = _position;
				}

				if (GUILayout.Button( new GUIContent( "RESET", "Reset offset values" ), ICEEditorStyle.CMDButtonDouble ))
				{
					_offset = Vector3.zero;
				}
			}
			EndHorizontalHelp( _help );


			return _offset;
		}

		public static Vector3 OffsetGroup( string _title, Vector3 _offset, string _help = ""  )
		{
			BeginHorizontal();
				_offset = EditorGUILayout.Vector3Field( _title, _offset );				
				if (GUILayout.Button( new GUIContent( "RESET", "Reset offset values" ), ICEEditorStyle.CMDButtonDouble ))
					_offset = Vector3.zero;
			EndHorizontalHelp( _help );			
			
			return _offset;
		}

		public static Quaternion RotationGroup( string _title, Quaternion _rotation, string _help = ""  )
		{
			Vector4 _vector = Converter.QuaternionToVector4( _rotation );

			BeginHorizontal();
				_vector = EditorGUILayout.Vector4Field( _title, _vector );				
				if (GUILayout.Button( new GUIContent( "RESET", "Reset rotation values" ), ICEEditorStyle.CMDButtonDouble ))
					_vector = Vector4.zero;
			EndHorizontalHelp( _help );			

			return Converter.Vector4ToQuaternion( _vector );
		}

		public static Vector3 EulerGroup( string _title, Vector3 _rotation, string _help = ""  )
		{
			BeginHorizontal();
			_rotation = EditorGUILayout.Vector3Field( _title, _rotation );				
			if (GUILayout.Button( new GUIContent( "RESET", "Reset rotation values" ), ICEEditorStyle.CMDButtonDouble ))
				_rotation = Vector3.zero;
			EndHorizontalHelp( _help );			

			return _rotation;
		}

		public static int Popup( string _title, string _hint, int _selected, string[] _options, string _help )
		{
			BeginHorizontal();
			int _value = Popup( _title, _hint, _selected, _options ); 
			EndHorizontalHelp( _help );
			return _value;
		}

		public static int Popup( string _title, string _hint, int _selected, string[] _options )
		{
			GUIContent[] _content_options = new GUIContent[_options.Length];
			for(int i = 0 ; i < _options.Length ; i++ )
			{
				_content_options[i] = new GUIContent();
				_content_options[i].text = _options[i];

			}
			return EditorGUILayout.Popup( new GUIContent( _title, _hint), _selected, _content_options ); 
		}

		
		public static int Popup( string _title, string _hint, int _selected, GUIContent[] _options )
		{
			return EditorGUILayout.Popup( new GUIContent( _title, _hint), _selected, _options ); 
		}

		public static int Popup( string _title, string _hint, int _selected, GUIContent[] _options, string _help )
		{
			BeginHorizontal();
			int _value = EditorGUILayout.Popup( new GUIContent( _title, _hint), _selected, _options ); 
			EndHorizontalHelp( _help );
			return _value;
		}

		
		public static Enum EnumPopup( string _title, string _hint, Enum _selected )
		{
			return EditorGUILayout.EnumPopup( new GUIContent( _title, _hint) , _selected );
		}

		public static Enum EnumPopup( string _title, string _hint, Enum _selected, string _help  )
		{
			BeginHorizontal();
				Enum _value = EnumPopup( _title, _hint, _selected );
			EndHorizontalHelp( _help );
			return _value;
		}
			


		public static bool ToggleLeft( string _title, string _hint, bool _toggle, bool _bold )
		{
			if( _bold )
				return EditorGUILayout.ToggleLeft( new GUIContent( _title, _hint ) ,_toggle, EditorStyles.boldLabel );
			else
				return EditorGUILayout.ToggleLeft( new GUIContent( _title, _hint ) ,_toggle );
		}

		public static bool ToggleLeft( string _title, string _hint, bool _toggle, bool _bold, string _help )
		{
			BeginHorizontal();
				bool _value = ToggleLeft( _title, _hint, _toggle , _bold );
				GUILayout.FlexibleSpace();			
			EndHorizontalHelp( _help );
			return _value;
		}

		public static bool Toggle( string _title, string _hint, bool _toggle, string _help = ""  )
		{
			if( _help == "" )
				return EditorGUILayout.Toggle( new GUIContent( _title, _hint ) ,_toggle );
			else
			{
				BeginHorizontal();
					bool _value = Toggle( _title, _hint, _toggle );
					GUILayout.FlexibleSpace();			
				EndHorizontalHelp( _help );
				return _value;
			}			
		}

		public static bool InfoToggle( string _title, string _hint, bool _toggle, string _help = "" )
		{
			BeginHorizontal();
				bool _value = Toggle( _title, _hint, _toggle );
				GUILayout.FlexibleSpace();			
			EndHorizontalHelp( _help );
			return _value;
		}

		public static string Tag( string _title, string _hint, string _value, string _help = ""  )
		{
			if( _help == "" )
				return EditorGUILayout.TagField( new GUIContent( _title, _hint ), _value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.TagField( new GUIContent( _title, _hint ), _value );
				EndHorizontalHelp( _help );
				return _value;
			}			
		}

		public static string ColliderPopup( GameObject _object, string _title, string _hint, string _value, string _help = ""  )
		{

			Collider[] _colliders = _object.GetComponentsInChildren<Collider>();

			GUIContent[] _options = new GUIContent[ _colliders.Length + 1 ];
			int _selected = 0;

			_options[0] = new GUIContent( " " );
			for( int i = 0 ; i < _colliders.Length ; i++ )
			{
				Collider _collider = _colliders[i];

				int _index = i + 1;

				_options[ _index ] = new GUIContent( _collider.name );

				if( _collider.name == _value )
					_selected = _index;
			}


			_selected = Popup( _title, _hint , _selected, _options, _help );

			return _options[_selected].text;
		}

/*
		public static List<string> layers;
		public static List<int> layerNumbers;
		public static string[] layerNames;
		public static long lastUpdateTick;
		public static LayerMask LayerMask( string label, LayerMask selected, bool showSpecial) {
			
			if (layers == null || (System.DateTime.Now.Ticks - lastUpdateTick > 10000000L && Event.current.type == EventType.Layout)) {
				lastUpdateTick = System.DateTime.Now.Ticks;
				if (layers == null) {
					layers = new List<string>();
					layerNumbers = new List<int>();
					layerNames = new string[4];
				} else {
					layers.Clear ();
					layerNumbers.Clear ();
				}
				
				int emptyLayers = 0;
				for (int i=0;i<32;i++) {
					string layerName = LayerMask.LayerToName(i);
					
					if (layerName != "") {
						
						for (;emptyLayers>0;emptyLayers--) layers.Add ("Layer "+(i-emptyLayers));
						layerNumbers.Add (i);
						layers.Add (layerName);
					} else {
						emptyLayers++;
					}
				}
				
				if (layerNames.Length != layers.Count) {
					layerNames = new string[layers.Count];
				}
				for (int i=0;i<layerNames.Length;i++) layerNames[i] = layers[i];
			}
			
			selected.value =  EditorGUILayout.MaskField (label,selected.value,layerNames);
			
			return selected;
		}*/

		public static int Layer( string _title, string _hint, int _value, string _help = ""  )
		{
			if( _help == "" )
				return EditorGUILayout.LayerField( new GUIContent( _title, _hint ), _value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.LayerField( new GUIContent( _title, _hint ), _value );		
				EndHorizontalHelp( _help );
				return _value;
			}			
		}


		public static float Float( string _title, string _hint, float _value, string _help = ""  )
		{
			if( _help == "" )
				return EditorGUILayout.FloatField( new GUIContent( _title, _hint ) ,_value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.FloatField( new GUIContent( _title, _hint ) ,_value );
				GUILayout.FlexibleSpace();			
				EndHorizontalHelp( _help );
				return _value;
			}			
		}

		public static int Integer( string _title, string _hint, int _value, string _help = ""  )
		{
			if( _help == "" )
				return EditorGUILayout.IntField( new GUIContent( _title, _hint ) ,_value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.IntField( new GUIContent( _title, _hint ) ,_value );
				GUILayout.FlexibleSpace();			
				EndHorizontalHelp( _help );
				return _value;
			}			
		}

		public static string Text( string _title, string _hint, string _value, string _help = ""  )
		{
			if( _help == "" )
				return EditorGUILayout.TextField( new GUIContent( _title, _hint ) ,_value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.TextField( new GUIContent( _title, _hint ) ,_value );
				EndHorizontalHelp( _help );
				return _value;
			}			
		}

		public static void Label( string _title, string _hint, string _content )
		{
			EditorGUILayout.LabelField( new GUIContent( _title, _hint ), new GUIContent( _content, _hint ) );
		}

		public static void Label( string _text, bool _bold )
		{
			if( _bold )
				EditorGUILayout.LabelField( _text, EditorStyles.boldLabel );
			else
				EditorGUILayout.LabelField( _text );
		}

		public static void Label( string _text, bool _bold, string _help )
		{
			if( _help == "" )
				Label( _text, _bold );
			else
			{
				BeginHorizontal();
					Label( _text, _bold );
					GUILayout.FlexibleSpace();
				EndHorizontalHelp( _help );		
			}
		}

		public static string InfoLabel( string _text, bool _bold, int _priority, ref bool _show_info, string _info, string _help )
		{
			BeginHorizontal();
				Label( _text, _bold );
				GUILayout.FlexibleSpace();

				if( _info.Trim() != "" )
					GUI.backgroundColor = Color.magenta;

				DrawPriorityInfo( _priority );
				_show_info = ButtonCheck( "i", "Show info text", _show_info, ICEEditorStyle.CMDButton ); 

			EndHorizontalHelp( _help );	

			if( _show_info )
			{
				EditorStyles.textField.wordWrap = true;
				_info = EditorGUILayout.TextArea( _info );
			}

			return _info;
		}
		public static string InfoLabel( string _text, bool _bold, ref bool _show_info, string _info, string _help )
		{
			if( _help == "" )
				Label( _text, _bold );
			else
			{
				BeginHorizontal();
				Label( _text, _bold );
				GUILayout.FlexibleSpace();

				if( _info.Trim() != "" )
					GUI.backgroundColor = Color.magenta;

				_show_info = ButtonCheck( "i", "Show info text", _show_info, ICEEditorStyle.CMDButton ); 

				EndHorizontalHelp( _help );		
			}

			if( _show_info )
			{
				EditorStyles.textField.wordWrap = true;
				_info = EditorGUILayout.TextArea( _info );
			}

			return _info;
		}

		public static Rect BeginHorizontal()
		{
			return EditorGUILayout.BeginHorizontal();
		}

		public static void EndHorizontal( ICEInfoDataObject _object, string _help = "" )
		{
			if( _object != null )
				EndHorizontal( ref _object.ShowInfoText , ref _object.InfoText, _help  );
			else
				EndHorizontal( _help  );
		}

		public static void EndHorizontal( ref bool _show_info , ref string _info, string _help = "" )
		{
			if( _info.Trim() != "" )
				GUI.backgroundColor = Color.magenta;

			_show_info = ButtonCheck( "i", "Show info text", _show_info, ICEEditorStyle.CMDButton ); 

			if( _help == "" )
				EditorGUILayout.EndHorizontal();
			else
				EndHorizontalHelp( _help );

			if( _show_info )
			{
				EditorStyles.textField.wordWrap = true;
				_info = EditorGUILayout.TextArea( _info );
			}
		}

		public static void EndHorizontal( string _help = "" )
		{
			if( _help == "" )
				EditorGUILayout.EndHorizontal();
			else
				EndHorizontalHelp( _help );
		}

		public static void EndHorizontalHelp( string _help )
		{
			if( _help != "" )
				ICEEditorInfo.HelpButton();
			
			EditorGUILayout.EndHorizontal();
			ICEEditorInfo.ShowHelp( _help );
		}

		public static void EndProperty( Rect _rect, string _help = "" )
		{
			if( _help == "" )
				EditorGUI.EndProperty();
			else
			{
				ICEEditorInfo.HelpButton( _rect );
				EditorGUI.EndProperty();
				ICEEditorInfo.ShowHelp( _help );
			}
		}

		public static int IntField( string _title, string _hint, int _value, string _help = "" )
		{
			if( _help == "" )
				return EditorGUILayout.IntField( new GUIContent( _title, _hint ), _value );
			else
			{
				BeginHorizontal();
				_value = EditorGUILayout.IntField( new GUIContent( _title, _hint ), _value );
				EndHorizontalHelp( _help );
				return _value;
			}	


		}

		public static bool InfoFoldout( bool _foldout, string title, ref bool _show_info , ref string _info , string _help = "", bool _bold = true )
		{
			BeginHorizontal();
			bool _value = EditorGUILayout.Foldout( _foldout , title, (_bold?ICEEditorStyle.Foldout:ICEEditorStyle.FoldoutNormal) );
			GUILayout.FlexibleSpace();	

			if( _info.Trim() != "" )
				GUI.backgroundColor = Color.magenta;

			_show_info = ButtonCheck( "i", "Show info text", _show_info, ICEEditorStyle.CMDButton ); 

			EndHorizontalHelp( _help );

			if( _show_info )
			{
				EditorStyles.textField.wordWrap = true;
				_info = EditorGUILayout.TextArea( _info );
			}

			return _value;
		}

		public static bool Foldout( bool _foldout, string title, string _help = "", bool _bold = true )
		{
			if( _help == "" )
				return EditorGUILayout.Foldout( _foldout , title, (_bold?ICEEditorStyle.Foldout:ICEEditorStyle.FoldoutNormal) );
			else
			{
				BeginHorizontal();
					bool _value = EditorGUILayout.Foldout( _foldout , title, (_bold?ICEEditorStyle.Foldout:ICEEditorStyle.FoldoutNormal) );
					GUILayout.FlexibleSpace();			
				EndHorizontalHelp( _help );
				return _value;
			}
		}


		public static void RangeSliderGroup( string title, ref float value_min, ref float value_max, float step, float min, float max )
		{
	
			EditorGUILayout.MinMaxSlider( new GUIContent( title ), ref value_min, ref value_max, min, max, GUILayout.ExpandWidth(true) );

			GUILayout.FlexibleSpace();

			value_min = EditorGUILayout.FloatField( value_min, GUILayout.MinWidth(70), GUILayout.MaxWidth(70) );
			
			if (GUILayout.Button("<", ICEEditorStyle.CMDButton ))
				value_min -= step;
			
			if (GUILayout.Button(">", ICEEditorStyle.CMDButton ))
				value_min += step; 



			value_max = EditorGUILayout.FloatField( value_max, GUILayout.MinWidth(70), GUILayout.MaxWidth(70) );



			if (GUILayout.Button("<", ICEEditorStyle.CMDButton ))
				value_max -= step;
			
			if (GUILayout.Button(">", ICEEditorStyle.CMDButton ))
				value_max += step; 
			
			if( value_min < min )
				value_min = min;
			
			if( value_max > max )
				value_max = max;
			
	
		}

		public static float DurationSlider( string _title, string _tooltip, float _value, float _precision, float _min, float _max, float _default, ref bool _transit, string _help = ""  )
		{
			BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _transit == true );				
					_value = BasicSlider( _title, _tooltip, _value, _precision, _min, _max );
					
					if( _default != _value )
						GUI.backgroundColor = Color.yellow;
					
					if ( GUILayout.Button( new GUIContent( "RESET", "Set value to " + _default ), ICEEditorStyle.ButtonMiddle ))
						_value = _default;

				EditorGUI.EndDisabledGroup();
			
				if( _transit )
					GUI.backgroundColor = Color.green;
				else
					GUI.backgroundColor = Color.yellow;

				if ( GUILayout.Button( new GUIContent( "TRANSIT", "Use target as transit point without stopping and changing behaviours" ), ICEEditorStyle.ButtonMiddle ))
					_transit = ! _transit;

				
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			
			EndHorizontalHelp( _help );
			
			return _value;
			
			
		}

		public static float MaxDefaultSlider( string _title, string _tooltip, float _value, float _precision, float _min, ref float _max, float _default = 0 , string _help = "" )
		{
			BeginHorizontal();
			_value = MaxBasicSlider( _title, _tooltip, _value, _precision, _min, ref _max );
			_value = ButtonDefault( _value, _default );
			EndHorizontalHelp( _help );
			return _value;

		}

		public static AnimationCurve DefaultCurve( string _title, string _tooltip, AnimationCurve _curve, AnimationCurve _default_curve,string _help = "" )
		{
			BeginHorizontal();
			_curve = EditorGUILayout.CurveField( new GUIContent( _title, _tooltip), _curve );

			if( _default_curve == null || _default_curve.length == 0 )
			{
				Keyframe[] _keys = new Keyframe[2];
				_keys[0] = new Keyframe( 3, 0.6f);
				_keys[1] = new Keyframe( 6, 0.3f);

				_default_curve = new AnimationCurve(_keys);
			}
				

			if( ! AnimationTools.CompareAnimationCurve( _curve, _default_curve ) )
				GUI.backgroundColor = Color.yellow;

			if ( GUILayout.Button( new GUIContent( "D", "Set default curve" ), ICEEditorStyle.CMDButtonDouble ))
				_curve = _default_curve;

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

			EndHorizontalHelp( _help );
			return _curve;
		}


		public static float DefaultSlider( string _title, string _tooltip, float _value, float _precision, float _min = 0, float _max = 0, float _default = 0 , string _help = "" )
		{
			_default = Round( _default, _precision );

			BeginHorizontal();
				_value = BasicSlider( _title, _tooltip, _value, _precision, _min, _max );
				_value = ButtonDefault( _value, _default );
			EndHorizontalHelp( _help );
			return _value;
			
			
		}



		public static float BasicDefaultSlider( string _title, string _tooltip, float _value, float _precision, float _min = 0, float _max = 0, float _default = 0 )
		{
			_value = BasicSlider( _title, _tooltip, _value, _precision, _min, _max );
			_value = ButtonDefault( _value, _default );

			return _value;			
		}

		public static float Slider( string _title, string _tooltip, float _value, float _precision, float _min = 0, float _max = 0, string _help = ""  )
		{
			BeginHorizontal();
				_value = BasicSlider( _title, _tooltip, _value, _precision, _min, _max );
			EndHorizontalHelp( _help );
			return _value;
		}

		public static float MaxBasicSlider( string _title, string _tooltip, float _value, float _precision, float _min, ref float _max, string _help = ""  )
		{
			_value = Round( _value, _precision );
			_value = MinMaxCheck( _value, _min, _max  );
			_value = EditorGUILayout.Slider( new GUIContent( _title, _tooltip ),_value,_min, _max );

			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			_max = EditorGUILayout.FloatField( _max, GUILayout.Width( 50 ));
			EditorGUI.indentLevel = _indent;

			_value = PlusMinusGroup( _value, _precision  );
			_value = MinMaxCheck( _value, _min, _max  );
			_value = Round( _value, _precision );
			return _value;
		}

		public static float BasicSlider( string _title, string _tooltip, float _value, float _precision, float _min = 0, float _max = 0, string _help = ""  )
		{
			_value = Round( _value, _precision );
			_value = MinMaxCheck( _value, _min, _max  );
			_value = EditorGUILayout.Slider( new GUIContent( _title, _tooltip ),_value,_min, _max );			
			_value = PlusMinusGroup( _value, _precision  );
			_value = MinMaxCheck( _value, _min, _max  );
			_value = Round( _value, _precision );
			return _value;
		}

		public static void MinMaxGroupSimple( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, ref float _max_value, float _precision, float _size = 40, string _help = "" )
		{
			_min = Round( _min, _precision );
			_max = Round( _max, _precision );

			ICEEditorLayout.BeginHorizontal();			
				EditorGUILayout.PrefixLabel( new GUIContent( _title, _tooltip ) );				
				int _indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
					_min = EditorGUILayout.FloatField( _min , GUILayout.Width( _size ) );				
					EditorGUILayout.MinMaxSlider( ref _min, ref _max, _min_value, _max_value ); 				
					_max = EditorGUILayout.FloatField( _max , GUILayout.Width( _size ) );
					_max_value = EditorGUILayout.FloatField( _max_value, GUILayout.Width( _size ) ); 

					_min = MinMaxCheck( _min, _min_value, _max );
					_max = MinMaxCheck( _max, _min, _max_value );
				EditorGUI.indentLevel = _indent;
			ICEEditorLayout.EndHorizontal( _help );

			_min = Round( _min, _precision );
			_max = Round( _max, _precision );
		}

		public static void MinMaxGroup( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, float _max_value, float _precision, string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();			
			EditorGUILayout.PrefixLabel( new GUIContent( _title, _tooltip ) );				
			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			_min = ICEEditorLayout.PlusMinusGroup( _min, _precision );
			_min = EditorGUILayout.FloatField( _min , GUILayout.Width( 40 ) );				
			EditorGUILayout.MinMaxSlider( ref _min, ref _max, _min_value, _max_value ); 				
			_max = EditorGUILayout.FloatField( _max , GUILayout.Width( 40 ) );
			_max = ICEEditorLayout.PlusMinusGroup( _max, _precision );
			
			_min = MinMaxCheck( _min, _min_value, _max );
			_max = MinMaxCheck( _max, _min, _max_value );
			EditorGUI.indentLevel = _indent;
			ICEEditorLayout.EndHorizontal( _help );
		}

		public static void RandomMinMaxGroupExt( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, ref float _max_value,float _min_default, float _max_default, float _size, float _precision, string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();			
			EditorGUILayout.PrefixLabel( new GUIContent( _title, _tooltip ) );				
			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			_min = ICEEditorLayout.PlusMinusGroup( _min, _precision );
			_min = EditorGUILayout.FloatField( _min , GUILayout.MinWidth( _size ) );				
			EditorGUILayout.MinMaxSlider( ref _min, ref _max, _min_value, _max_value ); 				
			_max = EditorGUILayout.FloatField( _max , GUILayout.MinWidth( _size ) );
			_max = ICEEditorLayout.PlusMinusGroup( _max, _precision );
			_max_value = EditorGUILayout.FloatField( _max_value, GUILayout.MinWidth( _size ) ); 


			if( Button( "RND", "Random Values", ICEEditorStyle.CMDButtonDouble ) )
			{
				_min = UnityEngine.Random.Range( _min_value, _max_value );
				_max = UnityEngine.Random.Range( _min, _max_value );
			}

			ICEEditorLayout.ButtonMinMaxDefault( ref _min, ref _max, _min_default, _max_default );

			_min = MinMaxCheck( _min, _min_value, _max );
			_max = MinMaxCheck( _max, _min, _max_value );

			EditorGUI.indentLevel = _indent;
			ICEEditorLayout.EndHorizontal( _help );
		}

		public static void RandomMinMaxGroup( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, float _max_value,float _min_default, float _max_default, float _precision, string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();			
			EditorGUILayout.PrefixLabel( new GUIContent( _title, _tooltip ) );				
			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			_min = ICEEditorLayout.PlusMinusGroup( _min, _precision );
			_min = EditorGUILayout.FloatField( _min , GUILayout.Width( 40 ) );				
			EditorGUILayout.MinMaxSlider( ref _min, ref _max, _min_value, _max_value ); 				
			_max = EditorGUILayout.FloatField( _max , GUILayout.Width( 40 ) );
			_max = ICEEditorLayout.PlusMinusGroup( _max, _precision );



			if( Button( "RND", "Random Values", ICEEditorStyle.CMDButtonDouble ) )
			{
				_min = UnityEngine.Random.Range( _min_value, _max_value );
				_max = UnityEngine.Random.Range( _min, _max_value );
			}

			ICEEditorLayout.ButtonMinMaxDefault( ref _min, ref _max, _min_default, _max_default );

			_min = MinMaxCheck( _min, _min_value, _max );
			_max = MinMaxCheck( _max, _min, _max_value );

			EditorGUI.indentLevel = _indent;
			ICEEditorLayout.EndHorizontal( _help );
		}

		public static float MinMaxCheck( float _value, float _min, float _max  )
		{
			if( _value < _min )
				_value = _min;
			
			if( _max > _min && _value > _max )
				_value = _max;

			return _value;
		}

		public static float PlusMinusGroup( float _value, float _precision )
		{
			return PlusMinusGroup( _value, _precision, ICEEditorStyle.CMDButton );
		}

		public static float PlusMinusGroup( float _value, float _precision, GUIStyle _style  )
		{
			if( GUILayout.Button( new GUIContent( "<", "minus " + _precision ), _style ) )
				_value -= _precision;
			
			if( GUILayout.Button( new GUIContent( ">", "plus " + _precision ), _style ) )
				_value += _precision; 

			return _value;
		}

		public static float ToggleSlider( string _title, string _tooltip, float _value, float _precision, float _min, float _max, float _default, ref bool _toggle, string _toggle_title, string _toggle_tooltip, string _help = ""  )
		{
			BeginHorizontal();
			
			EditorGUI.BeginDisabledGroup( _toggle == true );
			_value = BasicSlider( _title, _tooltip, _value, _precision, _min, _max );
			_value = ButtonDefault( _value, _default );
			EditorGUI.EndDisabledGroup();
			
			if( _toggle )
				GUI.backgroundColor = Color.yellow;
			else 
				GUI.backgroundColor = Color.green;
			
			if ( GUILayout.Button( new GUIContent( _toggle_title, _toggle_tooltip ), ICEEditorStyle.CMDButtonDouble ))
				_toggle = ! _toggle;
			
			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			
			EndHorizontalHelp( _help );
			return _value;
		}

		public static float RandomSlider( string _title, string _tooltip, float _value, float _precision, float _min, float _max, ref bool _toggle, float _default, string _help = ""  )
		{
			BeginHorizontal();
			
			EditorGUI.BeginDisabledGroup( _toggle == true );
			_value = BasicSlider( _title, _tooltip, _value, _precision, _min, _max );
			_value = ButtonDefault( _value, _default );
			EditorGUI.EndDisabledGroup();
			
			if( _toggle )
				GUI.backgroundColor = Color.yellow;
			else 
				GUI.backgroundColor = Color.green;
			
			if ( GUILayout.Button( new GUIContent( "RND", "Use Random Value" ), ICEEditorStyle.CMDButtonDouble ))
				_toggle = ! _toggle;
			
			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			
			EndHorizontalHelp( _help );
			return _value;
		}

		public static float AutoSlider( string _title, string _tooltip, float _value, float _precision, float _min, float _max, ref bool _toggle, float _default, string _help = ""  )
		{
			BeginHorizontal();
			
			EditorGUI.BeginDisabledGroup( _toggle == true );
				_value = BasicSlider( _title, _tooltip, _value, _precision, _min, _max );
				_value = ButtonDefault( _value, _default );
			EditorGUI.EndDisabledGroup();
			
			GUI.backgroundColor = Color.green;
			_toggle = ButtonCheck( "Auto", "Automatic", _toggle , ICEEditorStyle.CMDButtonDouble );

			EndHorizontalHelp( _help );
			return _value;
		}

		public static Vector3 Velocity( string _title, string _tooltip, Vector3 _velocity, float _precision, float _min, float _max, Vector3 _default_velocity )
		{
			Label( _title, false );
			EditorGUI.indentLevel++;

			bool _toggle = false;

			_velocity.x = AutoSlider( "Sidewards \t(x)", "x-Velocity", _velocity.x, _precision, _min, _max, ref _toggle, 0 );

			if( _toggle )
				_velocity.x = _default_velocity.x;
			_toggle = false;

			_velocity.y = AutoSlider( "Vertical \t(y)", "y-Velocity", _velocity.y, _precision, _min, _max, ref _toggle,0 );

			if( _toggle )
				_velocity.y = _default_velocity.y;
			_toggle = false;

			_velocity.z = AutoSlider( "Forwards \t(z)", "z-Velocity", _velocity.z, _precision, _min, _max, ref _toggle, 0 );

			if( _toggle )
				_velocity.z = _default_velocity.z;
			_toggle = false;

			EditorGUI.indentLevel--;
			return _velocity;
		}

		public static string AnimatorParametersPopup( Animator _animator, string title, string _hint, string key )
		{
			if( _animator == null || _animator.parameterCount == 0 )
				return "";

			List<string> _options = new List<string>();
			int _selected = 0;
			for( int _i = 0 ; _i < _animator.parameterCount; _i++ )
			{
				AnimatorControllerParameter _parameter = _animator.parameters[_i];

				if( _parameter !=  null )
				{
					_options.Add( _parameter.name );

					if( _parameter.name == key )
						_selected = _i;
				}
			}

			_selected = Popup( title, _hint, _selected, _options.ToArray() );

			return (string)_options[ _selected ];
		}

		public static AnimatorControllerParameter AnimatorParametersPopupData( Animator _animator, string _key, params GUILayoutOption[] _layouts )
		{
			if( _animator == null || _animator.parameterCount == 0 )
				return null;
			
			GUIContent[] _options = new GUIContent[_animator.parameterCount];
			int _selected = 0;
			for( int _i = 0 ; _i < _animator.parameterCount; _i++ )
			{
				AnimatorControllerParameter _parameter = _animator.parameters[_i];
				
				if( _parameter !=  null )
				{
					_options[_i] = new GUIContent();
					_options[_i].text = _parameter.name;
					
					if( _parameter.name == _key )
						_selected = _i;
				}
			}

		
			_selected = EditorGUILayout.Popup( _selected, _options, _layouts ); 

			return (AnimatorControllerParameter)_animator.parameters[ _selected ];
		}


		public static string DrawListPopup( string title, string key, List<string> list )
		{
			string[] options = list.ToArray();
			int options_index = 0;
			
			for( int i = 0 ; i < options.Length ; i++)
			{
				if( (string)options[i] == key )
				{
					options_index = i;
					break;
				}
			}
			
			options_index = EditorGUILayout.Popup( title, options_index, options );
			
			return (string)options[ options_index ];
		}

		public static void DrawProgressBarFull( string _title, float _value, string _help = "" )
		{
			BeginHorizontal();					
			Rect _rect = GUILayoutUtility.GetRect(0,15);
			EditorGUI.ProgressBar( _rect, _value/100, _title + " (" +_value + "%)" );			
			EndHorizontalHelp( _help );
		}

		public static void DrawProgressBar( string _title, float _value, string _help = "" )
		{
			BeginHorizontal();			
				EditorGUILayout.PrefixLabel( _title );			
				Rect _rect = GUILayoutUtility.GetRect(0,16);
				EditorGUI.ProgressBar( _rect, _value/100, _value + "%" );			
			EndHorizontalHelp( _help );
		}

		public static float DrawValueButtons( float value, float step, float min = 0, float max = 0 )
		{
			
			if (GUILayout.Button( new GUIContent( "<", "minus " + step ), ICEEditorStyle.CMDButton ))
				value -= step;
			
			if (GUILayout.Button( new GUIContent( ">", "plus " + step ), ICEEditorStyle.CMDButton ))
				value += step; 
			
			if( value < min )
				value = min;
			
			if( max > min && value > max )
				value = max;
			
			return value;
		}
	}
}
