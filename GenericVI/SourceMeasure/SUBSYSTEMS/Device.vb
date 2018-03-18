Imports isr.VI.ExceptionExtensions
''' <summary> Implements a generic source measure device. </summary>
''' <remarks> An instrument is defined, for the purpose of this library, as a device with a front
''' panel. </remarks>
''' <license> (c) 2013 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/10/2013" by="David" revision="3.0.5001"> Created. </history>
Public Class Device
    Inherits DeviceBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SourceMeasure.Device" /> class. </summary>
    Public Sub New()
        MyBase.New()
        Me.ResourcesFilter = VI.ResourceNamesManager.BuildInstrumentFilter()
        AddHandler My.Settings.PropertyChanged, AddressOf Me._Settings_PropertyChanged
    End Sub

    ''' <summary> Creates a new Device. </summary>
    ''' <returns> A Device. </returns>
    Public Shared Function Create() As Device
        Dim device As Device = Nothing
        Try
            device = New Device
        Catch
            If device IsNot Nothing Then device.Dispose()
            device = Nothing
            Throw
        End Try
        Return device
    End Function


#Region " IDisposable Support "

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_ArmLayerSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_ContactCheckLimit", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_ComplianceLimit", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_CompositeLimit", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_DigitalOutput", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_UpperLowerLimit", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_Calculate2Subsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_TriggerSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SystemSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_StatusSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SourceVoltageSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SourceSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SourceCurrentSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SenseVoltageSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SenseSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SenseResistanceSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_SenseCurrentSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_RouteSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_OutputSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_MeasureVoltageSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_MeasureSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_MeasureCurrentSubsystem", Justification:="@Closing")>
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="_FormatSubsystem", Justification:="@Closing")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                ' ?listeners must clear, otherwise closing could raise an exception.
                ' Me.Talker.Listeners.Clear()
                If Me.IsDeviceOpen Then Me.OnClosing(New isr.Core.Pith.CancelDetailsEventArgs)
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception disposing device", "Exception {0}", ex.ToFullBlownString)
        Finally


            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Initializes the Device. Used after reset to set a desired initial state. </summary>
    Public Overrides Sub InitKnownState()
        MyBase.InitKnownState()
        Me.StatusSubsystem.EnableServiceRequest(ServiceRequests.All)
    End Sub

#End Region

