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


namespace Articy.Test.GlobalVariables
{
    
    
    [Articy.Unity.ArticyCodeGenerationHashAttribute(636994029984657969)]
    public class ArticyScriptFragments : BaseScriptFragments, ISerializationCallbackReceiver
    {
        
        #region Fields
        private System.Collections.Generic.Dictionary<int, System.Func<ArticyGlobalVariables, Articy.Unity.IBaseScriptMethodProvider, bool>> Conditions = new System.Collections.Generic.Dictionary<int, System.Func<ArticyGlobalVariables, Articy.Unity.IBaseScriptMethodProvider, bool>>();
        
        private System.Collections.Generic.Dictionary<int, System.Action<ArticyGlobalVariables, Articy.Unity.IBaseScriptMethodProvider>> Instructions = new System.Collections.Generic.Dictionary<int, System.Action<ArticyGlobalVariables, Articy.Unity.IBaseScriptMethodProvider>>();
        #endregion
        
        #region Script fragments
        /// <summary>
        /// ObjectID: 0x100000000000127
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928231?pane=selected&amp;tab=current
        /// </summary>
        public void Script_0x100000000000127Text(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            aGlobalVariablesState.Game.intro = true;
        }
        
        /// <summary>
        /// ObjectID: 0x10000000000012B
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928235?pane=selected&amp;tab=current
        /// </summary>
        public bool Script_0x10000000000012BText(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            return aGlobalVariablesState.Game.side == 0;
        }
        
        /// <summary>
        /// ObjectID: 0x100000000000131
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928241?pane=selected&amp;tab=current
        /// </summary>
        public bool Script_0x100000000000131Text(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            return aGlobalVariablesState.Game.side == 1;
        }
        
        /// <summary>
        /// ObjectID: 0x10000000000013E
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928254?pane=selected&amp;tab=current
        /// </summary>
        public bool Script_0x10000000000013EText(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            return aGlobalVariablesState.Game.likes == true;
        }
        
        /// <summary>
        /// ObjectID: 0x1000000000001E2
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928418?pane=selected&amp;tab=current
        /// </summary>
        public bool Script_0x1000000000001E2Text(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            return aGlobalVariablesState.Game.likes == false;
        }
        
        /// <summary>
        /// ObjectID: 0x100000000000149
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928265?pane=selected&amp;tab=current
        /// </summary>
        public bool Script_0x100000000000149Text(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            return aGlobalVariablesState.Game.side == 0;
        }
        
        /// <summary>
        /// ObjectID: 0x10000000000014E
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928270?pane=selected&amp;tab=current
        /// </summary>
        public bool Script_0x10000000000014EText(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            return aGlobalVariablesState.Game.side == 1;
        }
        
        /// <summary>
        /// ObjectID: 0x1000000000001D0
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928400?pane=selected&amp;tab=current
        /// </summary>
        public void Script_0x1000000000001D0Text(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            aGlobalVariablesState.Game.likes = true;
        }
        
        /// <summary>
        /// ObjectID: 0x1000000000001D6
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928406?pane=selected&amp;tab=current
        /// </summary>
        public void Script_0x1000000000001D6Text(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            aGlobalVariablesState.Game.likes = false;
        }
        
        /// <summary>
        /// ObjectID: 0x100000000000231
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928497?pane=selected&amp;tab=current
        /// </summary>
        public void Script_0x100000000000231Text(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            aGlobalVariablesState.Game.likes = false;
        }
        
        /// <summary>
        /// ObjectID: 0x10000000000023D
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928509?pane=selected&amp;tab=current
        /// </summary>
        public void Script_0x10000000000023DText(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            aGlobalVariablesState.Game.likes = true;
        }
        
        /// <summary>
        /// ObjectID: 0x1000000000001A8
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928360?pane=selected&amp;tab=current
        /// </summary>
        public void Script_0x1000000000001A8Text(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            aGlobalVariablesState.Game.side = 1;
        }
        
        /// <summary>
        /// ObjectID: 0x1000000000001A2
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928354?pane=selected&amp;tab=current
        /// </summary>
        public void Script_0x1000000000001A2Text(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            aGlobalVariablesState.Game.side = 0;
        }
        
        /// <summary>
        /// ObjectID: 0x1000000000002D6
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928662?pane=selected&amp;tab=current
        /// </summary>
        public void Script_0x1000000000002D6Text(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            aGlobalVariablesState.Game.answer = true;
        }
        
        /// <summary>
        /// ObjectID: 0x1000000000002DC
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928668?pane=selected&amp;tab=current
        /// </summary>
        public void Script_0x1000000000002DCText(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            aGlobalVariablesState.Game.answer = false;
        }
        
        /// <summary>
        /// ObjectID: 0x1000000000002E9
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928681?pane=selected&amp;tab=current
        /// </summary>
        public bool Script_0x1000000000002E9Text(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            return aGlobalVariablesState.Game.answer == true;
        }
        
        /// <summary>
        /// ObjectID: 0x1000000000002ED
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037928685?pane=selected&amp;tab=current
        /// </summary>
        public bool Script_0x1000000000002EDText(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            return aGlobalVariablesState.Game.answer == false;
        }
        
        /// <summary>
        /// ObjectID: 0x10000000000066D
        /// Articy Object ref: articy://localhost/view/1066e276-0c4b-46d2-9635-ce2737d2a5e8/72057594037929581?pane=selected&amp;tab=current
        /// </summary>
        public bool Script_0x10000000000066DText(ArticyGlobalVariables aGlobalVariablesState, Articy.Unity.IBaseScriptMethodProvider aMethodProvider)
        {
            return aGlobalVariablesState.Game.answer == false;
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
            Instructions.Add(635199166, this.Script_0x100000000000127Text);
            Conditions.Add(1587717461, this.Script_0x10000000000012BText);
            Conditions.Add(505011123, this.Script_0x100000000000131Text);
            Conditions.Add(644638871, this.Script_0x10000000000013EText);
            Conditions.Add(-1100904398, this.Script_0x1000000000001E2Text);
            Conditions.Add(1892824010, this.Script_0x100000000000149Text);
            Conditions.Add(644646358, this.Script_0x10000000000014EText);
            Instructions.Add(514817981, this.Script_0x1000000000001D0Text);
            Instructions.Add(-1132852857, this.Script_0x1000000000001D6Text);
            Instructions.Add(-115316828, this.Script_0x100000000000231Text);
            Instructions.Add(-496627987, this.Script_0x10000000000023DText);
            Instructions.Add(1902618328, this.Script_0x1000000000001A8Text);
            Instructions.Add(-1100908490, this.Script_0x1000000000001A2Text);
            Instructions.Add(-1350671274, this.Script_0x1000000000002D6Text);
            Instructions.Add(-2102542547, this.Script_0x1000000000002DCText);
            Conditions.Add(1272546282, this.Script_0x1000000000002E9Text);
            Conditions.Add(-496570277, this.Script_0x1000000000002EDText);
            Conditions.Add(-1194727402, this.Script_0x10000000000066DText);
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