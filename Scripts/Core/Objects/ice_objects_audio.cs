// ##############################################################################
//
// ice_objects_audio.cs | ICE.World.Objects.AudioDataObject | AudioObject | AudioSourceObject
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
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

using ICE.World;
using ICE.World.Objects;

namespace ICE.World.Objects
{
	[System.Serializable]
	public class AudioDataObject : ICEImpulsTimerObject
	{
		public AudioDataObject(){}
		public AudioDataObject( ICEWorldBehaviour _component ) : base( _component ){}
		public AudioDataObject( AudioDataObject _audio ) : base( _audio as ICEImpulsTimerObject )
		{
			base.Init( _audio.OwnerComponent );

			Enabled = _audio.Enabled;
			Loop = _audio.Loop;
			MaxDistance = _audio.MaxDistance;
			MinDistance =_audio.MinDistance;
			MaxPitch = _audio.MaxPitch;
			MinPitch = _audio.MinPitch;
			RolloffMode = _audio.RolloffMode;
			Volume = _audio.Volume;

			m_Clips = new List<AudioClip>(); 
			foreach( AudioClip _clip in _audio.Clips )
				m_Clips.Add( _clip );
		}

		[XmlIgnore]
		public List<AudioClip> Clips{
			set{m_Clips = value;}
			get{ return m_Clips = ( m_Clips == null ? new List<AudioClip>():m_Clips );}
		}

		[SerializeField]
		protected List<AudioClip> m_Clips = new List<AudioClip>(); 

		public float Volume = 0.5f;	
		public float MinPitch = 1.0f;	
		public float MaxPitch = 1.5f;	
		public float MinDistance = 2;	
		public float MaxDistance = 7;	
		public bool Loop = false;
		public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;
	}

	[System.Serializable]
	public class AudioObject : AudioDataObject
	{
		public AudioObject(){}
		public AudioObject( ICEWorldBehaviour _component ) : base( _component ){}
		public AudioObject( AudioObject _audio ) : base( _audio as AudioDataObject ){}

		private AudioClip m_Selected = null;
		[XmlIgnore]
		public AudioClip Selected{
			get{return m_Selected;}
		}

		public void AddClip( AudioClip _clip )
		{
			if( _clip != null )
				Clips.Add( _clip );
		}

		public void AddClip()
		{
			m_Clips.Add( new AudioClip() );
		}

		public void DeleteClip( int _index )
		{
			if( _index >= 0 && _index < m_Clips.Count )
				m_Clips.RemoveAt( _index );
		}

		public AudioClip GetClip()
		{
			if( m_Clips.Count == 0 )
				return null;

			reroll:	
			AudioClip _clip = m_Clips[Random.Range(0,m_Clips.Count)];
			
			if( _clip != null )
			{
				if ( m_Clips.Count > 1 && _clip == m_Selected )
					goto reroll;

				m_Selected = _clip;
			}

			return m_Selected;
		}
	}
	
	[System.Serializable]
	public class AudioSourceObject : ICEOwnerObject
	{
		public AudioSourceObject(){}
		public AudioSourceObject( ICEWorldBehaviour _component  ) : base( _component )
		{ Init( _component ); }
			
		private AudioSource m_AudioSource = null;
		public AudioSource Source{
			set{ m_AudioSource = value; }
			get{ return m_AudioSource;}
		}

		public AudioClip CurrentClip{
			get{ return ( m_AudioSource != null ? m_AudioSource.clip:null ); }
		}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );
			
			if( m_AudioSource == null && Application.isPlaying )
			{
				m_AudioSource = m_Owner.AddComponent<AudioSource>(); 

				m_AudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
				m_AudioSource.volume = 0.5f;
				m_AudioSource.pitch = 1f;
				m_AudioSource.minDistance = 10f;
				m_AudioSource.maxDistance = 100f;
				m_AudioSource.spatialBlend = 1f;
			}
		}

		public void PlayOneShot( AudioObject _audio )
		{
			if( m_AudioSource == null )
				return;

			if( _audio.Enabled && _audio.GetClip() != null )
			{
				if( m_AudioSource.isPlaying )
					m_AudioSource.Stop();

				m_AudioSource.clip = _audio.Selected;				
				m_AudioSource.volume = _audio.Volume;
				m_AudioSource.pitch = Random.Range( _audio.MinPitch, _audio.MaxPitch) * Time.timeScale;
				m_AudioSource.rolloffMode = _audio.RolloffMode;	
				m_AudioSource.minDistance = _audio.MinDistance;
				m_AudioSource.maxDistance = _audio.MaxDistance;
				m_AudioSource.spatialBlend = 1.0f;							

				m_AudioSource.loop = false;				
				m_AudioSource.PlayOneShot( _audio.Selected );
			}
			else
			{
				m_AudioSource.Stop();
			}
		}

		public void Play( AudioObject _data )
		{
			if( m_AudioSource == null )
				return;

			if( _data.Enabled && _data.GetClip() != null )
			{
				if( m_AudioSource.isPlaying )
					m_AudioSource.Stop();

				m_AudioSource.clip = _data.Selected;				
				m_AudioSource.volume = _data.Volume;
				m_AudioSource.pitch = Random.Range( _data.MinPitch, _data.MaxPitch) * Time.timeScale;
				m_AudioSource.rolloffMode = _data.RolloffMode;	
				m_AudioSource.minDistance = _data.MinDistance;
				m_AudioSource.maxDistance = _data.MaxDistance;
				m_AudioSource.spatialBlend = 1.0f;							

				if( _data.Loop )
				{
					m_AudioSource.loop = true;				
					m_AudioSource.Play();
				}
				else
				{
					m_AudioSource.loop = false;
					m_AudioSource.PlayOneShot( _data.Selected );
				}
			}
			else
			{
				m_AudioSource.Stop();
			}
		}

		/*
		 * 

		private AudioSource m_AudioSource = null;	
		private AudioClip m_AudioClip = null;			
		private AudioClip m_AudioClipLast = null;	
		private void PlaySound( SurfaceDataObject _surface )
		{
			if( _surface == null || _surface.Sounds == null || ! _surface.SoundsEnabled || _surface.Sounds.Count == 0)
				return;
			
		reroll:
				m_AudioClip = _surface.Sounds[Random.Range(0,_surface.Sounds.Count)]; // get a random sound
			
			if( m_AudioClip != null )
			{
				if (  _surface.Sounds.Count > 1 && m_AudioClip == m_AudioClipLast )
					goto reroll;
				
				//m_AudioSource.maxDistance 
				m_AudioSource.pitch = Random.Range(_surface.Audio.MinPitch, _surface.Audio.MaxPitch ) * Time.timeScale;
				m_AudioSource.clip = m_AudioClip;
				m_AudioSource.Play();
				m_AudioClipLast = m_AudioClip;
			}
		}*/

		public void Stop()
		{
			if( m_AudioSource == null )
				return;

			m_AudioSource.Stop();
		}
	}
}
