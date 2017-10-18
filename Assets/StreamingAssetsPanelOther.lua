require "ExtendGlobal"

PanelOther = class()

--[[
function PanelOther:Awake()
    print("PanelOther Awake")
end

function PanelOther:Start()
    print("PanelOther Start")
end

function PanelOther:OnEnable()
    print("PanelOther OnEnable")
end

function PanelOther:OnDisable()
    print("PanelOther OnDisable")
end

function PanelOther:Update()
    --print("PanelOther Update")
end

function PanelOther:OnDestroy()
    print("PanelOther OnDestroy")
end
--]]


--local panelHotfix = gameobject:GetComponent('PanelHotfix')
--local activeMessage = CS.Easy.FrameUnity.XLuaExtension.Require(gameobject);

--print(gameobject, panelHotfix, activeMessage);
function PanelOther:TransformGameObject(gameObject)
    print("===>TransformGameObject: " .. gameObject.name);
    self.gameObject = gameObject;
end

function PanelOther:Open(data)
    self.gameObject:SetActive(not self.gameObject.activeSelf)
end

function PanelOther:OpenChild(data)
end

function PanelOther:Close()
end

function PanelOther:Back()
end

function PanelOther:Home()
end
