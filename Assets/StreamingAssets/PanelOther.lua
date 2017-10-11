require "ExtendGlobal"

PanelOther = class()

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
