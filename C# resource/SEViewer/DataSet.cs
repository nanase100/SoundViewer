using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;

namespace SoundViewer
{
    class DataSet
    {
        public string m_fileName    { get; set; }
        public string m_summary     { get; set; }
        public bool m_isExist       { get; set; }
        public string m_genre1      { get; set; }
        public string m_genre2      { get; set; }

    };


	public class readJsonType1  {
		public List<int>	ウインドウ座標					{ get; set; } = new List<int>();
		public List<int>	音リストのカラム幅				{ get; set; } = new List<int>();
		public int			画面分割幅						{ get; set; }
		public List<string>	音ファイルのパス				{ get; set; } = new List<string>();
		public List<string>	音リストテキストのパス			{ get; set; } = new List<string>();
		public List<string>	コピー文						{ get; set; } = new List<string>();
		public List<bool>	機能オプションONOFF				{ get; set; } = new List<bool>();
	
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

        public List<string> copyStr { get; set; }

        public int m_left		{ set; get; }
        public int m_top		{ set; get; }
        public int m_width		{ set; get; }
        public int m_height		{ set; get; }
        public int m_col1Size	{ set; get; }
        public int m_col2Size	{ set; get; }
        public int m_col3Size	{ set; get; }
        public int m_col4Size	{ set; get; }

		public int m_splitSize             { set; get; }

		public List<int>		m_toolOption		{ set; get; } = new List<int>();

		readJsonType1 jsonData;

        //メインのデータ
        private Dictionary<string, DataSet>[] m_dataMaster;

        //絞込の後に画面に表示される m_dataMasterから抜き出した参照リスト
        //private List<DataSet> m_dataRefList = new List<DataSet>();

        public HashSet<string>[] m_genreList { get; set; }
        public HashSet<string>[] m_genreList2 { get; set; }
		public List<Color>[]		m_genreColorList { get; set; }

        public Dictionary<string, DataSet>  GetDataSet(int type)
		{
			return m_dataMaster[type];
		}

