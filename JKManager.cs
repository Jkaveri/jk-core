using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JK.Core
{
    /// <summary>
    /// Abstract class for manage Classes
    /// ex: UsersManager
    /// </summary>
    /// <typeparam name="T">entity to manage</typeparam>
    /// <example>
    /// <c>Create</c>
    /// <code>
    ///     public class UsersManager:JKManager<Users>{
    ///     
    ///     }
    /// </code>
    /// <c>Use:</c>
    /// <code>
    ///     UsersManager manager = new UsersManager();
    ///   gvUsers.DataSource =  manager.GetListEntities(1,10);//get list 10 user page 1
    /// gvUsers.DataBind();
    /// ltrPaginate.Text = manager.HtmlPaginateNav();//Get HTml paginate navigate
    /// </code>
    /// </example>
    public class JKManager<T> : Paginate where T : class, new()
    {
        /// <summary>
        /// Current module name is en
        /// </summary>
        protected string ModuleName
        {
            get
            {
                return typeof(T).Name;
            }
        }
        /// <summary>
        /// Get entities has Deleted(deleted = 1) if set true
        /// </summary>
        protected bool GetDeletedEntity { get; set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        protected JKManager()
        {
            GetDeletedEntity = false;//default GetDeletedEntity = false;
        }
        /// <summary>
        /// Get list entities in table with pagination
        /// </summary>
        /// <param name="page">page to get</param>
        /// <param name="pageSize">page size</param>
        /// <param name="whereClause">filter clause</param>
        /// <param name="orderBy">Order by whic column</param>
        /// <param name="order">DESC|ASC</param>
        /// <returns>IEnumerable list entities</returns>
        public IEnumerable<T> GetListEntities(int page = 1, int pageSize = 10, string whereClause = "", string orderBy = "ID", string order = "ASC")
        {
            var objs = new List<T>();
            CurrentPage = page;
            PageSize = pageSize;
            if (!GetDeletedEntity)
            {
                whereClause += (!String.IsNullOrEmpty(whereClause)) ? " and Deleted = 0 " : " Deleted = 0";

                _whereClause = whereClause;
            }
            using (var db = new DB())
            {
                db.Query(new DBQuery { TableName = ModuleName, PageSize = pageSize, PageNum = page, OrderBy = orderBy, Order = order, WhereClause = whereClause });
                if (db.HasResult)
                {
                    while (db.Read())
                    {
                        var obj = new T();
                        db.SetValues(obj);
                        objs.Add(obj);
                    }
                }

            }
            return objs;
        }
        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id">int; id of entity</param>
        /// <returns>T endity</returns>
        public virtual T GetEntity(int id)
        {
            if (id < 0) throw new Exception("Id isn't set");
            var obj = new T();
            using (var db = new DB())
            {
                db.Retrieve(id, obj);
                return obj;
            }
        }
        /// <summary>
        /// Save data of object into db
        /// </summary>
        /// <param name="t">T; object to save</param>
        /// <returns>T;</returns>
        public virtual T SetEntity(T t)
        {
            if(t == null)
            {
                throw new Exception("Object is null");
            }
            using (var db = new DB())
            {
                db.Save(t);
                return t;
            }
        }
        /// <summary>
        /// Delete object in db
        /// </summary>
        /// <param name="t">T; Object to delete</param>
        /// <returns>true if success</returns>
        public virtual bool DelEntity(T t)
        {
            
            if(t == null)
            {
                throw new Exception("Object is null");
            }
            using (var db = new DB())
            {
              return  db.Delete(t);   
            }
        }
        /// <summary>
        /// Save list entities in to db
        /// </summary>
        /// <param name="listEntities"></param>
        /// <returns></returns>
        [Obsolete("this method is constructing...",true)]
        public bool SaveListEntities(List<T> listEntities)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Overide total record of Paginate class
        /// </summary>
        protected override int TotalRecord
        {
            get
            {
                using (var db = new DB())
                {
                    string getDeleted = (GetDeletedEntity) ? "" : " WHERE " + _whereClause;
                    db.Query("SELECT COUNT(*) as Count FROM " + ModuleName + getDeleted);
                    if (db.HasResult)
                    {
                        db.Read();
                        return db.GetInt32(0);
                    }
                    return 0;
                }
            }
        }
        /// <summary>
        /// where clause
        /// </summary>
        private string _whereClause;
    }
}
