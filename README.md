# FrameUnity3D
一些通用模块的封装，Note:由于我过滤了NGUI的东西，所以如果下载下来要用我的工程的话，要自己导入一下ngui。
## 功能点：
### 热更新(用的XLua)目前实现功能：
* 热更一个原有的界面
* 热更一个新的界面
>热更核心思想：尽量做到开发的时候不关心热更只负责写c#(现在如果用到接口，可能需要给lua也添加一个接口)，热更的时候不关心开发只负责写lua。这部分我会再整理下，弄得更加简单易懂
#### 下面我来说说我的方法：
热更的时候，是不是不能像原本那样拖拽控件到脚本上，因为我本身就是这样做的，这样非常方便,而且整洁，整个界面的控件就像一个设计好的类：</br>
![Aaron Swartz](https://raw.githubusercontent.com/ea5y/FrameUnity3D/master/ReadMeImage/1.png) </br>
在热更的时候，我们也是可以这么做的，只是稍微麻烦一点点，（首先声明，我的习惯是一个界面不带滚动的就一个脚本，带滚动的就加相应的Item脚本）热更的时候流程是这样的：</br>
##### 热更一个原有的界面：
用到6个文件1.PanelMain.cs 2.PanelBase.cs 3.LuaBehaviour.cs 4.HotfixManager.cs 5.Hotfix.lua 6.PanelMain.lua</br>
1.  在原有的界面prefab（这里拿PanelMain.prefab举例）上增加新的控件，然后打包成assetbundle更新，原有的界面挂上PanelMain.cs脚本 该脚本继承PanelBase.cs, PanelBase.cs又继承自LuaBehaviour.cs， LuaBehaviour.cs又继承MonoBehaviour,继承关系是这样的，我们只要使用一个PanelMain.cs就行了，然后在Injection数组中放入（拖入）新增的UI控件，方便我们在lua代码中获取新增的UI控件
2.  写一个相应的lua热更代码。（这里拿PanelMain.lua举例用到Xlua的hotfix和hotfix_ex函数）
3.  程序运行，在下载完热更资源（assetbundle文件PanelMain， lua文件PanelMain.lua）后启动热更,在热更入口中（Hotfix.lua）require 'PanelMain' 替换原有的函数
##### 热更一个新的界面：
用到6个界面1.PanelHotfix.cs 2.Luabehaviour.cs 3.HotfixManager.cs 4.Hotfix.lua 5.PanelOther.lua 6.ExtendGlobal.lua</br>
1.  新建一个prefab（这里拿PanelOther.prefab举例）作为一个新的界面，挂上PanelHotfix.cs脚本，该脚本继承自LuaBehaviour.cs，LuaBehaviour.cs继承自MonoBehaviour,继承关系是这样的，我们只要使用一个PanelHotfix.cs就行了，然后在Injection数组中放入（拖入）新增的UI控件，方便我们在lua代码中获取新增的UI控件
2.  写一个相应的lua热更代码。（这里拿PanelOther.lua举例，直接自己构造一个类结构）
3.  程序运行，在下载完热更资源（assetbundle文件PanelOther， lua文件PanelOther.lua）后启动热更,在热更入口中（Hotfix.lua）require 'PanelOther' 载入lua代码，启动完成后，导出PanelOther.lua的接口，用于c#的调用。
>持续更新中... ... (下面的还要再组织一下语言)

### 管理工具：
1.  资源下载管理器：负责资源更新的时候下载资源，目前把所有下载到本地的assetbundle和lua文件都视作资源，会在一个进度条同时下载</br>
(新增)在编辑器开启EDITOR_MODE的时候，不会进行资源更新，因为直接从编辑器资源加载asset了
2.  热更管理器：负责热更的启动，获取Lua接口等等
3.  Asset对象池管理器：（我现在prefab等等要打包的资源都是打的ScriptableObject,在里面引用要打包的资源，这样开发的时候就不用频繁复制粘贴了）负责当需要从bundle下载一个asset资源的时候，只需要从这个Asset对象池找就行了，找不到会下载一个给你，并存在对象池中，下次用的时候就不会再load了，对象池包含回收功能，每隔一段时间，会进行asset资源的回收，保证内存不会过高。</br>
(修改)由于我用了垃圾回收管理器，所以我就没有用timer，而是集中扔到垃圾收集管理器中处理。</br>
(新增)在编辑器开启EDITOR_MODE的到时候，asset直接加载编辑器资源，而不是从bundle加载，方便开发
4.  垃圾回收管理器：（新增）集中处理垃圾，目前包括asset的释放，和bundle的卸载，通过线程池实现。
5.  场景管理器：负责切换场景（我现在切换场景的时候，有一个专门的加载场景,这个加载场景还用在了下载资源的时候，之前是想着，带加载功能的过渡场景的时候统一用这个，根据不同类型切换不同的界面ui）现在这块东西联系比较紧，后续看能不能改进，但是用起来很好用。
6.  界面管理器：负责管理界面打开，关闭，返回等等
7.  UI管理器：负责实例化界面prefab（这个同上面这个好像可以放到一起，有时间弄一下）
8.  玩家管理器：负责玩家状态同步（目前简单的实现了同步位置，状态同步，对性能方面没多少考虑，有时间还要研究下的）
### 网络通信：
目前我用的是开源服务器Scut,github上有，可以搜一下，所以现在是在客户端写了个socket跟scut对接
### UI层级分明：
多摄像机，每个摄像机负责一个层
### 游戏配置窗口：
宏以及其他的配置，例如平台管理等等，以后准备都在一个集中的窗口之中操作（有待完善）
 
>持续更新中... ...
