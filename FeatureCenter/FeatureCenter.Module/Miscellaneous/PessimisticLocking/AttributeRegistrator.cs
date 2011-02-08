﻿using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using FeatureCenter.Base;
using Xpand.ExpressApp.AdditionalViewControlsProvider.Logic;
using Xpand.ExpressApp.Attributes;

namespace FeatureCenter.Module.Miscellaneous.PessimisticLocking {
    public class AttributeRegistrator : Xpand.ExpressApp.Core.AttributeRegistrator {
        private const string PessimisticLocking_DetailView = "PessimisticLocking_DetailView";
        public override IEnumerable<Attribute> GetAttributes(ITypeInfo typesInfo) {
            if (typesInfo.Type != typeof(PLCustomer)) yield break;
            yield return new AdditionalViewControlsRuleAttribute(Captions.ViewMessage + " " + Captions.HeaderPessimisticLocking, "1=1", "1=1", Captions.ViewMessagePessimisticLocking, Position.Bottom) { ViewType = ViewType.DetailView, View = PessimisticLocking_DetailView };
            yield return new AdditionalViewControlsRuleAttribute(Captions.Header + " " + Captions.HeaderPessimisticLocking, "1=1", "1=1", Captions.HeaderPessimisticLocking, Position.Top) { ViewType = ViewType.DetailView, View = PessimisticLocking_DetailView };
            yield return new XpandNavigationItemAttribute(Captions.Miscellaneous + "PessimisticLocking", PessimisticLocking_DetailView, "Name='Benjamin CISCO'");
            yield return new CloneViewAttribute(CloneViewType.DetailView, PessimisticLocking_DetailView);
            yield return new WhatsNewAttribute("2/2/2011");
        }
    }
}