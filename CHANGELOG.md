## [1.0.0] - 2024-07-04
### First Release
- Has Services, Bootstrapping, Component Extensions, Attributes for Runtime
- Contains Drawers for: ConditionalProperty & MinMaxSlider for Editor
- Contains Subsystem Registration for ClearFieldsWithAttribute (ClearOnLoad) - for statics

## [1.0.1] - 2024-07-05
## Hotfix
- Added OrNull to ComponentExtensions
- Removed null coalescing operator from ComponentExtensions when attempting to grab initial Component, replaced with ternary conditional operators
- Components using ComponentExtensions can now use null coalescing for gameobjects, see documentation for details

## [1.1.0] - 2024-08-03
## Feature Addition: SceneField
- SceneField that relates a scene asset to it's name via an implicit operator
- Added SceneFieldPropertyDrawer that allows for scene drag and drop dependency injection via the inspector

## [1.2.1] - 2024-08-05
## Feature Addition: MonoBehaviour Extensions
- Updated package organization name
- Added some simple extensions for MonoBehaviour