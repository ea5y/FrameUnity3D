--[[
require "ExtendGlobal"

PanelMain = class()

function PanelMain:Awake()
    print("PanelMain Awake")
end

function PanelMain:Start()
    print("PanelMain Start")
end

function PanelMain:OnEnable()
    print("PanelMain OnEnable")
end

function PanelMain:OnDisable()
    print("PanelMain OnDisable")
end

function PanelMain:Update()
    --print("PanelMain Awake")
end

function PanelMain:OnDestroy()
    print("PanelMain OnDestroy")
end
--]]

--Stateful
--[[
xlua.hotfix(CS.Easy.FrameUnity.Panel.PanelMain, {
        ['.ctor'] = function(csobj)
            return {
                csobj
            }
        end;
        Start = function(self)
            --self.csobj.Awake()
            print("Stateful")
        end;
        RegisterBtnEvent = function(self)
            local ok,btnG = self.UIDic:TryGetValue('Button_1')
            print(self.UIDic:TryGetValue('Button_1'))
            local btn = btnG:GetComponent('UIButton')
            print(btn)
            local OnNewBtnClick = function()
                print("Click")
            end
            CS.EventDelegate.Add(btn.onClick, OnNewBtnClick)
            --print("register")
        end;
    } 
)
--]]
--Stateless
local util = require 'xlua.util'
local panelOther = require 'PanelOther'
xlua.private_accessible(CS.Easy.FrameUnity.Panel.PanelMain)
util.hotfix_ex(CS.Easy.FrameUnity.Panel.PanelMain, 'Start',
        function(self)
            CS.Easy.FrameUnity.Panel.PanelMain.Inst:Start();
            print("Lua: awake")
            local ok,btn = self.HotfixUIDic:TryGetValue('Button_1')
            btn = btn:GetComponent('UIButton')
            print(btn)
            local OnNewBtnClick = function()
                print("Click")
                _G.PanelOther:Open(nil);
            end
            CS.EventDelegate.Add(btn.onClick, OnNewBtnClick)
        end
)
