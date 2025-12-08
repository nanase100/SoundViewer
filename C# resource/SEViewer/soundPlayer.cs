using SEViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Un4seen.Bass;
using Un4seen.Bass.Misc;

namespace SEViewer
{
	//-----------------------------------------------------------------------------------------------
	//
	//実際に音を鳴らすクラス。*.wavは.net自前の機能。*.oggは他の方の作られたdll任せ。"WaveIO.cs"が*.ogg機能。
	//
	//-----------------------------------------------------------------------------------------------
	class soundPlayer : IDisposable
	{
		private double m_length = 0;
		private int playHandle;
		BASS_CHANNELINFO acOgginfo = null;

		public Graphics m_graphics = null; // 波形データを描画するためのGraphicsオブジェクト

		public int[] waveData = new int[1]; // 波形データを格納する配列

		static private Form1 m_parent = null;

		public soundPlayer(Form1 parent)
		{
			m_parent = parent;

			if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
			{
				return;
			}



			//Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_STREAM_CACHE, 10000); // バイト単位で設定
			// ストリーミングバッファサイズを設定（ミリ秒単位）
			///bool ret2 = Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_BUFFER, 5000); // 5秒分のデータをキャッシュ

		}

		public void Dispose()
		{
			Bass.BASS_Stop();
			Bass.BASS_PluginFree(0);
			Bass.BASS_Free();
		}





		//-----------------------------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------------------------
		public bool PlaySound(string fileName, int vol = 255, bool isLoop = false, double second = 0)
		{
			if (second == 0)
			{
				StopSound();

				//if( playHandle == 0 ) 
				playHandle = LoadFile(fileName);
			}

			Bass.BASS_ChannelPlay(playHandle, false);
			Bass.BASS_ChannelSetPosition(playHandle, second);
			acOgginfo = Bass.BASS_ChannelGetInfo(playHandle);
			//info.freq//サンプリングレート
			return true;
		}




		private void GetData(string filePath)
		{

			// OGGファイルを開く
			int stream = Bass.BASS_StreamCreateFile(filePath, 0, 0, BASSFlag.BASS_STREAM_DECODE);

			// 総サンプル数を取得
			long lengthBytes = Bass.BASS_ChannelGetLength(stream);
			int sampleCount = (int)(lengthBytes / 2); // 16bit PCMの場合

			// バッファを用意

			waveData = new int[sampleCount];

			// PCMデータを取得
			//GCHandle handle = GCHandle.Alloc(waveData, GCHandleType.Pinned);
			//int bytesRead = Bass.BASS_ChannelGetData(stream, handle.AddrOfPinnedObject(), (int)lengthBytes);
			//handle.Free();
			int bytesRead = Bass.BASS_ChannelGetData(stream, waveData, (int)lengthBytes);


		}









		protected int LoadFile(string filepath)
		{
			if (playHandle != 0) StopSound();

			playHandle = Bass.BASS_StreamCreateFile(filepath, 0, 0, BASSFlag.BASS_DEFAULT);

			if (playHandle == 0) throw new ArgumentException("cannot create a stream.");

			GetData(filepath);

			m_length = Bass.BASS_ChannelBytes2Seconds(playHandle, Bass.BASS_ChannelGetLength(playHandle));

			int syncHandle = Bass.BASS_ChannelSetSync(playHandle, BASSSync.BASS_SYNC_END, 0, proc, IntPtr.Zero);
			if (syncHandle == 0) throw new InvalidOperationException("cannot set sync");

			return playHandle;
		}


		//-----------------------------------------------------------------------------------------------
		//再生されている音を止める
		//-----------------------------------------------------------------------------------------------
		public void StopSound()
		{
			Bass.BASS_ChannelStop(playHandle);
		}

		public void PauseSound()
		{
			//MessageBox.Show(Bass.BASS_ChannelGetPosition(playHandle).ToString());
			Bass.BASS_ChannelPause(playHandle);
		}

		public void SetVolume(int volume)
		{
			float setVol = (float)volume / 100;

			Bass.BASS_SetVolume(setVol);
		}

		public int GetVolume()
		{
			float setVol = Bass.BASS_GetVolume();

			return (int)(setVol * 100);
		}

		public int GetNowPlayPercent(double now = -1, int scale = 1000)
		{
			double ret = 0;

			if (playHandle == 0)
			{
				ret = -1;
			}
			else
			{
				if (now == -1) now = this.GetNowPosition();
				ret = (now * scale / m_length);
			}
			return (int)ret;
		}

		//playSpeedは1.5f で1.5倍速
		public bool SetSpeed(float playSpeed)
		{
			if (playHandle == 0) return false;
			//bool a = Bass.BASS_ChannelSetAttribute(playHandle,BASSAttribute.BASS_ATTRIB_TEMPO_PITCH, 60 );
			//if( a == false )
			//{
			//	var b = Bass.BASS_ErrorGetCode();
			//}

			//return Bass.BASS_ChannelSetAttribute(playHandle,BASSAttribute.BASS_ATTRIB_FREQ, 44100 * playSpeed );
			return Bass.BASS_ChannelSetAttribute(playHandle, BASSAttribute.BASS_ATTRIB_FREQ, acOgginfo.freq * playSpeed);


		}

		public double GetNowPosition()
		{
			var pos = Bass.BASS_ChannelGetPosition(playHandle);
			var now = Bass.BASS_ChannelBytes2Seconds(playHandle, pos);
			return now;
			;
		}

		public SYNCPROC proc = new SYNCPROC((h, channel, data, user) =>
		{
			
		});

		public void SetNowPosition(double seconds)
		{
			Bass.BASS_ChannelSetPosition(playHandle, seconds);
		}

		//-----------------------------------------------------------------------------------------------
		//音の長さ取得
		//-----------------------------------------------------------------------------------------------
		public double GetLengthSecond()
		{
			return m_length;
		}

		public double GetLengthSecondTmp(string fileName = "")
		{
			double ret = 0;
			if (fileName != "")
			{
				//var checkHandle = GetHandle(fileName);
				var checkHandle = Bass.BASS_StreamCreateFile(fileName, 0, 0, BASSFlag.BASS_DEFAULT);
				if (checkHandle == 0) throw new ArgumentException("cannot create a stream.");

				ret = Bass.BASS_ChannelBytes2Seconds(checkHandle, Bass.BASS_ChannelGetLength(checkHandle));
				Bass.BASS_StreamFree(checkHandle);
			}
			return ret;
		}
	}
}
