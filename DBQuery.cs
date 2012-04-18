using System;

namespace JK.Core
{
    public class DBQuery
    {
        /// <summary>
        /// Order DESC|ASC
        /// </summary>
        private string _order;
        /// <summary>
        /// Order By column
        /// </summary>
        private string _orderBy;
        /// <summary>
        /// Page Num
        /// </summary>
        private int _pageNum;

        /// <summary>
        /// Page size (how many entities per page)
        /// </summary>
        private int _pageSize;
        /// <summary>
        /// Table name to query
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Order By; default is ID
        /// </summary>
        public string OrderBy
        {
            get { return String.IsNullOrEmpty(_orderBy) ? "ID" : _orderBy; }
            set { _orderBy = value; }
        }
        /// <summary>
        /// Order; Default is ASC
        /// </summary>
        public string Order
        {
            get { return String.IsNullOrEmpty(_order) ? "ASC" : _order; }
            set
            {
                if (!value.ToUpper().Equals("ASC") && !value.ToUpper().Equals("DESC"))
                    throw new Exception("Order can onnly be declared to be \"ASC\" or \"DESC\"");
                _order = value;
            }
        }
        /// <summary>
        /// Where clause;
        /// <example>ID=1 AND DELETED = 0</example>
        /// </summary>
        public string WhereClause { get; set; }
        /// <summary>
        /// PAGE NUM
        /// </summary>
        public int PageNum
        {
            get { return _pageNum < 0 ? 1 : _pageNum; }
            set { _pageNum = value; }
        }
        /// <summary>
        /// PAGE SIZE
        /// </summary>
        public int PageSize
        {
            get { return _pageSize < 0 ? 1 : _pageSize; }
            set { _pageSize = value; }
        }
    }
}