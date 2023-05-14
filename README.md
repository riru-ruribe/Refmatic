# Refmatic
'Refmatic' is coined word combining reference and automatic, a tools for Unity.

## Overview
・In scenes and other objects, you can automatically refer to fields on the 'SerializeField' attribute, detect hierarchy changes, etc. and automatically update them.

・When assets in a specific folder are added/deleted or otherwise changed, references can be updated to the objects that refer to them.

## Meta Data
Scripts that use attributes provided by 'Refmatic' and folders to be automatically updated are recorded in 'ScriptableObject' as metadata.

Specifically, the following classes
- **RefmaticAttributeMetaPrefs**
  - automatically updated when changes are detected in AssetPostprocessor.OnPostprocessAllAssets after compilation.
  - not need to be aware.
- **RefmaticFolderMetaPrefs**
  - changed the Inspector layout of the folder to its own format.
  - if you checked the 'AutoRefmatic Imported' flag, the folder is subject to automatic updates. and will be recorded.
  - if synchronization is required, such as when a folder is deleted, it can be updated from the menu "Beryl > Refmatic > Meta > Validate FolderMetaPrefs".

## Attributes
Below is a list of available attributes.
- **RefmaticExecutableChild**
  - reference the object matching the field name from the child hierarchy.
- **RefmaticElementChild**
  - use in conjunction with 'RefmaticExecutableChild', allows changing name lookup method.
- **RefmaticExecutableLoad**
  - reference assets using 'AssetDatabase.FindAssets'. also make sure to set 'RefmaticElementLoad'.
- **RefmaticElementLoad**
  - allows changing name lookup method. recommended to specify searchInFolders in detail, because find for assets is very time consuming.
- **RefmaticExecutableMyself**
  - as the name suggests, you can reference the component from myself.

For the following attributes, please also check the "Resolvers".
- **RefmaticGenericKeySelector**
  - can change serialization format of **TKey** when using 'SerializableGeneric<TKey, TValue>'.
- **RefmaticLoadTypeSelector**
  - resolve the difference between the class referenced by 'Component' and the class loaded from 'Assets' folder.
- **RefmaticSingleSelector**
  - change the serialization format of the loaded or found object.
- **RefmaticTupleSelector**
  - change the serialization format of the loaded or found array objects.

## Resolvers
Allows some freedom in how assets and objects are referenced.

Selectors for general used in project can be recorded in 'ScriptableObject'.

Specifically, the following classes
- **RefmaticAssetPickResolver**
  - can change the loading format of your assets.
- **RefmaticGenericKeyResolver**
- **RefmaticLoadTypeResolver**
- **RefmaticSingleResolver**
- **RefmaticTupleResolver**

When implementing a special Selector, if it is an inner class, it will not be registered with the 'ScriptableObject'.

## Use Cases
Detailed sample is provided in the following folder.

**Assets/Beryl/Samples/Runtime/Refmatic/UseCases/**

However, it is certainly not very practical.