#Region " PUBLISHER "

    ''' <summary> Publishes all values by raising the property changed events. </summary>
    Public Overrides Sub Publish()
        Me.Subsystems.Publish()
        If Me.Publishable Then
            For Each p As Reflection.PropertyInfo In Reflection.MethodInfo.GetCurrentMethod.DeclaringType.GetProperties()
                Me.SafePostPropertyChanged(p.Name)
            Next
        End If
    End Sub

#End Region

#Region " SESSION "

    ''' <summary> Allows the derived device to take actions before closing. Removes subsystems and
    ''' event handlers. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnClosing(ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnClosing(e)
        If Not e.Cancel Then
            Me.MeasureSubsystem = Nothing
            Me.FormatSubsystem = Nothing
            Me.OutputSubsystem = Nothing
            Me.RouteSubsystem = Nothing
            Me.SenseCurrentSubsystem = Nothing
            Me.SenseVoltageSubsystem = Nothing
            Me.SenseResistanceSubsystem = Nothing
            Me.SenseSubsystem = Nothing
            Me.SourceCurrentSubsystem = Nothing
            Me.SourceVoltageSubsystem = Nothing
            Me.SourceSubsystem = Nothing
            Me.ArmLayerSubsystem = Nothing
            Me.TriggerSubsystem = Nothing
            Me.Calculate2Subsystem = Nothing
            Me.ContactCheckLimit = Nothing
            Me.ComplianceLimit = Nothing
            Me.CompositeLimit = Nothing
            Me.DigitalOutput = Nothing
            Me.UpperLowerLimit = Nothing
            Me.SystemSubsystem = Nothing
            Me.StatusSubsystem = Nothing
            Me.Subsystems.DisposeItems()
        End If
    End Sub

    ''' <summary> Allows the derived device to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Protected Overrides Sub OnOpening(ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnOpening(e)
        If Not e.Cancel Then
            ' STATUS must be the first subsystem.
            Me.StatusSubsystem = New StatusSubsystem(Me.Session)
            Me.SystemSubsystem = New SystemSubsystem(Me.StatusSubsystem)
            ' better add before the format subsystem, which reset initializes the readings.
            Me.MeasureSubsystem = New MeasureSubsystem(Me.StatusSubsystem)
            ' the measure subsystem reading are initialized when the format system is reset
            Me.FormatSubsystem = New FormatSubsystem(Me.StatusSubsystem)
            Me.OutputSubsystem = New OutputSubsystem(Me.StatusSubsystem)
            Me.RouteSubsystem = New RouteSubsystem(Me.StatusSubsystem)
            Me.SenseCurrentSubsystem = New SenseCurrentSubsystem(Me.StatusSubsystem)
            Me.SenseVoltageSubsystem = New SenseVoltageSubsystem(Me.StatusSubsystem)
            Me.SenseResistanceSubsystem = New SenseResistanceSubsystem(Me.StatusSubsystem)
            Me.SenseSubsystem = New SenseSubsystem(Me.StatusSubsystem)
            Me.SourceCurrentSubsystem = New SourceCurrentSubsystem(Me.StatusSubsystem)
            Me.SourceVoltageSubsystem = New SourceVoltageSubsystem(Me.StatusSubsystem)
            Me.SourceSubsystem = New SourceSubsystem(Me.StatusSubsystem)
            Me.ArmLayerSubsystem = New ArmLayerSubsystem(Me.StatusSubsystem)
            Me.TriggerSubsystem = New TriggerSubsystem(Me.StatusSubsystem)
            Me.Calculate2Subsystem = New Calculate2Subsystem(Me.StatusSubsystem)
            Me.ContactCheckLimit = New ContactCheckLimit(Me.StatusSubsystem)
            Me.ComplianceLimit = New ComplianceLimit(Me.StatusSubsystem)
            Me.CompositeLimit = New CompositeLimit(Me.StatusSubsystem)
            Me.DigitalOutput = New DigitalOutput(Me.StatusSubsystem)
            Me.UpperLowerLimit = New UpperLowerLimit(Me.StatusSubsystem)
        End If
    End Sub

#End Region

#Region " STATUS SESSION "

    ''' <summary> Allows the derived device status subsystem to take actions before closing. Removes subsystems and
    ''' event handlers. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    Protected Overrides Sub OnStatusClosing(ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnClosing(e)
        If Not e.Cancel Then
            Me.StatusSubsystem = Nothing
            Me.Subsystems.DisposeItems()
        End If
    End Sub

    ''' <summary> Allows the derived device status subsystem to take actions before opening. </summary>
    ''' <param name="e"> Event information to send to registered event handlers. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")>
    Protected Overrides Sub OnStatusOpening(ByVal e As isr.Core.Pith.CancelDetailsEventArgs)
        If e Is Nothing Then Throw New ArgumentNullException(NameOf(e))
        MyBase.OnOpening(e)
        If Not e.Cancel Then
            ' check the language status. 
            Me.StatusSubsystem = New StatusSubsystem(Me.Session)
        End If
    End Sub

#End Region

#Region " SUBSYSTEMS "

    Private _ArmLayerSubsystem As ArmLayerSubsystem
    ''' <summary>
    ''' Gets or sets the ArmLayer Subsystem.
    ''' </summary>
    ''' <value>The ArmLayer Subsystem.</value>
    Public Property ArmLayerSubsystem As ArmLayerSubsystem
        Get
            Return Me._ArmLayerSubsystem
        End Get
        Set(value As ArmLayerSubsystem)
            If Me._ArmLayerSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.ArmLayerSubsystem)
                Me.ArmLayerSubsystem.Dispose()
                Me._ArmLayerSubsystem = Nothing
            End If
            Me._ArmLayerSubsystem = value
            If Me._ArmLayerSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.ArmLayerSubsystem)
            End If
        End Set
    End Property

    Private _CompositeLimit As CompositeLimit
    ''' <summary>
    ''' Gets or sets the Composite Limit.
    ''' </summary>
    ''' <value>The Composite Limit.</value>
    Public Property CompositeLimit As CompositeLimit
        Get
            Return Me._CompositeLimit
        End Get
        Set(value As CompositeLimit)
            If Me._CompositeLimit IsNot Nothing Then
                Me.CompositeLimit.Dispose()
                Me._CompositeLimit = Nothing
            End If
            Me._CompositeLimit = value
            If Me._CompositeLimit IsNot Nothing Then
            End If
        End Set
    End Property

    Private _ComplianceLimit As ComplianceLimit
    ''' <summary>
    ''' Gets or sets the Compliance Limit.
    ''' </summary>
    ''' <value>The Compliance Limit.</value>
    Public Property ComplianceLimit As ComplianceLimit
        Get
            Return Me._ComplianceLimit
        End Get
        Set(value As ComplianceLimit)
            If Me._ComplianceLimit IsNot Nothing Then
                Me.ComplianceLimit.Dispose()
                Me._ComplianceLimit = Nothing
            End If
            Me._ComplianceLimit = value
            If Me._ComplianceLimit IsNot Nothing Then
            End If
        End Set
    End Property

    Private _ContactCheckLimit As ContactCheckLimit
    ''' <summary>
    ''' Gets or sets the ContactCheck Limit.
    ''' </summary>
    ''' <value>The ContactCheck Limit.</value>
    Public Property ContactCheckLimit As ContactCheckLimit
        Get
            Return Me._ContactCheckLimit
        End Get
        Set(value As ContactCheckLimit)
            If Me._ContactCheckLimit IsNot Nothing Then
                Me.ContactCheckLimit.Dispose()
                Me._ContactCheckLimit = Nothing
            End If
            Me._ContactCheckLimit = value
            If Me._ContactCheckLimit IsNot Nothing Then
            End If
        End Set
    End Property

    Private _UpperLowerLimit As UpperLowerLimit
    ''' <summary>
    ''' Gets or sets the UpperLower Limit.
    ''' </summary>
    ''' <value>The UpperLower Limit.</value>
    Public Property UpperLowerLimit As UpperLowerLimit
        Get
            Return Me._UpperLowerLimit
        End Get
        Set(value As UpperLowerLimit)
            If Me._UpperLowerLimit IsNot Nothing Then
                Me.UpperLowerLimit.Dispose()
                Me._UpperLowerLimit = Nothing
            End If
            Me._UpperLowerLimit = value
            If Me._UpperLowerLimit IsNot Nothing Then
            End If
        End Set
    End Property

    Private _Calculate2Subsystem As Calculate2Subsystem
    ''' <summary>
    ''' Gets or sets the Calculate2 Subsystem.
    ''' </summary>
    ''' <value>The Calculate2 Subsystem.</value>
    Public Property Calculate2Subsystem As Calculate2Subsystem
        Get
            Return Me._Calculate2Subsystem
        End Get
        Set(value As Calculate2Subsystem)
            If Me._Calculate2Subsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.Calculate2Subsystem)
                Me.Calculate2Subsystem.Dispose()
                Me._Calculate2Subsystem = Nothing
            End If
            Me._Calculate2Subsystem = value
            If Me._Calculate2Subsystem IsNot Nothing Then
                Me.AddSubsystem(Me.Calculate2Subsystem)
            End If
        End Set
    End Property

    Private _DigitalOutput As DigitalOutput
    ''' <summary>
    ''' Gets or sets the Trigger Subsystem.
    ''' </summary>
    ''' <value>The Trigger Subsystem.</value>
    Public Property DigitalOutput As DigitalOutput
        Get
            Return Me._DigitalOutput
        End Get
        Set(value As DigitalOutput)
            If Me._DigitalOutput IsNot Nothing Then
                Me.RemoveSubsystem(Me.DigitalOutput)
                Me.DigitalOutput.Dispose()
                Me._DigitalOutput = Nothing
            End If
            Me._DigitalOutput = value
            If Me._DigitalOutput IsNot Nothing Then
                Me.AddSubsystem(Me.DigitalOutput)
            End If
        End Set
    End Property

    Private _MeasureSubsystem As MeasureSubsystem
    ''' <summary>
    ''' Gets or sets the Measure Subsystem.
    ''' </summary>
    ''' <value>The Measure Subsystem.</value>
    Public Property MeasureSubsystem As MeasureSubsystem
        Get
            Return Me._MeasureSubsystem
        End Get
        Set(value As MeasureSubsystem)
            If Me._MeasureSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.MeasureSubsystem)
                Me.MeasureSubsystem.Dispose()
                Me._MeasureSubsystem = Nothing
            End If
            Me._MeasureSubsystem = value
            If Me._MeasureSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.MeasureSubsystem)
            End If
        End Set
    End Property

    Private _OutputSubsystem As OutputSubsystem
    ''' <summary>
    ''' Gets or sets the Output Subsystem.
    ''' </summary>
    ''' <value>The Output Subsystem.</value>
    Public Property OutputSubsystem As OutputSubsystem
        Get
            Return Me._OutputSubsystem
        End Get
        Set(value As OutputSubsystem)
            If Me._OutputSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.OutputSubsystem)
                Me.OutputSubsystem.Dispose()
                Me._OutputSubsystem = Nothing
            End If
            Me._OutputSubsystem = value
            If Me._OutputSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.OutputSubsystem)
            End If
        End Set
    End Property

    Private _RouteSubsystem As RouteSubsystem
    ''' <summary>
    ''' Gets or sets the Route Subsystem.
    ''' </summary>
    ''' <value>The Route Subsystem.</value>
    Public Property RouteSubsystem As RouteSubsystem
        Get
            Return Me._RouteSubsystem
        End Get
        Set(value As RouteSubsystem)
            If Me._RouteSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.RouteSubsystem)
                Me.RouteSubsystem.Dispose()
                Me._RouteSubsystem = Nothing
            End If
            Me._RouteSubsystem = value
            If Me._RouteSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.RouteSubsystem)
            End If
        End Set
    End Property

    Private _SenseCurrentSubsystem As SenseCurrentSubsystem
    ''' <summary>
    ''' Gets or sets the Sense Current Subsystem.
    ''' </summary>
    ''' <value>The Sense Current Subsystem.</value>
    Public Property SenseCurrentSubsystem As SenseCurrentSubsystem
        Get
            Return Me._SenseCurrentSubsystem
        End Get
        Set(value As SenseCurrentSubsystem)
            If Me._SenseCurrentSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SenseCurrentSubsystem)
                Me.SenseCurrentSubsystem.Dispose()
                Me._SenseCurrentSubsystem = Nothing
            End If
            Me._SenseCurrentSubsystem = value
            If Me._SenseCurrentSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SenseCurrentSubsystem)
            End If
        End Set
    End Property

    Private _SenseResistanceSubsystem As SenseResistanceSubsystem
    ''' <summary>
    ''' Gets or sets the Sense Resistance Subsystem.
    ''' </summary>
    ''' <value>The Sense Resistance Subsystem.</value>
    Public Property SenseResistanceSubsystem As SenseResistanceSubsystem
        Get
            Return Me._SenseResistanceSubsystem
        End Get
        Set(value As SenseResistanceSubsystem)
            If Me._SenseResistanceSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SenseResistanceSubsystem)
                Me.SenseResistanceSubsystem.Dispose()
                Me._SenseResistanceSubsystem = Nothing
            End If
            Me._SenseResistanceSubsystem = value
            If Me._SenseResistanceSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SenseResistanceSubsystem)
            End If
        End Set
    End Property

    Private _SenseVoltageSubsystem As SenseVoltageSubsystem
    ''' <summary>
    ''' Gets or sets the Sense Voltage Subsystem.
    ''' </summary>
    ''' <value>The Sense Voltage Subsystem.</value>
    Public Property SenseVoltageSubsystem As SenseVoltageSubsystem
        Get
            Return Me._SenseVoltageSubsystem
        End Get
        Set(value As SenseVoltageSubsystem)
            If Me._SenseVoltageSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SenseVoltageSubsystem)
                Me.SenseVoltageSubsystem.Dispose()
                Me._SenseVoltageSubsystem = Nothing
            End If
            Me._SenseVoltageSubsystem = value
            If Me._SenseVoltageSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SenseVoltageSubsystem)
            End If
        End Set
    End Property

    Private _SenseSubsystem As SenseSubsystem
    ''' <summary>
    ''' Gets or sets the Sense Subsystem.
    ''' </summary>
    ''' <value>The Sense Subsystem.</value>
    Public Property SenseSubsystem As SenseSubsystem
        Get
            Return Me._SenseSubsystem
        End Get
        Set(value As SenseSubsystem)
            If Me._SenseSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SenseSubsystem)
                Me.SenseSubsystem.Dispose()
                Me._SenseSubsystem = Nothing
            End If
            Me._SenseSubsystem = value
            If Me._SenseSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SenseSubsystem)
            End If
        End Set
    End Property

    Private _SourceCurrentSubsystem As SourceCurrentSubsystem
    ''' <summary>
    ''' Gets or sets the Source Current Subsystem.
    ''' </summary>
    ''' <value>The Source Current Subsystem.</value>
    Public Property SourceCurrentSubsystem As SourceCurrentSubsystem
        Get
            Return Me._SourceCurrentSubsystem
        End Get
        Set(value As SourceCurrentSubsystem)
            If Me._SourceCurrentSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SourceCurrentSubsystem)
                Me.SourceCurrentSubsystem.Dispose()
                Me._SourceCurrentSubsystem = Nothing
            End If
            Me._SourceCurrentSubsystem = value
            If Me._SourceCurrentSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SourceCurrentSubsystem)
            End If
        End Set
    End Property

    Private _SourceVoltageSubsystem As SourceVoltageSubsystem
    ''' <summary>
    ''' Gets or sets the Source Voltage Subsystem.
    ''' </summary>
    ''' <value>The Source Voltage Subsystem.</value>
    Public Property SourceVoltageSubsystem As SourceVoltageSubsystem
        Get
            Return Me._SourceVoltageSubsystem
        End Get
        Set(value As SourceVoltageSubsystem)
            If Me._SourceVoltageSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SourceVoltageSubsystem)
                Me.SourceVoltageSubsystem.Dispose()
                Me._SourceVoltageSubsystem = Nothing
            End If
            Me._SourceVoltageSubsystem = value
            If Me._SourceVoltageSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SourceVoltageSubsystem)
            End If
        End Set
    End Property

    Private _SourceSubsystem As SourceSubsystem
    ''' <summary>
    ''' Gets or sets the Source Subsystem.
    ''' </summary>
    ''' <value>The Source Subsystem.</value>
    Public Property SourceSubsystem As SourceSubsystem
        Get
            Return Me._SourceSubsystem
        End Get
        Set(value As SourceSubsystem)
            If Me._SourceSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.SourceSubsystem)
                Me.SourceSubsystem.Dispose()
                Me._SourceSubsystem = Nothing
            End If
            Me._SourceSubsystem = value
            If Me._SourceSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.SourceSubsystem)
            End If
        End Set
    End Property

    Private _TriggerSubsystem As TriggerSubsystem
    ''' <summary>
    ''' Gets or sets the Trigger Subsystem.
    ''' </summary>
    ''' <value>The Trigger Subsystem.</value>
    Public Property TriggerSubsystem As TriggerSubsystem
        Get
            Return Me._TriggerSubsystem
        End Get
        Set(value As TriggerSubsystem)
            If Me._TriggerSubsystem IsNot Nothing Then
                Me.RemoveSubsystem(Me.TriggerSubsystem)
                Me.TriggerSubsystem.Dispose()
                Me._TriggerSubsystem = Nothing
            End If
            Me._TriggerSubsystem = value
            If Me._TriggerSubsystem IsNot Nothing Then
                Me.AddSubsystem(Me.TriggerSubsystem)
            End If
        End Set
    End Property

