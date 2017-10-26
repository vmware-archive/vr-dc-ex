# VMware VR Datacenter Experience

![VRDCEX](VR-DC-EX_Logo.png "VRDCEX")

## Overview
Bring your datacenter to life (virtually), teleport to different locations, pick up your virtual machines to find out more information and drop them in the trash can to delete them.

## Demo
Check out this [video demo](https://www.youtube.com/watch?v=jOpsBClEuNs&feature=youtu.be&t=47m39s) where Alan Renouf (Sr. Product Line Manager) walks Pat Gelsinger (VMware CEO) through the VR Datacenter Experience.

## Stay in contact
Join [VMware Code](https://code.vmware.com/join) as well as find us in the [#VR Slack Channel](https://vmwarecode.slack.com/messages/VR) for any questions you may have. Come join the fun!

## Try it out

### Prerequisites

* [HTC Vive](https://www.vive.com/us/)
* [vCenter Server 6.5](https://my.vmware.com/web/vmware/info/slug/datacenter_cloud_infrastructure/vmware_vsphere/6_5) or greater
    * Ensure that you have at least one vSphere Cluster configured which contains at least one ESXi host
* A build of this [project](https://github.com/vmware/vr-dc-ex/releases)
* A config file will be created in **%userprofile%\appdata\locallow\vmware\vr datacenter experience\config.ini** the first time you run the app, edit this file to store your connection details to your vCenter 6.5 server.

### Build

1. Download and Install [Unity](https://unity3d.com/)
2. Clone this project
3. Open the project in Unity
4. Build and Run

## No test vCenter? Dont worry - use a mock endpoint
You can use the following instructions to run a mock vCenter 6.5 REST endpoint with some basic functionality, just enough to get this demo going with 4 hosts and a number of VMs.

To start the endpoint do the following before launching the app:

1. Ensure you have Java installed!
2. CD into wiremock directory.
3. Launch vSphere Endpoint for On-Prem DC using the bat file on windows, sh file on linux or via the following command:

```
java -jar wiremock-standalone-2.5.0.jar --https-port=8082 --verbose --root-dir ./on-prem/
```

4. Set your ini file to use the following hosturl: https://localhost:8082

## Releases
Check out our pre-built releases [here](https://github.com/vmware/vr-dc-ex/releases)

## Contributing

The VR Datacenter Experience project team welcomes contributions from the community. If you wish to contribute code and you have not
signed our contributor license agreement (CLA), our bot will update the issue when you open a Pull Request. For any
questions about the CLA process, please refer to our [FAQ](https://cla.vmware.com/faq). For more detailed information,
refer to [CONTRIBUTING.md](CONTRIBUTING.md).

## License

This project is under the BSD-2 License, please see more [here](https://github.com/vmware/vr-dc-ex/blob/master/LICENSE.txt)
