using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using WYNK.Data.Common;
using WYNK.Data.Model;

namespace WYNK.Data.Repository.Implementation
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected WYNKContext WYNKContext { get; set; }
        protected CMPSContext CMPSContext { get; set; }
       

        public RepositoryBase(WYNKContext context , CMPSContext Cmpscontext)
        {
            WYNKContext = context;
            CMPSContext = Cmpscontext;
            
        }


        public IEnumerable<T> FindAll()
        {
            return WYNKContext.Set<T>();
        }

        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return WYNKContext.Set<T>().Where(expression);
        }

        public void Create(T entity)
        {
            WYNKContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            WYNKContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            WYNKContext.Set<T>().Remove(entity);
        }

        public bool Save()
        {
            return WYNKContext.SaveChanges() > 0;
        }




        public async Task<IEnumerable<Dropdown>> GetDropdown(string tableName, string valueColumn, string textColumn, string whereColumn = null, string whereValue = null)
        {
            try
            {
                var table = (IQueryable)WYNKContext.GetType().GetProperty(tableName).GetValue(WYNKContext, null);

                KeyValuePair<PropertyInfo, PropertyInfo> sourceDestPropMap1 = new KeyValuePair<PropertyInfo, PropertyInfo>(
                    typeof(Dropdown).GetProperty("Text"), table.ElementType.GetProperty(textColumn));

                KeyValuePair<PropertyInfo, PropertyInfo> sourceDestPropMap2 = new KeyValuePair<PropertyInfo, PropertyInfo>(
                    typeof(Dropdown).GetProperty("Value"), table.ElementType.GetProperty(valueColumn));

                var paramExpr = Expression.Parameter(table.ElementType, "x");
                var propertyA = Expression.Property(paramExpr, sourceDestPropMap1.Value);
                var propertyB = Expression.Property(paramExpr, sourceDestPropMap2.Value);
                var propertyBToString = Expression.Call(propertyB, typeof(object).GetMethod("ToString"));

                object query = null;
                if (!string.IsNullOrWhiteSpace(whereColumn) && !string.IsNullOrWhiteSpace(whereValue))
                {
                    var whereProp = Expression.Property(paramExpr, whereColumn);
                    dynamic value;
                    if (whereProp.Type.FullName.Contains("System.Int"))
                        value = Convert.ToInt32(whereValue);
                    else
                        value = whereValue;
                    var filter = Expression.Lambda(Expression.Equal(Expression.Property(paramExpr, whereColumn), Expression.Constant(value)), paramExpr);
                    query = Call(Where.MakeGenericMethod(paramExpr.Type), table, filter);
                }
                var createObject = Expression.New(typeof(Dropdown));
                var initializePropertiesOnObject = Expression.MemberInit(
                    createObject,
                    new[]
                    {
                        Expression.Bind(sourceDestPropMap1.Key, propertyA),
                        Expression.Bind(sourceDestPropMap2.Key, propertyBToString)
                    });

                var selectExpression = Expression.Lambda(initializePropertiesOnObject, paramExpr);

                query = Call(Select.MakeGenericMethod(paramExpr.Type, typeof(Dropdown)), query != null ? query : table, selectExpression);
                return (IEnumerable<Dropdown>)query;

            }
            catch (Exception e)
            {

                throw;
            }
        }
        private static readonly MethodInfo Select = GetGenericMethodDefinition<
            Func<IQueryable<object>, Expression<Func<object, object>>, IQueryable<object>>>((source, selector) =>
            Queryable.Select(source, selector));
        private static readonly MethodInfo Where = GetGenericMethodDefinition<
            Func<IQueryable<object>, Expression<Func<object, bool>>, object>>((source, predicate) =>
            Queryable.Where(source, predicate));
        private static readonly MethodInfo FirstOrDefault = GetGenericMethodDefinition<
            Func<IQueryable<object>, object>>(source =>
            Queryable.FirstOrDefault(source));
        private static MethodInfo GetGenericMethodDefinition<TDelegate>(Expression<TDelegate> e)
        {
            return ((MethodCallExpression)e.Body).Method.GetGenericMethodDefinition();
        }
        private static object Call(MethodInfo method, params object[] parameters)
        {
            return method.Invoke(null, parameters);
        }

    }
}
