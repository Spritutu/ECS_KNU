using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Scanlab
{
    public sealed class MemoryDB : IDisposable
    {
        private IMemory memory;
        private string dbFileName;
        private uint maxDataSize;
        private SQLiteConnection connFile;
        private bool disposed;

        /// <summary>메모리 데이터 베이스 생성자</summary>
        /// <param name="memory">메모리 인터페이스 객체</param>
        /// <param name="dbFileName">데이터베이스 파일 이름 (예:test.db)</param>
        /// <param name="maxDataSize">데이타(Json 변환포맷) 최대 크기 (bytes)</param>
        public MemoryDB(IMemory memory, string dbFileName, uint maxDataSize = 1024)
        {
            this.memory = memory;
            this.dbFileName = dbFileName;
            this.maxDataSize = maxDataSize;
        }

        /// <summary>종결자</summary>
        ~MemoryDB()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
        }

        /// <summary>자원 해제</summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
            {
                this.memory.PropertyChanged -= new PropertyChangedEventHandler(this.Memory_PropertyChanged);
                ((DbConnection)this.connFile)?.Close();
                this.connFile?.Dispose();
            }
            this.disposed = true;
        }

        /// <summary>초기화 및 동기화</summary>
        private bool Initialize()
        {
            if (!File.Exists(this.dbFileName))
            {
                SQLiteConnection.CreateFile(this.dbFileName);
                Logger.Log(Logger.Type.Warn, "Database file has been created by automatically: " + this.memory.Name, Array.Empty<object>());
            }
            try
            {
                this.connFile = new SQLiteConnection("Data Source=" + this.dbFileName + ";Version=3;");
                ((DbConnection)this.connFile).Open();
                this.PrepareTable();
                this.ReverseSync();
                this.ForwardSync();
                this.memory.PropertyChanged += new PropertyChangedEventHandler(this.Memory_PropertyChanged);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Type.Fatal, (object)ex);
                return false;
            }
        }

        /// <summary>데이터 베이스 테이블 준비 (없으면 생성)</summary>
        /// <returns></returns>
        private bool PrepareTable()
        {
            try
            {
                using (SQLiteCommand sqLiteCommand = new SQLiteCommand("CREATE TABLE IF NOT EXISTS [Memory]( [Idx] INTEGER PRIMARY KEY ASC AUTOINCREMENT NOT NULL, [Name] CHAR(64) NOT NULL, " + string.Format("[Value] VARCHAR({0}))", (object)this.maxDataSize), this.connFile))
                    ((DbCommand)sqLiteCommand).ExecuteNonQuery();
                using (SQLiteCommand sqLiteCommand = new SQLiteCommand("CREATE INDEX IF NOT EXISTS [IdxName] ON [Memory]([Name])", this.connFile))
                    ((DbCommand)sqLiteCommand).ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Type.Fatal, (object)ex);
                return false;
            }
        }

        /// <summary>DB 에 저장되어 있는 모든 속성 데이타를 실제 메모리 객체에 반영(동기화)</summary>
        /// <returns></returns>
        private int ReverseSync()
        {
            int num = 0;
            System.Type type = this.memory.GetType();
            foreach (PropertyInfo property1 in type.GetProperties())
            {
                PropertyInfo property2 = type.GetProperty(property1.Name);
                System.Type propertyType = property2.PropertyType;
                if (!Attribute.IsDefined((MemberInfo)property2, typeof(JsonIgnoreAttribute)))
                {
                    try
                    {
                        using (SQLiteCommand sqLiteCommand = new SQLiteCommand("SELECT COUNT(*) FROM [Memory] WHERE [Name] = '" + property1.Name + "'", this.connFile))
                        {
                            if (1L != (long)((DbCommand)sqLiteCommand).ExecuteScalar())
                            {
                                Logger.Log(Logger.Type.Error, property1.Name + " is not exist in " + this.dbFileName, Array.Empty<object>());
                                continue;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(Logger.Type.Fatal, (object)ex);
                        break;
                    }
                    using (SQLiteCommand sqLiteCommand = new SQLiteCommand("SELECT [Value] FROM [Memory] WHERE [Name] = '" + property1.Name + "'", this.connFile))
                    {
                        using (SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader())
                        {
                            if (((DbDataReader)sqLiteDataReader).RecordsAffected > 0)
                            {
                                if (((DbDataReader)sqLiteDataReader).Read())
                                {
                                    string str = ((DbDataReader)sqLiteDataReader).GetString(0);
                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        /*
                                        object obj1 = (object)JsonConvert.DeserializeObject<object>(str);


                                        // ISSUE: reference to a compiler-generated field
                                        if (MemoryDB..// \u003Eo__11.\u003C\u003Ep__1 == null)
                    { 
                                            // ISSUE: reference to a compiler-generated field
                                            MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(MemoryDB), (IEnumerable<CSharpArgumentInfo>)new CSharpArgumentInfo[1]
                                            {
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                                            }));
                                        }
                                        // ISSUE: reference to a compiler-generated field
                                        Func<CallSite, object, bool> target = MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__1.Target;
                                        // ISSUE: reference to a compiler-generated field
                                        CallSite<Func<CallSite, object, bool>> p1 = MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__1;
                                        // ISSUE: reference to a compiler-generated field
                                        if (MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__0 == null)
                    {
                                            // ISSUE: reference to a compiler-generated field
                                            MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(MemoryDB), (IEnumerable<CSharpArgumentInfo>)new CSharpArgumentInfo[2]
                                            {
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null),
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                                            }));
                                        }
                                        // ISSUE: reference to a compiler-generated field
                                        // ISSUE: reference to a compiler-generated field
                                        object obj2 = MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__0.Target((CallSite)MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__0, (object)null, obj1);
                                        if (!target((CallSite)p1, obj2))
                                        {
                                            // ISSUE: reference to a compiler-generated field
                                            if (MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__2 == null)
                      {
                                                // ISSUE: reference to a compiler-generated field
                                                MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__2 = CallSite<Func<CallSite, System.Type, object, System.Type, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ChangeType", (IEnumerable<System.Type>)null, typeof(MemoryDB), (IEnumerable<CSharpArgumentInfo>)new CSharpArgumentInfo[3]
                                                {
                          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
                                                }));
                                            }
                                            // ISSUE: reference to a compiler-generated field
                                            // ISSUE: reference to a compiler-generated field
                                            object obj3 = MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__2.Target((CallSite)MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__2, typeof(Convert), obj1, propertyType);
                                            // ISSUE: reference to a compiler-generated field
                                            if (MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__3 == null)
                      {
                                                // ISSUE: reference to a compiler-generated field
                                                MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__3 = CallSite<Action<CallSite, PropertyInfo, IMemory, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SetValue", (IEnumerable<System.Type>)null, typeof(MemoryDB), (IEnumerable<CSharpArgumentInfo>)new CSharpArgumentInfo[3]
                                                {
                          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                                                }));
                                            }
                                            // ISSUE: reference to a compiler-generated field
                                            // ISSUE: reference to a compiler-generated field
                                            MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__3.Target((CallSite)MemoryDB.\u003C\u003Eo__11.\u003C\u003Ep__3, property2, this.memory, obj3);
                                            ++num;
                                        }*/
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return num;
        }

        /// <summary>속성들을 모두 조회하여 DB 에 없으면 삽입한다 (동기화)</summary>
        /// <returns></returns>
        private void ForwardSync()
        {
            System.Type type = this.memory.GetType();
            foreach (PropertyInfo property1 in type.GetProperties())
            {
                PropertyInfo property2 = type.GetProperty(property1.Name);
                System.Type propertyType = property2.PropertyType;
                if (!Attribute.IsDefined((MemberInfo)property2, typeof(JsonIgnoreAttribute)))
                {
                    try
                    {
                        using (SQLiteCommand sqLiteCommand1 = new SQLiteCommand("SELECT COUNT(*) FROM [Memory] WHERE [Name] = '" + property1.Name + "'", this.connFile))
                        {
                            if (1L != (long)((DbCommand)sqLiteCommand1).ExecuteScalar())
                            {
                                using (SQLiteCommand sqLiteCommand2 = new SQLiteCommand("INSERT INTO[Memory] ([Name]) VALUES('" + property2.Name + "')", this.connFile))
                                    ((DbCommand)sqLiteCommand2).ExecuteNonQueryAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(Logger.Type.Fatal, (object)ex);
                        break;
                    }
                }
            }
        }

        private void Memory_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyInfo property = this.memory.GetType().GetProperty(e.PropertyName);
            if (Attribute.IsDefined((MemberInfo)property, typeof(JsonIgnoreAttribute)))
                return;
            this.Update(ref property);
        }

        /// <summary>속성 값이 변경될 경우 DB 에 업데이트</summary>
        /// <param name="prop">PropertyInfo 객체</param>
        /// <returns></returns>
        private bool Update(ref PropertyInfo prop)
        {
            try
            {
                string str = string.Empty;
                lock (this.memory.SyncRoot)
                    str = JsonConvert.SerializeObject(prop.GetValue((object)this.memory), (Formatting)1);
                using (SQLiteCommand sqLiteCommand = new SQLiteCommand("UPDATE [Memory] SET [Value] = '" + str + "' WHERE [Name] = '" + prop.Name + "'", this.connFile))
                    ((DbCommand)sqLiteCommand).ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Type.Fatal, (object)ex);
                return false;
            }
        }
    }
}
