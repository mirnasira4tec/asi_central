using System;
using System.Collections.Generic;

namespace asi.asicentral.model.news
{
    public class NewsSource
    {
        public NewsSource()
        {
            if (this.GetType() == typeof(NewsSource))
            {
                this.News = new List<News>();
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<News> News { get; set; }
    }
}
