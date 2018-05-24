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
		private string				m_searchStr		= "";
		private List<string>		m_showFileList  = new List<string>();
		private List<string>		m_fileList		= new List<string>();

		private List<string>		m_favGenreList = new List<string>();

		private List<int>			m_selectGenreState1	= new List<int>();
		private List<int>			m_selectGenreState2	= new List<int>();
		
		private				ToolTip ToolTip1;

		private bool m_receiveEventFlg = false;

		private ListViewItemComparer listViewItemSorter;
		

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
			
			 //ToolTipを作成する
			ToolTip1 = new ToolTip(this.components);
			//フォームにcomponentsがない場合
			//ToolTip1 = new ToolTip();
			
			//ToolTipが表示されるまでの時間
			ToolTip1.InitialDelay = 500;

			//ToolTipが表示されている時に、別のToolTipを表示するまでの時間
			ToolTip1.ReshowDelay = 1000;

			//ToolTipを表示する時間
			ToolTip1.AutoPopDelay = 2000;
			//フォームがアクティブでない時でもToolTipを表示する
			ToolTip1.ShowAlways = true;

			m_soundPlayer = new soundPlayer();

            System.Math.Max(10, System.Math.Min(5, 8));

			for( int i = 0; i < DataSetManager.MAX_CATEGORY; i++ ){
				m_selectGenreState1.Add(0);
				m_selectGenreState2.Add(0);
			}
			//----------------------------------------------------------------
			m_exePath = System.IO.Directory.GetCurrentDirectory();//System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			m_exePath = MainFunction.Add_EndPathSeparator(m_exePath);

            tabCategorySE.Text = System.IO.Path.GetFileNameWithoutExtension(Program.m_data.txtPath[0]);
            tabCategoryBGM.Text = System.IO.Path.GetFileNameWithoutExtension(Program.m_data.txtPath[1]);
            tabCategoryBGV.Text = System.IO.Path.GetFileNameWithoutExtension(Program.m_data.txtPath[2]);
            tabCategoryUSEFULL.Text = System.IO.Path.GetFileNameWithoutExtension(Program.m_data.txtPath[3]);
			tabCategoryFAV.Text = System.IO.Path.GetFileNameWithoutExtension(Program.m_data.txtPath[4]);

			//----------------------------------------------------------------

			string[]		   m_fileListTmpGet;

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
            UpdateGenreComboBox2(0);

            comboBox4.SelectedIndex = 0;
			NewCreateFileList();

			UpdateList("");

			copyStrSelect.SelectedIndex = 1;

			//----------------------------------------------------------------
			SetListViewItem();

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
			ChangeSoundTab(0);
			SetTreeViewItem(0);
			GetFavGenre();
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

            if( m_soundModeType != 4 )
			{
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
			}else{
				foreach( var tmpStr in m_favGenreList )
				{
					if( tmpStr != null )comboBox5.Items.Add( tmpStr );
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

			if (menuItemCheck1.Checked)
			{
				bresult = PostMessage(hWnd, 0x0302, lParam, lParam);
			}
			// WM_KEYUPメッセージを送信すると2回送られるのでコメントアウト
			// bresult = PostMessage(hWnd, 0x101, wParam, lParam);
		}

		//-----------------------------------------------------------------------------------------------
		//ロードしてデータセットと合わせてリストビューにセットする
		//-----------------------------------------------------------------------------------------------
		public void SetListViewItem()
		{
			if (m_receiveEventFlg == true) return;

			listView1.BeginUpdate();
			listView1.Items.Clear();
			listView1.ListViewItemSorter = null;

			foreach (string filePath in m_showFileList)
			{
				//if( Program.m_data.GetDataSet(m_soundModeType)[filePath].m_isExist == false ) continue;

				if (Program.m_data.GetSummary(filePath, m_soundModeType) == "")
					continue;

				System.Windows.Forms.ListViewItem tmpItem = listView1.Items.Add(filePath);
			
				if (Program.m_data.GetDataSet(m_soundModeType)[filePath].m_isExist == false && m_soundModeType != 4 )
				{
					tmpItem.ForeColor = Color.Red;
				}else{
					tmpItem.ForeColor = Color.Black;
				}
				tmpItem.SubItems.Add(Program.m_data.GetGenre(  filePath,m_soundModeType));
                tmpItem.SubItems.Add(Program.m_data.GetGenre2(filePath, m_soundModeType));
                tmpItem.SubItems.Add(Program.m_data.GetSummary(filePath,m_soundModeType));
				
			}
			int id = copyStrSelect.SelectedIndex-1;
			m_receiveEventFlg = true;
			if( id >= 0 ){
				textCopyStr.Text = Program.m_data.copyStr[id];
			}else{
				textCopyStr.Text = "";
			}
			m_receiveEventFlg = false;
			listView1.ListViewItemSorter = listViewItemSorter;
			listView1.EndUpdate();
		}

		//-----------------------------------------------------------------------------------------------
		//
		//
		public void SetTreeViewItem( int soundCategory )
		{
			//int loopCount = Program.m_data.m_genreList.Count();
			//for( int i = 0; i < ; i++ )
			//{
			//	treeView1.Nodes.Add( Program.m_data.m_genreList[i].ToString());
			//}
			
			treeView1.BeginUpdate();
			treeView1.Nodes.Clear();

			TreeNode topNode = treeView1.Nodes.Add( "全て表示");
			TreeNode tmpNode;
			int i = 0;

			foreach (string genre in Program.m_data.m_genreList[soundCategory]){
                tmpNode = topNode.Nodes.Add(genre);

				if( Program.m_data.m_genreColorList[soundCategory][i] != Color.Black ){
					tmpNode.ForeColor = Program.m_data.m_genreColorList[soundCategory][i];
					//tmpNode.ForeColor = Color.Black;
				}else{
					tmpNode.BackColor = Color.White;
					tmpNode.ForeColor = Color.Black;
				}
				i++;
			}

			topNode.Expand();
			treeView1.SelectedNode = topNode;
			treeView1.EndUpdate();
		}

		//-----------------------------------------------------------------------------------------------
		//セーブする
		//-----------------------------------------------------------------------------------------------
		public void SaveSet()
		{
            string m_exePath = System.IO.Directory.GetCurrentDirectory();
			m_exePath = MainFunction.Add_EndPathSeparator(m_exePath);
			Program.m_data.settingSave(m_exePath + "option.txt" );
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
                }else{
                    copyStringToClipboard(copyFileName);
                }
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
			
			if( menuItemCheck4.Checked )
			{
				PlaySelectSound();
			}
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

			Program.m_data.m_toolOption[0] = (menuItemCheck1.Checked?1:0);
			Program.m_data.m_toolOption[1] = (menuItemCheck2.Checked?1:0);
			Program.m_data.m_toolOption[2] = (menuItemCheck3.Checked?1:0);
			Program.m_data.m_toolOption[3] = (menuItemCheck4.Checked?1:0);
			Program.m_data.m_toolOption[4] = (menuItemCheck5.Checked?1:0);


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
			if( m_receiveEventFlg ) return;

			int id = copyStrSelect.SelectedIndex-1;
			Program.m_data.copyStr[id] = textCopyStr.Text;
		}
		
        //-----------------------------------------------------------------------------------------------
        //フォームロードイベント
        //-----------------------------------------------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
		{
			// WaveOutの状態を気にしつつ、データを出力するためのタイマー
			timer1.Interval = 10;

            this.Left	= Program.m_data.m_left;
            this.Top	= Program.m_data.m_top;
            this.Width	= Program.m_data.m_width;
            this.Height = Program.m_data.m_height;

            listView1.Columns[0].Width = Program.m_data.m_col1Size;
            listView1.Columns[1].Width = Program.m_data.m_col2Size;
            listView1.Columns[2].Width = Program.m_data.m_col3Size;
            listView1.Columns[3].Width = Program.m_data.m_col4Size;
			
			splitContainer1.SplitterDistance = Program.m_data.m_splitSize;

			menuItemCheck1.Checked =	(Program.m_data.m_toolOption[0] == 1 ? true : false );
			menuItemCheck2.Checked =	(Program.m_data.m_toolOption[1] == 1 ? true : false );
			menuItemCheck3.Checked =	(Program.m_data.m_toolOption[2] == 1 ? true : false );
			menuItemCheck4.Checked =	(Program.m_data.m_toolOption[3] == 1 ? true : false );
			menuItemCheck5.Checked =	(Program.m_data.m_toolOption[4] == 1 ? true : false );
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

            if(e.KeyCode == Keys.F3 && m_soundModeType != 4 )
            {
				//リスト4にいま選択中の音を放り込む
				if (listView1.SelectedItems.Count != 0)
				{
					Program.m_data.AddDictionary(listView1.SelectedItems[0].Text, 4, listView1.SelectedItems[0].SubItems[3].Text, listView1.SelectedItems[0].SubItems[1].Text, listView1.SelectedItems[0].SubItems[2].Text);
					GetFavGenre();
				}else{
					System.Windows.Forms.MessageBox.Show("音をお気に入りリストへ追加するには、いずれかの音を選んでからF1を押して下さい");
				}
			}

			 if(e.KeyCode == Keys.F4 && m_soundModeType == 4 )
            {
				//リスト4にいま選択中の音を放り込む
				if (listView1.SelectedItems.Count != 0)
				{
					Program.m_data.DelDictionary(listView1.SelectedItems[0].Text, 4);
					GetFavGenre();
					SetListViewItem();
				}else{
					System.Windows.Forms.MessageBox.Show("削除するお気に入り音を選んでください");
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
			UpdateList(m_searchStr);
            UpdateGenreComboBox2(m_soundModeType,true);
            SetListViewItem();
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
			int mode = copyStrSelect.SelectedIndex;
			if (a_mode != -1) mode = a_mode;

			string changeStr = "";

			if( mode == 0 ) changeStr = "%s";
			else			changeStr = Program.m_data.copyStr[mode-1];

			copyFileName = changeStr.Replace("%s", copyFileName); 
			
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
		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox tmp = (System.Windows.Forms.CheckBox)sender;
			if (tmp.Checked == true)
			{
				ChangeSoundTab( 0 );
			}
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox tmp = (System.Windows.Forms.CheckBox)sender;
			if (tmp.Checked == true) ChangeSoundTab( 1 );
		}

		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox tmp = (System.Windows.Forms.CheckBox)sender;
			if (tmp.Checked == true) ChangeSoundTab( 2 );
		}

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox tmp = (System.Windows.Forms.CheckBox)sender;
            if (tmp.Checked == true) ChangeSoundTab( 3 );
        }

		private void checkBox8_CheckedChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.CheckBox tmp = (System.Windows.Forms.CheckBox)sender;
			if (tmp.Checked == true) ChangeSoundTab( 4 );

		}

		private void ChangeSoundTab( int soundCategory )
		{

			m_selectGenreState1[m_soundModeType] = comboBox3.SelectedIndex;
			m_selectGenreState2[m_soundModeType] = comboBox5.SelectedIndex;

			m_receiveEventFlg	= true;
			m_soundModeType		= soundCategory;

            UpdateGenreComboBox(soundCategory);
            UpdateGenreComboBox2(soundCategory);

			tabCategorySE.CheckState		= ( soundCategory == 0 ? CheckState.Checked : CheckState.Unchecked );
            tabCategoryBGM.CheckState		= ( soundCategory == 1 ? CheckState.Checked : CheckState.Unchecked );
			tabCategoryBGV.CheckState		= ( soundCategory == 2 ? CheckState.Checked : CheckState.Unchecked );
            tabCategoryUSEFULL.CheckState	= ( soundCategory == 3 ? CheckState.Checked : CheckState.Unchecked );
			tabCategoryFAV.CheckState		= ( soundCategory == 4 ? CheckState.Checked : CheckState.Unchecked );

			NewCreateFileList();
			UpdateList(m_searchStr);
			m_receiveEventFlg = false;
			SetListViewItem();
				
			if( menuItemCheck3.Checked == true )
			{
				copyStrSelect.SelectedIndex = soundCategory + 1;
			}

			
			SetTreeViewItem(soundCategory);
			
			if( menuItemCheck5.Checked  )
			{
				comboBox3.SelectedIndex = m_selectGenreState1[soundCategory];
				comboBox5.SelectedIndex = m_selectGenreState2[soundCategory];
			}


			//Button1とButton2にToolTipが表示されるようにする
			ToolTip1.SetToolTip(button3, Program.m_data.commonCopyStr[m_soundModeType] );
		}


		private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateList(m_searchStr);
			SetListViewItem();
		}

        private void label2_Click(object sender, EventArgs e)
        {

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
            UpdateList(m_searchStr);
			SetListViewItem();
        }

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			m_soundPlayer.SetOggVolume( trackBar1.Value );
		}




		private void menuItemCombo1_TextChanged(object sender, EventArgs e)
		{
			m_searchStr = menuItemCombo1.Text;
			UpdateList(m_searchStr);
			SetListViewItem();
		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			m_receiveEventFlg = true;
			int id = copyStrSelect.SelectedIndex-1;
			if( id == -1 ){
				textCopyStr.Text = "";
				textCopyStr.Enabled = false;
			}else{
				textCopyStr.Text = Program.m_data.copyStr[id];
				textCopyStr.Enabled = true;
			}
			m_receiveEventFlg = false;
		}

		

		private void menuItemCheck1_Click(object sender, EventArgs e)
		{
			menuItemCheck1.Checked = !menuItemCheck1.Checked;
		}

		private void menuItemCheck2_Click(object sender, EventArgs e)
		{
			menuItemCheck2.Checked = !menuItemCheck2.Checked;
		}

		private void menuItemCheck3_Click(object sender, EventArgs e)
		{
			menuItemCheck3.Checked = !menuItemCheck3.Checked;
		}

		private void menuItemCheck4_Click(object sender, EventArgs e)
		{
			menuItemCheck4.Checked = !menuItemCheck4.Checked;
		}
		
		private void menuItemCheck5_Click(object sender, EventArgs e)
		{
			menuItemCheck5.Checked = !menuItemCheck5.Checked;
		}


		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if( e.Node.Text == "全て表示" ) comboBox3.SelectedIndex = 0;
			else							comboBox3.SelectedIndex = e.Node.Index+1;
			
			//UpdateList( m_searchStr );
            //SetListViewItem();

		}

		private void GetFavGenre()
		{
			m_favGenreList.Clear();
			

			foreach (KeyValuePair<string, DataSet> tmpDat in Program.m_data.GetDataSet(4))
			{
				string genre = tmpDat.Value.m_genre2;
			//	drawinStr = genre.Split('∴')[0].ToString();
			//	genre2Str = genre.Split('∴')[1].ToString();
			
				if( genre != "" && m_favGenreList.IndexOf(genre) == -1 )
					m_favGenreList.Add(genre);
				
			}
		}

		private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
		{
			Program.m_data.m_splitSize = splitContainer1.SplitterDistance;

			if( this.Width > 930 ){
				tabCategorySE.Left = Program.m_data.m_splitSize + 16;
				tabCategoryBGM.Left		= tabCategorySE.Left + 60;
				tabCategoryBGV.Left		= tabCategorySE.Left + 120;
				tabCategoryUSEFULL.Left = tabCategorySE.Left + 180;
				tabCategoryFAV.Left		= tabCategorySE.Left + 240;
			}else{
				int leftOffset = 930 - this.Width;
				tabCategorySE.Left = Program.m_data.m_splitSize + 16 - leftOffset;
				tabCategoryBGM.Left		= tabCategorySE.Left + 60;
				tabCategoryBGV.Left		= tabCategorySE.Left + 120;
				tabCategoryUSEFULL.Left = tabCategorySE.Left + 180;
				tabCategoryFAV.Left		= tabCategorySE.Left + 240;

			}
		}

		private void splitContainer1_Panel2_SizeChanged(object sender, EventArgs e)
		{
			int harfWidth = splitContainer1.Panel2.Width / 2 - 6;
			comboBox3.Width = harfWidth;
			comboBox5.Left = harfWidth + 10;
			comboBox5.Width = harfWidth;
		}

		private void splitContainer1_Panel1_SizeChanged(object sender, EventArgs e)
		{
			int stopBtnWidth = groupBox1.Width * 7 / 10 - 6;
			int playBtnWidth = groupBox1.Width - stopBtnWidth  - 20;
			button1.Width = playBtnWidth;
			button2.Left = playBtnWidth + 10;
			button2.Width = stopBtnWidth;
		}

		private void menuItemCombo1_KeyDown(object sender, KeyEventArgs e)
		{
			if( e.KeyCode == Keys.Enter ){
				menuItemCombo1.Items.Add(menuItemCombo1.Text);
			}
		}

		private void button3_Click_1(object sender, EventArgs e)
		{
			CopyCommonCopyStr( m_soundModeType );
		}

		private void CopyCommonCopyStr( int id )
		{
			System.Windows.Forms.Clipboard.SetText( Program.m_data.commonCopyStr[id] );
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
