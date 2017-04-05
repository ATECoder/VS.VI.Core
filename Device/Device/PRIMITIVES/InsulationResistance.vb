Imports isr.Core.Pith
''' <summary> An insulation test configuration. </summary>
''' <remarks> David, 3/9/2016. </remarks>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="3/9/2016" by="David" revision=""> Created. </history>
Public Class InsulationResistance
    Inherits PropertyPublisherBase
    Implements IPresettablePropertyPublisher

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets values to their known execution clear state. </summary>
    Public Sub ClearExecutionState() Implements IPresettable.ClearExecutionState
    End Sub

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Use this method to customize the reset. </remarks>
    Public Sub InitKnownState() Implements IPresettable.InitKnownState
    End Sub

    ''' <summary> Sets values to their known execution preset state. </summary>
    Public Sub PresetKnownState() Implements IPresettable.PresetKnownState
    End Sub

    ''' <summary> Sets values to their known execution reset state. </summary>
    Public Sub ResetKnownState() Implements IPresettable.ResetKnownState
        Me.DwellTime = TimeSpan.FromSeconds(2)
        Me.CurrentLimit = 0.00001
        Me.PowerLineCycles = 1
        Me.VoltageLevel = 10
        Me.ResistanceLowLimit = 10000000
        Me.ResistanceRange = 1000000000
        Me.ContactCheckEnabled = True
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

    ''' <summary> Gets or sets the dwell time. </summary>
    ''' <value> The dwell time. </value>
    Public Property DwellTime As TimeSpan

    ''' <summary> Gets or sets the current limit. </summary>
    ''' <value> The current limit. </value>
    Public Property CurrentLimit As Double

    ''' <summary> Gets or sets the power line cycles. </summary>
    ''' <value> The power line cycles. </value>
    Public Property PowerLineCycles As Double

    ''' <summary> Gets or sets the voltage level. </summary>
    ''' <value> The voltage level. </value>
    Public Property VoltageLevel As Double

    ''' <summary> Gets or sets the resistance low limit. </summary>
    ''' <value> The resistance low limit. </value>
    Public Property ResistanceLowLimit As Double

    ''' <summary> Gets or sets the resistance range. </summary>
    ''' <value> The resistance range. </value>
    Public Property ResistanceRange As Double

    ''' <summary> Gets or sets the contact check enabled. </summary>
    ''' <value> The contact check enabled. </value>
    Public Property ContactCheckEnabled As Boolean

    ''' <summary>
    ''' Gets the current range.
    ''' Based on the ratio of the voltage to the minimum resistance. 
    ''' If the resistance is zero, returns the current limit.
    ''' </summary>
    Public ReadOnly Property CurrentRange() As Double
        Get
            If Me.ResistanceLowLimit > 0 Then
                Return Me.VoltageLevel / Me.ResistanceLowLimit
            Else
                Return 1.01 * Me.CurrentLimit
            End If
        End Get
    End Property


End Class
