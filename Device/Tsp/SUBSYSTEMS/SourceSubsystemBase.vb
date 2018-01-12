Imports System.ComponentModel
Imports isr.Core.Pith
Imports isr.Core.Pith.EnumExtensions
''' <summary>  Defines the contract that must be implemented by a Source Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public Class SourceSubsystemBase
    Inherits SourceMeasureUnitBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.Tsp.StatusSubsystemBase">status subsystem</see>. </param>
    Public Sub New(ByVal statusSubsystem As VI.Tsp.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets values to their known execution reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.SourceFunction = SourceFunctionMode.VoltageDC
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

#Region " SOURCE FUNCTION "

    ''' <summary> The Source Function. </summary>
    Private _SourceFunction As SourceFunctionMode?

    ''' <summary> Gets or sets the cached Source Function. </summary>
    ''' <value> The <see cref="SourceFunctionMode">Source Function</see> or none if not set or
    ''' unknown. </value>
    Public Overloads Property SourceFunction As SourceFunctionMode?
        Get
            Return Me._SourceFunction
        End Get
        Protected Set(ByVal value As SourceFunctionMode?)
            If Not Nullable.Equals(Me.SourceFunction, value) Then
                Me._SourceFunction = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Source Function. </summary>
    ''' <param name="value"> The  Source Function. </param>
    ''' <returns> The <see cref="SourceFunctionMode">Source Function</see> or none if unknown. </returns>
    Public Function ApplySourceFunction(ByVal value As SourceFunctionMode) As SourceFunctionMode?
        Me.WriteSourceFunction(value)
        Return Me.QuerySourceFunction()
    End Function

    ''' <summary> Queries the Source Function. </summary>
    ''' <returns> The <see cref="SourceFunctionMode">Source Function</see> or none if unknown. </returns>
    Public Function QuerySourceFunction() As SourceFunctionMode?
        Dim currentValue As String = Me.SourceFunction.ToString
        Me.Session.MakeEmulatedReplyIfEmpty(currentValue)
        currentValue = Me.Session.QueryPrintTrimEnd("{0}.source.func", Me.SourceMeasureUnitReference)
        If String.IsNullOrWhiteSpace(currentValue) Then
            Dim message As String = "Failed fetching Source Function"
            Debug.Assert(Not Debugger.IsAttached, message)
            Me.SourceFunction = New SourceFunctionMode?
        Else
            Dim se As New StringEnumerator(Of SourceFunctionMode)
            Me.SourceFunction = se.ParseContained(currentValue.BuildDelimitedValue)
        End If
        Return Me.SourceFunction
    End Function

    ''' <summary> Writes the Source Function Without reading back the value from the device. </summary>
    ''' <param name="value"> The Source Function. </param>
    ''' <returns> The <see cref="SourceFunctionMode">Source Function</see> or none if unknown. </returns>
    Public Function WriteSourceFunction(ByVal value As SourceFunctionMode) As SourceFunctionMode?
        Me.Session.WriteLine("{0}.source.func = {0}.{1}", Me.SourceMeasureUnitReference, value.ExtractBetween())
        Me.SourceFunction = value
        Return Me.SourceFunction
    End Function

#End Region

End Class

''' <summary> Specifies the source function modes. </summary>
Public Enum SourceFunctionMode
    <Description("None")> None
    <Description("DC Voltage (OUTPUT_DCVOLTS)")> VoltageDC
    <Description("DC Current (OUTPUT_DCAMPS)")> CurrentDC
End Enum

