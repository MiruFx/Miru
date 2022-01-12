<!-- 
Introduction
  razor view with loads of facilities
  htmltags, conventions, taghelpers
Views
  Location
    located in /features
    /shared
ViewModel
  feature
Html (Split into Html.md)

  intro: htmltags 
  conventions (config)
    TODO: rails-ujs, bootstrap

  Components
    Table
        Header
        Row
    Form
        Summary
        Label
        Input
        Validation
    Display
        Display Label
        Display

  TODO: helpers
    input
    label
    form
  TODO: taghelpers
    input
    label
    form
    
    miru-if
    miru-if-not
    miru-if-has
    miru-if-any
-->

[[toc]]

# Views & Html

Miru Views are standard [ASP.NET MVC](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/overview) Views.

Miru offers some facilities based on top of [HtmlTags](https://github.com/HtmlTags/htmltags).

## Views

Views are located into Features folders `/src/{App}/Features/{Feature}/{View}.cshtml`.

Shared views are located in `/src/{App}/Features/Shared`.

![](/Views-Location.png)

## Html

Miru comes with a lot of facilities to generate Html based on your Features.

### Conventions

You can configure conventions that will be applied when generating Html. For example, for every property named `Year` the input has maxlength of four:

```csharp
// Convention:
Editors.IfPropertyNameIs("Year").ModifyTag(tag => tag.MaxLength(4));
```

```html
<!-- View: -->
<mi for="Year" />
```

```html
<!-- Generated Html: -->
<input type="text" value="" name="Year" id="Year" maxlength="4" class="form-control">
```

Html Conventions are located in `src/{App}/Config/HtmlConfig.cs`.

