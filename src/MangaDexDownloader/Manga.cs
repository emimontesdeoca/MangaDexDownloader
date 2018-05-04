using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDexDownloader
{
    public class Manga
    {
        public string Id { get; set; }
        public string Scanlator { get; set; }
        public string Language { get; set; }
        public List<Chapter> Chapters { get; set; }

        public Manga(string scanlator, string language, List<Chapter> chapters)
        {
            Id = new Guid().ToString();
            Scanlator = scanlator;
            Language = language;
            Chapters = chapters;
        }

        public Manga()
        {
            this.Chapters = new List<Chapter>();
        }
    }
}
