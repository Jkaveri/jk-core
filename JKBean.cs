using System;
using System.Linq;

namespace JK.Core
{
    public abstract class JKBean
    {
        #region Fields

        private object _module;

        #endregion

        /// <summary>
        /// Set Current Module for Bean
        /// </summary>
        /// <example>
        ///     <code>
        /// public class Users{
        ///     public Users(){
        ///         base.setModule(this);
        ///     }
        ///     ....
        /// }
        /// </code>
        /// </example>
        /// <param name="obj">obj in module</param>
        protected void SetModule(object obj)
        {
            _module = obj;
            ModuleName = obj.GetType().Name;
        }

        /// <summary>
        /// Collect data for object
        /// </summary>
        /// <param name="id">a number that is ID of object</param>
        /// <returns>true if success and otherwiwse</returns>
        public virtual bool Retrieve(int id)
        {
            if (id < 0) throw new Exception("Id isn't set");
            if (_module == null) throw new Exception("Module not found");
            using (var db = new DB())
            {
                return db.Retrieve(id, _module);
            }
        }

        /// <summary>
        /// Save data of object to db
        /// </summary>
        /// <returns>Return true if success</returns>
        public virtual bool Save()
        {
            using (var db = new DB())
            {
                try
                {
                    return db.Save(_module);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Save Object (Update or Insert)
        /// </summary>
        /// <param name="isUpdate">bool; set true for update and false for insert</param>
        /// <returns>bool; true if success and otherwise</returns>
        public virtual bool Save(bool isUpdate)
        {
            using (var db = new DB())
            {
                return db.Save(_module, isUpdate);
            }
        }

        /// <summary>
        /// Delete an object
        /// By Set Deleted Propery is true(so Table in db have to Deleted)
        /// </summary>
        public virtual bool Delete(bool forever = false)
        {
            try
            {
                using (var db = new DB())
                {
                    if (IsSystem) throw new Exception("This object is deny by systems");
                    return db.Delete(_module, forever);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Properties
        /// <summary>
        /// ID of module, every module must have a ID (for Identity)
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Module name which inherit 
        /// </summary>
        private string ModuleName { get; set; }
        /// <summary>
        /// Determine if this object is existed in db
        /// Can overide for customize logic 
        /// </summary>
        public virtual bool IsUpdate
        {
            get
            {
                try
                {
                    if (ID <= 0) return false;
                    using (var db = new DB())
                    {
                        string tableName = _module.GetType().Name;
                        db.Query("SELECT * FROM " + tableName + " WHERE ID = @ID", ID);
                        return db.HasResult;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public virtual bool IsSystem
        {
            get
            {
                var setting = SiteSettings.GetInstance();
                var key = "JKBean::Systems::" + ModuleName;
                var ids = setting.GetValue(key);
                if(ids.Length > 0)
                {
                    var idArr =  Utils.SplitInt32(',',ids);
                    return idArr.Any(t => ID == t);
                }
                return false;
            }
        }
        #endregion
    }
}