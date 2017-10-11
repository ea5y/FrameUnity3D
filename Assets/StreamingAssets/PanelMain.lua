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
--[[
xlua.hotfix(CS.Easy.FrameUnity.Panel.PanelMain, {
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
        end

    } 
)
--]]
--[[
xlua.hotfix(CS.Easy.FrameUnity.Panel.PanelMain, "RegisterBtnEvent",
        function(self)
            local ok,btnG = self.UIDic:TryGetValue('Button_1')
            print(self.UIDic:TryGetValue('Button_1'))
            local btn = btnG:GetComponent('UIButton')
            print(btn)
            local OnNewBtnClick = function()
                print("Click")
            end
            CS.EventDelegate.Add(btn.onClick, OnNewBtnClick)
            --print("register")
        end
)
--]]
