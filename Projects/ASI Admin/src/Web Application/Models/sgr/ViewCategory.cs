using asi.asicentral.model.sgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.Models.sgr
{
    public class ViewCategory : Category
    {
        public int CompanyID { set; get; }

        public static ViewCategory CreateCategoryFrom(Category category)
        {
            ViewCategory viewCategory = new ViewCategory();
            category.CopyTo(viewCategory);
            return viewCategory;
        }

        public Category GetCategory()
        {
            Category category = new Category();
            this.CopyTo(category);
            return category;
        }
    }
}