require "ExtendGlobal"

PanelOther = class()

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
