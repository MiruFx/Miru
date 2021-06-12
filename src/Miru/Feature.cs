using System;
using MediatR;

namespace Miru
{
    public class Feature
    {
        public object Model { get; set; }

        public Type Type => Model.GetType();
    }

    public class Feature<TFeature> : Feature where TFeature : new()
    {
        public Feature()
        {
            Model = new TFeature();
        }

        public Feature(TFeature instance)
        {
            Model = instance;
        }
    }
}