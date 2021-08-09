using System;

namespace Playground.Features.Userfy
{
    public class PermissionAttribute : Attribute
    {
        public Permissions Permission { get; }

        public PermissionAttribute(Permissions permission)
        {
            Permission = permission;
        }
    }
}