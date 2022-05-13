using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Media;

using System.ComponentModel;

using Un4seen.Bass;

namespace SEViewer
{
    //-----------------------------------------------------------------------------------------------
    //
    //実際に音を鳴らすクラス。*.wavは.net自前の機能。*.oggは他の方の作られたdll任せ。"WaveIO.cs"が*.ogg機能。
    //
    //-----------------------------------------------------------------------------------------------
    class soundPlayer
    {
		private double m_length = 0;
		private int playHandle = 0;

		static private Form1 m_parent = null;


		public soundPlayer(Form1 parent)
		{
			m_parent = parent;

			if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
			{
				return;
			}

		}

		public void Dispose()
		{
			Bass.BASS_Stop();
			Bass.BASS_PluginFree(0);
			Bass.BASS_Free();
		}

		//-----------------------------------------------------------------------------------------------
		//ファイルを再生する
		//-----------------------------------------------------------------------------------------------
		public bool PlaySound(string fileName, int vol = 255, bool isLoop = false, double second = 0)
		{
			StopSound();
			playHandle = GetHandle(fileName);

			//if ((Bass.BASS_ChannelFlags(playHandle, BASSFlag.BASS_DEFAULT, BASSFlag.BASS_DEFAULT) & BASSFlag.BASS_SAMPLE_LOOP) == BASSFlag.BASS_SAMPLE_LOOP)
			if(isLoop==false)
			{
				// loop flag was set, so remove it
				Bass.BASS_ChannelFlags(playHandle, BASSFlag.BASS_DEFAULT, BASSFlag.BASS_SAMPLE_LOOP);
			}
			else
			{
				// loop flag was not set, so set it
				Bass.BASS_ChannelFlags(playHandle, BASSFlag.BASS_SAMPLE_LOOP, BASSFlag.BASS_SAMPLE_LOOP);
			}
			
			Bass.BASS_ChannelPlay(playHandle, false);

//			int syncHandle = Bass.BASS_ChannelSetSync(playHandle, BASSSync.BASS_SYNC_END, 0, proc, IntPtr.Zero);
	//		if (syncHandle == 0) throw new InvalidOperationException("cannot set sync");

			Bass.BASS_ChannelSetPosition(playHandle, second);
			SetVolume(vol);
			//ret = Bass.BASS_ChannelGetLength(playHandle);
			m_length = Bass.BASS_ChannelBytes2Seconds(playHandle, Bass.BASS_ChannelGetLength(playHandle));

			return true;
		}

		protected int GetHandle(string filepath)
		{
			int handle = Bass.BASS_StreamCreateFile(filepath, 0, 0, BASSFlag.BASS_DEFAULT);
			if (handle == 0) throw new ArgumentException("cannot create a stream.");
			return handle;
		}


		//-----------------------------------------------------------------------------------------------
		//再生されている音を止める
		//-----------------------------------------------------------------------------------------------
		public void StopSound()
		{
			Bass.BASS_ChannelStop(playHandle);
			playHandle = 0;
		}

		public void SetVolume( int volume )
		{
			float fVolume = volume/(float)255;
			Bass.BASS_ChannelSetAttribute(playHandle, BASSAttribute.BASS_ATTRIB_VOL, fVolume);

		}


		public int GetNowPlayPercent()
		{
			double ret = 0;

			if (playHandle == 0)
			{
				ret = -1;
			}
			else
			{
				var now = this.GetNowPosition();
				ret = (now * 100 / m_length);
			}
			return (int)ret;
		}

		public double GetNowPosition()
		{
			var pos = Bass.BASS_ChannelGetPosition(playHandle);
			var now = Bass.BASS_ChannelBytes2Seconds(playHandle, pos);
			return now;
		}
		//-----------------------------------------------------------------------------------------------
		//音の長さ取得
		//-----------------------------------------------------------------------------------------------
		public double GetLength()
		{
			return m_length;
		}
		public double GetLengthTmp(string fileName = "")
		{
			double ret = 0;
			if (fileName != "")
			{
				var checkHandle = GetHandle(fileName);
				ret = Bass.BASS_ChannelBytes2Seconds(checkHandle, Bass.BASS_ChannelGetLength(checkHandle));
				Bass.BASS_StreamFree(checkHandle);
			}
			return ret;
		}

		
	}
}
