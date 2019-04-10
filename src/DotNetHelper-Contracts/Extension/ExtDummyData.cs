//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Text;
//using TheMoFaDe.Extensions;

//namespace TheMoFaDe.Extensions
//{
//    public interface IDataRequirement<T> : IEnumerable<T>
//    {


//    }

//    public interface IListBuilderImpl<T> : IListBuilder<T>
//    {
//        IObjectBuilder<T> CreateObjectBuilder();
//        int Capacity { get; }
//  //      IDeclarationQueue<T> Declarations { get; }
//        IDeclaration<T> AddDeclaration(IDeclaration<T> declaration);
//   //     IUniqueRandomGenerator ScopeUniqueRandomGenerator { get; }
//    }

//    public interface ISingleObjectBuilder<T> : IBuildable<T>
//        	    {
        	        
//        	    }

//    public interface IBuildable<T>
//    {
//      //  BuilderSettings BuilderSettings { get; set; }
//        T Build();
//    }

//    public interface IObjectBuilder<T> : ISingleObjectBuilder<T>
//    {
//        /// <summary>
//        /// Specify a constructor in the form WithConstructor( () => new MyClass(arg1, arg2) )
//        /// </summary>
//        /// <param name="constructor"></param>
//        /// <returns>An object builder</returns>
//        IObjectBuilder<T> WithConstructor(Expression<Func<T>> constructor);
//        IObjectBuilder<T> WithConstructor(Expression<Func<int, T>> constructor);

//        IObjectBuilder<T> With<TFunc>(Func<T, TFunc> func);
//        IObjectBuilder<T> With(Action<T, int> action);

//        IObjectBuilder<T> Do(Action<T> action);
//        IObjectBuilder<T> Do(Action<T, int> action);

//        IObjectBuilder<T> DoMultiple<TAction>(Action<T, TAction> action, IList<TAction> list);
//  //      IObjectBuilder<T> WithPropertyNamer(IPropertyNamer propertyNamer);
//        void CallFunctions(T obj);
//        void CallFunctions(T obj, int objIndex);
//        T Construct(int index);
//        T Name(T obj);
//    }

//    public interface IDeclaration<T>
//    {
//        void Construct();
//        void CallFunctions(IList<T> masterList);
//        void AddToMaster(T[] masterList);
//        int NumberOfAffectedItems { get; }
//        IList<int> MasterListAffectedIndexes { get; }

//        int Start { get; }
//        int End { get; }
//        IListBuilderImpl<T> ListBuilderImpl { get; }

//        IObjectBuilder<T> ObjectBuilder { get; }
//    }
//}

//public static class DummyDataExtensions
//    {
//        /// <summary>
//        /// Sets the value of one of the type's public properties
//        /// </summary>
//        public static IDataRequirement<T> With<T, TFunc>(this IDataRequirement<T> requirement, Func<T, TFunc> func)
//        {
//            var declaration = GetDeclaration(requirement);

//            declaration.ObjectBuilder.With(func);
//            return (IDataRequirement<T>)declaration;
//        }

//        /// <summary>
//        /// Sets the value of one of the type's public properties and provides the index of the object being set
//        /// </summary>
//        public static IDataRequirement<T> With<T>(this IDataRequirement<T> requirement, Action<T, int> func)
//        {
//            var declaration = GetDeclaration(requirement);

//            declaration.ObjectBuilder.With(func);
//            return (IDataRequirement<T>)declaration;
//        }

//        /// <summary>
//        /// Sets the value of one of the type's private properties or readonly fields
//        /// </summary>
//        public static IDataRequirement<T> With<T, TProperty>(this IDataRequirement<T> requirement, Expression<Func<T, TProperty>> property, TProperty value)
//        {
//            var declaration = GetDeclaration(requirement);

//            declaration.ObjectBuilder.With(property, value);
//            return (IDataRequirement<T>)declaration;
//        }

//        /// <summary>
//        /// Sets the value of one of the type's public properties
//        /// </summary>
//        public static IDataRequirement<T> And<T, TFunc>(this IDataRequirement<T> requirement, Func<T, TFunc> func)
//        {
//            return With(requirement, func);
//        }

//        /// <summary>
//        /// Sets the value of one of the type's public properties and provides the index of the object being set
//        /// </summary>
//        public static IDataRequirement<T> And<T>(this IDataRequirement<T> requirement, Action<T, int> func)
//        {
//            return With(requirement, func);
//        }

//        /// <summary>
//        /// Sets the value of one of the type's private properties or readonly fields
//        /// </summary>
//        public static IDataRequirement<T> And<T, TProperty>(this IDataRequirement<T> requirement, Expression<Func<T, TProperty>> property, TProperty value)
//        {
//            return With(requirement, property, value);
//        }


//        /// <summary>
//        /// Performs an action on the type.
//        /// </summary>
//        public static IDataRequirement<T> And<T>(this IDataRequirement<T> requirement, Action<T> action)
//        {
//            return Do(requirement, action);
//        }

//        /// <summary>
//        /// Specify the constructor for the type like this:
//        /// 
//        /// WithConstructor( () => new MyType(arg1, arg2) )
//        /// </summary>
//        public static IDataRequirement<T> WithConstructor<T>(this IDataRequirement<T> requirement, Expression<Func<T>> constructor)
//        {
//            var declaration = GetDeclaration(requirement);
//            declaration.ObjectBuilder.WithConstructor(constructor);
//            return (IDataRequirement<T>)declaration;
//        }

//        /// <summary>
//        /// Specify the constructor for the type like this:
//        /// 
//        /// WithConstructor( () => new MyType(arg1, arg2) )
//        /// </summary>
//        public static IDataRequirement<T> WithConstructor<T>(this IDataRequirement<T> requirement, Expression<Func<int, T>> constructor)
//        {
//            var declaration = GetDeclaration(requirement);
//            declaration.ObjectBuilder.WithConstructor(constructor);
//            return (IDataRequirement<T>)declaration;
//        }


//        /// <summary>
//        /// Performs an action on the object.
//        /// </summary>
//        public static IDataRequirement<T> Do<T>(this IDataRequirement<T> requirement, Action<T> action)
//        {
//            var declaration = GetDeclaration(requirement);
//            declaration.ObjectBuilder.Do(action);
//            return (IDataRequirement<T>)declaration;
//        }

//        /// <summary>
//        /// Performs an action on the object.
//        /// </summary>
//        public static IDataRequirement<T> Do<T>(this IDataRequirement<T> requirement, Action<T, int> action)
//        {
//            var declaration = GetDeclaration(requirement);
//            declaration.ObjectBuilder.Do(action);
//            return (IDataRequirement<T>)declaration;
//        }


//        /// <summary>
//        /// Performs an action for each item in a list.
//        /// </summary>
//        public static IDataRequirement<T> DoForEach<T, U>(this IDataRequirement<T> requirement, Action<T, U> action, IList<U> list)
//        {
//            var declaration = GetDeclaration(requirement);
//            declaration.ObjectBuilder.DoMultiple(action, list);
//            return (IDataRequirement<T>)declaration;
//        }


//        private static IDeclaration<T> GetDeclaration<T>(IDataRequirement<T> requirement)
//        {
//            var declaration = requirement as IDeclaration<T>;

//            if (declaration == null)
//                throw new ArgumentException("Must be of type IDeclaration<T>");

//            return declaration;
//        }
    
//}
