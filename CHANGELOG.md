## [2.2.2] 2024-11-01
### Organization Update: Packaging & Folders
- Updating Package information and folder structure
- Better integration with other strangeman packages

## [2.1.2] - 2024-08-15
## Feature Update: Initialize
- Reverting back to old Initialize interface
- Added additional Initialize interface: InitializeWith
- Note: I do not like updating interfaces like this, contracts should not have to be edited, in the future I will be adding new interfaces to expand functionality over editing old ones to increase theirs

## [2.1.1] - 2024-08-14
## Feature Addition: Application Interactor
- Added Application Interactor to assit with interfacing with Unity's Application class
- Changed and documentation across package
- Added documentation url to package

## [2.0.0] - 2024-08-12
## Feature Addition and Update: Extensions & More
- Rewrite of IInitializable for single parameter
- Added BiDictionary support
- Added Visitor pattern simple implementation
- Added Validation strategy
- Added Builder pattern with reflection
- Added Type extension for Builder reflection

## [1.2.1] - 2024-08-05
## Feature Addition: MonoBehaviour Extensions
- Updated package organization name
- Added some simple extensions for MonoBehaviour

## [1.1.0] - 2024-08-03
## Feature Addition: SceneField
- SceneField that relates a scene asset to it's name via an implicit operator
- Added SceneFieldPropertyDrawer that allows for scene drag and drop dependency injection via the inspector

## [1.0.1] - 2024-07-05
## Hotfix
- Added OrNull to ComponentExtensions
- Removed null coalescing operator from ComponentExtensions when attempting to grab initial Component, replaced with ternary conditional operators
- Components using ComponentExtensions can now use null coalescing for gameobjects, see documentation for details

## [1.0.0] - 2024-07-04
### First Release
- Has Services, Bootstrapping, Component Extensions, Attributes for Runtime
- Contains Drawers for: ConditionalProperty & MinMaxSlider for Editor
- Contains Subsystem Registration for ClearFieldsWithAttribute (ClearOnLoad) - for statics