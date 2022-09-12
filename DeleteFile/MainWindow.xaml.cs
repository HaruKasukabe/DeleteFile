using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack;
using Microsoft.WindowsAPICodePack.Dialogs;


namespace DeleteFile
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        // 変数宣言
        string FileName;
        bool SelectFlg = false; // ファイルを選択したか?
        string[] DFName = {"*.aps", "*.dep", "*.idb", "*.ilk", "*.ncb", "*.obj", "*.pdb", "*.res", "*.suo", "*.user",
        "BuildLog.thm","*.filters","*.exp","*.exe"}; // 削除するファイルの拡張子一覧
        string[] DDirectory = { "x64", "x86", "res" }; // 削除するサブディレクトリの一覧

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Select_File(object sender, RoutedEventArgs e)
        {
            // フォルダを選択するためのエクスプローラー起動
            using (var cofd = new CommonOpenFileDialog()
            {
                 // エクスプローラーのタイトル
                Title = "フォルダを選択してください",
                // 初期のディレクトリを指定
                InitialDirectory = @"C:\",
                // フォルダ選択モードにする
                IsFolderPicker = true,
            })
            {
                // もし開かれていないならreturnする(処理終了)
                if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }

                // FileNameで選択されたフォルダを取得する
                // 警告を出すためにフォルダが選ばれていることをフラグ管理
                SelectFlg = true; 
                // フォルダが選ばれたことをメッセージで表記
                System.Windows.MessageBox.Show($"{cofd.FileName}を選択しました");

                // cofdはローカル変数なのでグローバル変数にパスを代入
                FileName = cofd.FileName;
            }
        }
        private void Delete_File(object sender, RoutedEventArgs e)
        {
            // ファイルが選択されている場合のみこの処理を通る
            if (SelectFlg)
            {
                // 拡張子分for文を回す
                for (int j = 0; j < DFName.Length; ++j)
                {
                    // j番目の拡張子ファイルを変数に代入
                    string[] files = Directory.GetFiles(@FileName, DFName[j]);
                    for (int i = 0; i < files.Length; ++i)
                    {
                        // ファイルを削除
                        File.Delete(files[i]);
                        // 新しいファイルを作成(上書き)する文字列を記載する関数
                        File.WriteAllText(FileName + "DeleteFile.txt", files[i]);
                    }
                }
                // 次はサブディレクトリを削除する
                for (int j = 0; j < DDirectory.Length; ++j)
                {
                    // j番目のサブディレクトリを変数に代入
                    string[] file = Directory.GetDirectories(FileName, DDirectory[j]);
                    for (int i = 0; i < file.Length; ++i)
                    {
                        // サブディレクトリの中身ごと削除
                        Directory.Delete(file[i], true);
                        // 削除したサブディレクトリをテキストに追記
                        File.AppendAllText(FileName + "DeleteFile.txt", file[i]);

                    }
                }
                // ファイルを削除したことをメッセージで表記
                System.Windows.MessageBox.Show("ファイルを削除しました");
            }
            // ディレクトリを指定する前にファイルを削除しようとした場合
            else
            {
                // 警告音を出す
                System.Media.SystemSounds.Beep.Play();
                // 削除するディレクトリが選ばれていないことを警告
                System.Windows.MessageBox.Show("削除するファイルを選択してください","エラー");
            }
        }
        // バージョン情報取得
        private void ShowVersion(object sender, RoutedEventArgs e)
        {

        }
    }
}
