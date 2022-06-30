using System.Collections.Generic;
using System.Linq;

namespace actchargers
{
    public class ConnectionManagerGenericList<T>
    {
        object lockKey = new object();
        Dictionary<string, T> list;
        public ConnectionManagerGenericList()
        {
            list = new Dictionary<string, T>();
        }
        public int getListCount()
        {
            lock (lockKey)
            {
                return list.Count;
            }
        }
        public string[] getListKeys()
        {
            lock (lockKey)
            {
                return list.Keys.ToArray();
            }
        }
        public bool keyExists(string sn)
        {
            lock (lockKey)
            {
                return list.ContainsKey(sn);
            }
        }
        public T getDeviceByKey(string sn)
        {
            lock (lockKey)
            {
                if (this.keyExists(sn))
                    return list[sn];
                return default(T);
            }
        }
        public void addDevice(string sn, T d)
        {
            lock (lockKey)
            {
                if (list.Keys.Contains(sn))
                    list[sn] = d;
                else
                    list.Add(sn, d);
            }
        }
        public bool removeDevice(string sn)
        {
            lock (lockKey)
            {
                if (list.Keys.Contains(sn))
                {
                    list.Remove(sn);
                    return true;
                }
                return false;
            }
        }


        public void assignListFromList(ConnectionManagerGenericList<T> anotherList)
        {
            lock (lockKey)
            {
                list = new Dictionary<string, T>(anotherList.list);
            }
        }

        public bool listCompare(ConnectionManagerGenericList<T> anotherList)
        {
            lock (anotherList.lockKey)
                lock (this.lockKey)
                {
                    if (this.list.Count != anotherList.list.Count)
                        return true;
                    foreach (string k in anotherList.list.Keys)
                    {
                        if (!this.list.ContainsKey(k))
                        {
                            return true;
                        }
                    }
                    foreach (string k in this.list.Keys)
                    {
                        if (!anotherList.list.ContainsKey(k))
                        {
                            return true;
                        }
                    }
                }
            return false;
        }
    }
}
