using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Collections;

using System.Text.RegularExpressions;

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SEViewer
{
   

	public partial class Form1 : Form
	{
		private int					m_soundModeType = 0;
		private soundPlayer			m_soundPlayer	= null;
		
		private string				m_exePath		= "";
		
		private List<string>		m_showFileList  = new List<string>();
		private List<string>		m_fileList		= new List<string>();

		private bool m_receiveEventFlg = false;

		private ListViewItemComparer listViewItemSorter;

		private const int HOTKEY_COUNT	= 5;
		private HotKey[] hotKey				= new HotKey[HOTKEY_COUNT];

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindow(String sClassName, String sWindowText);
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindowEx(IntPtr hWnd, IntPtr hwndChildAfter, String lpszClass, String lpszWindow);
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern bool PostMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern Int32 SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

		//-----------------------------------------------------------------------------------------------
		//初期化
		//-----------------------------------------------------------------------------------------------
		public Form1()
		{
			InitializeComponent();

			//hotKey				=  new HotKey(MOD_KEY.ALT | MOD_KEY.CONTROL | MOD_KEY.SHIFT, Keys.F);
			hotKey[0] = new HotKey(MOD_KEY.CONTROL, Keys.D1);
			hotKey[1] = new HotKey(MOD_KEY.CONTROL, Keys.D2);
			hotKey[2] = new HotKey(MOD_KEY.CONTROL, Keys.D3);
			hotKey[3] = new HotKey(MOD_KEY.CONTROL, Keys.D4);
			hotKey[4] = new HotKey(MOD_KEY.CONTROL, Keys.D5);

			hotKey[0].HotKeyPush += new EventHandler(hotKey_HotKeyPush01);
			hotKey[1].HotKeyPush += new EventHandler(hotKey_HotKeyPush02);
			hotKey[2].HotKeyPush += new EventHandler(hotKey_HotKeyPush03);
			hotKey[3].HotKeyPush += new EventHandler(hotKey_HotKeyPush04);
			hotKey[4].HotKeyPush += new EventHandler(hotKey_HotKeyPush05);

			m_soundPlayer = new soundPlayer();


            System.Math.Max(10, System.Math.Min(5, 8));

			//----------------------------------------------------------------
			//System.IO.Directory.SetCurrentDirectory("E:/ito-develop/クルセイドハート/svn/Tool/SEViwer");
			m_exePath = System.IO.Directory.GetCurrentDirectory();//System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			m_exePath = MainFunction.Add_EndPathSeparator(m_exePath);

			

            checkBox2.Text = System.IO.Path.GetFileNameWithoutExtension(Program.m_data.txtPath[0]);
            checkBox3.Text = System.IO.Path.GetFileNameWithoutExtension(Program.m_data.txtPath[1]);
            checkBox4.Text = System.IO.Path.GetFileNameWithoutExtension(Program.m_data.txtPath[2]);
            checkBox6.Text = System.IO.Path.GetFileNameWithoutExtension(Program.m_data.txtPath[3]);
			checkBox8.Text = System.IO.Path.GetFileNameWithoutExtension(Program.m_data.txtPath[4]);

			//----------------------------------------------------------------

			string[]		   m_fileListTmpGet;//TODO:test

            for (int i = 0; i < DataSetManager.MAX_CATEGORY; i++)
			{

				//まずはフォルダ内のwavファイルを全て列挙
				m_fileListTmpGet = SEViewer.MainFunction.Get_PathFromDirectroy(Program.m_data.soundPath[i], "*.wav", true);

				foreach (string fileName in m_fileListTmpGet)
				{
					m_fileList.Add(fileName);
				}

				//wavファイル情報をリストに入れておく
				Program.m_data.AddDictionary(m_fileList, i);
				m_fileList.Clear();

				//まずはフォルダ内のoggファイルを全て列挙
				m_fileListTmpGet = SEViewer.MainFunction.Get_PathFromDirectroy(Program.m_data.soundPath[i], "*.ogg", true);

				foreach (string fileName in m_fileListTmpGet)
				{
					m_fileList.Add(fileName);
				}

				//oggをリストに入れてみる
				Program.m_data.AddDictionary(m_fileList,i);
				m_fileList.Clear();

			}
			//----------------------------------------------------------------


            UpdateGenreComboBox(0);
            UpdateGenreComboBox2(1);

            comboBox4.SelectedIndex = 0;
			NewCreateFileList();

			UpdateList("");

			comboBox2.SelectedIndex = 1;

			//----------------------------------------------------------------
			LoadSet();

			//ListViewItemComparerの作成と設定
			listViewItemSorter = new ListViewItemComparer();
			listViewItemSorter.ColumnModes =
			   new ListViewItemComparer.ComparerMode[]
			{
				ListViewItemComparer.ComparerMode.String,
				ListViewItemComparer.ComparerMode.String,
				ListViewItemComparer.ComparerMode.String
			};

			//ListViewItemSorterを指定する
			listView1.ListViewItemSorter = listViewItemSorter;

	//		comboBox3.DropDownHeight = listView1.Height - (groupBox4.Location.Y + comboBox3.Location.Y);

			//updateProcssList();
		}


        public void UpdateGenreComboBox(int id)
        {
            //ジャンルボックス初期化
            comboBox3.Items.Clear();
            comboBox3.Items.Add("ジャンル指定なし");


            foreach (string genre in Program.m_data.m_genreList[id])
                comboBox3.Items.Add(genre);

                

            comboBox3.SelectedIndex = 0;
        }

        public void UpdateGenreComboBox2(int id, bool isDrawin = false)
        {
            //ジャンルボックス初期化;

            comboBox5.Items.Clear();
            comboBox5.Items.Add("ジャンル指定なし");

            string drawinStr;
            string genre2Str;
            string checkStr = comboBox3.SelectedItem.ToString();

            if(checkStr == "ジャンル指定なし") isDrawin =false;

            foreach (string genre in Program.m_data.m_genreList2[id])
            {
                drawinStr = genre.Split('∴')[0].ToString();
                genre2Str = genre.Split('∴')[1].ToString();

                if( !isDrawin || drawinStr == checkStr)
                {
                    if(comboBox5.Items.IndexOf(genre2Str) == -1 )
                        comboBox5.Items.Add(genre2Str);
                }
            }

            
            comboBox5.SelectedIndex = 0;
        }
        //-----------------------------------------------------------------------------------------------
        //実オーディオファイルリストから絞り込む
        //-----------------------------------------------------------------------------------------------
        public void UpdateList(string regPattern)
		{
			m_showFileList.Clear();
            bool genreCheck = false;

			try
			{
				Regex regGeter = new Regex(regPattern, RegexOptions.IgnoreCase);

				foreach (string tmpFilePath in m_fileList)
				{
                    genreCheck = CheckGenreCrossFit(Program.m_data.GetGenre(tmpFilePath, m_soundModeType), Program.m_data.GetGenre2(tmpFilePath, m_soundModeType),comboBox3.SelectedItem.ToString(), comboBox5.SelectedItem.ToString());

                    if ( (regPattern == "" || 
						
						( comboBox4.SelectedIndex == 0 && regGeter.IsMatch(Program.m_data.GetSummary(tmpFilePath, m_soundModeType))  )||
						( comboBox4.SelectedIndex == 1 && regGeter.IsMatch(tmpFilePath)
                        ))
                        && genreCheck
                    )
					{
	
                         m_showFileList.Add(tmpFilePath);
					
					}
				}
			}
			catch
			{
			}
		}


        bool CheckGenreCrossFit( string check1, string check2, string selectGenre1, string selectGenre2)
        {
            //ジャンル1の絞込
            if (selectGenre1 == "ジャンル指定なし" && selectGenre2 == "ジャンル指定なし") return true;

            if(selectGenre1 == "ジャンル指定なし" && (check2 == selectGenre1 || check2 == selectGenre2)) return true;
            if(selectGenre2 == "ジャンル指定なし" && (check1 == selectGenre1 || check1 == selectGenre2)) return true;
            if(check1 == selectGenre1 && check2 == selectGenre2)return true;
            if(check2 == selectGenre1 && check1 == selectGenre2)return true;

            return false;

        }
		//-----------------------------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------------------------
		private void SendKey()
		{
			IntPtr hWnd;
			string sClassName = "Hidemaru32Class";
			string sWindowText = null;
			IntPtr wParam, lParam;
			bool bresult;
			// NotepadのWindowハンドル取得
			if ((hWnd = FindWindow(sClassName, sWindowText)) == IntPtr.Zero)
			{
				sClassName = "EmEditorMainFrame3";
				if ((hWnd = FindWindow(sClassName, sWindowText)) == IntPtr.Zero)
				{
					//MessageBox.Show("秀丸・またはエムエディターを起動してください");
					return;
				}
			}
			// NotepadのテキストエリアのWindowハンドル取得
			//	sClassName = "Edit";
			//hWnd = FindWindowEx(hWnd, IntPtr.Zero, sClassName, sWindowText);
			hWnd = FindWindowEx(hWnd, IntPtr.Zero, null, sWindowText);
			wParam = new IntPtr(0x41);
			lParam = IntPtr.Zero;
			// WM_KEYDOWNメッセージ送信
			// WM_PASTEメッセージ送信
			//bresult = PostMessage(hWnd, 0x100, wParam, lParam);

			if (checkBox1.Checked)
			{
				bresult = PostMessage(hWnd, 0x0302, lParam, lParam);
			}
			// WM_KEYUPメッセージを送信すると2回送られるのでコメントアウト
			// bresult = PostMessage(hWnd, 0x101, wParam, lParam);
		}

		//-----------------------------------------------------------------------------------------------
		//ロードしてデータセットと合わせてリストビューにセットする
		//-----------------------------------------------------------------------------------------------
		public void LoadSet()
		{

			if (m_receiveEventFlg == true) return;

			listView1.Items.Clear();
			listView1.ListViewItemSorter = null;

			foreach (string filePath in m_showFileList)
			{

				//if( Program.m_data.GetDataSet(m_soundModeType)[filePath].m_isExist == false ) continue;


				if (Program.m_data.GetSummary(filePath, m_soundModeType) == "")
					continue;


				System.Windows.Forms.ListViewItem tmpItem = listView1.Items.Add(filePath);

				
				
				if (Program.m_data.GetDataSet(m_soundModeType)[filePath].m_isExist == false)
				{
					tmpItem.ForeColor = Color.Red;
				}
				else
				{
					tmpItem.ForeColor = Color.Black;
				}
				tmpItem.SubItems.Add(Program.m_data.GetGenre(  filePath,m_soundModeType));
                tmpItem.SubItems.Add(Program.m_data.GetGenre2(filePath, m_soundModeType));
                tmpItem.SubItems.Add(Program.m_data.GetSummary(filePath,m_soundModeType));
				
			}

			textBox1.Text = Program.m_data.pattern1;
			textBox2.Text = Program.m_data.pattern2;
			textBox3.Text = Program.m_data.pattern3;
			textBox4.Text = Program.m_data.pattern4;
			textBox5.Text = Program.m_data.pattern5;

			listView1.ListViewItemSorter = listViewItemSorter;
		}

		//-----------------------------------------------------------------------------------------------
		//セーブする
		//-----------------------------------------------------------------------------------------------
		public void SaveSet()
		{
            string m_exePath = System.IO.Directory.GetCurrentDirectory();
			m_exePath = MainFunction.Add_EndPathSeparator(m_exePath);
			Program.m_data.Save(m_exePath );
			Program.m_data.SaveFavList();

		}

		//-----------------------------------------------------------------------------------------------
		//リストビューダブルクリック
		//-----------------------------------------------------------------------------------------------
		private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				PlaySelectSound();
			}
		}

		//-----------------------------------------------------------------------------------------------
		//リストビュークリックイベント
		//-----------------------------------------------------------------------------------------------
		private void listView1_MouseClick(object sender, MouseEventArgs e)
		{
			string copyFileName = System.IO.Path.GetFileNameWithoutExtension(listView1.SelectedItems[0].Text);

			this.Text = copyFileName;
		//	this.textBoxFileName.Text = listView1.SelectedItems[0].Text;
		//	this.textBoxSammary.Text = listView1.SelectedItems[0].SubItems[2].Text;
            

			//右クリックならばクリップボードにコピー
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
                //コントロール ctrl+クリックでファイル名のみ取得
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control || e.Button == MouseButtons.Middle)
                {
                    if (copyFileName != "") System.Windows.Forms.Clipboard.SetText(copyFileName);
                }
                else {
                    copyStringToClipboard(copyFileName);
                }
                
			}
		  
		}

		//-----------------------------------------------------------------------------------------------
		//コンボボックス入力
		//-----------------------------------------------------------------------------------------------
		private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				((ComboBox)sender).Items.Add( comboBox1.Text );

				UpdateList(comboBox1.Text);
				LoadSet();
			}
		}

		//-----------------------------------------------------------------------------------------------
		//リストビューセルチェンジ
		//-----------------------------------------------------------------------------------------------
		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count == 0) return;

			//タイトル更新
			string copyFileName = System.IO.Path.GetFileNameWithoutExtension(listView1.SelectedItems[0].Text);

			this.Text = copyFileName;
			
			if( checkBox5.Checked )
			{
				PlaySelectSound();
			}

