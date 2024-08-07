﻿using System;
using System.Collections;
using System.Collections.Generic;
using PrefsGUI.Utility;

namespace PrefsGUI
{
    /// <summary>
    /// List style PrefsGUI
    /// </summary>
    [Serializable]
    public class PrefsDictionary<TKey, TValue> : PrefsAny<SerializableDictionary<TKey, TValue>>, IDictionary<TKey, TValue>
    {
        public PrefsDictionary(string key) : base(key, new SerializableDictionary<TKey, TValue>())
        {
        }

        public PrefsDictionary(string key, IDictionary<TKey, TValue> defaultValue ) : base(key, new SerializableDictionary<TKey, TValue>(defaultValue))
        {
        }
        
        protected void UpdateValue(Action<SerializableDictionary<TKey, TValue>> action)
        {
            var value = Get();
            action(value);
            Set(value);
        }
        
        protected bool UpdateValueIfSuccess(Func<SerializableDictionary<TKey, TValue>, bool> func)
        {
            var value = Get();
            var success = func(value);
            {
                Set(value);
            }

            return success;
        }
        
        
        #region Dictionary Methods

        public bool ContainsValue(TValue value) => Get().ContainsValue(value);
        
        public bool TryAdd(TKey dictionaryKey, TValue value) => UpdateValueIfSuccess(d => d.TryAdd(dictionaryKey, value));
        
        #endregion
        
        
        #region IEnumerable
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        #endregion

        
        #region IEnumerable<KeyValuePair<TKey, TValue>>,

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Get().GetEnumerator();

        #endregion


        #region ICollection<KeyValuePair<TKey, TValue>>
        
        public int Count => Get().Count;
        
        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)Get()).IsReadOnly;

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        public void Clear() => UpdateValue(d => d.Clear());
        
        public bool Contains(KeyValuePair<TKey, TValue> item) => Get().Contains(item);
        
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)Get()).CopyTo(array, arrayIndex);
        }

        public bool Remove(TKey dictionaryKey) => UpdateValueIfSuccess(d => d.Remove(dictionaryKey));
        
        #endregion


        
        #region IDictionary<TKey, TValue>

        public TValue this[TKey dictionaryKey]
        {
            get => Get()[dictionaryKey];
            set => UpdateValue(d => d[dictionaryKey] = value);
        }

        public ICollection<TKey> Keys => Get().Keys;

        public ICollection<TValue> Values => Get().Values;

        public void Add(TKey dictionaryKey, TValue value) => UpdateValue(d => d.Add(dictionaryKey, value));
        
        public bool ContainsKey(TKey dictionaryKey) => Get().ContainsKey(dictionaryKey);
        
        public bool Remove(KeyValuePair<TKey, TValue> item) => UpdateValueIfSuccess(d => ((ICollection<KeyValuePair<TKey, TValue>>)d).Remove(item));
        
        public bool TryGetValue(TKey dictionaryKey, out TValue value) => Get().TryGetValue(dictionaryKey, out value);

        #endregion
    }
}