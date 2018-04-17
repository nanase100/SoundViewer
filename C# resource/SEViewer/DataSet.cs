using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;


using System.Text.RegularExpressions;

namespace SEViewer
{
    class DataSet
    {

        public string m_fileName    { get; set; }
        public string m_summary     { get; set; }
        public bool m_isExist       { get; set; }
        public string m_genre1      { get; set; }
        public string m_genre2      { get; set; }

    };


    //-----------------------------------------------------------------------------------------------
    //
    //se.txtのSEの情報などをまとめて持っておくクラス   
    //
    //-----------------------------------------------------------------------------------------------
    class DataSetManager
    {

        public const int MAX_CATEGORY = 5;

		public string[] soundPath	{ get; set; }

		public string[] txtPath { get; set; }

        public string pattern1 { get; set; }
        public string pattern2 { get; set; }
        public string pattern3 { get; set; }
		public string pattern4 { get; set; }
		public string pattern5 { get; set; }


        public int m_left { set; get; }
        public int m_top { set; get; }
        public int m_width { set; get; }
        public int m_height { set; get; }
        public int m_col1Size { set; get; }
        public int m_col2Size { set; get; }
        public int m_col3Size { set; get; }
        public int m_col4Size { set; get; }



        //メインのデータ
        private Dictionary<string, DataSet>[] m_dataMaster;

        //絞込の後に画面に表示される m_dataMasterから抜き出した参照リスト
        //private List<DataSet> m_dataRefList = new List<DataSet>();

        public HashSet<string>[] m_genreList { get; set; }
        public HashSet<string>[] m_genreList2 { get; set; }




        public Dictionary<string, DataSet>  GetDataSet(int type)
		{
			return m_dataMaster[type];
		}

        //-----------------------------------------------------------------------------------------------
        //コンストラクタ
        //-----------------------------------------------------------------------------------------------
        public DataSetManager()
        {
            m_dataMaster    = new Dictionary<string, DataSet>[MAX_CATEGORY];
            m_genreList     = new HashSet<string>[MAX_CATEGORY];
            m_genreList2    = new HashSet<string>[MAX_CATEGORY];
            for (int i = 0; i < MAX_CATEGORY; i++)
			{
				m_dataMaster[i] = new Dictionary<string, DataSet>();
                m_genreList[i]  = new HashSet<string>();
                m_genreList2[i] = new HashSet<string>();
            }

            soundPath   = new string[MAX_CATEGORY];
			txtPath     = new string[MAX_CATEGORY];
        }

        //-----------------------------------------------------------------------------------------------
        //ディクショナリアクセサ
        //-----------------------------------------------------------------------------------------------
		public bool SetSummary(string keyFileName, string summary, int type )
        {
			if (m_dataMaster[type].ContainsKey(keyFileName) == false) return false;

			m_dataMaster[type][keyFileName].m_summary = summary;

 
            return true;
        }


        //-----------------------------------------------------------------------------------------------
        //ディクショナリアクセサ
        //-----------------------------------------------------------------------------------------------
		public string GetSummary(string keyFileName, int type )
        {
			if (m_dataMaster[type].ContainsKey(keyFileName) == false) return "";

			return m_dataMaster[type][keyFileName].m_summary;

        }


        //-----------------------------------------------------------------------------------------------
        //ディクショナリアクセサ
        //-----------------------------------------------------------------------------------------------
		public string GetGenre(string keyFileName, int type )
        {
            if (m_dataMaster[type].ContainsKey(keyFileName) == false) return "";

			return m_dataMaster[type][keyFileName].m_genre1;
        }

        //-----------------------------------------------------------------------------------------------
        //ディクショナリアクセサ
        //-----------------------------------------------------------------------------------------------
        public string GetGenre2(string keyFileName, int type)
        {
            if (m_dataMaster[type].ContainsKey(keyFileName) == false) return "";

            return m_dataMaster[type][keyFileName].m_genre2;
        }

