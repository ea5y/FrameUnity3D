//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-12 17:22
//================================
using Easy.FrameUnity.Manager;
namespace Easy.FrameUnity.Base
{
    public class PanelHotfix : LuaBehaviour, IPanel
    {
        private ILuaTable _luaTable;
        public ILuaTable LuaTable
        {
            get
            {
                if (_luaTable == null)
                    throw new System.Exception("LuaTable is null");
                return _luaTable;
            }
            set { _luaTable = value; }
        }

        private void Awake()
        {
            base.MapHotfixUI();
            this.GetLuaTable();
            this.TransformGameObject();
        }

        public void GetLuaTable()
        {
            var panelName = gameObject.name.Split('(')[0];
            this.LuaTable = HotfixManager.Inst.GetLuaTable(panelName);
        }

        private void TransformGameObject()
        {
            this.LuaTable.TransformGameObject(gameObject);
        }

        public void Open(object data)
        {
            this.LuaTable.Open(data);
        }

        public void OpenChild(object data)
        {
            this.LuaTable.OpenChild(data);
        }

        public void Close()
        {
            this.LuaTable.Close();
        }

        public void Back()
        {
            this.LuaTable.Back();
        }

        public void Home()
        {
            this.LuaTable.Home();
        }
    }
}
