// Copyright ©2017 VMware, Inc. All Rights Reserved.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Security;
//using System.Web;
using System.Security.Cryptography.X509Certificates;
using vapitypes;

[System.Serializable]
public class VmList
{
    public List<Summary> value;
}

[System.Serializable]
public class Summary
{

    public string vm;
    public string name;
    public string powerState;
    public long? cpuCount;
    public long? memorySizeMiB;
}

public class RESTConnection : MonoBehaviour
{
	public String cookie;

	String username;
	String password;
	String hostUrl;
	GameObject ConnectionMsg;
	void Authenticate()
    {       
		INIParser ini = new INIParser ();
		if (System.IO.File.Exists (Application.persistentDataPath + "/config.ini")) {
			ini.Open (Application.persistentDataPath + "/config.ini");
			username = ini.ReadValue ("vCenter", "username", "administrator@vsphere.local");
			password = ini.ReadValue ("vCenter", "password", "VMware1!");
			hostUrl = ini.ReadValue ("vCenter", "hosturl", "https://localhost:8082");
			ConnectionMsg = GameObject.Find("Connection_MSG_Text");
			ConnectionMsg.GetComponentInChildren<Text>().text = "Connection data read from: " + Application.persistentDataPath + "/config.ini \n\nvCenter:" + hostUrl +"\n\nUser:" + username;
		} else {
			ini.Open (Application.persistentDataPath + "/config.ini");
			ini.WriteValue("vCenter", "username", "administrator@vsphere.local");
			ini.WriteValue("vCenter", "password", "VMware1!");
			ini.WriteValue("vCenter", "hosturl", "https://VC01");
			username = ini.ReadValue ("vCenter", "username", "administrator@vsphere.local");
			password = ini.ReadValue ("vCenter", "password", "VMware1!");
			hostUrl = ini.ReadValue ("vCenter", "hosturl", "https://localhost:8082");
		}
		ini.Close();

		HttpWebRequest request = CreatePostRequest("/rest/com/vmware/cis/session");
        String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
        request.Headers.Add("Authorization", "Basic " + encoded);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        if ((int)response.StatusCode == 200)
        {
            cookie = response.Headers["Set-Cookie"];
            cookie = cookie.Substring(cookie.IndexOf('=') + 1);
            cookie = cookie.Substring(0, cookie.IndexOf(';'));
        }
    }

    private string GetDocumentContents(HttpWebResponse response)
    {
        using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
        {
            return reader.ReadToEnd();
        }
    }

    HttpWebRequest CreatePostRequest(String path) {
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(hostUrl + path);
		request.ContentType = "application/x-www-form-urlencoded";
		request.Method = "POST";
        if (!String.IsNullOrEmpty(cookie))
        {
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Uri(hostUrl), new Cookie("vmware-api-session-id", cookie));
        }
		return request;
	}

    HttpWebResponse fetch(String path) {
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(hostUrl + path);
		//request.ContentType = "application/x-www-form-urlencoded";
		request.Method = "GET";
		request.CookieContainer = new CookieContainer();
		request.CookieContainer.Add(new Uri(hostUrl), new Cookie("vmware-api-session-id", cookie));
		return (HttpWebResponse)request.GetResponse();
	}

    public VmList ListVms() {
        HttpWebResponse response = fetch("/rest/vcenter/vm");
        if ((int)response.StatusCode == 200)
        {
            String body = GetDocumentContents(response);
            VmList list = JsonUtility.FromJson<VmList>(body);
            return list;
       }
        return null;
    }

    [System.Serializable]
    public class Host
    {
        public string host;
        public string name;
        public string connection_state;
        public string power_state;
    }

    [System.Serializable]
    public class HostList
    {
        public List<Host> value;
    }

    public VmList ListVmsByHostId(string hostId)
    {
        HttpWebResponse response = fetch("/rest/vcenter/vm?filter.hosts.1=" + hostId);
        if ((int)response.StatusCode == 200)
        {
            String body = GetDocumentContents(response);
            VmList list = JsonUtility.FromJson<VmList>(body);
            return list;
        }
        return null;
    }
    public HostList ListHosts()
    {
        HttpWebResponse response = fetch("/rest/vcenter/host");
        if ((int)response.StatusCode == 200)
        {
            String body = GetDocumentContents(response);
            HostList list = JsonUtility.FromJson<HostList>(body);
            return list;
        }
        return null;
    }

	HttpWebResponse Power(String vmId, bool Start)
	{
        String power = Start ? "start" : "stop";
        HttpWebRequest request = CreatePostRequest("/rest/vcenter/vm" + vmId + "/power/" + power);
        return (HttpWebResponse)request.GetResponse();
	}

    public VmDetails GetVmDetails(String vmId)
    {
		HttpWebResponse response = fetch("/rest/vcenter/vm/" + vmId);
		if ((int)response.StatusCode == 200)
		{
			String body = GetDocumentContents(response);
			VmDetails details = JsonUtility.FromJson<VmDetails>(body);
            return details;
		}
		return null;
	}

    public String GetVmDetailsStr(VmDetails details) {
        long capacity = 0;
        List<Disk> disks = details.value.disks;
        foreach (Disk disk in disks) {
            capacity += (long)disk.value.capacity;
        }
        capacity = capacity / (1024 * 1024 * 1024);
        string capacityStr = capacity.ToString() + " GB";
        string networkNames = "\n";
        List<Nic> nics = details.value.nics;
        foreach (Nic nic in nics)
        {
            networkNames += "    " + nic.value.backing.network_name + "\n";
        }

        networkNames = networkNames.Substring(0, networkNames.Length - 1);
        string result = String.Format("CPU: {0}\nOS: {1}\nMemory: {2}\nStorage: {3}\nNetwork: {4}\nPower: {5}\nHardware: {6}\n", details.value.cpu.count, details.value.guest_OS, details.value.memory.size_MiB/1024 + " GB", capacityStr, networkNames, details.value.power_state, details.value.hardware.version);

        return result;
    }

	public String GetNumCPUStr(VmDetails details) {
		string result = String.Format ("{0}", details.value.cpu.count);

		return result;
	}

	public String GetMEMStr(VmDetails details) {
		string result = String.Format ("{0} GB", details.value.memory.size_MiB/1024);

		return result;
	}

	public String GetOSStr(VmDetails details) {
		string result = String.Format ("{0}", details.value.guest_OS);

		return result;
	}

	public String GetPowerStr(VmDetails details) {
		string result = String.Format ("{0}", details.value.power_state);

		return result;
	}

	// Use this for initialization
	public void Start()
	{
      
		ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;
        System.Net.ServicePointManager.SecurityProtocol |=
                System.Net.SecurityProtocolType.Tls;
        Authenticate();

	}

	private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
	{
		// all Certificates are accepted
		return true;
	}

	void Update()
	{

	}
}
