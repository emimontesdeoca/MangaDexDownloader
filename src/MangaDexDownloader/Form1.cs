using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MangaDexDownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            MangaCollection mangaCollection = new MangaCollection();
            mangaCollection.Mangas = new List<Manga>();

            InitializeComponent();
            string id = "39";
            string source_url = "https://mangadex.org/manga/" + id + "/chapters/";
            int pages = 1;

            /// Get all pages
            /// <li class="paging"><a href="/manga/
            using (WebClient client = new WebClient { Encoding = System.Text.Encoding.UTF8 })
            {
                try
                {
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                    string res = client.DownloadString(source_url);

                    string decoded = System.Net.WebUtility.HtmlDecode(res);
                    string[] li = decoded.Split(new[] { "paging" }, StringSplitOptions.None);
                    string lastLi = li[li.Length - 1].Split(new[] { "chapters/" }, StringSplitOptions.None)[1].Split(new[] { "'>" }, StringSplitOptions.None)[0];
                    pages = Int32.Parse(lastLi.Remove(lastLi.Length - 1, 1));
                }
                catch (Exception)
                {
                    /// There are no pages
                    pages = 1;
                }

            }

            for (int j = 1; j <= pages; j++)
            {
                using (WebClient client = new WebClient { Encoding = System.Text.Encoding.UTF8 })
                {
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                    string res = client.DownloadString(source_url + j);

                    string decoded = System.Net.WebUtility.HtmlDecode(res);
                    string table = decoded.Split(new[] { @"<tbody>" }, StringSplitOptions.None)[1].Replace("\t", "").Replace("\r", "").Replace("\n", "");
                    string[] tr = table.Split(new[] { @"chapter_" }, StringSplitOptions.None);

                    for (int i = 0; i < tr.Length; i++)
                    {
                        tr[i] = tr[i].Split(new[] { @"</tr>" }, StringSplitOptions.None)[0];

                        try
                        {
                            string[] td = tr[i].Split(new[] { @"<td" }, StringSplitOptions.None);

                            // Scanlator
                            string scanlator = td[5].Split(new[] { @"'>" }, StringSplitOptions.None)[1];
                            scanlator = scanlator.Split(new[] { @"<" }, StringSplitOptions.None)[0];

                            // Group ID
                            string group = td[5].Split(new[] { @"/group/" }, StringSplitOptions.None)[1];
                            group = group.Split(new[] { @"/" }, StringSplitOptions.None)[0];
                            // Language
                            string language = td[4].Split(new[] { @"title='" }, StringSplitOptions.None)[1];
                            language = language.Split(new[] { @"'" }, StringSplitOptions.None)[0];

                            // Chapter
                            string chapter = td[2].Split(new[] { @"/chapter/" }, StringSplitOptions.None)[1];
                            chapter = chapter.Split(new[] { @"""" }, StringSplitOptions.None)[0];
                            chapter = chapter.Split(new[] { @"'" }, StringSplitOptions.None)[0];

                            string finalChapter = "https://mangadex.org/chapter/" + chapter;

                            /// Create chapter
                            Chapter c = new Chapter();

                            c.Id = chapter;
                            c.Url = finalChapter;

                            /// Check and add
                            bool foundScanlator = false;

                            foreach (var item in mangaCollection.Mangas)
                            {
                                if (item.Scanlator == scanlator)
                                {
                                    item.Chapters.Add(c);
                                    foundScanlator = true;
                                    break;
                                }
                            }

                            if (!foundScanlator)
                            {
                                /// New manga
                                Manga m = new Manga();

                                /// Fill
                                m.Language = language;
                                m.Scanlator = scanlator;
                                m.Chapters.Add(c);

                                /// Add collection
                                mangaCollection.Mangas.Add(m);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }

        }
    }
}
