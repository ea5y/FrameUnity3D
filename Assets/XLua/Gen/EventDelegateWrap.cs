#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class EventDelegateWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(EventDelegate);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 6, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Equals", _m_Equals);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetHashCode", _m_GetHashCode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Set", _m_Set);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Execute", _m_Execute);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToString", _m_ToString);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "target", _g_get_target);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "methodName", _g_get_methodName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "parameters", _g_get_parameters);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isValid", _g_get_isValid);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isEnabled", _g_get_isEnabled);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "oneShot", _g_get_oneShot);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "target", _s_set_target);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "methodName", _s_set_methodName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "oneShot", _s_set_oneShot);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 6, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Execute", _m_Execute_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsValid", _m_IsValid_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Set", _m_Set_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Add", _m_Add_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Remove", _m_Remove_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					EventDelegate __cl_gen_ret = new EventDelegate();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<EventDelegate.Callback>(L, 2))
				{
					EventDelegate.Callback call = translator.GetDelegate<EventDelegate.Callback>(L, 2);
					
					EventDelegate __cl_gen_ret = new EventDelegate(call);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 3 && translator.Assignable<UnityEngine.MonoBehaviour>(L, 2) && (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
				{
					UnityEngine.MonoBehaviour target = (UnityEngine.MonoBehaviour)translator.GetObject(L, 2, typeof(UnityEngine.MonoBehaviour));
					string methodName = LuaAPI.lua_tostring(L, 3);
					
					EventDelegate __cl_gen_ret = new EventDelegate(target, methodName);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to EventDelegate constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Equals(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    object obj = translator.GetObject(L, 2, typeof(object));
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.Equals( obj );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetHashCode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.GetHashCode(  );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Set(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.MonoBehaviour target = (UnityEngine.MonoBehaviour)translator.GetObject(L, 2, typeof(UnityEngine.MonoBehaviour));
                    string methodName = LuaAPI.lua_tostring(L, 3);
                    
                    __cl_gen_to_be_invoked.Set( target, methodName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Execute(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.Execute(  );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        string __cl_gen_ret = __cl_gen_to_be_invoked.ToString(  );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Execute_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Collections.Generic.List<EventDelegate> list = (System.Collections.Generic.List<EventDelegate>)translator.GetObject(L, 1, typeof(System.Collections.Generic.List<EventDelegate>));
                    
                    EventDelegate.Execute( list );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsValid_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Collections.Generic.List<EventDelegate> list = (System.Collections.Generic.List<EventDelegate>)translator.GetObject(L, 1, typeof(System.Collections.Generic.List<EventDelegate>));
                    
                        bool __cl_gen_ret = EventDelegate.IsValid( list );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Set_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<System.Collections.Generic.List<EventDelegate>>(L, 1)&& translator.Assignable<EventDelegate.Callback>(L, 2)) 
                {
                    System.Collections.Generic.List<EventDelegate> list = (System.Collections.Generic.List<EventDelegate>)translator.GetObject(L, 1, typeof(System.Collections.Generic.List<EventDelegate>));
                    EventDelegate.Callback callback = translator.GetDelegate<EventDelegate.Callback>(L, 2);
                    
                        EventDelegate __cl_gen_ret = EventDelegate.Set( list, callback );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 2&& translator.Assignable<System.Collections.Generic.List<EventDelegate>>(L, 1)&& translator.Assignable<EventDelegate>(L, 2)) 
                {
                    System.Collections.Generic.List<EventDelegate> list = (System.Collections.Generic.List<EventDelegate>)translator.GetObject(L, 1, typeof(System.Collections.Generic.List<EventDelegate>));
                    EventDelegate del = (EventDelegate)translator.GetObject(L, 2, typeof(EventDelegate));
                    
                    EventDelegate.Set( list, del );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to EventDelegate.Set!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Add_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<System.Collections.Generic.List<EventDelegate>>(L, 1)&& translator.Assignable<EventDelegate.Callback>(L, 2)) 
                {
                    System.Collections.Generic.List<EventDelegate> list = (System.Collections.Generic.List<EventDelegate>)translator.GetObject(L, 1, typeof(System.Collections.Generic.List<EventDelegate>));
                    EventDelegate.Callback callback = translator.GetDelegate<EventDelegate.Callback>(L, 2);
                    
                        EventDelegate __cl_gen_ret = EventDelegate.Add( list, callback );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 2&& translator.Assignable<System.Collections.Generic.List<EventDelegate>>(L, 1)&& translator.Assignable<EventDelegate>(L, 2)) 
                {
                    System.Collections.Generic.List<EventDelegate> list = (System.Collections.Generic.List<EventDelegate>)translator.GetObject(L, 1, typeof(System.Collections.Generic.List<EventDelegate>));
                    EventDelegate ev = (EventDelegate)translator.GetObject(L, 2, typeof(EventDelegate));
                    
                    EventDelegate.Add( list, ev );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& translator.Assignable<System.Collections.Generic.List<EventDelegate>>(L, 1)&& translator.Assignable<EventDelegate.Callback>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    System.Collections.Generic.List<EventDelegate> list = (System.Collections.Generic.List<EventDelegate>)translator.GetObject(L, 1, typeof(System.Collections.Generic.List<EventDelegate>));
                    EventDelegate.Callback callback = translator.GetDelegate<EventDelegate.Callback>(L, 2);
                    bool oneShot = LuaAPI.lua_toboolean(L, 3);
                    
                        EventDelegate __cl_gen_ret = EventDelegate.Add( list, callback, oneShot );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& translator.Assignable<System.Collections.Generic.List<EventDelegate>>(L, 1)&& translator.Assignable<EventDelegate>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    System.Collections.Generic.List<EventDelegate> list = (System.Collections.Generic.List<EventDelegate>)translator.GetObject(L, 1, typeof(System.Collections.Generic.List<EventDelegate>));
                    EventDelegate ev = (EventDelegate)translator.GetObject(L, 2, typeof(EventDelegate));
                    bool oneShot = LuaAPI.lua_toboolean(L, 3);
                    
                    EventDelegate.Add( list, ev, oneShot );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to EventDelegate.Add!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Remove_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<System.Collections.Generic.List<EventDelegate>>(L, 1)&& translator.Assignable<EventDelegate.Callback>(L, 2)) 
                {
                    System.Collections.Generic.List<EventDelegate> list = (System.Collections.Generic.List<EventDelegate>)translator.GetObject(L, 1, typeof(System.Collections.Generic.List<EventDelegate>));
                    EventDelegate.Callback callback = translator.GetDelegate<EventDelegate.Callback>(L, 2);
                    
                        bool __cl_gen_ret = EventDelegate.Remove( list, callback );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 2&& translator.Assignable<System.Collections.Generic.List<EventDelegate>>(L, 1)&& translator.Assignable<EventDelegate>(L, 2)) 
                {
                    System.Collections.Generic.List<EventDelegate> list = (System.Collections.Generic.List<EventDelegate>)translator.GetObject(L, 1, typeof(System.Collections.Generic.List<EventDelegate>));
                    EventDelegate ev = (EventDelegate)translator.GetObject(L, 2, typeof(EventDelegate));
                    
                        bool __cl_gen_ret = EventDelegate.Remove( list, ev );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to EventDelegate.Remove!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_target(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.target);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_methodName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.methodName);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_parameters(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.parameters);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isValid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.isValid);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isEnabled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.isEnabled);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_oneShot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.oneShot);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_target(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.target = (UnityEngine.MonoBehaviour)translator.GetObject(L, 2, typeof(UnityEngine.MonoBehaviour));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_methodName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.methodName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_oneShot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EventDelegate __cl_gen_to_be_invoked = (EventDelegate)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.oneShot = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
