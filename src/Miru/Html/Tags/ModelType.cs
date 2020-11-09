namespace Miru.Html.Tags
{
    public enum ModelType
    {
        /// <summary>
        /// When pass <tag for="" />
        /// </summary>
        For,
        
        /// <summary>
        /// When model is in the view through @model Type
        /// </summary>
        Model,
        
        /// <summary>
        /// When there is no model at all
        /// </summary>
        NoModel
    }
}