#Region " FORMAT "

    Private _FormatSubsystem As FormatSubsystem
    ''' <summary>
    ''' Gets or sets the Format Subsystem.
    ''' </summary>
    ''' <value>The Format Subsystem.</value>
    Public Property FormatSubsystem As FormatSubsystem
        Get
            Return Me._FormatSubsystem
        End Get
        Set(value As FormatSubsystem)
            If Me._FormatSubsystem IsNot Nothing Then
                RemoveHandler Me.FormatSubsystem.PropertyChanged, AddressOf Me.FormatSubsystemPropertyChanged
                Me.RemoveSubsystem(Me.FormatSubsystem)
                Me.FormatSubsystem.Dispose()
                Me._FormatSubsystem = Nothing
            End If
            Me._FormatSubsystem = value
            If Me._FormatSubsystem IsNot Nothing Then
                AddHandler Me.FormatSubsystem.PropertyChanged, AddressOf FormatSubsystemPropertyChanged
                Me.AddSubsystem(Me.FormatSubsystem)
            End If
        End Set
    End Property


    ''' <summary> Handle the Format subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSubsystemPropertyChanged(ByVal subsystem As FormatSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.FormatSubsystemBase.Elements)
                Me.MeasureSubsystem.Readings.Initialize(subsystem.Elements)
        End Select
    End Sub

    ''' <summary> Format subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub FormatSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            If sender IsNot Nothing AndAlso e IsNot Nothing Then
                Me.OnSubsystemPropertyChanged(TryCast(sender, FormatSubsystem), e.PropertyName)
            End If
        Catch ex As Exception
            Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling '{0}' property change. {1}",
                             e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#Region " STATUS "

    Private _StatusSubsystem As StatusSubsystem
    ''' <summary>
    ''' Gets or sets the Status Subsystem.
    ''' </summary>
    ''' <value>The Status Subsystem.</value>
    Public Property StatusSubsystem As StatusSubsystem
        Get
            Return Me._StatusSubsystem
        End Get
        Set(value As StatusSubsystem)
            If Me._StatusSubsystem IsNot Nothing Then
                RemoveHandler Me.StatusSubsystem.PropertyChanged, AddressOf Me.StatusSubsystemPropertyChanged
                Me.RemoveSubsystem(Me.StatusSubsystem)
                Me.StatusSubsystem.Dispose()
                Me._StatusSubsystem = Nothing
            End If
            Me._StatusSubsystem = value
            If Me._StatusSubsystem IsNot Nothing Then
                AddHandler Me.StatusSubsystem.PropertyChanged, AddressOf StatusSubsystemPropertyChanged
                Me.AddSubsystem(Me.StatusSubsystem)
            End If
            Me.StatusSubsystemBase = value
        End Set
    End Property

    ''' <summary> Executes the subsystem property changed action. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overrides Sub OnPropertyChanged(ByVal subsystem As VI.StatusSubsystemBase, ByVal propertyName As String)
        Me.OnPropertyChanged(CType(subsystem, StatusSubsystem), propertyName)
    End Sub

    ''' <summary> Executes the subsystem property changed action. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")>
    Protected Overloads Sub OnPropertyChanged(ByVal subsystem As StatusSubsystem, ByVal propertyName As String)
        MyBase.OnPropertyChanged(subsystem, propertyName)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
        End Select
    End Sub

    ''' <summary> Status subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Protected Overloads Sub StatusSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Dim subsystem As StatusSubsystem = TryCast(sender, StatusSubsystem)
        If subsystem Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnPropertyChanged(subsystem, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               $"{Me.ResourceName} exception handling {NameOf(StatusSubsystem)}.{e.PropertyName} change;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " SYSTEM "

    Private _SystemSubsystem As SystemSubsystem
    ''' <summary>
    ''' Gets or sets the System Subsystem.
    ''' </summary>
    ''' <value>The System Subsystem.</value>
    Public Property SystemSubsystem As SystemSubsystem
        Get
            Return Me._SystemSubsystem
        End Get
        Set(value As SystemSubsystem)
            If Me._SystemSubsystem IsNot Nothing Then
                RemoveHandler Me.SystemSubsystem.PropertyChanged, AddressOf Me.SystemSubsystemPropertyChanged
                Me.RemoveSubsystem(Me.SystemSubsystem)
                Me.SystemSubsystem.Dispose()
                Me._SystemSubsystem = Nothing
            End If
            Me._SystemSubsystem = value
            If Me._SystemSubsystem IsNot Nothing Then
                AddHandler Me.SystemSubsystem.PropertyChanged, AddressOf SystemSubsystemPropertyChanged
                Me.AddSubsystem(Me.SystemSubsystem)
            End If
        End Set
    End Property


    ''' <summary> Handles the Status subsystem property changed event. </summary>
    ''' <param name="subsystem">    The subsystem. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Protected Overridable Overloads Sub OnPropertyChanged(ByVal subsystem As SystemSubsystem, ByVal propertyName As String)
        If subsystem Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(VI.SourceMeasure.SystemSubsystem.Options)
                ' read the contact check option after reading options.
                subsystem.QuerySupportsContactCheck()
        End Select
    End Sub

    ''' <summary> System subsystem property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub SystemSubsystemPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
        Try
            Me.OnPropertyChanged(TryCast(sender, SystemSubsystem), e?.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId,
                               "Exception handling property '{0}' changed event;. {1}", e.PropertyName, ex.ToFullBlownString)
        End Try
    End Sub

