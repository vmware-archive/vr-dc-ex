using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vapitypes
{
    [System.Serializable]
    public class Backing
    {
        public bool auto_detect;
        public string device_access_type;
        public string type;
        public string host_device;
    }

    [System.Serializable]
    public class Ide
    {
        public bool primary;
        public bool master;
    }

    [System.Serializable]
    public class Value2
    {
        public bool start_connected;
        public Backing backing;
        public bool allow_guest_control;
        public string label;
        public Ide ide;
        public string state;
        public string type;
    }

    [System.Serializable]
    public class Cdrom
    {
        public Value2 value;
        public string key;
    }

    [System.Serializable]
    public class Memory
    {
        public int hot_add_increment_size_MiB;
        public int size_MiB;
        public bool hot_add_enabled;
        public int hot_add_limit_MiB;
    }

    [System.Serializable]
    public class Scsi
    {
        public int bus;
        public int unit;
    }

    [System.Serializable]
    public class Backing2
    {
        public string vmdk_file;
        public string type;
    }

    [System.Serializable]
    public class Value3
    {
        public Scsi scsi;
        public Backing2 backing;
        public string label;
        public string type;
        public long capacity;
    }

    [System.Serializable]
    public class Disk
    {
        public Value3 value;
        public string key;
    }

    [System.Serializable]
    public class Cpu
    {
        public bool hot_remove_enabled;
        public int count;
        public bool hot_add_enabled;
        public int cores_per_socket;
    }

    [System.Serializable]
    public class Scsi2
    {
        public int bus;
        public int unit;
    }

    [System.Serializable]
    public class Value4
    {
        public Scsi2 scsi;
        public int pci_slot_number;
        public string label;
        public string type;
        public string sharing;
    }

    [System.Serializable]
    public class ScsiAdapter
    {
        public Value4 value;
        public string key;
    }

    [System.Serializable]
    public class Backing3
    {
        public string network_name;
        public string type;
        public string network;
    }

    [System.Serializable]
    public class Value5
    {
        public bool start_connected;
        public Backing3 backing;
        public string mac_address;
        public string mac_type;
        public bool allow_guest_control;
        public bool wake_on_lan_enabled;
        public string label;
        public string state;
        public string type;
        public bool upt_compatibility_enabled;
    }

    [System.Serializable]
    public class Nic
    {
        public Value5 value;
        public string key;
    }

    [System.Serializable]
    public class Boot
    {
        public int delay;
        public int retry_delay;
        public bool enter_setup_mode;
        public string type;
        public bool retry;
    }

    [System.Serializable]
    public class Hardware
    {
        public string upgrade_policy;
        public string upgrade_status;
        public string version;
    }

    [System.Serializable]
    public class Value
    {
        public List<Cdrom> cdroms;
        public Memory memory;
        public List<Disk> disks;
        public List<object> parallel_ports;
        public List<object> sata_adapters;
        public Cpu cpu;
        public List<ScsiAdapter> scsi_adapters;
        public string power_state;
        public List<object> floppies;
        public string name;
        public List<Nic> nics;
        public Boot boot;
        public List<object> serial_ports;
        public string guest_OS;
        public List<object> boot_devices;
        public Hardware hardware;
    }

    [System.Serializable]
    public class VmDetails
    {
        public Value value;
    }

}