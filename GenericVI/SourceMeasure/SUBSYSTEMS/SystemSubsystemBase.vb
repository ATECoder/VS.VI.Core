Imports isr.Core.Pith.ExceptionExtensions
''' <summary>
''' Defines a SCPI System Subsystem for a generic Source Measure instrument such as the Keithley 2400.
''' </summary>
''' <license>
''' (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
Public MustInherit Class SystemSubsystemBase
    Inherits VI.Scpi.SystemSubsystemBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystem" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">message based
    ''' session</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its initial post reset state. </summary>
    ''' <remarks> Additional Actions: <para>
    '''           Clears Error Queue.
    '''           </para></remarks>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Try
            Me.Talker?.Publish(TraceEventType.Verbose, My.MyLibrary.TraceEventId, "Reading options;. ")
            Me.QueryOptions()
        Catch ex As Exception
            Me.Talker?.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId,
                               "Exception reading options;. {0}", ex.ToFullBlownString)
        End Try
    End Sub

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.FourWireSenseEnabled = False
        Me.AutoZeroEnabled = True
        Me.ContactCheckEnabled = False
        Me.ContactCheckResistance = 50

    End Sub

#End Region

#Region " AUTO ZERO ENABLED "

    Private _AutoZeroEnabled As Boolean?

    ''' <summary> Gets or sets a cached value indicating whether auto zero is enabled. </summary>
    ''' <value> <c>True</c> if auto zero is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </value>
    Public Property AutoZeroEnabled() As Boolean?
        Get
            Return Me._AutoZeroEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.AutoZeroEnabled, value) Then
                Me._AutoZeroEnabled = value
                Me.SafePostPropertyChanged(NameOf(Me.AutoZeroEnabled))
            End If
        End Set
    End Property

    ''' <summary> Queries the auto zero enabled state. </summary>
    ''' <returns> <c>True</c> if auto zero is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public MustOverride Function QueryAutoZeroEnabled() As Boolean?

    ''' <summary> Writes and reads back the auto zero enabled state. </summary>
    ''' <param name="value"> If set to <c>True</c> enable. </param>
    ''' <returns> <c>True</c> if auto zero is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function ApplyAutoZeroEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteAutoZeroEnabled(value)
        Return Me.QueryAutoZeroEnabled()
    End Function

    ''' <summary> Writes the auto zero enabled state without reading back the actual value. </summary>
    ''' <param name="value"> if set to <c>True</c> [value]. </param>
    ''' <returns> <c>True</c> if auto zero is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public MustOverride Function WriteAutoZeroEnabled(ByVal value As Boolean) As Boolean?

#End Region

#Region " CONTACT CHECK ENABLED "

    Private _ContactCheckEnabled As Boolean?

    ''' <summary> Gets or sets a cached value indicating whether Contact Check is enabled. </summary>
    ''' <value> <c>True</c> if Contact Check is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </value>
    Public Property ContactCheckEnabled() As Boolean?
        Get
            Return Me._ContactCheckEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.ContactCheckEnabled, value) Then
                Me._ContactCheckEnabled = value
                Me.SafePostPropertyChanged(NameOf(Me.ContactCheckEnabled))
            End If
        End Set
    End Property

    ''' <summary> Queries the Contact Check enabled state. </summary>
    ''' <returns> <c>True</c> if Contact Check is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public MustOverride Function QueryContactCheckEnabled() As Boolean?

    ''' <summary> Writes and reads back the Contact Check enabled state. </summary>
    ''' <param name="value"> If set to <c>True</c> enable. </param>
    ''' <returns> <c>True</c> if Contact Check is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function ApplyContactCheckEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteContactCheckEnabled(value)
        Return Me.QueryContactCheckEnabled()
    End Function

    ''' <summary> Writes the Contact Check enabled state without reading back the actual value. </summary>
    ''' <param name="value"> if set to <c>True</c> [value]. </param>
    ''' <returns> <c>True</c> if Contact Check is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public MustOverride Function WriteContactCheckEnabled(ByVal value As Boolean) As Boolean?

#End Region

#Region " CONTACT CHECK RESISTANCE "

    ''' <summary> The Contact Check Resistance. </summary>
    Private _ContactCheckResistance As Double?

    ''' <summary> Gets or sets the cached source Contact Check Resistance. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property ContactCheckResistance As Double?
        Get
            Return Me._ContactCheckResistance
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.ContactCheckResistance, value) Then
                Me._ContactCheckResistance = value
                Me.SafePostPropertyChanged(NameOf(Me.ContactCheckResistance))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the ContactCheck Resistance. </summary>
    ''' <param name="value"> the ContactCheck Resistance. </param>
    ''' <returns> the ContactCheck Resistance. </returns>
    Public Function ApplyContactCheckResistance(ByVal value As Double) As Double?
        Me.WriteContactCheckResistance(value)
        Return Me.QueryContactCheckResistance
    End Function

    ''' <summary> Gets or sets the ContactCheck Resistance query command. </summary>
    ''' <value> the ContactCheck Resistance query command. </value>
    Protected MustOverride ReadOnly Property ContactCheckResistanceQueryCommand As String

    ''' <summary> Queries the ContactCheck Resistance. </summary>
    ''' <returns> the ContactCheck Resistance or none if unknown. </returns>
    Public Function QueryContactCheckResistance() As Double?
        Me.ContactCheckResistance = MyBase.Query(Me.ContactCheckResistance, Me.ContactCheckResistanceQueryCommand)
        Return Me.ContactCheckResistance
    End Function

    ''' <summary> Gets or sets the ContactCheck Resistance command format. </summary>
    ''' <value> the ContactCheck Resistance command format. </value>
    Protected MustOverride ReadOnly Property ContactCheckResistanceCommandFormat As String

    ''' <summary> Writes the ContactCheck Resistance without reading back the value from the device. </summary>
    ''' <remarks> This command sets the ContactCheck Resistance. </remarks>
    ''' <param name="value"> the ContactCheck Resistance. </param>
    ''' <returns> the ContactCheck Resistance. </returns>
    Public Function WriteContactCheckResistance(ByVal value As Double) As Double?
        Me.ContactCheckResistance = MyBase.Write(value, Me.ContactCheckResistanceCommandFormat)
        Return Me.ContactCheckResistance
    End Function

#End Region

#Region " FOUR WIRE SENSE ENABLED "

    Private _FourWireSenseEnabled As Boolean?

    ''' <summary> Gets or sets a value indicating whether four wire sense is enabled. </summary>
    ''' <value> <c>True</c> if four wire sense mode is enabled; otherwise, <c>False</c>. </value>
    Public Property FourWireSenseEnabled() As Boolean?
        Get
            Return Me._FourWireSenseEnabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.FourWireSenseEnabled, value) Then
                Me._FourWireSenseEnabled = value
                Me.SafePostPropertyChanged(NameOf(Me.FourWireSenseEnabled))
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Four Wire Sense enabled state. </summary>
    ''' <param name="value"> If set to <c>True</c> enable. </param>
    ''' <returns> <c>True</c> if Four Wire Sense is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public Function ApplyFourWireSenseEnabled(ByVal value As Boolean) As Boolean?
        Me.WriteFourWireSenseEnabled(value)
        Return Me.QueryWireSenseEnabled()
    End Function

    ''' <summary> Queries the Four Wire Sense enabled state. </summary>
    ''' <returns> <c>True</c> if Four Wire Sense is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public MustOverride Function QueryWireSenseEnabled() As Boolean?

    ''' <summary> Writes the Four Wire Sense enabled state without reading back the actual value. </summary>
    ''' <param name="value"> if set to <c>True</c> [value]. </param>
    ''' <returns> <c>True</c> if Four Wire Sense is enabled; <c>False</c> if not or none if not set or
    ''' unknown. </returns>
    Public MustOverride Function WriteFourWireSenseEnabled(ByVal value As Boolean) As Boolean?

#End Region

#Region " OPTIONS "

    ''' <summary> Gets or sets the option query command. </summary>
    ''' <value> The option query command. </value>
    Protected Overridable ReadOnly Property OptionQueryCommand As String

    ''' <summary> Gets or sets options for controlling the operation. </summary>
    ''' <value> The options. </value>
    Public Property Options As String

    Public Function QueryOptions() As String
        If String.IsNullOrWhiteSpace(Me.OptionQueryCommand) Then
            Me.Options = ""
        Else
            Me.Options = Me.Session.Query(Me.OptionQueryCommand)
        End If
        Return Me.Options
    End Function

#End Region

#Region " SUPPORTS CONTACT CHECK "

    Private _SupportsContactCheck As Boolean?

    Public Property SupportsContactCheck() As Boolean?
        Get
            Return Me._SupportsContactCheck
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.SupportsContactCheck, value) Then
                Me._SupportsContactCheck = value
                Me.SafePostPropertyChanged(NameOf(Me.SupportsContactCheck))
            End If
        End Set
    End Property

    ''' <summary> Queries support for contact check. </summary>
    ''' <remarks> David, 3/5/2016. </remarks>
    ''' <returns> <c>True</c> if supports contact check; otherwise <c>False</c>. </returns>
    Public MustOverride Function QuerySupportsContactCheck() As Boolean?

#End Region

End Class
