Imports System.Diagnostics.CodeAnalysis
#Region "CA1020:AvoidNamespacesWithFewTypes"

' Namespaces should generally have more than five types. 
' Namespaces, not currently having the prescribed type count, were defined with an eye towards inclusion of additional future types.
<Assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
    Scope:="namespace", Target:="isr.VI.Tsp2",
    Justification:="Ignoring this warning...we want these namespaces, but don't have enough classes to go in them to satisfy the rule.")>
<Assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="isr.VI.Tsp2.LuaSyntax")>
<Assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="isr.VI.Tsp2.TspSyntax")>
<Assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="isr.VI.Tsp2.TspSyntax.Display")>
<Assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="isr.VI.Tsp2.TspSyntax.EventLog")>
<Assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="isr.VI.Tsp2.TspSyntax.LocalNode")>
<Assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="isr.VI.Tsp2.TspSyntax.Node")>
<Assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="isr.VI.Tsp2.TspSyntax.Slot")>
<Assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="isr.VI.Tsp2.TspSyntax.SourceMeasureUnit")>
<Assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="isr.VI.Tsp2.TspSyntax.Status")>


#End Region

#Region "CA1703:ResourceStringsShouldBeSpelledCorrectly"
<Module: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope:="resource", Target:="isr.Tsp.Resources.resources", MessageId:="loadstring")> 
#End Region
