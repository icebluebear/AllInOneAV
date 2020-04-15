using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace DataBaseManager.Common
{
    public static class ModelConvertHelper
    {
        /// <summary>
        /// 匿名类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anonymous"></param>
        /// <param name="anonymousType"></param>
        /// <returns></returns>
        public static T CastAnonymous<T>(object anonymous, T anonymousType)
        {
            return (T)anonymous;
        }

        /// <summary>
        /// DataTable.Rows[rowIndex] 转换为 TModel
        /// </summary>
        /// <typeparam name="T">Model类型</typeparam>
        /// <param name="dataTable">DataTable数据源</param>
        /// <param name="rowIndex">需要转换的列</param>
        /// <returns>当DataTable为空或指定的rowIndex大于dt.Rows.Count时，返回null，否则返回default(T)</returns>
        public static T ToModel<T>(DataTable dataTable, int rowIndex = 0) where T : class, new()
        {
            T tModel = default(T);
            if (dataTable == null || dataTable.Rows.Count <= 0 || dataTable.Rows.Count < rowIndex)
            {
                return tModel;
            }

            tModel = new T();
            DataRow row = dataTable.Rows[rowIndex];
            List<PropertyInfo> propertyList = GetProperties<T>(dataTable);
            propertyList.ForEach((p) =>
            {
                if (row[p.Name] != DBNull.Value)
                {
                    if (!p.PropertyType.IsGenericType)
                    {
                        p.SetValue(tModel, Convert.ChangeType(row[p.Name], p.PropertyType, CultureInfo.CurrentCulture), null);
                    }
                    else
                    {
                        Type genericTypeDefinition = p.PropertyType.GetGenericTypeDefinition();
                        if (genericTypeDefinition == typeof(Nullable<>))
                        {
                            var value = Convert.ChangeType(row[p.Name], Nullable.GetUnderlyingType(p.PropertyType), CultureInfo.CurrentCulture);
                            p.SetValue(tModel, value, null);
                        }
                    }
                }
            });
            return tModel;
        }

        /// <summary>
        /// DataTable.Rows[0] 转换为 TModel
        /// </summary>
        /// <typeparam name="T">Model类型</typeparam>
        /// <param name="dataTable">DataTable数据源</param>
        /// <returns>当DataTable为空或指定的rowIndex大于dt.Rows.Count时，返回null，否则返回default(T)</returns>
        public static T ToModel<T>(this DataTable dataTable) where T : class, new()
        {
            return ToModel<T>(dataTable, 0);
        }

        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dataTable">DataTable</param>
        /// <returns>总是返回new List<T/>()</returns>
        public static List<T> ToList<T>(DataTable dataTable) where T : class, new()
        {
            List<T> tModelList = new List<T>();
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return tModelList;
            }

            List<PropertyInfo> propertyList = GetProperties<T>(dataTable);
            foreach (DataRow row in dataTable.Rows)
            {
                T ob = new T();
                propertyList.ForEach((p) =>
                {
                    if (row[p.Name] != DBNull.Value)
                    {
                        if (!p.PropertyType.IsGenericType)
                        {
                            p.SetValue(ob, Convert.ChangeType(row[p.Name], p.PropertyType, CultureInfo.CurrentCulture), null);
                        }
                        else
                        {
                            Type genericTypeDefinition = p.PropertyType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                var value = Convert.ChangeType(row[p.Name], Nullable.GetUnderlyingType(p.PropertyType), CultureInfo.CurrentCulture);
                                p.SetValue(ob, value, null);
                            }
                        }
                    }
                });
                tModelList.Add(ob);
            }
            return tModelList;
        }

        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable">DataTable</param>
        /// <param name="n">忽略此值，分辨重载用</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dataTable, int n = 0) where T : class, new()
        {
            return ToList<T>(dataTable);
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this DataTable dataTable, string keyColumn, string valueColumn, Func<object, TKey> keyConverter = null, Func<object, TValue> valueConverter = null)
        {
            var dic = new Dictionary<TKey, TValue>();

            foreach (DataRow row in dataTable.Rows)
            {
                TKey key = default(TKey);
                if (row[keyColumn] != DBNull.Value)
                {
                    if (keyConverter != null)
                    {
                        key = keyConverter(row[keyColumn]);
                    }
                    else
                    {
                        key = (TKey)Convert.ChangeType(row[keyColumn], typeof(TKey), CultureInfo.CurrentCulture);
                    }
                }

                TValue value = default(TValue);
                if (row[valueColumn] != DBNull.Value)
                {
                    if (valueConverter != null)
                    {
                        value = valueConverter(row[valueColumn]);
                    }
                    else
                    {

                        value = (TValue)Convert.ChangeType(row[valueColumn], typeof(TValue), CultureInfo.CurrentCulture);
                    }
                }

                dic.Add(key, value);
            }

            return dic;
        }

        /// <summary>
        /// 反射获取T的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static List<PropertyInfo> GetProperties<T>(DataTable dataTable) where T : class, new()
        {
            Type t = typeof(T);

            //string key = t.FullName;
            //PropertyInfo[] properties = CacheTools.GetCache<PropertyInfo[]>(key);

            //if (properties == null || properties.Length == 0)
            //{
            //    properties = t.GetProperties();
            //    CacheTools.CacheInsert(key, properties, TimeSpan.FromHours(1));
            //}

            PropertyInfo[] properties = t.GetProperties();

            List<PropertyInfo> propertyList = new List<PropertyInfo>();
            Array.ForEach<PropertyInfo>(properties, (p) =>
            {
                if (dataTable.Columns.IndexOf(p.Name) != -1 && p.CanWrite)
                    propertyList.Add(p);
            });
            return propertyList;
        }


        ///// <summary>  
        ///// 将集合类转换成DataTable  
        ///// </summary>
        ///// <param name="list">集合</param>  
        ///// <returns></returns>  
        //public static DataTable ToDataTable(this IList list)
        //{
        //    DataTable result = new DataTable();
        //    if (list.Count > 0)
        //    {
        //        PropertyInfo[] propertys = list[0].GetType().GetProperties();
        //        foreach (PropertyInfo pi in propertys)
        //        {
        //            result.Columns.Add(pi.Name, pi.PropertyType);
        //        }
        //        foreach (object t in list)
        //        {
        //            ArrayList tempList = new ArrayList();
        //            foreach (PropertyInfo pi in propertys)
        //            {
        //                object obj = pi.GetValue(t, null);
        //                tempList.Add(obj);
        //            }
        //            object[] array = tempList.ToArray();
        //            result.LoadDataRow(array, true);
        //        }
        //    }
        //    return result;
        //}

        /// <summary>  
        /// 将泛型集合类转换成DataTable  
        /// </summary>  
        /// <typeparam name="T">集合项类型</typeparam>  
        /// <param name="list">集合</param>  
        /// <returns>数据集(表)</returns>  
        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            return ToDataTable(list, null);
        }

        /// <summary>  
        /// 将泛型集合类转换成DataTable  
        /// </summary>  
        /// <typeparam name="T">集合项类型</typeparam>  
        /// <param name="list">集合</param>  
        /// <param name="propertyName">需要返回的列的列名</param>  
        /// <returns>数据集(表)</returns>  
        public static DataTable ToDataTable<T>(this IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);
            DataTable result = new DataTable();
            if (list == null || list.Count == 0)
                return result;
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
    }

}
