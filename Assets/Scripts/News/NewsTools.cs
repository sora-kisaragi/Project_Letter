using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Net;
using System.IO;
using UniRx;

namespace Hanachiru.News
{
    public static class NewsTools
    {
        private const string PATTERN_FOR_XML_OF_GOOGLE_API = "[(&nbsp)(&shy)(&ensp)(&thinsp)(ニュースですべての記事を表示)(;)(|)]";


        /// <summary>
        /// 指定したURLからJsonやXml(rss)などのデータを取得する
        /// </summary>
        /// <param name="url">対象のURL</param>
        /// <param name="onFinishReading">入手したデータ・終了を知らせるフラグを引数にもつコールバック</param>
        public static void GetDataFromNet(string url, Action<string, bool> onFinishReading)
        {
            //別スレッドで実行
            Observable.Start(() =>
            {
                try
                {
                    var req = (HttpWebRequest)WebRequest.Create(url);
                    var res = (HttpWebResponse)req.GetResponse();
                    using (var reader = new StreamReader(res.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
                catch
                {
                    return "";
                }
            })
            .ObserveOnMainThread()         //メインスレッドに切り替え
            .Subscribe(data =>
            {
                onFinishReading(data, true);
            });
        }


        /// <summary>
        /// テキストをUTF-8に変換する（URLエンコード）
        /// </summary>
        public static string EncodeUrl(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";

            return Uri.EscapeDataString(text);
        }

        /// <summary>
        /// テキストからHTMLタグを削除する
        /// </summary>
        public static string RemoveHtmlTags(string text, bool shouldDeleteSpecialCharacter = false)
        {
            if (string.IsNullOrEmpty(text)) return "";

            string s = RemoveCharacter(text, "<[^>]*?>");

            if (!shouldDeleteSpecialCharacter) return s;
            //print("なんかよくわからん");
            return RemoveCharacter(s, PATTERN_FOR_XML_OF_GOOGLE_API);
        }


        /// <summary>
        /// テキストから正規表現を用いて文字列を抽出する
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="pattern">正規表現</param>
        public static string ExtractCharacter(string text,string pattern)
        {
            if (string.IsNullOrEmpty(text)) return "";

            return Regex.Match(text, pattern).Value;
        }

        /// <summary>
        /// テキストから正規表現を用いて文字列を削除
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="pattern">正規表現</param>
        public static string RemoveCharacter(string text,string pattern)
        {
            if (string.IsNullOrEmpty(text)) return "";

            return Regex.Replace(text, pattern, string.Empty);
        }

        public static System.Xml.Linq.XElement ParseXml(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            System.Xml.Linq.XDocument xml = System.Xml.Linq.XDocument.Parse(data);
            System.Xml.Linq.XElement root = xml.Root;

            return root;
        }

        /// <summary>
        ///  Xml形式データから指定のタグのデータを抽出する
        /// </summary>
        public static string ExtractDataFromXML(System.Xml.Linq.XElement root, string tag, bool shouldSkip = false)
        {
            if (root == null) return "";

            var targetData = root.Descendants(tag);

            if (shouldSkip)
            {
                return targetData.Select(x => x.Value)
                    .Skip(1)
                    .FirstOrDefault();
            }

            return targetData.Select(x => x.Value)
                .FirstOrDefault();
        }

    }

}