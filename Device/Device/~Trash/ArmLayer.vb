Imports isr.Core.Pith
''' <summary> Defines an Arm Layer for the Trigger SCPI subsystem. </summary>
''' <license> (c) 2010 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/5/2013" by="David" revision=""> Created based on SCPI 5.1 library. </history>
Public Class ArmLayer
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
        Me.Count = 1
        Me.Delay = TimeSpan.Zero
        Me.Direction = VI.Direction.Acceptor
        Me.InputLineNumber = 1
        Me.OutputLineNumber = 2
        Me.ArmSource = VI.ArmSources.Immediate
        Me.TimerInterval = TimeSpan.FromSeconds(0.001)
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.AsyncNotifyPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " ARM SOURCE "

    ''' <summary> The arm source mode. </summary>
    Private _ArmSource As ArmSources

    ''' <summary> Gets or sets the cached arm Source. </summary>
    ''' <value> The <see cref="ArmSource">source Function Mode</see> or none if not set or
    ''' unknown. </value>
    Public Property ArmSource As ArmSources
        Get
            Return Me._ArmSource
        End Get
        Set(ByVal value As ArmSources)
            If Not Me.ArmSource.Equals(value) Then
                Me._ArmSource = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.ArmSource))
            End If
        End Set
    End Property

#End Region

#Region " DIRECTION "

    ''' <summary> The ARM Direction. </summary>
    Private _Direction As Direction

    ''' <summary> Gets or sets the cached source Direction. </summary>
    ''' <value> The <see cref="Direction">ARM Direction</see> or none if not set or
    ''' unknown. </value>
    Public Property Direction As Direction
        Get
            Return Me._Direction
        End Get
        Set(ByVal value As Direction)
            If Not Me.Direction.Equals(value) Then
                Me._Direction = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Direction))
            End If
        End Set
    End Property

#End Region

#Region " COUNT "

    ''' <summary> The Count. </summary>
    Private _Count As Integer

    ''' <summary> Gets or sets the cached source Count. </summary>
    ''' <value> The source Count or none if not set or unknown. </value>
    Public Property Count As Integer
        Get
            Return Me._Count
        End Get
        Set(ByVal value As Integer)
            If Not Nullable.Equals(Me.Count, value) Then
                Me._Count = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Count))
            End If
        End Set
    End Property

#End Region

#Region " DELAY "

    ''' <summary> The delay. </summary>
    Private _Delay As TimeSpan

    ''' <summary> Gets or sets the cached Delay. </summary>
    ''' <value> The source Delay or none if not set or unknown. </value>
    Public Property Delay As TimeSpan
        Get
            Return Me._Delay
        End Get
        Set(ByVal value As TimeSpan)
            If Not Nullable.Equals(Me.Delay, value) Then
                Me._Delay = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.Delay))
            End If
        End Set
    End Property

#End Region

#Region " INPUT LINE NUMBER "

    ''' <summary> The Input Line Number. </summary>
    Private _InputLineNumber As Integer

    ''' <summary> Gets or sets the cached source Input Line Number. </summary>
    ''' <value> The source Input Line Number or none if not set or unknown. </value>
    Public Property InputLineNumber As Integer
        Get
            Return Me._InputLineNumber
        End Get
        Set(ByVal value As Integer)
            If Not Nullable.Equals(Me.InputLineNumber, value) Then
                Me._InputLineNumber = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.InputLineNumber))
            End If
        End Set
    End Property

#End Region

#Region " OUTPUT LINE NUMBER "

    ''' <summary> The Output Line Number. </summary>
    Private _OutputLineNumber As Integer

    ''' <summary> Gets or sets the cached source Output Line Number. </summary>
    ''' <value> The source Output Line Number or none if not set or unknown. </value>
    Public Property OutputLineNumber As Integer
        Get
            Return Me._OutputLineNumber
        End Get
        Set(ByVal value As Integer)
            If Not Nullable.Equals(Me.OutputLineNumber, value) Then
                Me._OutputLineNumber = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.OutputLineNumber))
            End If
        End Set
    End Property

#End Region

#Region " TIMER INTERVAL "

    ''' <summary> The Timer Interval. </summary>
    Private _TimerInterval As TimeSpan

    ''' <summary> Gets or sets the cached source Timer Interval. </summary>
    ''' <value> The source Timer Interval or none if not set or unknown. </value>
    Public Property TimerInterval As TimeSpan
        Get
            Return Me._TimerInterval
        End Get
        Set(ByVal value As TimeSpan)
            If Not Me.TimerInterval.Equals(value) Then
                Me._TimerInterval = value
                Me.AsyncNotifyPropertyChanged(NameOf(Me.TimerInterval))
            End If
        End Set
    End Property

#End Region

End Class

