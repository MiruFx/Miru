using System;

namespace Miru
{
    public class FeatureResult
    {
        public object Model { get; set; }

        public Type Type => Model.GetType();
    }

    public class FeatureResult<TFeature> : FeatureResult where TFeature : new()
    {
        public FeatureResult()
        {
            Model = new TFeature();
        }

        public FeatureResult(TFeature instance)
        {
            Model = instance;
        }
    }
}