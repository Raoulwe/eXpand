using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win;
using DevExpress.Utils;
using Xpand.ExpressApp.Security;
using Xpand.ExpressApp.SystemModule;
using Xpand.ExpressApp.Win.Model;
using Xpand.Persistent.Base.PersistentMetaData;

namespace Xpand.ExpressApp.Win.SystemModule {
    [ToolboxItem(true)]
    [ToolboxTabName(XafAssemblyInfo.DXTabXafModules)]
    [Description("Overrides Controllers from the SystemModule and supplies additional basic Controllers that are specific for Windows Forms applications.")]
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [ToolboxBitmap(typeof(WinApplication), "Resources.WinSystemModule.ico")]
    public sealed class XpandSystemWindowsFormsModule : XpandModuleBase {
        public const string XpandWin = "Xpand.Win";

        public XpandSystemWindowsFormsModule() {
            RequiredModuleTypes.Add(typeof(XpandSystemModule));
        }
        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<IModelRootNavigationItems, IModelRootNavigationItemsAutoSelectedGroupItem>();
        }

        protected override IEnumerable<Type> GetDeclaredExportedTypes() {
            return new List<Type>();
        }

        public override void Setup(ApplicationModulesManager moduleManager) {
            base.Setup(moduleManager);
            if (Application != null)
                Application.LogonFailed += (o, eventArgs) => {
                    var logonParameters = SecuritySystem.LogonParameters as IXpandLogonParameters;
                    if (logonParameters != null && logonParameters.RememberMe) {
                        eventArgs.Handled = true;
                        logonParameters.RememberMe = false;
                        ((IXafApplication)Application).WriteLastLogonParameters(null, SecuritySystem.LogonParameters);
                    }

                };
        }


    }


}