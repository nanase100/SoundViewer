using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SEViewer
{
    //-----------------------------------------------------------------------------------------------
    //
    //便利命令を置いてあります
    //
    //-----------------------------------------------------------------------------------------------
    public class MainFunction
    {
         
        //-----------------------------------------------------------------------------------------------
		/// 指定したパスとワイルドカードでファイルを列挙して返す
		//-----------------------------------------------------------------------------------------------
        public static string[] Get_PathFromDirectroy(string fromPath, string searchPattern, bool isFileNameOnly = false )
        {
			if( System.IO.Directory.Exists( fromPath ) == false )
			{
				System.Windows.Forms.MessageBox.Show("音の読み込み元フォルダ： "+fromPath+"　は存在しないため読み込まれませんでした。");
				string[] nullList = {"" };
				return nullList;
			}

            string[] fielList = Directory.GetFiles(fromPath, searchPattern, System.IO.SearchOption.AllDirectories);

            if (isFileNameOnly)
            {
                int loopCount = fielList.Length;
                for (int i = 0; i < loopCount; i++ )
                {
                    fielList[i] = Path.GetFileName(fielList[i]);
                }
            }

            return fielList;
        }

        
        //-----------------------------------------------------------------------------------------------
		/// パスの末尾に"\"が付いていなければつけて返す
		//-----------------------------------------------------------------------------------------------
		public static string Add_EndPathSeparator( string path )
		{
			if( path.Length != 0 && path.EndsWith("\\") == false )
			{
				path = path + "\\";
			}

			return path;
		}
		//-----------------------------------------------------------------------------------------------
		/// パスの末尾に"\"が付いていれば削除して返す
		//-----------------------------------------------------------------------------------------------
		public static string Del_EndPathSeparator( string path )
		{
			if( path.Length != 0 && path.EndsWith( "\\" ) == true )
			{
				path = path.Remove( path.Length - 1, 1 );
			}

			return path;
		}
    }
}
