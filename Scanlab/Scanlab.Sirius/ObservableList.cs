
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 리스트 제네릭 컨테이너 + 추가 삭제에 대한 이벤트 통지 기능
    /// thread unsafe 컨테이너
    /// </summary>
    /// <typeparam name="T">데이타 타입</typeparam>
    public class ObservableList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        protected readonly List<T> list;
        protected bool enabled = true;

        /// <summary>추가전 이벤트 핸들러</summary>
        public event ObservableList<T>.BeforeAddItemEventHandler OnBeforeAddItem;

        /// <summary>추가후 이벤트 핸들러</summary>
        public event ObservableList<T>.AddItemEventHandler OnAddItem;

        /// <summary>삭제전 이벤트 핸들러</summary>
        public event ObservableList<T>.BeforeRemoveItemEventHandler OnBeforeRemoveItem;

        /// <summary>삭제후 이벤트 핸들러</summary>
        public event ObservableList<T>.RemoveItemEventHandler OnRemoveItem;

        /// <summary>동기화 객체</summary>
        [JsonIgnore]
        [Browsable(false)]
        public object SyncRoot { get; protected set; }

        protected virtual void AddItemEvent(int index, T item)
        {
            if (!this.enabled)
                return;
            ObservableList<T>.AddItemEventHandler onAddItem = this.OnAddItem;
            if (onAddItem == null)
                return;
            onAddItem(this, index, item);
        }

        protected virtual void BeforeAddItemEvent(int index, T item)
        {
            if (!this.enabled)
                return;
            ObservableList<T>.BeforeAddItemEventHandler onBeforeAddItem = this.OnBeforeAddItem;
            if (onBeforeAddItem == null)
                return;
            onBeforeAddItem(this, index, item);
        }

        protected virtual void BeforeRemoveItemEvent(int index, T item)
        {
            if (!this.enabled)
                return;
            ObservableList<T>.BeforeRemoveItemEventHandler beforeRemoveItem = this.OnBeforeRemoveItem;
            if (beforeRemoveItem == null)
                return;
            beforeRemoveItem(this, index, item);
        }

        protected virtual void RemoveItemEvent(int index, T item)
        {
            if (!this.enabled)
                return;
            ObservableList<T>.RemoveItemEventHandler onRemoveItem = this.OnRemoveItem;
            if (onRemoveItem == null)
                return;
            onRemoveItem(this, index, item);
        }

        /// <summary>
        /// Initializes a new instance of <c>ObservableCollection</c>.
        /// </summary>
        public ObservableList()
        {
            this.SyncRoot = new object();
            this.list = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of <c>ObservableCollection</c> and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The number of items the collection can initially store.</param>
        public ObservableList(int capacity)
          : this()
        {
            this.list = capacity >= 0 ? new List<T>(capacity) : throw new ArgumentOutOfRangeException(nameof(capacity), "The collection capacity cannot be negative.");
        }

        /// <summary>Gets or sets the object at the specified index.</summary>
        /// <param name="index"> The zero-based index of the element to get or set.</param>
        /// <returns>The object at the specified index.</returns>
        public virtual T this[int index]
        {
            get
            {
                lock (this.SyncRoot)
                    return this.list[index];
            }
            set
            {
                lock (this.SyncRoot)
                {
                    T obj1 = this.list[index];
                    T obj2 = value;
                    if (obj1.Equals((object)obj2))
                        return;
                    this.BeforeRemoveItemEvent(index, obj1);
                    this.BeforeAddItemEvent(index, obj2);
                    this.list[index] = value;
                    this.AddItemEvent(index, obj2);
                    this.RemoveItemEvent(index, obj1);
                }
            }
        }

        /// <summary>Gets the number of object contained in the collection.</summary>
        [Browsable(false)]
        [JsonIgnore]
        public virtual int Count
        {
            get
            {
                lock (this.SyncRoot)
                    return this.list.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public virtual bool IsReadOnly => false;

        [Browsable(false)]
        [JsonIgnore]
        public virtual bool EventEnabled
        {
            get => this.enabled;
            set => this.enabled = value;
        }

        /// <summary>Reverses the order of the elements in the entire list.</summary>
        public virtual void Reverse()
        {
            lock (this.SyncRoot)
                this.list.Reverse();
        }

        /// <summary>
        /// Sorts the elements in the entire System.Collections.Generic.List&lt;T&gt; using the specified System.Comparison&lt;T&gt;.
        /// </summary>
        /// <param name="comparision">The System.Comparison&lt;T&gt; to use when comparing elements.</param>
        public virtual void Sort(Comparison<T> comparision)
        {
            lock (this.SyncRoot)
                this.list.Sort(comparision);
        }

        /// <summary>
        /// Sorts the elements in a range of elements in System.Collections.Generic.List&lt;T&gt; using the specified comparer.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">The System.Collections.Generic.IComparer&lt;T&gt; implementation to use when comparing elements, or null to use the default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default.</param>
        public virtual void Sort(int index, int count, IComparer<T> comparer)
        {
            lock (this.SyncRoot)
                this.list.Sort(index, count, comparer);
        }

        /// <summary>
        /// Sorts the elements in a range of elements in System.Collections.Generic.List&lt;T&gt; using the specified comparer.
        /// </summary>
        /// <param name="comparer">The System.Collections.Generic.IComparer&lt;T&gt; implementation to use when comparing elements, or null to use the default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default.</param>
        public virtual void Sort(IComparer<T> comparer)
        {
            lock (this.SyncRoot)
                this.list.Sort(comparer);
        }

        /// <summary>
        /// Sorts the elements in the entire System.Collections.Generic.List&lt;T&gt; using the default comparer.
        /// </summary>
        public virtual void Sort()
        {
            lock (this.SyncRoot)
                this.list.Sort();
        }

        /// <summary>Adds an object to the collection.</summary>
        /// <param name="item"> The object to add to the collection.</param>
        /// <returns>True if the object has been added to the collection, or false otherwise.</returns>
        public virtual void Add(T item)
        {
            lock (this.SyncRoot)
            {
                int count = this.list.Count;
                this.BeforeAddItemEvent(count, item);
                this.list.Add(item);
                this.AddItemEvent(count, item);
            }
        }

        /// <summary>Adds an object list to the end of the collection.</summary>
        /// <param name="collection">The collection whose elements should be added.</param>
        public virtual void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            lock (this.SyncRoot)
            {
                foreach (T obj in collection)
                    this.Add(obj);
            }
        }

        /// <summary>
        /// Inserts an object into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert. The value can not be null.</param>
        /// <returns>True if the object has been inserted to the collection; otherwise, false.</returns>
        public virtual void Insert(int index, T item)
        {
            if (index < 0 || index > this.list.Count)
                throw new ArgumentOutOfRangeException(string.Format("The parameter index {0} must be in between {1} and {2}.", (object)index, (object)0, (object)this.list.Count));
            lock (this.SyncRoot)
            {
                this.list.Insert(index, item);
                this.AddItemEvent(index, item);
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection
        /// </summary>
        /// <param name="item">The object to remove from the collection.</param>
        /// <returns>True if object is successfully removed; otherwise, false.</returns>
        public virtual bool Remove(T item)
        {
            lock (this.SyncRoot)
            {
                if (!this.list.Contains(item))
                    return false;
                int index = this.list.IndexOf(item);
                this.BeforeRemoveItemEvent(index, item);
                this.list.Remove(item);
                this.RemoveItemEvent(index, item);
            }
            return true;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection
        /// </summary>
        /// <param name="items">The list of objects to remove from the collection.</param>
        /// <returns>True if object is successfully removed; otherwise, false.</returns>
        public virtual void Remove(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            List<int> intList = new List<int>();
            lock (this.SyncRoot)
            {
                foreach (T obj in items)
                    intList.Add(this.list.IndexOf(obj));
                int index = 0;
                foreach (T obj in items)
                {
                    if (!this.list.Contains(obj))
                    {
                        int num = index + 1;
                        break;
                    }
                    this.BeforeRemoveItemEvent(intList[index], obj);
                    this.list.Remove(obj);
                    this.RemoveItemEvent(intList[index], obj);
                    ++index;
                }
            }
        }

        /// <summary>
        /// Removes the object at the specified index of the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the object to remove.</param>
        public virtual void RemoveAt(int index)
        {
            if (index < 0 || index >= this.list.Count)
                throw new ArgumentOutOfRangeException(string.Format("The parameter index {0} must be in between {1} and {2}.", (object)index, (object)0, (object)this.list.Count));
            lock (this.SyncRoot)
            {
                T obj = this.list[index];
                this.BeforeRemoveItemEvent(index, obj);
                this.list.RemoveAt(index);
                this.RemoveItemEvent(index, obj);
            }
        }

        /// <summary>Removes all object from the collection.</summary>
        public virtual void Clear()
        {
            T[] array = new T[this.list.Count];
            lock (this.SyncRoot)
            {
                this.list.CopyTo(array, 0);
                foreach (T obj in array)
                    this.Remove(obj);
            }
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire collection.
        /// </summary>
        /// <param name="item">The object to locate in the collection.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire collection, if found; otherwise, –1.</returns>
        public virtual int IndexOf(T item)
        {
            lock (this.SyncRoot)
                return this.list.IndexOf(item);
        }

        /// <summary>Determines whether an object is in the collection.</summary>
        /// <param name="item">The object to locate in the collection.</param>
        /// <returns>True if item is found in the collection; otherwise, false.</returns>
        public virtual bool Contains(T item)
        {
            lock (this.SyncRoot)
                return this.list.Contains(item);
        }

        /// <summary>
        /// Copies the entire collection to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array"> The one-dimensional System.Array that is the destination of the elements copied from the collection. The System.Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            lock (this.SyncRoot)
                this.list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            lock (this.SyncRoot)
                return (IEnumerator<T>)this.list.GetEnumerator();
        }

        public virtual T[] ToArray()
        {
            lock (this.SyncRoot)
                return this.list.ToArray();
        }

        public virtual List<T> ToList()
        {
            lock (this.SyncRoot)
                return this.list;
        }

        void ICollection<T>.Add(T item) => this.Add(item);

        void IList<T>.Insert(int index, T item) => this.Insert(index, item);

        IEnumerator IEnumerable.GetEnumerator() => (IEnumerator)this.GetEnumerator();

        /// <summary>아이템 추가전 이벤트 핸들러 델리게이트</summary>
        /// <param name="sender"></param>
        /// <param name="index"></param>
        /// <param name="e"></param>
        public delegate void BeforeAddItemEventHandler(ObservableList<T> sender, int index, T e);

        /// <summary>아이템 추가 후 이벤트 핸들러 델리게이트</summary>
        /// <param name="sender"></param>
        /// <param name="index"></param>
        /// <param name="e"></param>
        public delegate void AddItemEventHandler(ObservableList<T> sender, int index, T e);

        /// <summary>아이템 삭제전 이벤트 핸들러 델리게이트</summary>
        /// <param name="sender"></param>
        /// <param name="index"></param>
        /// <param name="e"></param>
        public delegate void BeforeRemoveItemEventHandler(ObservableList<T> sender, int index, T e);

        /// <summary>아이템 삭제후 이벤트 핸들러 델리게이트</summary>
        /// <param name="sender"></param>
        /// <param name="index"></param>
        /// <param name="e"></param>
        public delegate void RemoveItemEventHandler(ObservableList<T> sender, int index, T e);
    }
}
