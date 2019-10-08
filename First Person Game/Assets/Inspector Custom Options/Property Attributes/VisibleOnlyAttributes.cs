using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Display an individual field as visible-only in the inspector.<br/>
/// Custom <see cref="PropertyDrawer"/>s will not work when using this.<br/>
/// Use <see cref="BeginVisibleOnlyGroupAttribute"/> and <see cref="EndVisibleOnlyGroupAttribute"/> when using with custom <see cref="PropertyDrawer"/>s.
/// </summary>
/// <seealso cref="BeginVisibleOnlyGroupAttribute"/>
/// <seealso cref="EndVisibleOnlyGroupAttribute"/>
public class VisibleOnlyAttribute : PropertyAttribute { }

/// <summary>
/// Display a group of items as visible-only starting from this attribute and ending with <see cref="EndVisibleOnlyGroupAttribute"/><br/>
/// Compatible with custom <see cref="PropertyDrawer"/>s<br/>
/// Close the group with <see cref="EndVisibleOnlyGroupAttribute"/>
/// </summary>
/// <seealso cref="EndVisibleOnlyGroupAttribute"/>
/// <seealso cref="VisibleOnlyAttribute"/>
public class BeginVisibleOnlyGroupAttribute : PropertyAttribute { }

/// <summary>
/// Display a group of items as visible-only starting from <see cref="BeginVisibleOnlyGroupAttribute"/> and ending with this attribute<br/>
/// Compatible with custom <see cref="PropertyDrawer"/>s<br/>
/// Returns to editable fields after this
/// </summary>
/// <seealso cref="BeginVisibleOnlyGroupAttribute"/>
/// <seealso cref="VisibleOnlyAttribute"/>
public class EndVisibleOnlyGroupAttribute : PropertyAttribute { }
