using FluentValidation;

namespace Miru.Validation
{
    public class ValidatorFactory
    {
        private readonly IMiruApp _app;

        public ValidatorFactory(IMiruApp app)
        {
            _app = app;
        }

        public IValidator<TRequest> ValidatorFor<TRequest>()
        {
            return _app.TryGet<IValidator<TRequest>>();
        }
    }
}