        //-----------------------------------------------------------------------------------------------
        //Load　se.txtの内容とフォルダ内の*.wav,*.oggを列挙する
        //-----------------------------------------------------------------------------------------------
        public bool Load(string exePath )
        {

            //ここから設定の読み込み

            try {

                System.IO.StreamReader streamDataOption = new System.IO.StreamReader(exePath, System.Text.Encoding.Default);


                for (int i = 0; i < MAX_CATEGORY; i++)
                {
                    soundPath[i] = streamDataOption.ReadLine();

                    soundPath[i] = MainFunction.Add_EndPathSeparator(soundPath[i]);
                }

                txtPath[0] = streamDataOption.ReadLine();
                txtPath[1] = streamDataOption.ReadLine();
                txtPath[2] = streamDataOption.ReadLine();
                txtPath[3] = streamDataOption.ReadLine();
				txtPath[4] = streamDataOption.ReadLine();

				// 置き換え文字列を1つずつ読み込む
				pattern1 = streamDataOption.ReadLine();
                pattern2 = streamDataOption.ReadLine();
                pattern3 = streamDataOption.ReadLine();
                pattern4 = streamDataOption.ReadLine();
                pattern5 = streamDataOption.ReadLine();


                //window座標とか
                m_left = 0;
                m_top = 0;

                m_width = 780;
                m_height = 480;

                m_col1Size = 200;
                m_col2Size = 80;
                m_col3Size = 80;
                m_col4Size = 400;

                string buff;
                Regex regDataOption = new Regex("(.*),(.*),(.*),(.*),(.*),(.*),(.*),(.*)");
                Match matchResult;
                buff = streamDataOption.ReadLine();

                if (buff != null)
                    { 

                    matchResult = regDataOption.Match(buff);
                    if (!(matchResult.Success == false || buff.Length == 0))
                    {
    
                        m_left = int.Parse(matchResult.Groups[1].Value);
                        m_top = int.Parse(matchResult.Groups[2].Value);

                        m_width = int.Parse(matchResult.Groups[3].Value);
                        m_height = int.Parse(matchResult.Groups[4].Value);

                        m_col1Size = int.Parse(matchResult.Groups[5].Value);
                        m_col2Size = int.Parse(matchResult.Groups[6].Value);
                        m_col3Size = int.Parse(matchResult.Groups[7].Value);
                        m_col4Size = int.Parse(matchResult.Groups[8].Value);

                    }
                }

                streamDataOption.Close();

                if (m_left < 0) m_left = 0;
                if (m_top < 0) m_top = 0;
                if (m_height < 100) m_height = 100;
                if (m_width < 100) m_width = 100;

                //-----------------------------------------------------

                DataSet tmpData;
                Regex regGeter = new Regex("(.*),(.*)", RegexOptions.IgnoreCase);
                Regex regGeter2 = new Regex("(.*),(.*),(.*)", RegexOptions.IgnoreCase);
                Regex regIgnore = new Regex(@"//|^\n", RegexOptions.IgnoreCase);

                Regex regGenre = new Regex("※(.*)");

                string nowGenre = "未定ジャンル";
                string uniGenre = "";

                Match matcIgnoreResult = null;
                Match matchStringSplit = null;

                for (int i = 0; i < MAX_CATEGORY; i++)
                {
                    nowGenre = "未定ジャンル";
                    if (!(System.IO.File.Exists(txtPath[i])))
                    {
                        //txtPath[i] = "";
                        continue;
                    }

                    //csvを読み込
                    System.IO.StreamReader streamData = new System.IO.StreamReader(txtPath[i], System.Text.Encoding.Default);

                    // 読み込んだ結果をすべて格納するための変数を宣言する
                    string stResult = string.Empty;
                    
                    while (streamData.Peek() >= 0)
                    {
                        string stBuffer = streamData.ReadLine();

                        matcIgnoreResult = regIgnore.Match(stBuffer);
                        if (matcIgnoreResult.Success == true) continue;

                        uniGenre = "";

                        //まずジャンルを読み込むか判別
                        matchStringSplit = regGenre.Match(stBuffer);
                        if (matchStringSplit.Success == true)
                        {
                            nowGenre = matchStringSplit.Groups[1].Value;
                            m_genreList[i].Add(nowGenre);
                        }
                        else
                        {
                            matchStringSplit = regGeter2.Match(stBuffer);
                            if (matchStringSplit.Success == false)
                            {
                                matchStringSplit = regGeter.Match(stBuffer);
                                if (matchStringSplit.Success == false)
                                    continue;
                                else
                                    uniGenre = "";
                            }
                            else
                            {
                                uniGenre = matchStringSplit.Groups[3].ToString();
                                m_genreList2[i].Add(nowGenre +"∴"+ uniGenre);
                            }
                               
                            tmpData				= new DataSet();
                            tmpData.m_fileName  = matchStringSplit.Groups[1].ToString();
                            tmpData.m_summary   = matchStringSplit.Groups[2].ToString();
                            tmpData.m_genre1    = nowGenre;
                            tmpData.m_genre2    = uniGenre;
                            tmpData.m_isExist   = false;

                            m_dataMaster[i][tmpData.m_fileName] = tmpData;
                        }
                    }
                    
                    streamData.Close();
                }
            }

            catch( System.Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("設定ファイル(option.txt)が存在しないか、内容に問題があったため、ツールは自動的に終了します。\n設定ファイルがあるか、内容に問題がないか確認してください", "");
                return false;
            }
            //-----------------------------------------------------

            return true;
        }

