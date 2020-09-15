using System;

namespace Nerdomat.Tools
{
    public class Atributes
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
        public class ModuleName : Attribute
        {
            private string name;
            public ModuleName(string name)
            {
                this.name = name;
            }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
        public class ModuleActive : Attribute
        {
            private bool active;
            public ModuleActive(bool active)
            {
                this.active = active;
            }
        }

        [AttributeUsage(AttributeTargets.Method)]
        public class ModuleAdmin : Attribute
        {
            public ModuleAdmin() { }
        }
    }
}