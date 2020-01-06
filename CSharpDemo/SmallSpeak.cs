using Less.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CSharpDemo
{
    public abstract class BaseSmallSpeak
    {
        /// <summary>
        /// 抓取网页并转码
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="Post_Parament"></param>
        /// <returns></returns>
        public abstract string HttpGet(string Url, string Post_Parament = null);
        /// <summary>
        /// 把网页源码转换成数据
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public abstract string HandleHtml(string html);
        /// <summary>
        /// 在这个方法里面获取数据
        /// </summary>
        /// <param name="Menu_Content"></param>
        /// <param name="Novel_Name"></param>
        public abstract void GetContent(string Menu_Content, string Novel_Name);
    }
    class SmallSpeak : BaseSmallSpeak
    {
        //全部目录url
        const string Url_Html = "https://wap.dingdiann.com/ddk85801/all.html";
        readonly string Url_Txt = "https://wap.dingdiann.com";

        /// <summary>
        /// 抓取网页并转码
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="Post_Parament"></param>
        /// <returns></returns>
        public override string HttpGet(string Url, string Post_Parament = null)
        {
            if (string.IsNullOrWhiteSpace(Url))
                Url = Url_Html;
            string html;
            HttpWebRequest Web_Request = (HttpWebRequest)WebRequest.Create(Url);
            Web_Request.Timeout = 30000;
            Web_Request.Method = "GET";
            Web_Request.UserAgent = "Mozilla/4.0";
            Web_Request.Headers.Add("Accept-Encoding", "gzip, deflate");
            //Web_Request.Credentials = CredentialCache.DefaultCredentials;

            //设置代理属性WebProxy-------------------------------------------------
            //WebProxy proxy = new WebProxy("111.13.7.120", 80);
            ////在发起HTTP请求前将proxy赋值给HttpWebRequest的Proxy属性
            //Web_Request.Proxy = proxy;

            HttpWebResponse Web_Response = (HttpWebResponse)Web_Request.GetResponse();

            if (Web_Response.ContentEncoding.ToLower() == "gzip")       // 如果使用了GZip则先解压
            {
                using (Stream Stream_Receive = Web_Response.GetResponseStream())
                {
                    using (var Zip_Stream = new GZipStream(Stream_Receive, CompressionMode.Decompress))
                    {
                        using (StreamReader Stream_Reader = new StreamReader(Zip_Stream, Encoding.UTF8))
                        {
                            html = Stream_Reader.ReadToEnd();
                        }
                    }
                }
            }
            else
            {
                using (Stream Stream_Receive = Web_Response.GetResponseStream())
                {
                    using (StreamReader Stream_Reader = new StreamReader(Stream_Receive, Encoding.Default))
                    {
                        html = Stream_Reader.ReadToEnd();
                    }
                }
            }
            return html;
        }

        public override string HandleHtml(string html)
        {

            string Novel_Name = Regex.Match(html, @"(?<=<span class=""title"">)([\S\s]*?)(?=</span>)").Value;

            Regex Regex_Menu = new Regex(@"(?is)(?<=<div  id=""chapterlist"" class=""directoryArea"">).+?(?=</div>)");
            string Result_Menu = Regex_Menu.Match(html).Value;   //获取章节集合

            Regex Regex_List = new Regex(@"(?is)(?<=<p>).+?(?=</p>)");
            var Result_List = Regex_List.Matches(Result_Menu);  //获取列表集合

            string Menu_Content = string.Empty;
            for (int i = 0; i < Result_List.Count; i++)
            {
                if (i != 0)
                {
                    Menu_Content += Result_List[i].ToString();
                }
            }
            GetContent(Menu_Content, Novel_Name);
            return string.Empty;
        }

        public override void GetContent(string Menu_Content, string Novel_Name)
        {
            Regex Regex_Href = new Regex(@"(?is)<a[^>]*?href=(['""]?)(?<url>[^'""\s>]+)\1[^>]*>(?<text>(?:(?!</?a\b).)*)</a>");
            MatchCollection Result_Match_List = Regex_Href.Matches(Menu_Content);   //获取href链接和a标签 innerHTML 

            string Novel_Path = Directory.GetCurrentDirectory() + "\\" + Novel_Name + ".txt";     //小说地址
            File.Create(Novel_Path).Close();
            StreamWriter Write_Content = new StreamWriter(Novel_Path);


            foreach (Match Result_Single in Result_Match_List)
            {
                string Url_Text = Result_Single.Groups["url"].Value;
                string Content_Text = Result_Single.Groups["text"].Value;

                string Content_Html = HttpGet(Url_Txt + Url_Text, "");//获取内容页

                Regex Rege_Content = new Regex(@"(?is)(?<=<div id=""chaptercontent"" class=""Readarea ReadAjax_content"">).+?(?=</div>)");
                string Result_Content = Rege_Content.Match(Content_Html).Value; //获取文章内容


                Regex Regex_Main = new Regex(@"(&nbsp;&nbsp;&nbsp;&nbsp;)(.*)");
                string Rsult_Main = Regex_Main.Match(Result_Content).Value; //正文            
                string Screen_Content = Rsult_Main.Replace("&nbsp;", "").Replace("<br />", "\r\n").Replace("<br/>", "\r\n");
                Regex_Main = new Regex(@"(?<=<a)([\S\s]*?)(?=</p>)");
                //替换开头的标签
                Screen_Content = Screen_Content.Replace("<a" + Regex_Main.Match(Screen_Content).Value + "</p>", "");

                Write_Content.WriteLine(Content_Text + "\r\n");//写入章节标题
                Write_Content.WriteLine(Screen_Content);//写入内容
            }
            Write_Content.Dispose();
            Write_Content.Close();
            MessageBox.Show(Novel_Name + ".txt 创建成功！");
            //System.Diagnostics.Process.Start(Directory.GetCurrentDirectory());
        }

    }

    class SmallSpeakPUA : BaseSmallSpeak
    {
        //全部目录url
        const string Url_Html = "http://www.iampua.com/book";
        readonly string Url_Txt = "http://www.iampua.com";

        /// <summary>
        /// 抓取网页并转码
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="Post_Parament"></param>
        /// <returns></returns>
        public override string HttpGet(string Url, string Post_Parament = null)
        {
            if (string.IsNullOrWhiteSpace(Url))
                Url = Url_Html;
            string html;
            HttpWebRequest Web_Request = (HttpWebRequest)WebRequest.Create(Url);
            Web_Request.Timeout = 30000;
            Web_Request.Method = "GET";
            Web_Request.UserAgent = "Mozilla/4.0";
            Web_Request.Headers.Add("Accept-Encoding", "gzip, deflate");
            //Web_Request.Credentials = CredentialCache.DefaultCredentials;

            //设置代理属性WebProxy-------------------------------------------------
            //WebProxy proxy = new WebProxy("111.13.7.120", 80);
            ////在发起HTTP请求前将proxy赋值给HttpWebRequest的Proxy属性
            //Web_Request.Proxy = proxy;

            HttpWebResponse Web_Response = (HttpWebResponse)Web_Request.GetResponse();

            if (Web_Response.ContentEncoding.ToLower() == "gzip")       // 如果使用了GZip则先解压
            {
                using (Stream Stream_Receive = Web_Response.GetResponseStream())
                {
                    using (var Zip_Stream = new GZipStream(Stream_Receive, CompressionMode.Decompress))
                    {
                        using (StreamReader Stream_Reader = new StreamReader(Zip_Stream, Encoding.UTF8))
                        {
                            html = Stream_Reader.ReadToEnd();
                        }
                    }
                }
            }
            else
            {
                using (Stream Stream_Receive = Web_Response.GetResponseStream())
                {
                    using (StreamReader Stream_Reader = new StreamReader(Stream_Receive, Encoding.Default))
                    {
                        html = Stream_Reader.ReadToEnd();
                    }
                }
            }
            return html;
        }

        public override string HandleHtml(string html)
        {
            //所有书籍url
            var document = HtmlParser.Parse(html);
            var span = document.getElementsByTagName("span");

            //目录
            Dictionary<string, Dictionary<string, string>> content = new Dictionary<string, Dictionary<string, string>>();
            foreach (var item in span)
            {
                if (item.firstChild.nodeName == "A")
                {
                    var child = item.firstChild as Element;
                    content.Add(child.textContent, new Dictionary<string, string>());
                    //目录url
                    var href = Url_Txt + child.getAttribute("href");
                    document = HtmlParser.Parse(HttpGet(href));
                    var ul = document.getElementsByTagName("ul")[0] as Element;
                    foreach (var itemLi in ul.childNodes)
                    {
                        if (itemLi.nodeName == "LI")
                        {
                            //内容url
                            href = Url_Txt + (itemLi.firstChild.firstChild as Element).getAttribute("href");
                            document = HtmlParser.Parse(HttpGet(href));
                            var p = document.getElementsByTagName("p")[0] as Element;
                            //标题 和 内容
                            var c = p.innerHTML.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "\t").Replace("<br>", string.Empty);
                            content[child.textContent].Add(((Element)itemLi).textContent, c);
                        }
                    }
                }
            }

            foreach (var item in content)
            {
                var path = Directory.GetCurrentDirectory() + "\\" + item.Key + ".txt";
                File.Create(path).Close();
                StreamWriter stream = new StreamWriter(path);
                foreach (var itemContent in item.Value)
                {
                    stream.WriteLine(itemContent.Key);
                    stream.WriteLine(itemContent.Value);
                }
                stream.Dispose();
                stream.Close();
            }

            return string.Empty;
        }

        public override void GetContent(string Menu_Content, string Novel_Name)
        {
            MessageBox.Show(Novel_Name + ".txt 创建成功！");
            //System.Diagnostics.Process.Start(Directory.GetCurrentDirectory());
        }
    }

}
