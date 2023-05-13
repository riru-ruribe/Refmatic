using UnityEditor;
using UnityEngine;
using Beryl.Util;

namespace Beryl.Refmatic
{
    [InitializeOnLoad]
    static class AutoRefmaticObserver
    {
        [System.Diagnostics.Conditional("LOG_ENABLED_REFMATIC")]
        static void LogEvent(ObjectChangeKind kind, GameObject go)
        {
#if UNITY_2021_2_OR_NEWER
            var sb = new UniSpanSb(2048);
            sb.NameWithParent(go.transform);
            sb.PrependColon();
            sb.Prepend(kind);
            Debug.LogWarning(sb.ResultWithDispose);
#else
            Debug.LogWarning($"{kind.ToString()}: {go.transform.NameWithParent()}");
#endif
        }

        static AutoRefmaticObserver()
        {
            ObjectChangeEvents.changesPublished += (ref ObjectChangeEventStream stream) =>
            {
                if (!RefmaticEditorPrefs.AutoMode.Has) return;

                for (int i = 0, len = stream.length; i < len; i++)
                {
                    var kind = stream.GetEventType(i);
                    switch (kind)
                    {
                        case ObjectChangeKind.CreateGameObjectHierarchy:
                            {
                                CreateGameObjectHierarchyEventArgs args;
                                stream.GetCreateGameObjectHierarchyEvent(i, out args);
                                var go = EditorUtility.InstanceIDToObject(args.instanceId) as GameObject;
                                LogEvent(kind, go);
                                RefmaticContext.OnChangedInChild<IAutoRefmatic>(go.transform);
                            }
                            break;
                        case ObjectChangeKind.ChangeGameObjectStructureHierarchy:
                            {
                                ChangeGameObjectStructureHierarchyEventArgs args;
                                stream.GetChangeGameObjectStructureHierarchyEvent(i, out args);
                                var go = EditorUtility.InstanceIDToObject(args.instanceId) as GameObject;
                                LogEvent(kind, go);
                                RefmaticContext.OnChangedInChild<IAutoRefmatic>(go.transform);
                            }
                            break;
                        case ObjectChangeKind.ChangeGameObjectStructure:
                            {
                                ChangeGameObjectStructureEventArgs args;
                                stream.GetChangeGameObjectStructureEvent(i, out args);
                                var go = EditorUtility.InstanceIDToObject(args.instanceId) as GameObject;
                                LogEvent(kind, go);
                                RefmaticContext.OnChanged<IAutoRefmatic>(go.transform);
                            }
                            break;
                        case ObjectChangeKind.ChangeGameObjectParent:
                            {
                                ChangeGameObjectParentEventArgs args;
                                stream.GetChangeGameObjectParentEvent(i, out args);
                                var go = EditorUtility.InstanceIDToObject(args.previousParentInstanceId) as GameObject;
                                if (go != null)
                                {
                                    LogEvent(kind, go);
                                    RefmaticContext.OnChanged<IAutoRefmatic>(go.transform);
                                }
                                go = EditorUtility.InstanceIDToObject(args.newParentInstanceId) as GameObject;
                                if (go != null)
                                {
                                    LogEvent(kind, go);
                                    RefmaticContext.OnChanged<IAutoRefmatic>(go.transform);
                                }
                            }
                            break;
                        case ObjectChangeKind.ChangeGameObjectOrComponentProperties:
                            {
                                ChangeGameObjectOrComponentPropertiesEventArgs args;
                                stream.GetChangeGameObjectOrComponentPropertiesEvent(i, out args);
                                var go = EditorUtility.InstanceIDToObject(args.instanceId) as GameObject;
                                if (go == null) continue; // deleted
                                LogEvent(kind, go);
                                RefmaticContext.OnChangedInChild<IAutoRefmatic>(go.transform);
                            }
                            break;
#if UNITY_2021_2_OR_NEWER
                        case ObjectChangeKind.DestroyGameObjectHierarchy:
                            {
                                DestroyGameObjectHierarchyEventArgs args;
                                stream.GetDestroyGameObjectHierarchyEvent(i, out args);
                                var go = EditorUtility.InstanceIDToObject(args.parentInstanceId) as GameObject;
                                if (go == null) continue; // not parent
                                LogEvent(kind, go);
                                RefmaticContext.OnChanged<IAutoRefmatic>(go.transform);
                            }
                            break;
#endif
                    }
                }
            };
        }
    }
}