        //-----------------------------------------------------------------------------------------------
        //コンストラクタ
        //-----------------------------------------------------------------------------------------------
        public DataSetManager()
        {
            m_dataMaster	    = new Dictionary<string, DataSet>[MAX_CATEGORY];
            m_genreList		    = new HashSet<string>[MAX_CATEGORY];
            m_genreList2		= new HashSet<string>[MAX_CATEGORY];
			m_genreColorList    = new List<Color>[MAX_CATEGORY];
			

            for (int i = 0; i < MAX_CATEGORY; i++)
			{
				m_dataMaster[i]		= new Dictionary<string, DataSet>();
                m_genreList[i]		= new HashSet<string>();
                m_genreList2[i]		= new HashSet<string>();
				m_genreColorList[i] = new List<Color>();
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
        public bool settingLoad(string settingFilePath )
        {

			try{
            //ここから設定の読み込み

			jsonData = JsonConvert.DeserializeObject<readJsonType1>(File.ReadAllText(settingFilePath));
			
			
			for (int i = 0; i < MAX_CATEGORY; i++)
            {
                soundPath[i] = MainFunction.Add_EndPathSeparator(jsonData.音ファイルのパス[i]);

				txtPath[i] = jsonData.音リストテキストのパス[i];
            }
			
			copyStr = new List<string>();
			for( int i = 0; i < jsonData.コピー文.Count; i++ )
				copyStr.Add( jsonData.コピー文[i] );

            //window座標とか
            m_left = 0;
            m_top = 0;
            m_width = 780;
            m_height = 480;

            m_col1Size = 200;
            m_col2Size = 80;
            m_col3Size = 80;
            m_col4Size = 400;

			m_splitSize = jsonData.画面分割幅;

			//window座標とか
            m_left	= jsonData.ウインドウ座標[0];
            m_top	= jsonData.ウインドウ座標[1];
            m_width = jsonData.ウインドウ座標[2];
            m_height= jsonData.ウインドウ座標[3];

            m_col1Size = jsonData.音リストのカラム幅[0];
            m_col2Size = jsonData.音リストのカラム幅[1];
            m_col3Size = jsonData.音リストのカラム幅[2];
            m_col4Size = jsonData.音リストのカラム幅[3];

			//ツールオプションのon/off
			for( int i = 0; i < jsonData.機能オプションONOFF.Count; i++ )
			{
				m_toolOption.Add((jsonData.機能オプションONOFF[i] == true?1:0));
			}

            if (m_left < 0)		m_left = 0;
            if (m_top < 0)		m_top = 0;
            if (m_height < 100) m_height = 100;
            if (m_width < 100)	m_width = 100;

			}
			catch( System.Exception ex)
			{
			//	System.Windows.Forms.MessageBox.Show("設定ファイル(option.txt)が見つからない、または内容が正しくありません。\noption.txtの記述はjson形式となっています。");
				return false;
			}
            //-----------------------------------------------------

            DataSet tmpData;
            Regex regGeter = new Regex("(.*),(.*)", RegexOptions.IgnoreCase);
            Regex regGeter2 = new Regex("(.*),(.*),(.*)", RegexOptions.IgnoreCase);
            Regex regIgnore = new Regex(@"//|^\n", RegexOptions.IgnoreCase);

			Regex regGenre = new Regex("※(.*)");
            Regex regGenreEx = new Regex("※(.*),#(..)(..)(..)");

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

                    //まずジャンル色付きを読み込むか判別
					matchStringSplit = regGenreEx.Match(stBuffer);
					if (matchStringSplit.Success == true )	{
						
						nowGenre = matchStringSplit.Groups[1].Value;
						int R = System.Convert.ToInt32( matchStringSplit.Groups[2].Value, 16 );
						int G = System.Convert.ToInt32( matchStringSplit.Groups[3].Value, 16 );
						int B = System.Convert.ToInt32( matchStringSplit.Groups[4].Value, 16 );
						Color genreColor = Color.FromArgb(R, G, B);

						m_genreList[i].Add(nowGenre);
						m_genreColorList[i].Add( genreColor );
					}else{
						matchStringSplit = regGenre.Match(stBuffer);
						if (matchStringSplit.Success == true)
						{
							nowGenre = matchStringSplit.Groups[1].Value;
							m_genreList[i].Add(nowGenre);
							m_genreColorList[i].Add( Color.Black );
						}else{
							matchStringSplit = regGeter2.Match(stBuffer);
							if (matchStringSplit.Success == false)
							{
								matchStringSplit = regGeter.Match(stBuffer);
								if (matchStringSplit.Success == false)
									continue;
								else
									uniGenre = "";
							}else{
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
                }
                streamData.Close();
            }
            
            //-----------------------------------------------------

            return true;
        }

        //-----------------------------------------------------------------------------------------------
        //オプション情報を保存する。
        //-----------------------------------------------------------------------------------------------
        public bool settingSave(string settingFilePath)
        {
            //ここから設定の書き込み			
			for (int i = 0; i < MAX_CATEGORY; i++)
            {
                soundPath[i] = MainFunction.Add_EndPathSeparator(jsonData.音ファイルのパス[i]);

				jsonData.音リストテキストのパス[i] = txtPath[i];
            }
			
			for( int i = 0; i < copyStr.Count; i++ )
				jsonData.コピー文[i] = copyStr[i];
            
			jsonData.画面分割幅 = m_splitSize;

            //window座標とか
            jsonData.ウインドウ座標[0] = m_left;
            jsonData.ウインドウ座標[1] = m_top;
            jsonData.ウインドウ座標[2] = m_width;
            jsonData.ウインドウ座標[3] = m_height;

            jsonData.音リストのカラム幅[0] =  m_col1Size;
            jsonData.音リストのカラム幅[1] =  m_col2Size;
            jsonData.音リストのカラム幅[2] =  m_col3Size;
            jsonData.音リストのカラム幅[3] =  m_col4Size;

			//ツールオプションのon/off
			for( int i = 0; i < jsonData.機能オプションONOFF.Count; i++ )
			{
				jsonData.機能オプションONOFF[i] = ( m_toolOption[i]== 1?true:false);;
			}

            var outputStr = JsonConvert.SerializeObject(jsonData);

			outputStr = format_json(outputStr);

			File.WriteAllText(settingFilePath, outputStr );

			return true;
        }


		private static string format_json(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
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

		public void DelDictionary( string key, int dicType )
		{
			m_dataMaster[dicType].Remove( key );
		}
	};
}
