﻿namespace Intel.UPNP
{
    using Intel.Utilities;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal sealed class UPnPInternalSmartControlPoint
    {
        private ArrayList activeDeviceList = ArrayList.Synchronized(new ArrayList());
        private UPnPDeviceFactory deviceFactory = new UPnPDeviceFactory();
        private LifeTimeMonitor deviceLifeTimeClock = new LifeTimeMonitor();
        private Hashtable deviceTable = new Hashtable();
        private object deviceTableLock = new object();
        private LifeTimeMonitor deviceUpdateClock = new LifeTimeMonitor();
        private UPnPControlPoint genericControlPoint;
        private NetworkInfo hostNetworkInfo;
        private WeakEvent OnAddedDeviceEvent = new WeakEvent();
        private WeakEvent OnDeviceExpiredEvent = new WeakEvent();
        private WeakEvent OnRemovedDeviceEvent = new WeakEvent();
        private WeakEvent OnUpdatedDeviceEvent = new WeakEvent();

        public event DeviceHandler OnAddedDevice
        {
            add
            {
                this.OnAddedDeviceEvent.Register(value);
            }
            remove
            {
                this.OnAddedDeviceEvent.UnRegister(value);
            }
        }

        public event DeviceHandler OnDeviceExpired
        {
            add
            {
                this.OnDeviceExpiredEvent.Register(value);
            }
            remove
            {
                this.OnDeviceExpiredEvent.UnRegister(value);
            }
        }

        public event DeviceHandler OnRemovedDevice
        {
            add
            {
                this.OnRemovedDeviceEvent.Register(value);
            }
            remove
            {
                this.OnRemovedDeviceEvent.UnRegister(value);
            }
        }

        public event DeviceHandler OnUpdatedDevice
        {
            add
            {
                this.OnUpdatedDeviceEvent.Register(value);
            }
            remove
            {
                this.OnUpdatedDeviceEvent.UnRegister(value);
            }
        }

        public UPnPInternalSmartControlPoint()
        {
            this.deviceFactory.OnDevice += new UPnPDeviceFactory.UPnPDeviceHandler(this.DeviceFactoryCreationSink);
            this.deviceLifeTimeClock.OnExpired += new LifeTimeMonitor.LifeTimeHandler(this.DeviceLifeTimeClockSink);
            this.deviceUpdateClock.OnExpired += new LifeTimeMonitor.LifeTimeHandler(this.DeviceUpdateClockSink);
            this.hostNetworkInfo = new NetworkInfo(new NetworkInfo.InterfaceHandler(this.NetworkInfoNewInterfaceSink));
            this.hostNetworkInfo.OnInterfaceDisabled += new NetworkInfo.InterfaceHandler(this.NetworkInfoOldInterfaceSink);
            this.genericControlPoint = new UPnPControlPoint(this.hostNetworkInfo);
            this.genericControlPoint.OnSearch += new UPnPControlPoint.SearchHandler(this.UPnPControlPointSearchSink);
            this.genericControlPoint.OnNotify += new SSDP.NotifyHandler(this.SSDPNotifySink);
            this.genericControlPoint.FindDeviceAsync("upnp:rootdevice");
        }

        private void DeviceFactoryCreationSink(UPnPDeviceFactory sender, UPnPDevice device, Uri locationURL)
        {
            if (!this.deviceTable.Contains(device.UniqueDeviceName))
            {
                EventLogger.Log(this, EventLogEntryType.Error, "UPnPDevice[" + device.FriendlyName + "]@" + device.LocationURL + " advertised UDN[" + device.UniqueDeviceName + "] in xml but not in SSDP");
            }
            else
            {
                lock (this.deviceTableLock)
                {
                    DeviceInfo info2 = (DeviceInfo) this.deviceTable[device.UniqueDeviceName];
                    if (info2.Device != null)
                    {
                        EventLogger.Log(this, EventLogEntryType.Information, "Unexpected UPnP Device Creation: " + device.FriendlyName + "@" + device.LocationURL);
                        return;
                    }
                    DeviceInfo info = (DeviceInfo) this.deviceTable[device.UniqueDeviceName];
                    info.Device = device;
                    this.deviceTable[device.UniqueDeviceName] = info;
                    this.deviceLifeTimeClock.Add(device.UniqueDeviceName, device.ExpirationTimeout);
                    this.activeDeviceList.Add(device);
                }
                this.OnAddedDeviceEvent.Fire(this, device);
            }
        }

        private void DeviceLifeTimeClockSink(LifeTimeMonitor sender, object obj)
        {
            DeviceInfo info;
            lock (this.deviceTableLock)
            {
                if (!this.deviceTable.ContainsKey(obj))
                {
                    return;
                }
                info = (DeviceInfo) this.deviceTable[obj];
                this.deviceTable.Remove(obj);
                this.deviceUpdateClock.Remove(obj);
                if (this.activeDeviceList.Contains(info.Device))
                {
                    this.activeDeviceList.Remove(info.Device);
                }
                else
                {
                    info.Device = null;
                }
            }
            if (info.Device != null)
            {
                info.Device.Removed();
            }
            if (info.Device != null)
            {
                info.Device.Removed();
                this.OnDeviceExpiredEvent.Fire(this, info.Device);
            }
        }

        private void DeviceUpdateClockSink(LifeTimeMonitor sender, object obj)
        {
            lock (this.deviceTableLock)
            {
                if (this.deviceTable.ContainsKey(obj))
                {
                    DeviceInfo info = (DeviceInfo) this.deviceTable[obj];
                    if (info.PendingBaseURL != null)
                    {
                        info.BaseURL = info.PendingBaseURL;
                        info.MaxAge = info.PendingMaxAge;
                        info.SourceEP = info.PendingSourceEP;
                        info.LocalEP = info.PendingLocalEP;
                        info.NotifyTime = DateTime.Now;
                        info.Device.UpdateDevice(info.BaseURL, info.LocalEP.Address);
                        this.deviceTable[obj] = info;
                        this.deviceLifeTimeClock.Add(info.UDN, info.MaxAge);
                    }
                }
            }
        }

        public UPnPDevice[] GetCurrentDevices()
        {
            return (UPnPDevice[]) this.activeDeviceList.ToArray(typeof(UPnPDevice));
        }

        private void NetworkInfoNewInterfaceSink(NetworkInfo sender, IPAddress Intfce)
        {
            if (this.genericControlPoint != null)
            {
                this.genericControlPoint.FindDeviceAsync("upnp:rootdevice");
            }
        }

        private void NetworkInfoOldInterfaceSink(NetworkInfo sender, IPAddress Intfce)
        {
            ArrayList list = new ArrayList();
            lock (this.deviceTableLock)
            {
                foreach (UPnPDevice device in this.GetCurrentDevices())
                {
                    if (device.InterfaceToHost.Equals(Intfce))
                    {
                        list.Add(this.UnprotectedRemoveMe(device));
                    }
                }
            }
            foreach (UPnPDevice device2 in list)
            {
                device2.Removed();
                this.OnRemovedDeviceEvent.Fire(this, device2);
            }
            this.genericControlPoint.FindDeviceAsync("upnp:rootdevice");
        }

        internal void RemoveMe(UPnPDevice _d)
        {
            UPnPDevice parentDevice = _d;
            UPnPDevice device2 = null;
            while (parentDevice.ParentDevice != null)
            {
                parentDevice = parentDevice.ParentDevice;
            }
            lock (this.deviceTableLock)
            {
                if (!this.deviceTable.ContainsKey(parentDevice.UniqueDeviceName))
                {
                    return;
                }
                device2 = this.UnprotectedRemoveMe(parentDevice);
            }
            if (device2 != null)
            {
                device2.Removed();
            }
            if (device2 != null)
            {
                this.OnRemovedDeviceEvent.Fire(this, device2);
            }
        }

        public void Rescan()
        {
            lock (this.deviceTableLock)
            {
                IDictionaryEnumerator enumerator = this.deviceTable.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string key = (string) enumerator.Key;
                    this.deviceLifeTimeClock.Add(key, 20);
                }
            }
            this.genericControlPoint.FindDeviceAsync("upnp:rootdevice");
        }

        internal void SSDPNotifySink(IPEndPoint source, IPEndPoint local, Uri LocationURL, bool IsAlive, string USN, string SearchTarget, int MaxAge, HTTPMessage Packet)
        {
            UPnPDevice device = null;
            if (SearchTarget == "upnp:rootdevice")
            {
                if (!IsAlive)
                {
                    lock (this.deviceTableLock)
                    {
                        device = this.UnprotectedRemoveMe(USN);
                    }
                    if (device != null)
                    {
                        device.Removed();
                    }
                    if (device != null)
                    {
                        this.OnRemovedDeviceEvent.Fire(this, device);
                    }
                }
                else
                {
                    lock (this.deviceTableLock)
                    {
                        if (!this.deviceTable.ContainsKey(USN))
                        {
                            DeviceInfo info = new DeviceInfo();
                            info.Device = null;
                            info.UDN = USN;
                            info.NotifyTime = DateTime.Now;
                            info.BaseURL = LocationURL;
                            info.MaxAge = MaxAge;
                            info.LocalEP = local;
                            info.SourceEP = source;
                            this.deviceTable[USN] = info;
                            this.deviceFactory.CreateDevice(info.BaseURL, info.MaxAge);
                        }
                        else
                        {
                            DeviceInfo info2 = (DeviceInfo) this.deviceTable[USN];
                            if (info2.Device != null)
                            {
                                if (info2.BaseURL.Equals(LocationURL))
                                {
                                    this.deviceUpdateClock.Remove(info2);
                                    info2.PendingBaseURL = null;
                                    info2.PendingMaxAge = 0;
                                    info2.PendingLocalEP = null;
                                    info2.PendingSourceEP = null;
                                    info2.NotifyTime = DateTime.Now;
                                    this.deviceTable[USN] = info2;
                                    this.deviceLifeTimeClock.Add(info2.UDN, MaxAge);
                                }
                                else if (info2.NotifyTime.AddSeconds(10.0).Ticks < DateTime.Now.Ticks)
                                {
                                    info2.PendingBaseURL = LocationURL;
                                    info2.PendingMaxAge = MaxAge;
                                    info2.PendingLocalEP = local;
                                    info2.PendingSourceEP = source;
                                    this.deviceTable[USN] = info2;
                                    this.deviceUpdateClock.Add(info2.UDN, 3);
                                }
                            }
                        }
                    }
                }
            }
        }

        internal UPnPDevice UnprotectedRemoveMe(UPnPDevice _d)
        {
            UPnPDevice parentDevice = _d;
            while (parentDevice.ParentDevice != null)
            {
                parentDevice = parentDevice.ParentDevice;
            }
            return this.UnprotectedRemoveMe(parentDevice.UniqueDeviceName);
        }

        internal UPnPDevice UnprotectedRemoveMe(string UDN)
        {
            UPnPDevice device = null;
            DeviceInfo info = (DeviceInfo) this.deviceTable[UDN];
            device = info.Device;
            this.deviceTable.Remove(UDN);
            this.deviceLifeTimeClock.Remove(info.UDN);
            this.deviceUpdateClock.Remove(info);
            this.activeDeviceList.Remove(device);
            return device;
        }

        private void UPnPControlPointSearchSink(IPEndPoint source, IPEndPoint local, Uri LocationURL, string USN, string SearchTarget, int MaxAge)
        {
            lock (this.deviceTableLock)
            {
                if (!this.deviceTable.ContainsKey(USN))
                {
                    DeviceInfo info = new DeviceInfo();
                    info.Device = null;
                    info.UDN = USN;
                    info.NotifyTime = DateTime.Now;
                    info.BaseURL = LocationURL;
                    info.MaxAge = MaxAge;
                    info.LocalEP = local;
                    info.SourceEP = source;
                    this.deviceTable[USN] = info;
                    this.deviceFactory.CreateDevice(info.BaseURL, info.MaxAge);
                }
                else
                {
                    DeviceInfo info2 = (DeviceInfo) this.deviceTable[USN];
                    if (info2.Device != null)
                    {
                        if (info2.BaseURL.Equals(LocationURL))
                        {
                            this.deviceUpdateClock.Remove(info2);
                            info2.PendingBaseURL = null;
                            info2.PendingMaxAge = 0;
                            info2.PendingLocalEP = null;
                            info2.PendingSourceEP = null;
                            info2.NotifyTime = DateTime.Now;
                            this.deviceTable[USN] = info2;
                            this.deviceLifeTimeClock.Add(info2.UDN, MaxAge);
                        }
                        else if (info2.NotifyTime.AddSeconds(10.0).Ticks < DateTime.Now.Ticks)
                        {
                            info2.PendingBaseURL = LocationURL;
                            info2.PendingMaxAge = MaxAge;
                            info2.PendingLocalEP = local;
                            info2.PendingSourceEP = source;
                            this.deviceUpdateClock.Add(info2.UDN, 3);
                        }
                    }
                }
            }
        }

        public delegate void DeviceHandler(UPnPInternalSmartControlPoint sender, UPnPDevice Device);

        [StructLayout(LayoutKind.Sequential)]
        private struct DeviceInfo
        {
            public UPnPDevice Device;
            public DateTime NotifyTime;
            public string UDN;
            public Uri BaseURL;
            public int MaxAge;
            public IPEndPoint LocalEP;
            public IPEndPoint SourceEP;
            public Uri PendingBaseURL;
            public int PendingMaxAge;
            public IPEndPoint PendingLocalEP;
            public IPEndPoint PendingSourceEP;
        }
    }
}

