﻿using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using eXpand.ExpressApp.WorldCreator.Core;
using eXpand.ExpressApp.WorldCreator.PersistentTypesHelpers;
using eXpand.Persistent.Base.PersistentMetaData;
using eXpand.Persistent.BaseImpl.ImportExport;
using eXpand.Persistent.BaseImpl.PersistentMetaData;
using eXpand.Tests.eXpand.WorldCreator;

namespace eXpand.Tests.eXpand.IO {
    public interface IModelTypeHandler<T1, T2>
    {
        ITypeHandler<T1, T2> OneToMany();
        ITypeHandler<T1, T2> ManyToMany();
    }
    public interface ITypeHandler<T1, T2>
    {
        Type T1Type { get; }
        Type T2Type { get; }
        IModelInstancesHandler<T1, T2> CreateInstances();
        ObjectSpace ObjectSpace { get; }
    }
    public interface IModelInstancesHandler<T1, T2>
    {
        T1 T1instance { get; }
        T2 T2Instance { get; }
        
    }

    public class ModelBuilder<T1, T2> : IModelTypeHandler<T1, T2>, ITypeHandler<T1, T2>, IModelInstancesHandler<T1,T2>
    {
        Type _t1Type;

        Type _t2Type;

        ObjectSpace _objectSpace;

        object _t2;

        object _t1;
        PersistentAssemblyBuilder _persistentAssemblyBuilder;

        ModelBuilder() {
        }

        public static ModelBuilder<T1,T2> Build()
        {
            return new ModelBuilder<T1, T2>();
        }

        public PersistentAssemblyBuilder PersistentAssemblyBuilder {
            get { return _persistentAssemblyBuilder; }
        }

        public ITypeHandler<T1, T2> OneToMany()
        {
            _persistentAssemblyBuilder = GetPersistentAssemblyBuilder();
            var type1 = typeof(T1);
            var type2 = typeof(T2);
            var type1Name = getName(type1);
            var type2Name = getName(type2);
            var classHandler = _persistentAssemblyBuilder.CreateClasses(new[] { type1Name, type2Name });
            classHandler.CreateReferenceMembers(info => info.Name == type2Name ? new[] { info.PersistentAssemblyInfo.PersistentClassInfos[0] } : null,true);
            createTypes(type1, type2, _persistentAssemblyBuilder, type1Name, type2Name);
            return this;
        }

        public ITypeHandler<T1, T2> ManyToMany()
        {
            PersistentAssemblyBuilder persistentAssemblyBuilder = GetPersistentAssemblyBuilder();
            var type1 = typeof (T1);
            var type2 = typeof(T2);
            var type1Name = getName(type1);
            var type2Name = getName(type2);
            var classHandler = persistentAssemblyBuilder.CreateClasses(new[] { type1Name, type2Name });
            IPersistentAssemblyInfo persistentAssemblyInfo = persistentAssemblyBuilder.PersistentAssemblyInfo;
            var persistentClassInfo1 = persistentAssemblyInfo.PersistentClassInfos[0];
            var persistentClassInfo2 = persistentAssemblyInfo.PersistentClassInfos[1];
            classHandler.CreateCollectionMember(persistentClassInfo1, type2Name+"s", persistentClassInfo2);
            classHandler.CreateCollectionMember(persistentClassInfo2, type1Name + "s", persistentClassInfo1, type2Name+"s");
            createTypes(type1, type2, persistentAssemblyBuilder, type1Name, type2Name);
            return this;
        }

        void createTypes(Type type1, Type type2, PersistentAssemblyBuilder persistentAssemblyBuilder, string type1Name, string type2Name) {
            var persistentClassInfo1 = persistentAssemblyBuilder.PersistentAssemblyInfo.PersistentClassInfos[0];
            var persistentClassInfo2 = persistentAssemblyBuilder.PersistentAssemblyInfo.PersistentClassInfos[1];
            addInterface(type1, persistentClassInfo1);
            addInterface(type2, persistentClassInfo2);
            _objectSpace.CommitChanges();

            var compileModule = new CompileEngine().CompileModule(persistentAssemblyBuilder, Path.GetDirectoryName(Application.ExecutablePath));
            _t1Type = compileModule.Assembly.GetTypes().Where(type => type.Name == type1Name).Single();
            _t2Type = compileModule.Assembly.GetTypes().Where(type => type.Name == type2Name).Single();
        }

        PersistentAssemblyBuilder GetPersistentAssemblyBuilder() {
            var artifactHandler = new TestAppLication<ClassInfoGraphNode>().Setup();
            _objectSpace = artifactHandler.ObjectSpace;
            return PersistentAssemblyBuilder.BuildAssembly(_objectSpace, "a" + Guid.NewGuid().ToString().Replace("-", ""));
        }

        string getName(Type type) {
            if (type.IsInterface && type.Name.StartsWith("I"))
                return type.Name.Substring(1);
            return type.Name;
        }

        void addInterface(Type type1, IPersistentClassInfo persistentClassInfo1) {
            var t1InterfaceInfo = (IInterfaceInfo) _objectSpace.CreateObject(typeof(InterfaceInfo));
            t1InterfaceInfo.Name = type1.FullName;
            t1InterfaceInfo.Assembly = new AssemblyName(type1.Assembly.FullName + "").Name;
            persistentClassInfo1.Interfaces.Add(t1InterfaceInfo);
        }


        Type ITypeHandler<T1, T2>.T1Type {
            get { return _t1Type; }
        }

        Type ITypeHandler<T1, T2>.T2Type {
            get { return _t2Type; }
        }

        IModelInstancesHandler<T1, T2> ITypeHandler<T1, T2>.CreateInstances() {
            _t1 = _objectSpace.CreateObject(_t1Type);
            _t2 = _objectSpace.CreateObject(_t2Type);
            ((IList) ((XPBaseObject) _t1).GetMemberValue(getName(typeof(T2))+"s")).Add(_t2);
            return this;
        }

        ObjectSpace ITypeHandler<T1, T2>.ObjectSpace {
            get { return _objectSpace; }
        }

        T1 IModelInstancesHandler<T1, T2>.T1instance {
            get { return (T1) _t1; }
        }

        T2 IModelInstancesHandler<T1, T2>.T2Instance {
            get { return (T2)_t2; }
        }
    }
}