using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDexDownloader
{
    public class MangaCollection
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Manga> Mangas { get; set; }
    }
}
