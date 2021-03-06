﻿using System;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using Xpand.ExpressApp.ModelDifference.DataStore.BaseObjects;
using Xpand.Xpo;

namespace FeatureCenter.Module.Win.ApplicationDifferences.ExternalApplication {
    public class ExternalApplicationKeyParameter : ReadOnlyParameter {
        public ExternalApplicationKeyParameter() : base("ExternalApplicationKey", typeof(Guid)) { }

        public override object CurrentValue {
            get {
                return ((User)SecuritySystem.CurrentUser).Session.FindObject<ModelDifferenceObject>(
                        o => o.Name == "ExternalApplication" && o.PersistentApplication.Name == "ExternalApplication.Win").Oid;
            }
        }
    }

}