using App.Data.Model.Model;
using App.Entity.Common;
using App.Exceptions;
using App.Message;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static App.Utility.Enum;
using EFBulkInsert;

namespace App.Data.Model.Base
{
    public class Repository<TEntity> where TEntity:class
    {
        internal GloaithNationalEntities context;
        internal DbSet<TEntity> dbSet;

        public Repository(GloaithNationalEntities context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual void BulkInsert(List<TEntity> entity)
        {
            try
            {
                context.BulkInsert<TEntity>(entity);
            }
            catch(Exception ex)
            {
                throw new ExceptionOperation(HelperMessage.GetMessage(CodeMessage.GEN_001), (int)CodeHTTP.Error, ex.Message, ex.InnerException);
            }
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                if (orderBy != null)
                {
                    return orderBy(query).ToList();
                }
                else
                {
                    return query.ToList();
                }
            }catch(Exception ex)
            {
                throw new ExceptionOperation(HelperMessage.GetMessage(CodeMessage.GEN_001), (int)CodeHTTP.Error, ex.Message, ex.InnerException);

            }

        }

        public virtual TEntity GetByID(object id)
        {
            try
            {
                return dbSet.Find(id);
            }
            catch (Exception ex)
            {
                throw new ExceptionOperation(HelperMessage.GetMessage(CodeMessage.GEN_001), (int)CodeHTTP.Error, ex.Message, ex.InnerException);
            }
        }

        public virtual void Insert(TEntity entity)
        {
            try
            {
                dbSet.Add(entity);
            }
            catch (Exception ex)
            {
                throw new ExceptionOperation(HelperMessage.GetMessage(CodeMessage.GEN_001), (int)CodeHTTP.Error, ex.Message, ex.InnerException);
            }
        }

        public virtual void Delete(object id)
        {
            try
            {
                TEntity entityToDelete = dbSet.Find(id);
                Delete(entityToDelete);
            }
            catch (Exception ex)
            {
                throw new ExceptionOperation(HelperMessage.GetMessage(CodeMessage.GEN_001), (int)CodeHTTP.Error, ex.Message, ex.InnerException);
            }
        }
       
        public virtual void AllDelete()
        {
            try
            {
                context.Database.ExecuteSqlCommand($"TRUNCATE TABLE {typeof(TEntity).Name}");
            }
            catch(Exception ex)
            {
                throw new ExceptionOperation(HelperMessage.GetMessage(CodeMessage.GEN_001), (int)CodeHTTP.Error, ex.Message, ex.InnerException);
            }
        }
        public virtual void Delete(TEntity entityToDelete)
        {
            try
            {
                if (context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    dbSet.Attach(entityToDelete);
                }
                dbSet.Remove(entityToDelete);
            }
            catch (Exception ex)
            {
                throw new ExceptionOperation(HelperMessage.GetMessage(CodeMessage.GEN_001), (int)CodeHTTP.Error, ex.Message, ex.InnerException);
            }

        }

        public virtual void Update(TEntity entityToUpdate)
        {
            try
            {
                dbSet.Attach(entityToUpdate);
                context.Entry(entityToUpdate).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new ExceptionOperation(HelperMessage.GetMessage(CodeMessage.GEN_001), (int)CodeHTTP.Error, ex.Message, ex.InnerException);
            }
        }

        public virtual int TotalRegisters()
        {
            return dbSet.Count();
        }

        public T[] ExecuteSentence<T>(string sentence)
        {
            try
            {
                return context.Database.SqlQuery<T>(sentence).ToArray();
            }
            catch (Exception ex)
            {
                throw new ExceptionOperation(HelperMessage.GetMessage(CodeMessage.GEN_001), (int)CodeHTTP.Error, ex.Message, ex.InnerException);
            }
        }

        public T[] ExecuteProcedure<T>(SqlProcedure procedure)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendFormat("{0} {1}"
                    , procedure.ProcedureName
                    , string.Join(",", procedure.Parameters.Select(p => p.ParameterName + "=" + p.Value.ToString())));

                var selectedList = context
                             .Database
                             .SqlQuery<T>(query.ToString())
                             .ToArray<T>();

                return (T[])selectedList;
            }
            catch (Exception ex)
            {
                throw new ExceptionOperation(HelperMessage.GetMessage(CodeMessage.GEN_001), (int)CodeHTTP.Error, ex.Message, ex.InnerException);
            }

        }

        public T[] ExecuteProcedure<T>(string procedureName)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendFormat("{0}"
                    , procedureName);

                var selectedList = context
                             .Database
                             .SqlQuery<T>(query.ToString())
                             .ToArray<T>();

                return (T[])selectedList;
            }
            catch (Exception ex)
            {
                throw new ExceptionOperation(HelperMessage.GetMessage(CodeMessage.GEN_001), (int)CodeHTTP.Error, ex.Message, ex.InnerException);
            }

        }

        public int ExecuteProcedure(SqlProcedure procedure)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendFormat("{0} {1}"
                    , procedure.ProcedureName
                    , string.Join(",", procedure.Parameters.Select(p => p.ParameterName + "=" + p.Value.ToString())));

                var rowsAffected = context
                             .Database
                             .ExecuteSqlCommand(query.ToString());

                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw new ExceptionOperation(HelperMessage.GetMessage(CodeMessage.GEN_001), (int)CodeHTTP.Error, ex.Message, ex.InnerException);
            }

        }

    }
}
