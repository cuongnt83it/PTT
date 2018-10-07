using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.Mvc;

namespace PTT.Common
{
    public class ReflectionController
    {
        //Lấy danh sách các Controller
        public List<Type> GetControllers(string namespaces)
        {
            List<Type> listController = new List<Type>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            IEnumerable<Type> types = assembly.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type) && type.Namespace.Contains(namespaces)).OrderBy(x => x.Name);
            return types.ToList();
        }
        //Lấy danh sách các action theo controller
        public List<string> GetAction(Type controller)
        {
            List<string> listAction = new List<string>();
            IEnumerable<MemberInfo> memberinfo = controller.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public).Where(
                m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any()).OrderBy(x => x.Name);
            foreach (MemberInfo method in memberinfo)
            {
                if (method.ReflectedType.IsPublic && !method.IsDefined(typeof(NonActionAttribute)) && !listAction.Contains(method.Name))
                {
                    listAction.Add(method.Name.ToString());
                }

            }
            return listAction;
        }
    }

}