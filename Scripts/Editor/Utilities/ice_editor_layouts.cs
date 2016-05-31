// ##############################################################################
//
// ice_editor_layouts.cs | ICEEditorLayout
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
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ICE.World.EditorUtilities;
using ICE.World.Utilities;
//using ICE.Creatures;
//using ICE.Creatures.EnumTypes;
//using ICE.Creatures.EditorInfos;
//using ICE.Creatures.Attributes;


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
		NONE
	}

	public static class ICEEditorLayout
	{
		public static Color DefaultBackgroundColor;
		public static Color DefaultGUIColor;

		public static Color InfoColor = new HSBColor( 0.15f ,0.25f, 1 ).ToColor();

		static ICEEditorLayout() {	
			Init();
		}

		public static void Init(){
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

		public static void DrawPriorityInfo( int _priority, string _hint = "" )
		{
			if( _hint == "" )
				_hint = "Priority";
			
			GUI.backgroundColor = new HSBColor( _priority * 0.0025f ,0.5f,1f ).ToColor();
			ICEEditorLayout.Button( _priority.ToString(), _hint, ICEEditorStyle.CMDButtonDouble );
			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		}

		public static float Round( float _value, float _step )
		{
			if( Application.isPlaying )
				return _value;
			else
				return Mathf.Round( _value / _step ) * _step;
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

			EndHorizontalInfo( _help );
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
			EndHorizontalInfo( _help );


			return _offset;
		}

		public static Vector3 OffsetGroup( string _title, Vector3 _offset, string _help = ""  )
		{
			BeginHorizontal();
				_offset = EditorGUILayout.Vector3Field( _title, _offset );				
				if (GUILayout.Button( new GUIContent( "RESET", "Reset offset values" ), ICEEditorStyle.CMDButtonDouble ))
					_offset = Vector3.zero;
			EndHorizontalInfo( _help );			
			
			return _offset;
		}

		public static int Popup( string _title, string _hint, int _selected, string[] _options, string _help )
		{
			BeginHorizontal();
			int _value = Popup( _title, _hint, _selected, _options ); 
			EndHorizontalInfo( _help );
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
			EndHorizontalInfo( _help );
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
			EndHorizontalInfo( _help );
			return _value;
		}
			
		public static string AnimationPopup( Animation _animation, string _name, string _title = "" )
		{
			ICEEditorLayout.BeginHorizontal();
			_name = AnimationPopupBase( _animation, _name, _title );
			ICEEditorLayout.EndHorizontal();

			return _name;
		}

		public static string AnimationPopupBase( Animation _animation, string _name, string _title = "" )
		{
			if( _animation == null )
				return "";

			string[] _animation_names = new string[ _animation.GetClipCount() ];
			int[] _animation_index = new int[ _animation.GetClipCount() ];

			//

			int _i = 0;
			int _selected = 0;
			if( _animation.GetClipCount() > 0 )
			{
				foreach (AnimationState _state in _animation )
				{
					if( _state == null || _i >= _animation.GetClipCount() )
						continue;

					_animation_index[_i] = _i;
					_animation_names[_i] = _state.name;

					if( _name == _animation_names[_i] )
						_selected = _i;

					_i++;
				}
			}

			if( _title == "" )
				_title = "Animation";

			_selected = (int)EditorGUILayout.IntPopup( _title , _selected, _animation_names,_animation_index);
			//new GUIContent( _title , "Animation name and length in seconds" )

			_selected = (int)ICEEditorLayout.PlusMinusGroup( _selected, 1, ICEEditorStyle.CMDButtonDouble );

			if( _selected < 0 )
				_selected = 0;
			else if( _selected >= _animation.GetClipCount() - 1 )
				_selected = _animation.GetClipCount() - 1;

			return _animation_names[_selected];
		}

		public static int AnimationPopup( Animation _animation, int _selected, string _title = "" )
		{
			ICEEditorLayout.BeginHorizontal();
			_selected = AnimationPopupBase( _animation, _selected, _title );
			ICEEditorLayout.EndHorizontal();

			return _selected;
		}

		public static int AnimationPopupBase( Animation _animation, int _selected, string _title = "" )
		{
			if( _animation == null )
				return 0;



			string[] _animation_names = new string[ _animation.GetClipCount() ];
			int[] _animation_index = new int[ _animation.GetClipCount() ];

			int i = 0;	
			foreach (AnimationState _animation_state in _animation )
			{
				_animation_index[i] = i;
				_animation_names[i] = _animation_state.name;

				i++;
			}
			if( _title == "" )
				_title = "Animation";

			_selected = (int)EditorGUILayout.IntPopup( _title , _selected, _animation_names,_animation_index);
			//new GUIContent( _title , "Animation name and length in seconds" )

			_selected = (int)ICEEditorLayout.PlusMinusGroup( _selected, 1, ICEEditorStyle.CMDButtonDouble );

			if( _selected < 0 )
				_selected = 0;
			else if( _selected >= _animation.GetClipCount() - 1 )
				_selected = _animation.GetClipCount() - 1;

			return _selected;
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
			EndHorizontalInfo( _help );
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
				EndHorizontalInfo( _help );
				return _value;
			}			
		}

		public static bool InfoToggle( string _title, string _hint, bool _toggle, string _help = "" )
		{
			BeginHorizontal();
				bool _value = Toggle( _title, _hint, _toggle );
				GUILayout.FlexibleSpace();			
			EndHorizontalInfo( _help );
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
				EndHorizontalInfo( _help );
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
				EndHorizontalInfo( _help );
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
				EndHorizontalInfo( _help );
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
				EndHorizontalInfo( _help );
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
				EndHorizontalInfo( _help );
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
				EndHorizontalInfo( _help );		
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

			EndHorizontalInfo( _help );	

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

				EndHorizontalInfo( _help );		
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

		public static void EndHorizontal( ref bool _show_info , ref string _info, string _help = "" )
		{
			if( _info.Trim() != "" )
				GUI.backgroundColor = Color.magenta;

			_show_info = ButtonCheck( "i", "Show info text", _show_info, ICEEditorStyle.CMDButton ); 

			if( _help == "" )
				EditorGUILayout.EndHorizontal();
			else
				EndHorizontalInfo( _help );

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
				EndHorizontalInfo( _help );
		}

		public static void EndHorizontalInfo( string _help )
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
				EndHorizontalInfo( _help );
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

			EndHorizontalInfo( _help );

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
				EndHorizontalInfo( _help );
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

		public static float DurationSlider( string _title, string _tooltip, float _value, float _step, float _min, float _max, float _default, ref bool _transit, string _help = ""  )
		{
			BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _transit == true );				
					_value = BasicSlider( _title, _tooltip, _value, _step, _min, _max );
					
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
			
			EndHorizontalInfo( _help );
			
			return _value;
			
			
		}

		public static float MaxDefaultSlider( string _title, string _tooltip, float _value, float _step, float _min, ref float _max, float _default = 0 , string _help = "" )
		{
			BeginHorizontal();
			_value = MaxBasicSlider( _title, _tooltip, _value, _step, _min, ref _max );
			_value = ButtonDefault( _value, _default );
			EndHorizontalInfo( _help );
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

			EndHorizontalInfo( _help );
			return _curve;
		}


		public static float DefaultSlider( string _title, string _tooltip, float _value, float _step, float _min = 0, float _max = 0, float _default = 0 , string _help = "" )
		{
			_default = Round( _default, _step );

			BeginHorizontal();
				_value = BasicSlider( _title, _tooltip, _value, _step, _min, _max );
				_value = ButtonDefault( _value, _default );
			EndHorizontalInfo( _help );
			return _value;
			
			
		}



		public static float BasicDefaultSlider( string _title, string _tooltip, float _value, float _step, float _min = 0, float _max = 0, float _default = 0 )
		{
			_value = BasicSlider( _title, _tooltip, _value, _step, _min, _max );
			_value = ButtonDefault( _value, _default );

			return _value;			
		}

		public static float Slider( string _title, string _tooltip, float _value, float _step, float _min = 0, float _max = 0, string _help = ""  )
		{
			BeginHorizontal();
				_value = BasicSlider( _title, _tooltip, _value, _step, _min, _max );
			EndHorizontalInfo( _help );
			return _value;
		}

		public static float MaxBasicSlider( string _title, string _tooltip, float _value, float _step, float _min, ref float _max, string _help = ""  )
		{
			_value = Round( _value, _step );
			_value = MinMaxCheck( _value, _min, _max  );
			_value = EditorGUILayout.Slider( new GUIContent( _title, _tooltip ),_value,_min, _max );

			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			_max = EditorGUILayout.FloatField( _max, GUILayout.Width( 50 ));
			EditorGUI.indentLevel = _indent;

			_value = PlusMinusGroup( _value, _step  );
			_value = MinMaxCheck( _value, _min, _max  );
			_value = Round( _value, _step );
			return _value;
		}

		public static float BasicSlider( string _title, string _tooltip, float _value, float _step, float _min = 0, float _max = 0, string _help = ""  )
		{
			_value = Round( _value, _step );
			_value = MinMaxCheck( _value, _min, _max  );
			_value = EditorGUILayout.Slider( new GUIContent( _title, _tooltip ),_value,_min, _max );			
			_value = PlusMinusGroup( _value, _step  );
			_value = MinMaxCheck( _value, _min, _max  );
			_value = Round( _value, _step );
			return _value;
		}

		public static void MinMaxGroupSimple( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, ref float _max_value, float _step, float _size = 40, string _help = "" )
		{
			_min = Round( _min, _step );
			_max = Round( _max, _step );

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

			_min = Round( _min, _step );
			_max = Round( _max, _step );
		}

		public static void MinMaxGroup( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, float _max_value, float _step, string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();			
			EditorGUILayout.PrefixLabel( new GUIContent( _title, _tooltip ) );				
			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			_min = ICEEditorLayout.PlusMinusGroup( _min, _step );
			_min = EditorGUILayout.FloatField( _min , GUILayout.Width( 40 ) );				
			EditorGUILayout.MinMaxSlider( ref _min, ref _max, _min_value, _max_value ); 				
			_max = EditorGUILayout.FloatField( _max , GUILayout.Width( 40 ) );
			_max = ICEEditorLayout.PlusMinusGroup( _max, _step );
			
			_min = MinMaxCheck( _min, _min_value, _max );
			_max = MinMaxCheck( _max, _min, _max_value );
			EditorGUI.indentLevel = _indent;
			ICEEditorLayout.EndHorizontal( _help );
		}

		public static void RandomMinMaxGroupExt( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, ref float _max_value,float _min_default, float _max_default, float _size, float _step, string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();			
			EditorGUILayout.PrefixLabel( new GUIContent( _title, _tooltip ) );				
			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			_min = ICEEditorLayout.PlusMinusGroup( _min, _step );
			_min = EditorGUILayout.FloatField( _min , GUILayout.MinWidth( _size ) );				
			EditorGUILayout.MinMaxSlider( ref _min, ref _max, _min_value, _max_value ); 				
			_max = EditorGUILayout.FloatField( _max , GUILayout.MinWidth( _size ) );
			_max = ICEEditorLayout.PlusMinusGroup( _max, _step );
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

		public static void RandomMinMaxGroup( string _title, string _tooltip, ref float _min, ref float _max, float _min_value, float _max_value,float _min_default, float _max_default, float _step, string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();			
			EditorGUILayout.PrefixLabel( new GUIContent( _title, _tooltip ) );				
			int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			_min = ICEEditorLayout.PlusMinusGroup( _min, _step );
			_min = EditorGUILayout.FloatField( _min , GUILayout.Width( 40 ) );				
			EditorGUILayout.MinMaxSlider( ref _min, ref _max, _min_value, _max_value ); 				
			_max = EditorGUILayout.FloatField( _max , GUILayout.Width( 40 ) );
			_max = ICEEditorLayout.PlusMinusGroup( _max, _step );



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

		public static float PlusMinusGroup( float _value, float _step )
		{
			return PlusMinusGroup( _value, _step, ICEEditorStyle.CMDButton );
		}

		public static float PlusMinusGroup( float _value, float _step, GUIStyle _style  )
		{
			if( GUILayout.Button( new GUIContent( "<", "minus " + _step ), _style ) )
				_value -= _step;
			
			if( GUILayout.Button( new GUIContent( ">", "plus " + _step ), _style ) )
				_value += _step; 

			return _value;
		}

		public static float ToggleSlider( string _title, string _tooltip, float _value, float _step, float _min, float _max, float _default, ref bool _toggle, string _toggle_title, string _toggle_tooltip, string _help = ""  )
		{
			BeginHorizontal();
			
			EditorGUI.BeginDisabledGroup( _toggle == true );
			_value = BasicSlider( _title, _tooltip, _value, _step, _min, _max );
			_value = ButtonDefault( _value, _default );
			EditorGUI.EndDisabledGroup();
			
			if( _toggle )
				GUI.backgroundColor = Color.yellow;
			else 
				GUI.backgroundColor = Color.green;
			
			if ( GUILayout.Button( new GUIContent( _toggle_title, _toggle_tooltip ), ICEEditorStyle.CMDButtonDouble ))
				_toggle = ! _toggle;
			
			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			
			EndHorizontalInfo( _help );
			return _value;
		}

		public static float RandomSlider( string _title, string _tooltip, float _value, float _step, float _min, float _max, ref bool _toggle, float _default, string _help = ""  )
		{
			BeginHorizontal();
			
			EditorGUI.BeginDisabledGroup( _toggle == true );
			_value = BasicSlider( _title, _tooltip, _value, _step, _min, _max );
			_value = ButtonDefault( _value, _default );
			EditorGUI.EndDisabledGroup();
			
			if( _toggle )
				GUI.backgroundColor = Color.yellow;
			else 
				GUI.backgroundColor = Color.green;
			
			if ( GUILayout.Button( new GUIContent( "RND", "Use Random Value" ), ICEEditorStyle.CMDButtonDouble ))
				_toggle = ! _toggle;
			
			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			
			EndHorizontalInfo( _help );
			return _value;
		}

		public static float AutoSlider( string _title, string _tooltip, float _value, float _step, float _min, float _max, ref bool _toggle, float _default, string _help = ""  )
		{
			BeginHorizontal();
			
			EditorGUI.BeginDisabledGroup( _toggle == true );
				_value = BasicSlider( _title, _tooltip, _value, _step, _min, _max );
				_value = ButtonDefault( _value, _default );
			EditorGUI.EndDisabledGroup();
			
			GUI.backgroundColor = Color.green;
			_toggle = ButtonCheck( "Auto", "Automatic", _toggle , ICEEditorStyle.CMDButtonDouble );

			EndHorizontalInfo( _help );
			return _value;
		}

		public static Vector3 Velocity( string _title, string _tooltip, Vector3 _velocity, float _step, float _min, float _max, Vector3 _default_velocity )
		{
			Label( _title, false );
			EditorGUI.indentLevel++;

			bool _toggle = false;

			_velocity.x = AutoSlider( "Sidewards \t(x)", "x-Velocity", _velocity.x, _step, _min, _max, ref _toggle, 0 );

			if( _toggle )
				_velocity.x = _default_velocity.x;
			_toggle = false;

			_velocity.y = AutoSlider( "Vertical \t(y)", "y-Velocity", _velocity.y, _step, _min, _max, ref _toggle,0 );

			if( _toggle )
				_velocity.y = _default_velocity.y;
			_toggle = false;

			_velocity.z = AutoSlider( "Forwards \t(z)", "z-Velocity", _velocity.z, _step, _min, _max, ref _toggle, 0 );

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

		public static void DrawProgressBar( string _title, float _value, string _help = "" )
		{
			BeginHorizontal();			
				EditorGUILayout.PrefixLabel( _title );			
				Rect _rect = GUILayoutUtility.GetRect(0,15);
				EditorGUI.ProgressBar( _rect, _value/100, _value + "%" );			
			EndHorizontalInfo( _help );
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
