function awake()
    print("lua-PanelChild awake...")
end

function start()
    print("lua-PanelChild start...")
end

function update()
    --print("lua-PanelChild update...")
end

function onDestroy()
    print("lua-PanelChild onDestroy...")
end
--
--[[
xlua.hotfix(CS.Easy.FrameUnity.Panel.PanelMain,{
    Awake = function(self)
        RegisterBtnEvent()
    end;

    RegisterBtnEvent = function(self)
    end;

    OnNewBtnClick = function(self)
    end;

    awake = function(self)
    print("lua-PanelChild awake...")
    end;

    start = function(self)
    print("lua-PanelChild start...")
    end;

    update = function(self)
    --print("lua-PanelChild update...")
    end;

    onDestroy = function(self)
    print("lua-PanelChild onDestroy...")
    end;
})
--]]
