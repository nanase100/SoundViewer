using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Media;

using System.ComponentModel;

using Tsukikage.WinMM.WaveIO;
using Tsukikage.Audio;


namespace SoundViewer
{
    //-----------------------------------------------------------------------------------------------
    //
    //実際に音を鳴らすクラス。*.wavは.net自前の機能。*.oggは他の方の作られたdll任せ。"WaveIO.cs"が*.ogg機能。
    //
    //-----------------------------------------------------------------------------------------------
    class soundPlayer
    {
        bool isWave         = true;
        bool isLifeStream   = false;

        [DllImport("winmm.dll")]
		public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);


        /// OggStream
        OggDecodeStream waveStream = null;

        /// waveOut
        WaveOut waveOut            = null;

        private System.Media.SoundPlayer player = null;

        public void ReleaseWIO()
        {
            if (player != null)
            {
                player.Stop();
                player.Dispose();
                player = null;
            }


            if (waveOut != null)
            {
                waveOut.Close();
            }
            

            if (waveStream != null)
            {
                waveStream.Close();
                waveStream.Dispose();
            }

            
        }

        //-----------------------------------------------------------------------------------------------
        //ファイルを再生する
        //-----------------------------------------------------------------------------------------------
        public bool PlaySound(string fileName, int vol = 255, bool isLoop = false )
        {
			//再生されているときは止める
			if (player != null)
				StopSound();

			if (isLifeStream == true)
			{
				waveOut.Stop();
                waveOut.Close();
				waveStream.Seek(0, SeekOrigin.Begin);
				waveStream.Close();
				waveStream.Dispose();
				isLifeStream = false;
				
				//伊藤：連続で再生しているとオーディオデバイスを開きそこねるエラーが一部で起きている対策。【無理やり】
				System.Threading.Thread.Sleep(32);

			}//再生されているときは止める
			if (player != null) StopSound();
                


            if (Path.GetExtension(fileName) == ".wav")
            {
                //読み込む
                player = new System.Media.SoundPlayer(fileName);
                
                //非同期再生する
                if( isLoop == false )   player.Play();
				else					player.PlayLooping();

                isWave = true;
                return false;


				//次のようにすると、ループ再生される
				//player.PlayLooping();

				//次のようにすると、最後まで再生し終えるまで待機する
				//player.PlaySync();
            }
            else
            {
				
				//伊藤：連続で再生しているとオーディオデバイスを開きそこねるエラーが一部で起きている対策。【無理やり】
				System.Threading.Thread.Sleep(16);

                // 自動ループ機能を使ってOggVorbisデコーダを初期化する。引数の単位は頭からのサンプル数。
                // Oggファイルをそのまま聴くとフェードアウトになってるんですが、ゲーム中は切れ目なくループ再生したいわけで。
                // ループの先頭と末尾の位置は、Wave編集ソフトとかで開いて波形を見て決める。
                // ループしたい場所のなるべく近くで、左右チャンネルとも音量がほぼ0になっているタイミングを見つけて指定。
                // ループ位置を繋いだときに波形がジャンプしているとクリックノイズがのってしまう。
                // 100サンプル程度前後しても、その程度の音飛び・リズム狂いに気づける人はほとんどいないはずなので、
                // つなぎ目の音量優先で位置を決めましょう。
                waveStream = new OggDecodeStream(File.OpenRead(fileName));

                // WaveOut を初期化する。
                waveOut = new WaveOut(WaveOut.WaveMapper, waveStream.SamplesPerSecond, waveStream.BitsPerSample, waveStream.Channels);
                SetOggVolume( vol );
                isWave       = false;
                isLifeStream = true;
                return true;
            }
        }

        //-----------------------------------------------------------------------------------------------
        //再生されている音を止める
        //-----------------------------------------------------------------------------------------------
        public void StopSound()
        {
            if (isWave)
            {
                if( player != null )
                    player.Stop();
            }else{

				try
				{
					waveOut.Stop();
					waveStream.Seek(0, SeekOrigin.Begin);
					waveStream.Close();
					isLifeStream = false;

					waveOut.Close();
					waveOut = null;
				}
				//waveOut.Stop();で、ファイルを開いていな状態で閉じようとすると、例外を投げてくるので華麗にスルー。
				catch(Exception ex)
				{

				}
            }
        }

		public bool SetOggVolume( int volume )
		{
			if( waveOut == null ) return false;

			uint totalVol = (uint)((volume<<8)+(volume<<24));
			waveOutSetVolume( waveOut.Handle, totalVol );

			return true;
		}
        //-----------------------------------------------------------------------------------------------
        //データをストリーミングして読み込む
        //-----------------------------------------------------------------------------------------------
        public bool Streaming( bool isLoop = false )
        {
            if (isLifeStream == false) return false;

            // 再生位置を監視するにはタイマーやスレッドを使う。
            // 44100Hz 2ch 16bps => 176400Bytes/s なので、
            // 60fps ゲーム の場合、11025Bytes/fである。
            // それよりは大きくしないと、音が途切れる。

			int retByte = -1;
            //const int BLOCK_SIZE = 16384;
            const int BLOCK_SIZE = 32000;
            const int BUFFER_SIZE = BLOCK_SIZE * 4;
            while (waveOut.EnqueuedBufferSize < BUFFER_SIZE)
            {
                byte[] buffer = new byte[BLOCK_SIZE];
                retByte = waveStream.Read(buffer, 0, BLOCK_SIZE);

				//if( retByte < BLOCK_SIZE ) 
				if( retByte == 0 ) 
				{
					if( isLoop == false )
					{
						return false;
					}else{
						waveStream.Seek(0, SeekOrigin.Begin);
					}
				}
				
                waveOut.Write(buffer);
				return true;
            }

			return true;
        }

		//-----------------------------------------------------------------------------------------------
		//音の長さ取得
		//-----------------------------------------------------------------------------------------------
		public int GetSELength(string path)
		{

			int ret = 0;



			return ret;

		}
    }
}
