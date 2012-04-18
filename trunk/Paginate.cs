using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace JK.Core
{
    public abstract class Paginate
    {
        
        /// <summary>
        /// default constructor
        /// </summary>
        protected Paginate()
        {
            PageSize = 1;
            CurrentPage = 1;
            PageSize = 10;
            NextText = "Next";
            PrevText = "Prev";
            FirstText = "First";
            LastText = "Last";
            ShowNavigator = true;
            ShowPageNums = true;
            HideWhenOnlyOne = true;
        }
        /// <summary>
        /// Generate HTML Pagination navigate 
        /// </summary>
        /// <returns>a string; Output HTML</returns>
        public virtual string HtmlPaginateNav()
        {
            //get current url
            var url = HttpContext.Current.Request.RawUrl;
            Regex IsPageParameter = new Regex(@"[&|?]Page\=\d+");
            Regex HasQueryString = new Regex(@"[?]\w+\=\w+");
           url=  IsPageParameter.Replace(url, "");
            var hasQueryString = HasQueryString.IsMatch(url);
            url = (hasQueryString) ? url + "&" : url + "?";
            ////

            var builder = new StringBuilder();
            //page count
            var pageNums = TotalRecord / PageSize;
            if (TotalRecord % PageSize > 0)
            {
                pageNums += 1;
            }
            var hide = ""; //dispaly:none;
           
            if (HideWhenOnlyOne && pageNums <= 1) // if setting HideWhenOnlyOne = true and pageNums = 1 (only one page)
            {
                hide = " style='display:none' ";
            }
            builder.AppendLine("<div ${hide} class='pagination'>");
            builder.Replace("${hide}", hide);
            
          
            if (ShowNavigator)
            {
                if(CurrentPage > 1)//is first page
                {
                    builder.AppendFormat("<a href='{0}Page=1'>",url).Append(FirstText).Append("</a>");
                    builder.AppendFormat("<a href='{0}Page={1}'>",url, CurrentPage - 1).Append(PrevText).Append("</a>");
                }else
                {
                    builder.Append("<a class='disabled' href='#'>").Append(FirstText).Append("</a>");
                    builder.Append("<a class='disabled' href='#'>").Append(PrevText).Append("</a>");
                }
            }
            if (ShowPageNums)
                for (int i = 1; i <= pageNums; i++)
                {
                    builder.AppendLine();
                    builder.Append("<a class='number ")
                        .Append(((i == CurrentPage) ? "current" : ""))
                        .AppendFormat("' href='{1}Page={0}'>{0}</a>", i,url);
                }
            if (ShowNavigator)
            {
                if(CurrentPage < pageNums)//is last page
                {
                    builder.AppendFormat("\n<a href='{0}Page={1}'>",url, CurrentPage + 1).Append(NextText).Append("</a>");
                    builder.AppendFormat("\n<a href='{0}Page={1}'>",url, pageNums).Append(LastText).Append("</a>");
                }
                else
                {
                    builder.AppendLine("<a class='disabled' href='#'>").Append(NextText).Append("</a>");
                    builder.AppendLine("<a class='disabled' href='#'>").Append(LastText).Append("</a>");
                }
                
            }
            builder.AppendLine("</div>");
            return builder.ToString();
        }
        /// <summary>
        /// Total recored of get
        /// must be implement this property
        /// </summary>
        protected abstract int TotalRecord { get; }
        /// <summary>
        /// Page size : Default is 10
        /// </summary>
        protected int PageSize { get; set; }
        /// <summary>
        /// Current page: default is 1
        /// </summary>
        protected int CurrentPage { get; set; }
        /// <summary>
        /// Next navigate text: Default is "Next"
        /// </summary>
        public string NextText { get; set; }
        /// <summary>
        /// Previous navigate text: Default is "Prev"
        /// </summary>
        public string PrevText { get; set; }
        /// <summary>
        /// Go to first text: Default is "First"
        /// </summary>
        public string FirstText { get; set; }
        /// <summary>
        /// Go to last text; Default is <c>"Last"</c>
        /// </summary>
        public string LastText { get; set; }
        /// <summary>
        /// Show navigator (First, Last); Default is <c>true</c>
        /// </summary>
        public bool ShowNavigator { get; set; }
        /// <summary>
        /// Show page nums (1,2,3...); Default is <c>true</c>
        /// </summary>
        public bool ShowPageNums { get; set; }

        public bool HideWhenOnlyOne { get; set; }
    }
}
