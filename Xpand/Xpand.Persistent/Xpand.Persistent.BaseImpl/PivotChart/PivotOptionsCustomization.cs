﻿using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Xpand.Persistent.Base.PivotChart;

namespace Xpand.Persistent.BaseImpl.PivotChart {
    public class PivotOptionsCustomization : BaseObject, IPivotOptionsCustomization {
        public PivotOptionsCustomization(Session session) : base(session) {
        }
    }
}