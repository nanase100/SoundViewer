using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SoundViewer
{

    
    static class Program
    {

        static public DataSetManager m_data = new DataSetManager();

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {

       
            string exePath = System.IO.Directory.GetCurrentDirectory();// System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            exePath = MainFunction.Add_EndPathSeparator(exePath);

            if (Program.m_data.settingLoad(exePath + "option.txt") == false)
            {
                if (Program.m_data.settingLoad(exePath + "_option.txt") == false)
                {
                    System.Windows.Forms.MessageBox.Show("設定ファイル(option.txt)が存在しないか、内容に問題があったため、ツールは自動的に終了します。\n設定ファイルがあるか、内容に問題がないか確認してください", "");
                    return;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Environment.Exit(0);
        }
        
    }


}