#End Region

#End Region

#Region " MIXED SUBSYSTEMS: OUTPUT / MEASURE "

    ''' <summary>
    ''' Sets the specified voltage and current limit and turns on the output.
    ''' </summary>
    ''' <param name="voltage">The voltage.</param>
    ''' <param name="currentLimit">The current limit.</param>
    ''' <param name="overvoltageLimit">The over voltage limit.</param>
    ''' <remarks> See 2400 system manual page 102 of 592. </remarks>
    Public Sub OutputOn(ByVal voltage As Double, ByVal currentLimit As Double, ByVal overvoltageLimit As Double)

        ' turn output off.
        Me.OutputSubsystem.ApplyOutputOnState(False)

        Me.SourceSubsystem.WriteFunctionMode(SourceFunctionModes.VoltageDC)

        ' Set the voltage
        Me.SourceVoltageSubsystem.ApplyLevel(voltage)

        ' Set the over voltage level
        Me.SourceVoltageSubsystem.ApplyProtectionLevel(overvoltageLimit)

        ' Turn on over current protection
        Me.SenseCurrentSubsystem.ApplyProtectionLevel(currentLimit)

        Me.SenseSubsystem.ApplyFunctionModes(VI.Scpi.SenseFunctionModes.Current Or VI.Scpi.SenseFunctionModes.Voltage)

        Me.SenseSubsystem.ApplyConcurrentSenseEnabled(True)

        ' turn output on.
        Me.OutputSubsystem.ApplyOutputOnState(True)

        Me.StatusSubsystem.QueryOperationCompleted()

        ' operation completion is not sufficient to ensure that the Device has hit the correct voltage.

    End Sub

#End Region

#Region " SERVICE REQUEST "

#If False Then
    ''' <summary> Reads the event registers after receiving a service request. </summary>
    ''' <remarks> Handled by the <see cref="DeviceBase"/></remarks>
    Protected Overrides Sub ProcessServiceRequest()
        Me.StatusSubsystem.ReadEventRegisters()
        If Me.StatusSubsystem.MessageAvailable Then
            ' if we have a message this needs to be processed by the subsystem requesting the message.
            ' Only thereafter the registers should be read.
        End If
        If Not Me.StatusSubsystem.MessageAvailable AndAlso Me.StatusSubsystem.ErrorAvailable Then
            Me.StatusSubsystem.QueryDeviceErrors()
        End If
        If Me.StatusSubsystem.MeasurementAvailable Then
        End If
    End Sub
#End If

#End Region

#Region " MY SETTINGS "

    ''' <summary> Opens the settings editor. </summary>
    Public Shared Sub OpenSettingsEditor()
        Using f As Core.Pith.ConfigurationEditor = Core.Pith.ConfigurationEditor.Get
            f.Text = "Source Meter Settings Editor"
            f.ShowDialog(My.MySettings.Default)
        End Using
    End Sub

    ''' <summary> Applies the settings. </summary>
    Protected Overrides Sub ApplySettings()
        Dim settings As My.MySettings = My.MySettings.Default
        Me.OnSettingsPropertyChanged(settings, NameOf(My.MySettings.TraceLogLevel))
        Me.OnSettingsPropertyChanged(settings, NameOf(My.MySettings.TraceShowLevel))
    End Sub

    ''' <summary> Handle the Platform property changed event. </summary>
    ''' <param name="sender">       Source of the event. </param>
    ''' <param name="propertyName"> Name of the property. </param>
    Private Sub OnSettingsPropertyChanged(ByVal sender As My.MySettings, ByVal propertyName As String)
        If sender Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
        Select Case propertyName
            Case NameOf(My.MySettings.TraceLogLevel)
                Me.ApplyTalkerTraceLevel(Core.Pith.ListenerType.Logger, sender.TraceLogLevel)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Trace log level changed to {sender.TraceLogLevel}")
            Case NameOf(My.MySettings.TraceShowLevel)
                Me.ApplyTalkerTraceLevel(Core.Pith.ListenerType.Display, sender.TraceShowLevel)
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"Trace show level changed to {sender.TraceShowLevel}")
            Case NameOf(My.MySettings.InitializeTimeout)
                Me.StatusSubsystemBase.InitializeTimeout = sender.InitializeTimeout
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.InitializeTimeout}")
            Case NameOf(My.MySettings.ResetRefractoryPeriod)
                Me.StatusSubsystemBase.ResetRefractoryPeriod = sender.ResetRefractoryPeriod
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.ResetRefractoryPeriod}")
            Case NameOf(My.MySettings.DeviceClearRefractoryPeriod)
                Me.StatusSubsystemBase.DeviceClearRefractoryPeriod = sender.DeviceClearRefractoryPeriod
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.DeviceClearRefractoryPeriod}")
            Case NameOf(My.MySettings.InitRefractoryPeriod)
                Me.StatusSubsystemBase.InitRefractoryPeriod = sender.InitRefractoryPeriod
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.InitRefractoryPeriod}")
            Case NameOf(My.MySettings.ClearRefractoryPeriod)
                Me.StatusSubsystemBase.ClearRefractoryPeriod = sender.ClearRefractoryPeriod
                Me.Talker.Publish(TraceEventType.Information, My.MyLibrary.TraceEventId, $"{propertyName} changed to {sender.ClearRefractoryPeriod}")
        End Select
    End Sub

    ''' <summary> My settings property changed. </summary>
    ''' <param name="sender"> Source of the event. </param>
    ''' <param name="e">      Property Changed event information. </param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    Private Sub _Settings_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        Dim settings As My.MySettings = TryCast(sender, My.MySettings)
        If settings Is Nothing OrElse e Is Nothing Then Return
        Try
            Me.OnSettingsPropertyChanged(settings, e.PropertyName)
        Catch ex As Exception
            Me.Talker.Publish(TraceEventType.Error, My.MyLibrary.TraceEventId, $"Exception handling Settings.{e.PropertyName} property;. {ex.ToFullBlownString}")
        End Try
    End Sub

#End Region

#Region " TALKER "

    ''' <summary> Identifies the talkers. </summary>
    Protected Overrides Sub IdentifyTalkers()
        Mybase.IdentifyTalkers()
        My.MyLibrary.Identify(Me.Talker)
    End Sub

#End Region

End Class