        //-----------------------------------------------------------------------------------------------
        //オプション情報を保存する。
        //-----------------------------------------------------------------------------------------------
        public bool Save(string exePath)
        {

            //ここから設定の書き込み
			System.IO.StreamWriter streamData = new System.IO.StreamWriter(exePath + "option.txt", false, System.Text.Encoding.Default);


            foreach (string saveString in soundPath)
            {
                streamData.WriteLine(saveString);
            }

            foreach (string saveString in txtPath)
            {
                streamData.WriteLine(saveString);
            }

			
            streamData.WriteLine(pattern1);
            streamData.WriteLine(pattern2);
            streamData.WriteLine(pattern3);
			streamData.WriteLine(pattern4);
			streamData.WriteLine(pattern5);

            streamData.WriteLine( m_left + "," + m_top + "," + m_width + "," + m_height + "," + m_col1Size + ","+ m_col2Size + ","+m_col3Size + "," + m_col4Size);

            streamData.Close();

			


			return true;

        }

		public void SaveFavList()
		{
			//ここから設定の書き込み
			System.IO.StreamWriter streamData = new System.IO.StreamWriter(this.txtPath[MAX_CATEGORY-1], false, System.Text.Encoding.Default);



			string nowGenre = "";

			foreach ( KeyValuePair<string,DataSet> tmp in m_dataMaster[MAX_CATEGORY-1])
			{
				if (tmp.Value.m_genre1 == "" || tmp.Value.m_summary == "") continue;

				if (nowGenre != tmp.Value.m_genre1 )
				{
					streamData.WriteLine("※" + tmp.Value.m_genre1);
					nowGenre = tmp.Value.m_genre1;
				}
				streamData.WriteLine(tmp.Value.m_fileName + "," + tmp.Value.m_summary);
			}



			streamData.Close();

			return;
		}

        //-----------------------------------------------------------------------------------------------
        //ディクショナリーにファイル情報を足しつつ、既存であればファイルの存在を確認したということでフラグ立て
        //-----------------------------------------------------------------------------------------------
        public void AddDictionary( List<string> existFileList, int type )
        {
            DataSet tmpDat;
            foreach (string fileName in existFileList)
            {
                tmpDat              = new DataSet();
                tmpDat.m_fileName   = fileName;
                tmpDat.m_summary    = "";
                tmpDat.m_isExist    = true;
                try
                {
                    m_dataMaster[type].Add(fileName,tmpDat);
                }
                catch
                {
					m_dataMaster[type][fileName].m_isExist = true;
                }
            }
        }

		//-----------------------------------------------------------------------------------------------
		//ディクショナリーにファイル情報を足しつつ、既存であればファイルの存在を確認したということでフラグ立て
		//-----------------------------------------------------------------------------------------------
		public void AddDictionary(string fileName, int type, string summally = "", string genre1 = "", string genre2 = "")
		{
			DataSet tmpDat;

			tmpDat = new DataSet();
			tmpDat.m_fileName = fileName;
			tmpDat.m_summary = summally;
			tmpDat.m_isExist = true;
			tmpDat.m_genre1 = genre1;
			tmpDat.m_genre2 = genre2;

			try
			{
				m_dataMaster[type].Add(fileName, tmpDat);
			}
			catch(Exception ex)
			{
				if(summally != "" )	m_dataMaster[type][fileName].m_summary = summally;
				if(genre1 != "")	m_dataMaster[type][fileName].m_genre1 = genre1;
				if(genre2 != "")	m_dataMaster[type][fileName].m_genre2 = genre2;
				m_dataMaster[type][fileName].m_isExist = true;
			}
			
		}
	};
}
