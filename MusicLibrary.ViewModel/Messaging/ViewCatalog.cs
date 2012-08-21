using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MusicLibrary.ViewModel.Messaging
{
    public static class ViewCatalog
    {
        #region Fields
        private static Dictionary<Type, Type> _views;
        private static List<IMessageListener> _registeredViews;
        private static Assembly _viewAssembly;
        #endregion

        #region CTOR
        static ViewCatalog()
        {
            _registeredViews = new List<IMessageListener>();
            _views = new Dictionary<Type, Type>();
        }
        #endregion

        #region Public Methods
        public static void Initialize(Assembly viewAssembly)
        {
            _viewAssembly = viewAssembly;
            var typeColletion = viewAssembly.GetTypes();

            foreach (var classType in typeColletion)
            {
                var attribute = (classType as MemberInfo).GetCustomAttributes(false).SingleOrDefault(a => (a is ViewModelAttribute));
                if (attribute != null)
                {
                    _views.Add((attribute as ViewModelAttribute).ViewModelType, classType);
                }
            }
        }

        public static void RegisterView(IMessageSender viewModel)
        {
            Type viewModelType = viewModel.GetType();
            if (_views.ContainsKey(viewModelType))
            {
                IMessageListener listener = Activator.CreateInstance(_views[viewModelType]) as IMessageListener;
                listener.Initialize(viewModel);
                _registeredViews.Add(listener);
            }
        }

        public static void UnregisterView(IMessageListener view)
        {
            if (_registeredViews.Contains(view))
            {
                _registeredViews.Remove(view);
            }
        }
        #endregion
    }
}
