//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Articy.Unity;
using Articy.Unity.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Articy.The_Road_To_Moscow.GlobalVariables
{
    
    
    [Articy.Unity.ArticyCodeGenerationHashAttribute(637031442889005924)]
    public class ArticyScriptFragments : BaseScriptFragments, ISerializationCallbackReceiver
    {
        
        #region Fields
        private System.Collections.Generic.Dictionary<int, System.Func<ArticyGlobalVariables, Articy.Unity.IBaseScriptMethodProvider, bool>> Conditions = new System.Collections.Generic.Dictionary<int, System.Func<ArticyGlobalVariables, Articy.Unity.IBaseScriptMethodProvider, bool>>();
        
        private System.Collections.Generic.Dictionary<int, System.Action<ArticyGlobalVariables, Articy.Unity.IBaseScriptMethodProvider>> Instructions = new System.Collections.Generic.Dictionary<int, System.Action<ArticyGlobalVariables, Articy.Unity.IBaseScriptMethodProvider>>();
        #endregion
        
        #region Script fragments
        /// <summary>
        /// ObjectID: 0x1000000000003F1
        /// Articy Object ref: articy://localhost/view/b5f18cd2-d335-40b3-9f2b-0cf56b73c3d8/72057594037928945?pane=selected&amp;tab=current
        /// </summary>
        public bool Script_0x1000000000003F1Text(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            return true;
        }
        
        /// <summary>
        /// ObjectID: 0x1000000000003F8
        /// Articy Object ref: articy://localhost/view/b5f18cd2-d335-40b3-9f2b-0cf56b73c3d8/72057594037928952?pane=selected&amp;tab=current
        /// </summary>
        public bool Script_0x1000000000003F8Text(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            return false;
        }
        
        /// <summary>
        /// ObjectID: 0x1000000000003FF
        /// Articy Object ref: articy://localhost/view/b5f18cd2-d335-40b3-9f2b-0cf56b73c3d8/72057594037928959?pane=selected&amp;tab=current
        /// </summary>
        public bool Script_0x1000000000003FFText(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            return false;
        }
        #endregion
        
        #region Unity serialization
        public override void OnBeforeSerialize()
        {
        }
        
        public override void OnAfterDeserialize()
        {
            Conditions = new System.Collections.Generic.Dictionary<int, System.Func<ArticyGlobalVariables, Articy.Unity.IBaseScriptMethodProvider, bool>>();
            Instructions = new System.Collections.Generic.Dictionary<int, System.Action<ArticyGlobalVariables, Articy.Unity.IBaseScriptMethodProvider>>();
            Conditions.Add(886063062, this.Script_0x1000000000003F1Text);
            Conditions.Add(798086281, this.Script_0x1000000000003F8Text);
            Conditions.Add(-1660043809, this.Script_0x1000000000003FFText);
        }
        #endregion
        
        #region Script execution
        public override void CallInstruction(Articy.Unity.Interfaces.IGlobalVariables aGlobalVariables, int aHash, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            if ((Instructions.ContainsKey(aHash) == false))
            {
                return;
            }
            if ((aMethodProvider != null))
            {
                aMethodProvider.IsCalledInForecast = aGlobalVariables.IsInShadowState;
            }
            Instructions[aHash].Invoke(((ArticyGlobalVariables)(aGlobalVariables)), aMethodProvider);
        }
        
        public override bool CallCondition(Articy.Unity.Interfaces.IGlobalVariables aGlobalVariables, int aHash, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            if ((Conditions.ContainsKey(aHash) == false))
            {
                return true;
            }
            if ((aMethodProvider != null))
            {
                aMethodProvider.IsCalledInForecast = aGlobalVariables.IsInShadowState;
            }
            return Conditions[aHash].Invoke(((ArticyGlobalVariables)(aGlobalVariables)), aMethodProvider);
        }
        #endregion
    }
}
