using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Hanachiru.News
{
    public static class NewsReader
    {
        private static readonly string GOOGLE_NEWS_API_URL = "https://news.google.com/news/rss/{0}?hl=ja&gl=JP&ned=jp";
        private static readonly string SERCH_URL = "search/section/q/{0}/{0}/";

        /// <summary>
        /// GoogleNewsAPIを使ってニュース(タイトル・概要・リンク)を読み込む
        /// </summary>
        /// <param name="onFinishReading">読み込み終了後のコールバック</param>
        /// <param name="keywords">検索キーワード</param>
        public static IEnumerator ReadNews(Action<NewsData> onFinishReading, IReadOnlyCollection<string> keywords = null)
        {
            string title = null;
            string description = null;
            string link = null;

            string news = null;

            bool isFinished = false;

            //GoogleNewsAPIでXmlを取得
            NewsTools.GetDataFromNet(KeywordsToURL(keywords), (s, flag) => {
                news = s;
                isFinished = flag;
            });

            yield return new WaitUntil(() => isFinished);

            //newsからタイトル・概要・newsDetailURLの抽出
            var root = NewsTools.ParseXml(news);
            title = NewsTools.ExtractDataFromXML(root, "title", true);
            title = NewsTools.RemoveHtmlTags(title, true);
            description = NewsTools.ExtractDataFromXML(root, "description", true);
            description = NewsTools.RemoveHtmlTags(description, true);
            link = NewsTools.ExtractDataFromXML(root, "link", true);

            onFinishReading(new NewsData(title, description, link));
        }

        /// <summary>
        /// keywordのコレクションからGoogleNewsAPI用URLに変換
        /// </summary>
        private static string KeywordsToURL(IReadOnlyCollection<string> keywords)
        {
            if (keywords == null) return String.Format(GOOGLE_NEWS_API_URL, "");

            string serchText = "";

            foreach (var keyword in keywords)
            {
                serchText += NewsTools.EncodeUrl(keyword) + " ";
            }

            return String.Format(GOOGLE_NEWS_API_URL, String.Format(SERCH_URL, serchText));

        }

    }

    public class NewsData
    {
        public string Title { get; }

        public string Description { get; }

        public string Link { get; }

        public NewsData(string title, string description, string link)
        {
            Title = title;
            Description = description;
            Link = link;
        }
    }

}