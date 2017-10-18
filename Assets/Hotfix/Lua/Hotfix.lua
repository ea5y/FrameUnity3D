--local path = CS.UnityEngine.Application.dataPath .. '/Hotfix/Lua/'
local path = CS.URL.LUA_LOCAL_URL;
print('Require Path: ' .. path)
package.path = path .. '?.lua'
require 'PanelMain'
require 'PanelOther'
