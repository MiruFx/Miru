using Miru.Userfy;

namespace Miru.Testing.Userfy
{
    public static class TestRunConfigUserfyExtensions
    {
        public static void UserfyRequires<TUser>(this TestRunConfig run) where TUser : class, IUser
        {
            run.BeforeCase<IRequiresAuthenticatedUser>(_ =>
            {
                _.MakeSavingLogin<TUser>();
            });
        }
        
        public static void UserfyRequiresAdmin<TUser>(this TestRunConfig run) where TUser : class, ICanBeAdmin
        {
            run.BeforeCase<IRequiresAuthenticatedAdmin>(_ =>
            {
                _.MakeSavingLogin<TUser>(m => m.IsAdmin = true);
            });
            
            run.UserfyRequires<TUser>();
        }
    }
}