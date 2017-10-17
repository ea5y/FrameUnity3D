# FrameUnity3D
一些通用模块的封装
## 功能点：
### 热更新(用的XLua)目前实现功能：
1.  热更一个原有的界面
2.  热更一个新的界面
### 管理工具：
1.  资源下载管理器：负责资源更新的时候下载资源，目前把所有下载到本地的assetbundle和lua文件都视作资源，会在一个进度条同时下载
2.  热更管理器：负责热更的启动，获取Lua接口等等
3.  Asset对象池管理器：负责当需要从bundle下载一个asset资源的时候，只需要从这个Asset对象池找就行了，找不到会下载一个给你，并存在对象池中，下次用的时候就不会再load了，对象池包含回收功能，每隔一段时间，会进行asset资源的回收，保证内存不会过高
4.  场景管理器：负责切换场景（我现在切换场景的时候，有一个专门的加载场景,这个加载场景还用在了下载资源的时候，之前是想着，带加载功能的过渡场景的时候统一用这个，根据不同类型切换不同的界面ui）现在这块东西联系比较紧，后续看能不能改进，但是用起来很好用。
5.  界面管理器：负责管理界面打开，关闭，返回等等
6.  UI管理器：负责实例化界面prefab
7.  玩家管理器：负责玩家状态同步
