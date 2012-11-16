apploader
=========

portable app server written by c#. 

easy to write a program running with console or windows service.

contact: houkun@taobao.com

support .net2/3.5/4+

Unified host applies to a large number of small applications, ease of management and simplified operation and maintenance work.

One folder run as an app.

One process host multiple applications (run as appdomain).

## Build

Depends on 
https://github.com/ali-ent/work-tool.git (for building tools).
https://github.com/ali-ent/AppAgent.git (for process management)

folder:

|-work-tool

|-AppAgent

|-apploader


```shell
build
run.ps1
```

and see:

- build\apploader: 

the host app, tool folder contain scripts for install and cmd script

```xml
<add key="serviceRoot" value="C:\app"/>
```
the setting is the root of apps folder.

- build\lib



If you want to support .net2/3/3.5 via CLR2.0, you can change the project's target to .net3.5.

also you can use tools\apploader.zip directly.

## License

	Copyright (c) Alibaba.  All rights reserved. - http://www.alibaba-inc.com/

	Licensed under the Apache License, Version 2.0 (the "License");

	you may not use this file except in compliance with the License.

	You may obtain a copy of the License at
 
		 http://www.apache.org/licenses/LICENSE-2.0
 
	Unless required by applicable law or agreed to in writing, 

	software distributed under the License is distributed on an "AS IS" BASIS, 

	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

	See the License for the specific language governing permissions and limitations under the License.


