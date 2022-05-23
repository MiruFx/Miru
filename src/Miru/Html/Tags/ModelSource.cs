namespace Miru.Html.Tags;

public enum ModelSource
{
    /// <summary>
    /// When model is in the attribute 'for' <tag for="" />
    /// </summary>
    For,
        
    /// <summary>
    /// When model is in the view through @model Type
    /// </summary>
    ViewModel,
        
    /// <summary>
    /// When model is in the attribute 'model' <tag model="" />
    /// </summary>
    Model,
        
    /// <summary>
    /// When there is no model at all
    /// </summary>
    NoModel
}