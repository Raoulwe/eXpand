﻿using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using Xpand.ExpressApp.Logic.Conditional.Security.Improved;
using Xpand.ExpressApp.MasterDetail.Logic;

namespace Xpand.ExpressApp.MasterDetail.Security.Improved {
    public class MasterDetailPermission : ConditionalLogicRulePermission, IMasterDetailRule {
        public const string OperationName = "MasterDetail";

        public MasterDetailPermission(IMasterDetailRule logicRule)
            : base(OperationName, logicRule) {

        }
        public string ChildListView { get; set; }

        public string CollectionMember { get; set; }

        public bool SynchronizeActions { get; set; }

        IModelListView IMasterDetailRule.ChildListView {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        IModelMember IMasterDetailRule.CollectionMember {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        public override IList<string> GetSupportedOperations() {
            return new[] { OperationName };
        }
    }
}
