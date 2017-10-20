# FrameUnity3D
一些通用模块的封装
## 功能点：
### 热更新(用的XLua)目前实现功能：
1.  热更一个原有的界面
2.  热更一个新的界面
>热更核心思想：尽量做到开发的时候不关心热更只负责写c#(现在如果用到接口，可能需要给lua也添加一个接口)，热更的时候不关心开发只负责写lua。这部分我会再整理下，弄得更加简单易懂
#### 下面我来说说我的方法：
热更的时候，是不是不能像原本那样拖拽控件到脚本上，因为我本身就是这样做的，这样非常方便,而且整洁，整个界面的控件就像一个设计好的类：</br>
![Aaron Swartz](https://raw.githubusercontent.com/ea5y/FrameUnity3D/master/ReadMeImage/1.png) </br>
在热更的时候，我们也是可以这么做的，只是稍微麻烦一点点，（首先声明，我的习惯是一个界面不带滚动的就一个脚本，带滚动的就加相应的Item脚本）热更的时候流程是这样的：</br>
1. 按照界面列表的参数，动态实例化prefab --所以我是一个界面一个prefab(关于界面列表的参数从哪来，我是用的ScriptableObject,后面会详细说明一下这个东西)
2. 在实例化prefab的时候，我会将当前这个prefab的gameobject传送到lua里面(也就是用lua写的相应界面脚本，模拟的面向对象写法，可以说是一个类--lua命名空间还没弄，有空弄一下)</br>
![Aaron Swartz](https://raw.githubusercontent.com/ea5y/FrameUnity3D/master/ReadMeImage/2.png) ![Aaron Swartz](https://raw.githubusercontent.com/ea5y/FrameUnity3D/master/ReadMeImage/3.png) </br>
3. 我在lua拿到gameobject的时候，我就可以获取这个prefab上提前挂上的基础脚本PanelHotfix.cs(这个脚本是很早就用c#写好的,所以当我热更的时候，是可以挂上这样的脚本，当一个prefab挂上一个脚本的时候，这个prefab本身会增加一条关于这个脚本的数据，所以当你使用这个prefab的时候，会自动根据里面的guid引用相关脚本，拖一个控件到脚本上同理，也会在该prefab上增加相关数据)</br>
![Aaron Swartz](https://raw.githubusercontent.com/ea5y/FrameUnity3D/master/ReadMeImage/4.png) </br>
PanelHotfix.cs </br>
![Aaron Swartz](https://raw.githubusercontent.com/ea5y/FrameUnity3D/master/ReadMeImage/5.png) </br>
LuaBehavior.cs </br>
![Aaron Swartz](https://raw.githubusercontent.com/ea5y/FrameUnity3D/master/ReadMeImage/6.png) </br>
4. 拿到PanelHotfix之后，我就可以获取里面的HotfixUIDic，根据设置的控件名从该字典获取相应Gameobject，如果强迫证的同学想弄个类方便调用的话，可以在lua模拟一个类，然后把控件赋值进去,这样我就动态获取了热更的控件，而不用写好多查找gameobject的代码</br>
![Aaron Swartz](https://raw.githubusercontent.com/ea5y/FrameUnity3D/master/ReadMeImage/7.png) </br>
### 管理工具：
1.  资源下载管理器：负责资源更新的时候下载资源，目前把所有下载到本地的assetbundle和lua文件都视作资源，会在一个进度条同时下载
2.  热更管理器：负责热更的启动，获取Lua接口等等
3.  Asset对象池管理器：负责当需要从bundle下载一个asset资源的时候，只需要从这个Asset对象池找就行了，找不到会下载一个给你，并存在对象池中，下次用的时候就不会再load了，对象池包含回收功能，每隔一段时间，会进行asset资源的回收，保证内存不会过高（--目前还没有对bundle进行卸载的管理，后面会加）
4.  场景管理器：负责切换场景（我现在切换场景的时候，有一个专门的加载场景,这个加载场景还用在了下载资源的时候，之前是想着，带加载功能的过渡场景的时候统一用这个，根据不同类型切换不同的界面ui）现在这块东西联系比较紧，后续看能不能改进，但是用起来很好用。
5.  界面管理器：负责管理界面打开，关闭，返回等等
6.  UI管理器：负责实例化界面prefab（这个同上面这个好像可以放到一起，有时间弄一下）
7.  玩家管理器：负责玩家状态同步（目前简单的实现了同步位置，状态同步，对性能方面没多少考虑，有时间还要研究下的）
### 网络通信：
目前我用的是开源服务器Scut,github上有，可以搜一下，所以现在是在客户端写了个socket跟scut对接
### UI层级分明：
多摄像机，每个摄像机负责一个层

>持续更新中... ...
