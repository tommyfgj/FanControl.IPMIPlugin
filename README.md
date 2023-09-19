# FanControl.IPMIPlugin 
A plugin to Control fan through ipmitool. I use this to control extra fan for cooling my P40 under ESXI.(cheap game streaming and AIGC Calculation, hahaha...)

## Install

### Step1: Copy ipmitool.exe and libeay32.dll to Fan_Control root directory
the files are under .\FanControl.IPMIPluginTest\bin\Debug

### Step2: Edit FanControl.exe.config and add ipmi configuration like this
``` xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
   ........
  <appSettings>
  	<add key="ipmi.addr" value="192.168.1.2" />
  	<add key="ipmi.user" value="admin" />
  	<add key="ipmi.passwd" value="admin" />
	<add key="ipmi.fan_name" value="System Fan1" />
	<add key="ipmi.fan_index" value="01" />
	<add key="ipmi.fan_oem" value="Lenovo RD450x" />
	<add key="ipmi.fan_min_speed" value="10" />
  </appSettings>
</configuration>
```

### Step3: Run FanControl.exe and Click Assisted Setup under Control Option
