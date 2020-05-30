#if UNITY_EDITOR
using UnityEngine;

namespace Pablo
{
  public class ReadOnlyAttribute : PropertyAttribute { }
  public class DisableInPlayModeAttribute : PropertyAttribute { }
  public class DisableInEditorModeAttribute : PropertyAttribute { }
  public class ButtonAttribute : PropertyAttribute { }
  public class ShowInInspectorAttribute : PropertyAttribute { }
}
#endif