//			this.textBoxFileName.Text = listView1.SelectedItems[0].Text;
	//		this.textBoxSammary.Text  = listView1.SelectedItems[0].SubItems[1].Text;
		}

		//-----------------------------------------------------------------------------------------------
		//コンボボックス文字変更
		//-----------------------------------------------------------------------------------------------
		private void comboBox1_TextChanged(object sender, EventArgs e)
		{
			UpdateList(comboBox1.Text);
			LoadSet();
		}

		//-----------------------------------------------------------------------------------------------
		//再生ボタン
		//-----------------------------------------------------------------------------------------------
		private void button1_Click(object sender, EventArgs e)
		{
			PlaySelectSound();
		}

		private void PlaySelectSound()
		{

			timer1.Stop();

			bool isLoop = checkBox9.Checked;

			//左ダブルクリックなので再生

			string fileFullPath = Program.m_data.soundPath[m_soundModeType] + listView1.SelectedItems[0].Text;

			//お気に入りリストの場合はフォルダを横断して存在確認する
			if (m_soundModeType == DataSetManager.MAX_CATEGORY-1)
			{
				if (System.IO.File.Exists(fileFullPath) == false)
				{
					for (int i = 0; i < DataSetManager.MAX_CATEGORY-1; i++)
					{
						fileFullPath = Program.m_data.soundPath[i] + listView1.SelectedItems[0].Text;
						if (System.IO.File.Exists(fileFullPath)) break;
						if (i == DataSetManager.MAX_CATEGORY - 2) return;
					}
				}
			} else {
				if (System.IO.File.Exists(fileFullPath) == false ) return;
			}


			if (m_soundPlayer.PlaySound(fileFullPath, trackBar1.Value, isLoop))
			{
				timer1.Start();
			}

		
		}

		private void StopSound()
		{
			m_soundPlayer.StopSound();
			timer1.Stop();
		}
		

		//-----------------------------------------------------------------------------------------------
		//停止ボタン
		//-----------------------------------------------------------------------------------------------
		private void button2_Click(object sender, EventArgs e)
		{
			StopSound();
		}

		//-----------------------------------------------------------------------------------------------
		//フォームを閉じるイベント
		//-----------------------------------------------------------------------------------------------
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
            Program.m_data.m_left       = this.Left;
            Program.m_data.m_top        = this.Top;
            Program.m_data.m_width      = this.Width;
            Program.m_data.m_height     = this.Height;

            Program.m_data.m_col1Size = listView1.Columns[0].Width;
            Program.m_data.m_col2Size = listView1.Columns[1].Width;
            Program.m_data.m_col3Size = listView1.Columns[2].Width;
            Program.m_data.m_col4Size = listView1.Columns[3].Width;

            for (int i = 0; i < HOTKEY_COUNT; i++)
			{
				hotKey[i].Dispose();
			}
			timer1.Stop();
			m_soundPlayer.StopSound();
			m_soundPlayer.ReleaseWIO();
			SaveSet();
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			
		}

		//-----------------------------------------------------------------------------------------------
		//コピー機能の文字列変更イベント
		//-----------------------------------------------------------------------------------------------
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			Program.m_data.pattern1 = textBox1.Text;
		}

		//-----------------------------------------------------------------------------------------------
		//コピー機能の文字列変更イベント
		//-----------------------------------------------------------------------------------------------
		private void textBox2_TextChanged(object sender, EventArgs e)
		{
			Program.m_data.pattern2 = textBox2.Text;
		}

		//-----------------------------------------------------------------------------------------------
		//コピー機能の文字列変更イベント
		//-----------------------------------------------------------------------------------------------
		private void textBox3_TextChanged(object sender, EventArgs e)
		{
			Program.m_data.pattern3 = textBox3.Text;
		}

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Program.m_data.pattern4 = textBox4.Text;
        }


        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            Program.m_data.pattern5 = textBox5.Text;
        }

        //-----------------------------------------------------------------------------------------------
        //フォームロードイベント
        //-----------------------------------------------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
		{
			// WaveOutの状態を気にしつつ、データを出力するためのタイマー
			timer1.Interval = 10;

            this.Left = Program.m_data.m_left;
            this.Top = Program.m_data.m_top;
            this.Width = Program.m_data.m_width;
            this.Height = Program.m_data.m_height;

            listView1.Columns[0].Width = Program.m_data.m_col1Size;
            listView1.Columns[1].Width = Program.m_data.m_col2Size;
            listView1.Columns[2].Width = Program.m_data.m_col3Size;
            listView1.Columns[3].Width = Program.m_data.m_col4Size;
        }

		//-----------------------------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------------------------
		public void NewCreateFileList()
		{
			m_fileList.Clear();

			//foreach (KeyValuePair<string, DataSet> tmpDat in Program.m_data.m_dataMaster)
			foreach (KeyValuePair<string, DataSet> tmpDat in Program.m_data.GetDataSet(m_soundModeType))
			{
				m_fileList.Add(tmpDat.Value.m_fileName);
				
			}
			m_fileList.Sort();
		}

		//-----------------------------------------------------------------------------------------------
		//ogg再生のストリーミング呼び出し
		//-----------------------------------------------------------------------------------------------
		private void Feed()
		{
			m_soundPlayer.Streaming();
		}

		//-----------------------------------------------------------------------------------------------
		//ogg再生のタイマーイベント
		//-----------------------------------------------------------------------------------------------
		private void timer1_Tick(object sender, EventArgs e)
		{
			bool isLoop = checkBox9.Checked;
			if( m_soundPlayer.Streaming( isLoop ) == false )
			{
				//ここで止めるとバッファに残った音データを無視して停止してしまう…
//				StopSound();
			}
		}

		//-----------------------------------------------------------------------------------------------
		//リストビュー
		//-----------------------------------------------------------------------------------------------
		private void listView1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				PlaySelectSound();
			}
			else if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.Space )
			{
				string copyFileName = System.IO.Path.GetFileNameWithoutExtension(listView1.SelectedItems[0].Text);

				this.Text = copyFileName;
		//		this.textBoxFileName.Text = listView1.SelectedItems[0].Text;
		//		this.textBoxSammary.Text = listView1.SelectedItems[0].SubItems[1].Text;

				copyStringToClipboard(copyFileName);
				
			}
            else if(e.KeyCode == Keys.F1 && m_soundModeType != 4 )
            {
				//リスト4にいま選択中の音を放り込む
				if (listView1.SelectedItems.Count != 0)
				{
					Program.m_data.AddDictionary(listView1.SelectedItems[0].Text, 4, listView1.SelectedItems[0].SubItems[3].Text, listView1.SelectedItems[0].SubItems[1].Text, listView1.SelectedItems[0].SubItems[2].Text);
				}
				else
				{
					System.Windows.Forms.MessageBox.Show("F1によるお気にいり追加機能を使用するには、いずれかの音を選んでからF1を押して下さい");
				}


			}
		}



		//-----------------------------------------------------------------------------------------------
		//コピーボタンクリックイベント
		//-----------------------------------------------------------------------------------------------
		private void button3_Click(object sender, EventArgs e)
		{

			if (listView1.SelectedIndices.Count == 0) return;

			string copyFileName = System.IO.Path.GetFileNameWithoutExtension(listView1.SelectedItems[0].Text);

			this.Text = copyFileName;
	//		this.textBoxFileName.Text = listView1.SelectedItems[0].Text;
		//	this.textBoxSammary.Text = listView1.SelectedItems[0].SubItems[1].Text;

			copyStringToClipboard(copyFileName);

			if (copyFileName != "") System.Windows.Forms.Clipboard.SetText(copyFileName);
		}


		//-----------------------------------------------------------------------------------------------
		//ジャンル設定のコンボボックス選択イベント
		//-----------------------------------------------------------------------------------------------
		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateList(comboBox1.Text);
            UpdateGenreComboBox2(m_soundModeType,true);
            LoadSet();
		}


		//-----------------------------------------------------------------------------------------------
		//リストビューのカラムクリックイベント
		//-----------------------------------------------------------------------------------------------
		private void listView1_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			listViewItemSorter.Column = e.Column;
			listView1.Sort();
		}
		
		//-----------------------------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------------------------
		private void toolTip1_Popup(object sender, PopupEventArgs e)
		{

		}

		//-----------------------------------------------------------------------------------------------
		//特定の文章をクリップボードにコピー
		//-----------------------------------------------------------------------------------------------
		private void copyStringToClipboard(string copyFileName, int a_mode = -1)
		{
			int mode = comboBox2.SelectedIndex;
			if (a_mode != -1) mode = a_mode;

			//高尾さん要望。設定を無視して特定の文字列(ここではファイル名)をコピーする場合の処理追加
			if( copyFileName.IndexOf('-') == 0 )
			{
				copyFileName = copyFileName.Substring(1);
			}
			else
			{
				switch (mode)
				{
					case 1: copyFileName = textBox1.Text.Replace("%s", copyFileName); break;
					case 2: copyFileName = textBox2.Text.Replace("%s", copyFileName); break;
					case 3: copyFileName = textBox3.Text.Replace("%s", copyFileName); break;
					case 4: copyFileName = textBox4.Text.Replace("%s", copyFileName); break;
					case 5: copyFileName = textBox5.Text.Replace("%s", copyFileName); break;
				}
			}
			

			copyFileName = copyFileName.Replace("%n", System.Environment.NewLine);

			copyFileName = copyFileName.Replace("%i", listView1.SelectedItems[0].SubItems[3].Text);

			if (listView1.SelectedItems.Count != 0)
			{
				copyFileName = copyFileName.Replace("%g", listView1.SelectedItems[0].SubItems[1].Text);
			}

			copyFileName = copyFileName.Replace("%t", "	");

			if (copyFileName != "") System.Windows.Forms.Clipboard.SetText(copyFileName);

			SendKey();
		}

		//-----------------------------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------------------------
		void hotKey_HotKeyPush01(object sender, EventArgs e)
		{
			//MessageBox.Show("ホットキーが押されました。");
			//copyStringToClipboard(copyFileName);
			string copyFileName = (listView1.SelectedItems.Count == 0 ? "" : System.IO.Path.GetFileNameWithoutExtension(listView1.SelectedItems[0].Text));
			copyStringToClipboard(copyFileName,1);		  
		}

		//-----------------------------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------------------------
		void hotKey_HotKeyPush02(object sender, EventArgs e)
		{
			//MessageBox.Show("ホットキーが押されました。");
			//copyStringToClipboard(copyFileName);
			string copyFileName = (listView1.SelectedItems.Count == 0 ? "" : System.IO.Path.GetFileNameWithoutExtension(listView1.SelectedItems[0].Text));
			copyStringToClipboard(copyFileName,2);
		}
		
		//-----------------------------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------------------------
		void hotKey_HotKeyPush03(object sender, EventArgs e)
		{
			//MessageBox.Show("ホットキーが押されました。");
			//copyStringToClipboard(copyFileName);
			string copyFileName = (listView1.SelectedItems.Count == 0 ? "" : System.IO.Path.GetFileNameWithoutExtension(listView1.SelectedItems[0].Text));
			copyStringToClipboard(copyFileName,3);
		}
		
		//-----------------------------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------------------------
		void hotKey_HotKeyPush04(object sender, EventArgs e)
		{
			//MessageBox.Show("ホットキーが押されました。");
			//copyStringToClipboard(copyFileName);
			string copyFileName = (listView1.SelectedItems.Count == 0 ? "" : System.IO.Path.GetFileNameWithoutExtension(listView1.SelectedItems[0].Text));
			copyStringToClipboard(copyFileName,4);
		}
		
		//-----------------------------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------------------------
		void hotKey_HotKeyPush05(object sender, EventArgs e)
		{
			//MessageBox.Show("ホットキーが押されました。");
			//copyStringToClipboard(copyFileName);
		
			string copyFileName = (listView1.SelectedItems.Count == 0 ? "" : System.IO.Path.GetFileNameWithoutExtension(listView1.SelectedItems[0].Text));
			copyStringToClipboard(copyFileName,5);
			
		}
		
		//-----------------------------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------------------------
		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox tmp = (System.Windows.Forms.CheckBox)sender;
			if (tmp.Checked == true)
			{
				m_receiveEventFlg = true;
				m_soundModeType = 0;

                UpdateGenreComboBox(0);
                UpdateGenreComboBox2(0);

                checkBox3.CheckState = CheckState.Unchecked;
				checkBox4.CheckState = CheckState.Unchecked;
                checkBox6.CheckState = CheckState.Unchecked;
				checkBox8.CheckState = CheckState.Unchecked;

				NewCreateFileList();
				UpdateList(comboBox1.Text);
				m_receiveEventFlg = false;
				LoadSet();
				
				if( checkBox7.Checked == true )
				{
					comboBox2.SelectedIndex = 1;
				}
			}
			
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{

			System.Windows.Forms.CheckBox tmp = (System.Windows.Forms.CheckBox)sender;
			if (tmp.Checked == true)
			{
				m_receiveEventFlg = true;
				UpdateGenreComboBox(1);
                UpdateGenreComboBox2(1);

                m_soundModeType = 1;
				checkBox8.CheckState = CheckState.Unchecked;
				checkBox2.CheckState = CheckState.Unchecked;
				checkBox4.CheckState = CheckState.Unchecked;
                checkBox6.CheckState = CheckState.Unchecked;

				NewCreateFileList();
				UpdateList(comboBox1.Text);
				m_receiveEventFlg = false;
				LoadSet();
				
				if( checkBox7.Checked == true )
				{
					comboBox2.SelectedIndex = 2;
				}
			}
		}

		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{
            

			System.Windows.Forms.CheckBox tmp = (System.Windows.Forms.CheckBox)sender;
			if (tmp.Checked == true)
			{
				m_receiveEventFlg = true;

				UpdateGenreComboBox(2);
                UpdateGenreComboBox2(2);

                m_soundModeType = 2;
				checkBox8.CheckState = CheckState.Unchecked;
				checkBox3.CheckState = CheckState.Unchecked;
				checkBox2.CheckState = CheckState.Unchecked;
                checkBox6.CheckState = CheckState.Unchecked;

				NewCreateFileList();
				UpdateList(comboBox1.Text);
				m_receiveEventFlg = false;
				LoadSet();
				
				if( checkBox7.Checked == true )
				{
					comboBox2.SelectedIndex = 3;
				}
			}
		}

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox tmp = (System.Windows.Forms.CheckBox)sender;
            if (tmp.Checked == true)
            {
				m_receiveEventFlg = true;
				UpdateGenreComboBox(3);
                UpdateGenreComboBox2(3);

                m_soundModeType = 3;
				checkBox8.CheckState = CheckState.Unchecked;
				checkBox4.CheckState = CheckState.Unchecked;
                checkBox3.CheckState = CheckState.Unchecked;
                checkBox2.CheckState = CheckState.Unchecked;

                NewCreateFileList();
                UpdateList(comboBox1.Text);
				m_receiveEventFlg = false;
				LoadSet();

                if (checkBox7.Checked == true)
                {
                    comboBox2.SelectedIndex = 4;
                }
            }
        }

		private void checkBox8_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox tmp = (System.Windows.Forms.CheckBox)sender;
			if (tmp.Checked == true)
			{
				m_receiveEventFlg = true;
				UpdateGenreComboBox(4);
				UpdateGenreComboBox2(4);

				m_soundModeType = 4;
				checkBox6.CheckState = CheckState.Unchecked;
				checkBox4.CheckState = CheckState.Unchecked;
				checkBox3.CheckState = CheckState.Unchecked;
				checkBox2.CheckState = CheckState.Unchecked;

				NewCreateFileList();
				UpdateList(comboBox1.Text);
				m_receiveEventFlg = false;

				LoadSet();

			}
		}



		private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateList(comboBox1.Text);
			LoadSet();
		}

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
             System.Windows.Forms.MessageBox.Show( "■コピー時に置き換えられる、特殊な文字の一覧です。\n\n" +
                                                    "% s　：　ファイル名に置き換わります。\n" +
                                                    "% n　：　改行に置き換わります。\n" +
                                                    "% t　：　タブに置き換わります。\n" +
                                                    "% i　：　SEの内容説明文に置き換わります。\n" +
                                                    "% g　：　SEのジャンル名に置き換わります。\n" +
                                                    "\n" +
                                                    "\n" +

                                                    "■便利な操作等について\n" +
                                                    "\n" +
                                                    "Ctrキー＋数字 1～5キー：コピー文の1から5番を直接指定して即、コピーします。\n"+
                                                    "\n" +
                                                    "\n"

                                                    , "コピー文のヘルプ");
        }

        private void checkBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                string exePath = System.IO.Directory.GetCurrentDirectory();//System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                exePath = MainFunction.Add_EndPathSeparator(exePath);
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(exePath + SEViewer.Program.m_data.txtPath[0]);
            }
        }

        private void checkBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                string exePath = System.IO.Directory.GetCurrentDirectory();//System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                exePath = MainFunction.Add_EndPathSeparator(exePath);
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(exePath + SEViewer.Program.m_data.txtPath[1]);
            }
        }

        private void checkBox4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                string exePath = System.IO.Directory.GetCurrentDirectory();//System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                exePath = MainFunction.Add_EndPathSeparator(exePath);
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(exePath + SEViewer.Program.m_data.txtPath[2]);
            }
        }

        private void checkBox6_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                string exePath = System.IO.Directory.GetCurrentDirectory();//System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                exePath = MainFunction.Add_EndPathSeparator(exePath);
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(exePath + SEViewer.Program.m_data.txtPath[3]);
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateList(comboBox1.Text);
			LoadSet();
        }

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			m_soundPlayer.SetOggVolume( trackBar1.Value );
		}












		/*
		private void sendKey()
		{
			// 選択しているプロセスをアクティブ
			int pid = int.Parse(listBox1.SelectedValue.ToString());
			Process p = Process.GetProcessById(pid);
			SetForegroundWindow(p.MainWindowHandle);
			// キーストロークを送信
			//SendKeys.Send(textBox1.Text);
			
			//Ctrl+V を送信
			SendKeys.Send("^v");
		}

		private void updateProcssList()
		{
			// プロセス一覧を更新
			listBox1.DataSource = ProcessTable();
			listBox1.ValueMember = "PID";
			listBox1.DisplayMember = "NAME";
		}
		private DataTable ProcessTable()
		{
			// プロセスのリストを取得
			// http://d.hatena.ne.jp/tomoemon/20080430/p2
			Process[] ps = Process.GetProcesses();
			Array.Sort(ps, new ProcComparator());

			DataTable table = new DataTable();
			table.Columns.Add("PID");
			table.Columns.Add("NAME");

			foreach (Process p in ps)
			{
				DataRow row = table.NewRow();
				row.SetField<int>("PID", p.Id);
				row.SetField<string>("NAME", p.ProcessName + " - " + p.MainWindowTitle);

				table.Rows.Add(row);
			}
			table.AcceptChanges();

			return table;
		}
		

		private void listBox1_DoubleClick(object sender, EventArgs e)
		{
			sendKey();
		}
		// .....
		 * */
	}














	// プロセス名でソート ... for Array.Sort
	public class ProcComparator : IComparer<Process>
	{
		public int Compare(Process p, Process q)
		{
			return p.ProcessName.CompareTo(q.ProcessName);
		}
	}

	//-----------------------------------------------------------------------------------------------
	/// <summary>
	/// ListViewの項目の並び替えに使用するクラス。カラムクリックソートするときにしか使わない。
	/// </summary>
	//-----------------------------------------------------------------------------------------------
	public class ListViewItemComparer : IComparer
	{
		/// <summary>
		/// 比較する方法
		/// </summary>
		public enum ComparerMode
		{
			String,
			Integer,
			DateTime
		};

		private int _column;
		private SortOrder _order;
		private ComparerMode _mode;
		private ComparerMode[] _columnModes;

		/// <summary>
		/// 並び替えるListView列の番号
		/// </summary>
		public int Column
		{
			set
			{
				if (_column == value)
				{
					if (_order == SortOrder.Ascending)
						_order = SortOrder.Descending;
					else if (_order == SortOrder.Descending)
						_order = SortOrder.Ascending;
				}
				_column = value;
			}
			get
			{
				return _column;
			}
		}
		/// <summary>
		/// 昇順か降順か
		/// </summary>
		public SortOrder Order
		{
			set
			{
				_order = value;
			}
			get
			{
				return _order;
			}
		}
		/// <summary>
		/// 並び替えの方法
		/// </summary>
		public ComparerMode Mode
		{
			set
			{
				_mode = value;
			}
			get
			{
				return _mode;
			}
		}
		/// <summary>
		/// 列ごとの並び替えの方法
		/// </summary>
		public ComparerMode[] ColumnModes
		{
			set
			{
				_columnModes = value;
			}
		}

		/// <summary>
		/// ListViewItemComparerクラスのコンストラクタ
		/// </summary>
		/// <param name="col">並び替える列番号</param>
		/// <param name="ord">昇順か降順か</param>
		/// <param name="cmod">並び替えの方法</param>
		public ListViewItemComparer( int col, SortOrder ord, ComparerMode cmod )
		{
			_column = col;
			_order = ord;
			_mode = cmod;
		}
		public ListViewItemComparer()
		{
			_column = 0;
			_order = SortOrder.Ascending;
			_mode = ComparerMode.String;
		}

		//xがyより小さいときはマイナスの数、大きいときはプラスの数、
		//同じときは0を返す
		public int Compare(object x, object y)
		{
			int result = 0;
			//ListViewItemの取得
			ListViewItem itemx = (ListViewItem)x;
			ListViewItem itemy = (ListViewItem)y;

			//並べ替えの方法を決定
			if (_columnModes != null && _columnModes.Length > _column)
				_mode = _columnModes[_column];

			//並び替えの方法別に、xとyを比較する
			switch (_mode)
			{
				case ComparerMode.String:
					result = string.Compare(itemx.SubItems[_column].Text,
						itemy.SubItems[_column].Text);
					break;
				case ComparerMode.Integer:
					result = int.Parse(itemx.SubItems[_column].Text) -
						int.Parse(itemy.SubItems[_column].Text);
					break;
				case ComparerMode.DateTime:
					result = DateTime.Compare(
						DateTime.Parse(itemx.SubItems[_column].Text),
						DateTime.Parse(itemy.SubItems[_column].Text));
					break;
			}

			//降順の時は結果を+-逆にする
			if (_order == SortOrder.Descending)
				result = -result;
			else if (_order == SortOrder.None)
				result = 0;

			//結果を返す
			return result;
		}




	}


	

}
